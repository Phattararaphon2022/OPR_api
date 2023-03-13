using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;
using System.Web;
using ClassLibrary_BPC.hrfocus.controller;
using ClassLibrary_BPC.hrfocus.model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Hosting;
using System.Drawing;
using AntsCode.Util;
using System.Web.Security;
using HRFocusWCF;
using System.Security.Permissions;
using System.Net;
using System.IdentityModel.Tokens.Jwt;
using System.Configuration;
using System.Web.Script.Serialization;
using ClassLibrary_BPC.hrfocus.service;
using ClassLibrary_BPC.hrfocus.model.System;

namespace BPC_OPR
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "ModuleSystem" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select ModuleSystem.svc or ModuleSystem.svc.cs at the Solution Explorer and start debugging.

    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]

    public class ModuleSystem : IModuleSystem
    {

        BpcOpr objBpcOpr = new BpcOpr();

        private async Task<bool> doUploadFile(string fileName, Stream stream)
        {
            bool result = false;

            try
            {
                string FilePath = Path.Combine
                  (ClassLibrary_BPC.Config.PathFileImport + "\\Imports", fileName);

                MultipartParser parser = new MultipartParser(stream);

                if (parser.Success)
                {
                    //absolute filename, extension included.
                    var filename = parser.Filename;
                    var filetype = parser.ContentType;
                    var ext = Path.GetExtension(filename);

                    using (var file = File.Create(FilePath))
                    {
                        await file.WriteAsync(parser.FileContents, 0, parser.FileContents.Length);
                        result = true;

                    }
                }

            }
            catch { }

            return result;

        }

        #region MTBank
        public string getBankList(BasicRequest req)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "SYS001.1";
            log.apilog_by = req.username;
            log.apilog_data = "all";

            try
            {
                var authHeader = WebOperationContext.Current.IncomingRequest.Headers["Authorization"];
                if (authHeader == null || !objBpcOpr.doVerify(authHeader))
                {
                    output["success"] = false;
                    output["message"] = BpcOpr.MessageNotAuthen;

                    log.apilog_status = "500";
                    log.apilog_message = BpcOpr.MessageNotAuthen;
                    objBpcOpr.doRecordLog(log);

                    return output.ToString(Formatting.None);
                }

                cls_ctMTBank controller = new cls_ctMTBank();
                List<cls_MTBank> list = controller.getDataByFillter("");
                JArray array = new JArray();

                if (list.Count > 0)
                {
                    int index = 1;

                    foreach (cls_MTBank model in list)
                    {
                        JObject json = new JObject();
                        json.Add("bank_id", model.bank_id);
                        json.Add("bank_code", model.bank_code);
                        json.Add("bank_name_th", model.bank_name_th);
                        json.Add("bank_name_en", model.bank_name_en);
                        json.Add("modified_by", model.modified_by);
                        json.Add("modified_date", model.modified_date);
                        json.Add("index", index++);
                        array.Add(json);
                    }

                    output["success"] = true;
                    output["message"] = "";
                    output["data"] = array;

                    log.apilog_status = "200";
                    log.apilog_message = "";
                }
                else
                {
                    output["success"] = false;
                    output["message"] = "Data not Found";
                    output["data"] = array;

                    log.apilog_status = "404";
                    log.apilog_message = "Data not Found";
                }

                controller.dispose();
            }
            catch (Exception ex)
            {
                output["success"] = false;
                output["message"] = "(C)Retrieved data not successfully";

                log.apilog_status = "500";
                log.apilog_message = ex.ToString();
            }
            finally
            {
                objBpcOpr.doRecordLog(log);
            }

            return output.ToString(Formatting.None);
        }
        public string doManageMTBank(InputMTBank input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);


            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "SYS001.2";
            log.apilog_by = input.modified_by;
            log.apilog_data = tmp.ToString();

            try
            {
                var authHeader = WebOperationContext.Current.IncomingRequest.Headers["Authorization"];
                if (authHeader == null || !objBpcOpr.doVerify(authHeader))
                {
                    output["success"] = false;
                    output["message"] = BpcOpr.MessageNotAuthen;

                    log.apilog_status = "500";
                    log.apilog_message = BpcOpr.MessageNotAuthen;
                    objBpcOpr.doRecordLog(log);

                    return output.ToString(Formatting.None);
                }

                cls_ctMTBank controller = new cls_ctMTBank();
                cls_MTBank model = new cls_MTBank();

                model.bank_id = Convert.ToInt32(input.bank_id);
                model.bank_code = input.bank_code;
                model.bank_name_th = input.bank_name_th;
                model.bank_name_en = input.bank_name_en;
                model.modified_by = input.modified_by;

                string strID = controller.insert(model);

                if (!strID.Equals(""))
                {
                    output["success"] = true;
                    output["message"] = "Retrieved data successfully";
                    output["record_id"] = strID;

                    log.apilog_status = "200";
                    log.apilog_message = "";
                }
                else
                {
                    output["success"] = false;
                    output["message"] = "Retrieved data not successfully";

                    log.apilog_status = "500";
                    log.apilog_message = controller.getMessage();
                }

                controller.dispose();

            }
            catch (Exception ex)
            {
                output["success"] = false;
                output["message"] = "(C)Retrieved data not successfully";

                log.apilog_status = "500";
                log.apilog_message = ex.ToString();
            }
            finally
            {
                objBpcOpr.doRecordLog(log);
            }

            output["data"] = tmp;

            return output.ToString(Formatting.None);
        }
        public string doDeleteMTBank(InputMTBank input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "SYS001.3";
            log.apilog_by = input.modified_by;
            log.apilog_data = tmp.ToString();

            try
            {
                var authHeader = WebOperationContext.Current.IncomingRequest.Headers["Authorization"];
                if (authHeader == null || !objBpcOpr.doVerify(authHeader))
                {
                    output["success"] = false;
                    output["message"] = BpcOpr.MessageNotAuthen;
                    log.apilog_status = "500";
                    log.apilog_message = BpcOpr.MessageNotAuthen;
                    objBpcOpr.doRecordLog(log);

                    return output.ToString(Formatting.None);
                }

                cls_ctMTBank controller = new cls_ctMTBank();

                if (controller.checkDataOld(input.bank_code))
                {
                    bool blnResult = controller.delete(input.bank_code);

                    if (blnResult)
                    {
                        output["success"] = true;
                        output["message"] = "Remove data successfully";

                        log.apilog_status = "200";
                        log.apilog_message = "";
                    }
                    else
                    {
                        output["success"] = false;
                        output["message"] = "Remove data not successfully";

                        log.apilog_status = "500";
                        log.apilog_message = controller.getMessage();
                    }

                }
                else
                {
                    string message = "Not Found Project code : " + input.bank_code;
                    output["success"] = false;
                    output["message"] = message;

                    log.apilog_status = "404";
                    log.apilog_message = message;
                }

                controller.dispose();
            }
            catch (Exception ex)
            {
                output["success"] = false;
                output["message"] = "(C)Remove data not successfully";

                log.apilog_status = "500";
                log.apilog_message = ex.ToString();
            }
            finally
            {
                objBpcOpr.doRecordLog(log);
            }

            output["data"] = tmp;

            return output.ToString(Formatting.None);

        }
        public async Task<string> doUploadBank(string token, string by, string fileName, Stream stream)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "SYS001.4";
            log.apilog_by = by;
            log.apilog_data = "Stream";

            try
            {
                if (!objBpcOpr.doVerify(token))
                {
                    output["success"] = false;
                    output["message"] = BpcOpr.MessageNotAuthen;

                    log.apilog_status = "500";
                    log.apilog_message = BpcOpr.MessageNotAuthen;
                    objBpcOpr.doRecordLog(log);

                    return output.ToString(Formatting.None);
                }


                bool upload = await this.doUploadFile(fileName, stream);

                if (upload)
                {
                    cls_srvSystemImport srv_import = new cls_srvSystemImport();
                    string tmp = srv_import.doImportExcel("BANK", fileName, "TEST");

                    output["success"] = true;
                    output["message"] = tmp;

                    log.apilog_status = "200";
                    log.apilog_message = "";
                }
                else
                {
                    output["success"] = false;
                    output["message"] = "Upload data not successfully";

                    log.apilog_status = "500";
                    log.apilog_message = "Upload data not successfully";
                }

            }
            catch (Exception ex)
            {
                output["success"] = false;
                output["message"] = "(C)Upload data not successfully";

                log.apilog_status = "500";
                log.apilog_message = ex.ToString();
            }
            finally
            {
                objBpcOpr.doRecordLog(log);
            }

            return output.ToString(Formatting.None);
        }
        #endregion

        //#region Policy structure code
        ////public string getSYSCodestructureList(BasicRequest req)
        ////{
        ////    JObject output = new JObject();

        ////    cls_ctMTCodestructure objStruc = new cls_ctMTCodestructure();
        ////    List<cls_MTCodestructure> listStruc = objStruc.getData();

        ////    JArray array = new JArray();

        ////    if (listStruc.Count > 0)
        ////    {
        ////        int index = 1;

        ////        foreach (cls_MTCodestructure model in listStruc)
        ////        {
        ////            JObject json = new JObject();

        ////            json.Add("codestructure_code", model.codestructure_code);
        ////            json.Add("codestructure_name_th", model.codestructure_name_th);
        ////            json.Add("codestructure_name_en", model.codestructure_name_en);

        ////            json.Add("index", index);

        ////            index++;

        ////            array.Add(json);
        ////        }

        ////        output["result"] = "1";
        ////        output["result_text"] = "1";
        ////        output["data"] = array;
        ////    }
        ////    else
        ////    {
        ////        output["result"] = "0";
        ////        output["result_text"] = "Data not Found";
        ////        output["data"] = array;
        ////    }

        ////    return output.ToString(Formatting.None);
        ////}
        //public string getSYSCodestructureList()
        //{
        //    JObject output = new JObject();

        //    cls_SYSApilog log = new cls_SYSApilog();
        //    log.apilog_code = "POL001.1";
        //    //log.apilog_by = req.username;
        //    log.apilog_data = "all";

        //    try
        //    {
        //        var authHeader = WebOperationContext.Current.IncomingRequest.Headers["Authorization"];
        //        if (authHeader == null || !objBpcOpr.doVerify(authHeader))
        //        {
        //            output["success"] = false;
        //            output["message"] = BpcOpr.MessageNotAuthen;

        //            log.apilog_status = "500";
        //            log.apilog_message = BpcOpr.MessageNotAuthen;
        //            objBpcOpr.doRecordLog(log);

        //            return output.ToString(Formatting.None);
        //        }

        //        cls_ctMTPolcode controller = new cls_ctMTPolcode();
        //        List<cls_MTPolcode> list = controller.getDataByFillter("", "", "");
        //        JArray array = new JArray();

        //        if (list.Count > 0)
        //        {
        //            int index = 1;

        //            foreach (cls_MTPolcode model in list)
        //            {
        //                JObject json = new JObject();

        //                json.Add("company_code", model.company_code);
        //                json.Add("polcode_id", model.polcode_id);
        //                json.Add("polcode_type", model.polcode_type);
        //                json.Add("modified_by", model.modified_by);
        //                json.Add("modified_date", model.modified_date);
        //                json.Add("flag", model.flag);

        //                json.Add("index", index);
        //                //index++;

        //                array.Add(json);
        //            }

        //            output["success"] = true;
        //            output["message"] = "";
        //            output["data"] = array;

        //            log.apilog_status = "200";
        //            log.apilog_message = "";
        //        }
        //        else
        //        {
        //            output["success"] = false;
        //            output["message"] = "Data not Found";
        //            output["data"] = array;

        //            log.apilog_status = "404";
        //            log.apilog_message = "Data not Found";
        //        }

        //        controller.dispose();
        //    }
        //    catch (Exception ex)
        //    {
        //        output["success"] = false;
        //        output["message"] = "(C)Retrieved data not successfully";

        //        log.apilog_status = "500";
        //        log.apilog_message = ex.ToString();
        //    }
        //    finally
        //    {
        //        objBpcOpr.doRecordLog(log);
        //    }

        //    return output.ToString(Formatting.None);
        //}
        //public string doManageMTPolcode(InputMTPolcode input)
        //{
        //    JObject output = new JObject();

        //    var json_data = new JavaScriptSerializer().Serialize(input);
        //    var tmp = JToken.Parse(json_data);


        //    cls_SYSApilog log = new cls_SYSApilog();
        //    log.apilog_code = "POL001.2";
        //    //log.apilog_by = input.modified_by;
        //    log.apilog_data = tmp.ToString();

        //    try
        //    {
        //        var authHeader = WebOperationContext.Current.IncomingRequest.Headers["Authorization"];
        //        if (authHeader == null || !objBpcOpr.doVerify(authHeader))
        //        {
        //            output["success"] = false;
        //            output["message"] = BpcOpr.MessageNotAuthen;

        //            log.apilog_status = "500";
        //            log.apilog_message = BpcOpr.MessageNotAuthen;
        //            objBpcOpr.doRecordLog(log);

        //            return output.ToString(Formatting.None);
        //        }

        //        cls_ctMTPolcode controller = new cls_ctMTPolcode();
        //        cls_MTPolcode model = new cls_MTPolcode();

        //        model.company_code = input.company_code;
        //        model.polcode_id = input.polcode_id;
        //        model.polcode_type = input.polcode_type;
        //        model.modified_by = input.modified_by;
        //        model.flag = model.flag;     

        //        string strID = controller.insert(model);

        //        if (!strID.Equals(""))
        //        {
        //            output["success"] = true;
        //            output["message"] = "Retrieved data successfully";
        //            output["record_id"] = strID;

        //            log.apilog_status = "200";
        //            log.apilog_message = "";
        //        }
        //        else
        //        {
        //            output["success"] = false;
        //            output["message"] = "Retrieved data not successfully";

        //            log.apilog_status = "500";
        //            log.apilog_message = controller.getMessage();
        //        }

        //        controller.dispose();

        //    }
        //    catch (Exception ex)
        //    {
        //        output["success"] = false;
        //        output["message"] = "(C)Retrieved data not successfully";

        //        log.apilog_status = "500";
        //        log.apilog_message = ex.ToString();
        //    }
        //    finally
        //    {
        //        objBpcOpr.doRecordLog(log);
        //    }

        //    output["data"] = tmp;

        //    return output.ToString(Formatting.None);
        //}
        //public string doDeleteMTPolcode(InputMTPolcode input)
        //{
        //    JObject output = new JObject();

        //    var json_data = new JavaScriptSerializer().Serialize(input);
        //    var tmp = JToken.Parse(json_data);

        //    cls_SYSApilog log = new cls_SYSApilog();
        //    log.apilog_code = "POL001.3";
        //    log.apilog_by = input.modified_by;
        //    log.apilog_data = tmp.ToString();

        //    try
        //    {
        //        var authHeader = WebOperationContext.Current.IncomingRequest.Headers["Authorization"];
        //        if (authHeader == null || !objBpcOpr.doVerify(authHeader))
        //        {
        //            output["success"] = false;
        //            output["message"] = BpcOpr.MessageNotAuthen;
        //            log.apilog_status = "500";
        //            log.apilog_message = BpcOpr.MessageNotAuthen;
        //            objBpcOpr.doRecordLog(log);

        //            return output.ToString(Formatting.None);
        //        }

        //        cls_ctMTPolcode controller = new cls_ctMTPolcode();

        //        if (controller.checkDataOld(input.company_code, input.polcode_type))
        //        {
        //            bool blnResult = controller.delete(input.polcode_id.ToString());

        //            if (blnResult)
        //            {
        //                output["success"] = true;
        //                output["message"] = "Remove data successfully";

        //                log.apilog_status = "200";
        //                log.apilog_message = "";
        //            }
        //            else
        //            {
        //                output["success"] = false;
        //                output["message"] = "Remove data not successfully";

        //                log.apilog_status = "500";
        //                log.apilog_message = controller.getMessage();
        //            }

        //        }
        //        else
        //        {
        //            string message = "Not Found Project code : " + input.polcode_id;
        //            output["success"] = false;
        //            output["message"] = message;

        //            log.apilog_status = "404";
        //            log.apilog_message = message;
        //        }

        //        controller.dispose();
        //    }
        //    catch (Exception ex)
        //    {
        //        output["success"] = false;
        //        output["message"] = "(C)Remove data not successfully";

        //        log.apilog_status = "500";
        //        log.apilog_message = ex.ToString();
        //    }
        //    finally
        //    {
        //        objBpcOpr.doRecordLog(log);
        //    }

        //    output["data"] = tmp;

        //    return output.ToString(Formatting.None);

        //}
        //public string getNewCode(string com, string type, string emptype)
        //{
        //    JObject output = new JObject();

        //    cls_ctMTPolcode objPol = new cls_ctMTPolcode();

        //    List<cls_MTPolcode> listPol = objPol.getDataByFillter(com, "", type);


        //    if (listPol.Count > 0)
        //    {
        //        string strID = "";

        //        cls_MTPolcode polcode = listPol[0];

        //        cls_ctTRPolcode objTRPolcode = new cls_ctTRPolcode();
        //        List<cls_TRPolcode> listTRPolcode = objTRPolcode.getDataByFillter(polcode.polcode_id.ToString());

        //        foreach (cls_TRPolcode model in listTRPolcode)
        //        {

        //            switch (model.codestructure_code)
        //            {

        //                case "1CHA":
        //                    strID += model.polcode_text.Substring(0, model.polcode_lenght);
        //                    break;

        //                case "2COM":
        //                    strID += com.Substring(0, model.polcode_lenght);
        //                    break;

        //                case "3BRA":
        //                    break;

        //                case "4EMT":
        //                    strID += emptype;
        //                    break;

        //                case "5YEA":
        //                    DateTime dateNowY = DateTime.Now;
        //                    string formatY = "";
        //                    for (int i = 0; i < model.polcode_lenght; i++)
        //                    {
        //                        formatY += "y";
        //                    }
        //                    strID += dateNowY.ToString(formatY);
        //                    break;

        //                case "6MON":
        //                    DateTime dateNowM = DateTime.Now;
        //                    string formatM = "";
        //                    for (int i = 0; i < model.polcode_lenght; i++)
        //                    {
        //                        formatM += "M";
        //                    }
        //                    strID += dateNowM.ToString(formatM);
        //                    break;

        //                //case "MAUT":
        //                //    cls_ctMTWorker objWorker = new cls_ctMTWorker();
        //                //    int intRunningID = objWorker.doGetNextRunningID(com, strID);
        //                //    strID += intRunningID.ToString().PadLeft(model.polcode_lenght, '0');
        //                //    break;

        //            }


        //        }




        //        output["result"] = "1";
        //        output["result_text"] = "1";
        //        output["data"] = strID;
        //    }
        //    else
        //    {
        //        output["result"] = "0";
        //        output["result_text"] = "Data not Found";
        //        output["data"] = "";
        //    }

        //    return output.ToString(Formatting.None);
        //}
        //public async Task<string> doUploadMTPolcode(string token, string by, string fileName, Stream stream)
        //{
        //    JObject output = new JObject();

        //    cls_SYSApilog log = new cls_SYSApilog();
        //    log.apilog_code = "POL001.4";
        //    log.apilog_by = by;
        //    log.apilog_data = "Stream";

        //    try
        //    {
        //        if (!objBpcOpr.doVerify(token))
        //        {
        //            output["success"] = false;
        //            output["message"] = BpcOpr.MessageNotAuthen;

        //            log.apilog_status = "500";
        //            log.apilog_message = BpcOpr.MessageNotAuthen;
        //            objBpcOpr.doRecordLog(log);

        //            return output.ToString(Formatting.None);
        //        }


        //        bool upload = await this.doUploadFile(fileName, stream);

        //        if (upload)
        //        {
        //            cls_srvSystemImport srv_import = new cls_srvSystemImport();
        //            string tmp = srv_import.doImportExcel("EMPID", fileName, "TEST");

        //            output["success"] = true;
        //            output["message"] = tmp;

        //            log.apilog_status = "200";
        //            log.apilog_message = "";
        //        }
        //        else
        //        {
        //            output["success"] = false;
        //            output["message"] = "Upload data not successfully";

        //            log.apilog_status = "500";
        //            log.apilog_message = "Upload data not successfully";
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        output["success"] = false;
        //        output["message"] = "(C)Upload data not successfully";

        //        log.apilog_status = "500";
        //        log.apilog_message = ex.ToString();
        //    }
        //    finally
        //    {
        //        objBpcOpr.doRecordLog(log);
        //    }

        //    return output.ToString(Formatting.None);
        //}
        //#endregion


        #region Policy structure code
        public string getSYSCodestructureList()
        {

            JObject output = new JObject();

            cls_ctMTCodestructure objStruc = new cls_ctMTCodestructure();
            List<cls_MTCodestructure> listStruc = objStruc.getData();

            JArray array = new JArray();

            if (listStruc.Count > 0)
            {
                int index = 1;

                foreach (cls_MTCodestructure model in listStruc)
                {
                    JObject json = new JObject();

                    json.Add("codestructure_code", model.codestructure_code);
                    json.Add("codestructure_name_th", model.codestructure_name_th);
                    json.Add("codestructure_name_en", model.codestructure_name_en);

                    json.Add("index", index);

                    index++;

                    array.Add(json);
                }

                output["result"] = "1";
                output["result_text"] = "1";
                output["data"] = array;
            }
            else
            {
                output["result"] = "0";
                output["result_text"] = "Data not Found";
                output["data"] = array;
            }

            return output.ToString(Formatting.None);
        }
        public string getMTPolcode(string com, string type)
        {
            JObject output = new JObject();

            cls_ctMTPolcode objPol = new cls_ctMTPolcode();

            List<cls_MTPolcode> listPol = objPol.getDataByFillter(com, "", type);

            JArray array = new JArray();

            if (listPol.Count > 0)
            {
                int index = 1;

                foreach (cls_MTPolcode model in listPol)
                {
                    JObject json = new JObject();

                    json.Add("company_code", model.company_code);
                    json.Add("polcode_id", model.polcode_id);
                    json.Add("polcode_type", model.polcode_type);
                    json.Add("modified_by", model.modified_by);
                    json.Add("modified_date", model.modified_date);
                    json.Add("flag", model.flag);

                    json.Add("index", index);

                    index++;

                    array.Add(json);
                }

                output["result"] = "1";
                output["result_text"] = "1";
                output["data"] = array;
            }
            else
            {
                output["result"] = "0";
                output["result_text"] = "Data not Found";
                output["data"] = array;
            }

            return output.ToString(Formatting.None);
        }
        public string getTRPolcode(string id)
        {
            JObject output = new JObject();

            cls_ctTRPolcode objTRPolcode = new cls_ctTRPolcode();
            List<cls_TRPolcode> listTRPolcode = objTRPolcode.getDataByFillter(id);

            JArray array = new JArray();

            if (listTRPolcode.Count > 0)
            {
                int index = 1;

                foreach (cls_TRPolcode model in listTRPolcode)
                {
                    JObject json = new JObject();

                    json.Add("polcode_id", model.polcode_id);
                    json.Add("codestructure_code", model.codestructure_code);
                    json.Add("polcode_lenght", model.polcode_lenght);
                    json.Add("polcode_text", model.polcode_text);
                    json.Add("polcode_order", model.polcode_order);

                    json.Add("index", index);

                    index++;

                    array.Add(json);
                }

                output["result"] = "1";
                output["result_text"] = "1";
                output["data"] = array;
            }
            else
            {
                output["result"] = "0";
                output["result_text"] = "Data not Found";
                output["data"] = array;
            }

            return output.ToString(Formatting.None);
        }
        public string doManagePolcode(InputMTPolcode input)
        {
            JObject output = new JObject();

            try
            {
                cls_ctMTPolcode objMTPolcode = new cls_ctMTPolcode();
                cls_MTPolcode model = new cls_MTPolcode();

                model.company_code = input.company_code;
                model.polcode_id = input.polcode_id;
                model.polcode_type = input.polcode_type;
                model.modified_by = input.modified_by;
                model.flag = model.flag;

                string strID = objMTPolcode.insert(model);

                if (!strID.Equals(""))
                {
                    string polcode_data = input.polcode_data;

                    try
                    {
                        JObject jsonObject = new JObject();
                        var jsonArray = JsonConvert.DeserializeObject<List<cls_TRPolcode>>(polcode_data);


                        List<cls_TRPolcode> list_model = new List<cls_TRPolcode>();

                        int intID = Convert.ToInt32(strID);

                        foreach (cls_TRPolcode item in jsonArray)
                        {
                            item.polcode_id = intID;
                            list_model.Add(item);
                        }

                        if (list_model.Count > 0)
                        {
                            cls_ctTRPolcode objTRPolcode = new cls_ctTRPolcode();
                            objTRPolcode.insert(list_model);
                        }

                    }
                    catch (Exception ex)
                    {
                        string str = ex.ToString();
                    }


                    output["result"] = "1";
                    output["result_text"] = "0";
                }
                else
                {
                    output["result"] = "2";
                    output["result_text"] = objMTPolcode.getMessage();
                }

            }
            catch (Exception ex)
            {
                output["result"] = "0";
                output["result_text"] = ex.ToString();

            }

            return output.ToString(Formatting.None);

        }
        public string doDeleteMTPolcode(InputMTPolcode input)
        {
            JObject output = new JObject();

            try
            {
                cls_ctMTPolcode objMTPolcode = new cls_ctMTPolcode();

                bool blnResult = objMTPolcode.delete(input.polcode_id.ToString());

                if (blnResult)
                {
                    output["result"] = "1";
                    output["result_text"] = "0";
                }
                else
                {
                    output["result"] = "2";
                    output["result_text"] = objMTPolcode.getMessage();
                }

            }
            catch (Exception ex)
            {
                output["result"] = "0";
                output["result_text"] = ex.ToString();

            }

            return output.ToString(Formatting.None);

        }

        public string getNewCode(string com, string type, string emptype)
        {
            JObject output = new JObject();

            cls_ctMTPolcode objPol = new cls_ctMTPolcode();

            List<cls_MTPolcode> listPol = objPol.getDataByFillter(com, "", type);


            if (listPol.Count > 0)
            {
                string strID = "";

                cls_MTPolcode polcode = listPol[0];

                cls_ctTRPolcode objTRPolcode = new cls_ctTRPolcode();
                List<cls_TRPolcode> listTRPolcode = objTRPolcode.getDataByFillter(polcode.polcode_id.ToString());

                foreach (cls_TRPolcode model in listTRPolcode)
                {

                    switch (model.codestructure_code)
                    {

                        case "1CHA":
                            strID += model.polcode_text.Substring(0, model.polcode_lenght);
                            break;

                        case "2COM":
                            strID += com.Substring(0, model.polcode_lenght);
                            break;

                        case "3BRA":
                            break;

                        case "4EMT":
                            strID += emptype;
                            break;

                        case "5YEA":
                            DateTime dateNowY = DateTime.Now;
                            string formatY = "";
                            for (int i = 0; i < model.polcode_lenght; i++)
                            {
                                formatY += "y";
                            }
                            strID += dateNowY.ToString(formatY);
                            break;

                        case "6MON":
                            DateTime dateNowM = DateTime.Now;
                            string formatM = "";
                            for (int i = 0; i < model.polcode_lenght; i++)
                            {
                                formatM += "M";
                            }
                            strID += dateNowM.ToString(formatM);
                            break;

                        //case "MAUT":
                        //    cls_ctMTWorker objWorker = new cls_ctMTWorker();
                        //    int intRunningID = objWorker.doGetNextRunningID(com, strID);
                        //    strID += intRunningID.ToString().PadLeft(model.polcode_lenght, '0');
                        //    break;

                    }


                }




                output["result"] = "1";
                output["result_text"] = "1";
                output["data"] = strID;
            }
            else
            {
                output["result"] = "0";
                output["result_text"] = "Data not Found";
                output["data"] = "";
            }

            return output.ToString(Formatting.None);
        }

        #endregion


        #region MTReason
        public string getMTReasonList(InputMTReason input)
        {
            JObject output = new JObject();
            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "ATT001.1";
            log.apilog_by = input.username;
            log.apilog_data = "all";
            try
            {

                var authHeader = WebOperationContext.Current.IncomingRequest.Headers["Authorization"];
                if (authHeader == null || !objBpcOpr.doVerify(authHeader))
                {
                    output["success"] = false;
                    output["message"] = BpcOpr.MessageNotAuthen;

                    log.apilog_status = "500";
                    log.apilog_message = BpcOpr.MessageNotAuthen;
                    objBpcOpr.doRecordLog(log);

                    return output.ToString(Formatting.None);
                }

                cls_ctMTReason objReason = new cls_ctMTReason();
                List<cls_MTReason> listReason = objReason.getDataByFillter(input.reason_group, input.reason_id, input.reason_code, input.company_code);
                JArray array = new JArray();

                if (listReason.Count > 0)
                {

                    int index = 1;

                    foreach (cls_MTReason model in listReason)
                    {
                        JObject json = new JObject();
                        json.Add("company_code", model.company_code);
                        json.Add("reason_id", model.reason_id);
                        json.Add("reason_code", model.reason_code);
                        json.Add("reason_name_th", model.reason_name_th);
                        json.Add("reason_name_en", model.reason_name_en);
                        json.Add("reason_group", model.reason_group);
                        json.Add("modified_by", model.modified_by);
                        json.Add("modified_date", model.modified_date);
                        json.Add("flag", model.flag);

                        json.Add("index", index);

                        index++;

                        array.Add(json);
                    }
                    output["result"] = "1";
                    output["result_text"] = "1";
                    output["data"] = array;
                }
                else
                {
                    output["result"] = "0";
                    output["result_text"] = "Data not Found";
                    output["data"] = array;
                }
            }
            catch (Exception e)
            {
                return e.ToString();
            }

            return output.ToString(Formatting.None);
        }
        public string doManageMTReason(InputMTReason input)
        {
            JObject output = new JObject();
            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "ATT001.1";
            log.apilog_by = input.username;
            log.apilog_data = "all";
            try
            {

                var authHeader = WebOperationContext.Current.IncomingRequest.Headers["Authorization"];
                if (authHeader == null || !objBpcOpr.doVerify(authHeader))
                {
                    output["success"] = false;
                    output["message"] = BpcOpr.MessageNotAuthen;

                    log.apilog_status = "500";
                    log.apilog_message = BpcOpr.MessageNotAuthen;
                    objBpcOpr.doRecordLog(log);

                    return output.ToString(Formatting.None);
                }
                cls_ctMTReason objReason = new cls_ctMTReason();
                cls_MTReason model = new cls_MTReason();

                model.reason_id = input.reason_id.Equals("") ? 0 : Convert.ToInt32(input.reason_id);
                model.company_code = input.company_code;
                model.reason_code = input.reason_code;
                model.reason_name_th = input.reason_name_th;
                model.reason_name_en = input.reason_name_en;
                model.reason_group = input.reason_group;
                model.modified_by = input.modified_by;
                model.flag = input.flag;
                string strID = objReason.insert(model);
                if (!strID.Equals(""))
                {
                    output["success"] = true;
                    output["message"] = "Retrieved data successfully";
                    output["record_id"] = strID;

                    log.apilog_status = "200";
                    log.apilog_message = "";
                }
                else
                {
                    output["success"] = false;
                    output["message"] = "Retrieved data not successfully";

                    log.apilog_status = "500";
                    log.apilog_message = objReason.getMessage();
                }

                objReason.dispose();
            }
            catch (Exception ex)
            {
                output["result"] = "0";
                output["result_text"] = ex.ToString();

            }


            return output.ToString(Formatting.None);

        }
        public string doDeleteMTReason(InputMTReason input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "ATT001.3";
            log.apilog_by = input.username;
            log.apilog_data = tmp.ToString();

            try
            {
                var authHeader = WebOperationContext.Current.IncomingRequest.Headers["Authorization"];
                if (authHeader == null || !objBpcOpr.doVerify(authHeader))
                {
                    output["success"] = false;
                    output["message"] = BpcOpr.MessageNotAuthen;
                    log.apilog_status = "500";
                    log.apilog_message = BpcOpr.MessageNotAuthen;
                    objBpcOpr.doRecordLog(log);

                    return output.ToString(Formatting.None);
                }

                cls_ctMTReason objReason = new cls_ctMTReason();

                bool blnResult = objReason.delete(input.reason_id, input.company_code);

                if (blnResult)
                {
                    output["success"] = true;
                    output["message"] = "Remove data successfully";

                    log.apilog_status = "200";
                    log.apilog_message = "";
                }
                else
                {
                    output["success"] = false;
                    output["message"] = "Remove data not successfully";

                    log.apilog_status = "500";
                    log.apilog_message = objReason.getMessage();
                }
                objReason.dispose();
            }
            catch (Exception ex)
            {
                output["success"] = false;
                output["message"] = "(C)Remove data not successfully";

                log.apilog_status = "500";
                log.apilog_message = ex.ToString();
            }
            finally
            {
                objBpcOpr.doRecordLog(log);
            }

            output["data"] = tmp;

            return output.ToString(Formatting.None);


        }
        public async Task<string> doUploadMTReason(string token, string by, string fileName, Stream stream)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "ATT001.4";
            log.apilog_by = by;
            log.apilog_data = "Stream";

            try
            {
                if (!objBpcOpr.doVerify(token))
                {
                    output["success"] = false;
                    output["message"] = BpcOpr.MessageNotAuthen;

                    log.apilog_status = "500";
                    log.apilog_message = BpcOpr.MessageNotAuthen;
                    objBpcOpr.doRecordLog(log);

                    return output.ToString(Formatting.None);
                }


                bool upload = await this.doUploadFile(fileName, stream);

                if (upload)
                {
                    cls_srvSystemImport srv_import = new cls_srvSystemImport();
                    string tmp = srv_import.doImportExcel("REASON", fileName, by);


                    output["success"] = true;
                    output["message"] = tmp;

                    log.apilog_status = "200";
                    log.apilog_message = "";
                }
                else
                {
                    output["success"] = false;
                    output["message"] = "Upload data not successfully";

                    log.apilog_status = "500";
                    log.apilog_message = "Upload data not successfully";
                }

            }
            catch (Exception ex)
            {
                output["success"] = false;
                output["message"] = "(C)Upload data not successfully";

                log.apilog_status = "500";
                log.apilog_message = ex.ToString();
            }
            finally
            {
                objBpcOpr.doRecordLog(log);
            }

            return output.ToString(Formatting.None);
        }
        #endregion
        

        #region MTCardtype
        public string getCardtypeList(BasicRequest req)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "CRD001.1";
            log.apilog_by = req.username;
            log.apilog_data = "all";

            try
            {
                var authHeader = WebOperationContext.Current.IncomingRequest.Headers["Authorization"];
                if (authHeader == null || !objBpcOpr.doVerify(authHeader))
                {
                    output["success"] = false;
                    output["message"] = BpcOpr.MessageNotAuthen;

                    log.apilog_status = "500";
                    log.apilog_message = BpcOpr.MessageNotAuthen;
                    objBpcOpr.doRecordLog(log);

                    return output.ToString(Formatting.None);
                }

                cls_ctMTCardtype objCardtype = new cls_ctMTCardtype();
                List<cls_MTCardtype> list = objCardtype.getDataByFillter("");
                JArray array = new JArray();

                if (list.Count > 0)
                {
                    int index = 1;

                    foreach (cls_MTCardtype model in list)
                    {
                        JObject json = new JObject();

                        json.Add("cardtype_id", model.cardtype_id);
                        json.Add("cardtype_code", model.cardtype_code);
                        json.Add("cardtype_name_th", model.cardtype_name_th);
                        json.Add("cardtype_name_en", model.cardtype_name_en);
                        json.Add("created_by", model.created_by);
                        json.Add("created_date", model.created_date);
                        json.Add("modified_by", model.modified_by);
                        json.Add("modified_date", model.modified_date);

                        json.Add("index", index++);
                        array.Add(json);
                    }

                    output["success"] = true;
                    output["message"] = "";
                    output["data"] = array;

                    log.apilog_status = "200";
                    log.apilog_message = "";
                }
                else
                {
                    output["success"] = false;
                    output["message"] = "Data not Found";
                    output["data"] = array;

                    log.apilog_status = "404";
                    log.apilog_message = "Data not Found";
                }

                objCardtype.dispose();
            }
            catch (Exception ex)
            {
                output["success"] = false;
                output["message"] = "(C)Retrieved data not successfully";

                log.apilog_status = "500";
                log.apilog_message = ex.ToString();
            }
            finally
            {
                objBpcOpr.doRecordLog(log);
            }

            return output.ToString(Formatting.None);
        }
        public string doManageMTCardtype(InputMTCardtype input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);


            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "CRD001.2";
            log.apilog_by = input.modified_by;
            log.apilog_data = tmp.ToString();

            try
            {
                var authHeader = WebOperationContext.Current.IncomingRequest.Headers["Authorization"];
                if (authHeader == null || !objBpcOpr.doVerify(authHeader))
                {
                    output["success"] = false;
                    output["message"] = BpcOpr.MessageNotAuthen;

                    log.apilog_status = "500";
                    log.apilog_message = BpcOpr.MessageNotAuthen;
                    objBpcOpr.doRecordLog(log);

                    return output.ToString(Formatting.None);
                }

                cls_ctMTCardtype objCardtype = new cls_ctMTCardtype();
                cls_MTCardtype model = new cls_MTCardtype();

                model.cardtype_id = Convert.ToInt32(input.cardtype_id);
                model.cardtype_code = input.cardtype_code;
                model.cardtype_name_th = input.cardtype_name_th;
                model.cardtype_name_en = input.cardtype_name_en;
                model.created_by = input.created_by;
                model.modified_by = input.modified_by;

                string strID = objCardtype.insert(model);

                if (!strID.Equals(""))
                {
                    output["success"] = true;
                    output["message"] = "Retrieved data successfully";
                    output["record_id"] = strID;

                    log.apilog_status = "200";
                    log.apilog_message = "";
                }
                else
                {
                    output["success"] = false;
                    output["message"] = "Retrieved data not successfully";

                    log.apilog_status = "500";
                    log.apilog_message = objCardtype.getMessage();
                }

                objCardtype.dispose();

            }
            catch (Exception ex)
            {
                output["success"] = false;
                output["message"] = "(C)Retrieved data not successfully";

                log.apilog_status = "500";
                log.apilog_message = ex.ToString();
            }
            finally
            {
                objBpcOpr.doRecordLog(log);
            }

            output["data"] = tmp;

            return output.ToString(Formatting.None);
        }
        public string doDeleteMTCardtype(InputMTCardtype input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "CRD001.3";
            log.apilog_by = input.modified_by;
            log.apilog_data = tmp.ToString();

            try
            {
                var authHeader = WebOperationContext.Current.IncomingRequest.Headers["Authorization"];
                if (authHeader == null || !objBpcOpr.doVerify(authHeader))
                {
                    output["success"] = false;
                    output["message"] = BpcOpr.MessageNotAuthen;
                    log.apilog_status = "500";
                    log.apilog_message = BpcOpr.MessageNotAuthen;
                    objBpcOpr.doRecordLog(log);

                    return output.ToString(Formatting.None);
                }

                cls_ctMTCardtype objCardtype = new cls_ctMTCardtype();

                if (objCardtype.checkDataOld(input.cardtype_id))
                {
                    bool blnResult = objCardtype.delete(input.cardtype_code);

                    if (blnResult)
                    {
                        output["success"] = true;
                        output["message"] = "Remove data successfully";

                        log.apilog_status = "200";
                        log.apilog_message = "";
                    }
                    else
                    {
                        output["success"] = false;
                        output["message"] = "Remove data not successfully";

                        log.apilog_status = "500";
                        log.apilog_message = objCardtype.getMessage();
                    }

                }
                else
                {
                    string message = "Not Found Project code : " + input.cardtype_code;
                    output["success"] = false;
                    output["message"] = message;

                    log.apilog_status = "404";
                    log.apilog_message = message;
                }

                objCardtype.dispose();
            }
            catch (Exception ex)
            {
                output["success"] = false;
                output["message"] = "(C)Remove data not successfully";

                log.apilog_status = "500";
                log.apilog_message = ex.ToString();
            }
            finally
            {
                objBpcOpr.doRecordLog(log);
            }

            output["data"] = tmp;

            return output.ToString(Formatting.None);

        }
        public async Task<string> doUploadCardType(string token, string by, string fileName, Stream stream)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "RES001.4";
            log.apilog_by = by;
            log.apilog_data = "Stream";

            try
            {
                if (!objBpcOpr.doVerify(token))
                {
                    output["success"] = false;
                    output["message"] = BpcOpr.MessageNotAuthen;

                    log.apilog_status = "500";
                    log.apilog_message = BpcOpr.MessageNotAuthen;
                    objBpcOpr.doRecordLog(log);

                    return output.ToString(Formatting.None);
                }


                bool upload = await this.doUploadFile(fileName, stream);

                if (upload)
                {
                    cls_srvSystemImport srv_import = new cls_srvSystemImport();
                    string tmp = srv_import.doImportExcel("CARDTYPE", fileName, "TEST");

                    output["success"] = true;
                    output["message"] = tmp;

                    log.apilog_status = "200";
                    log.apilog_message = "";
                }
                else
                {
                    output["success"] = false;
                    output["message"] = "Upload data not successfully";

                    log.apilog_status = "500";
                    log.apilog_message = "Upload data not successfully";
                }

            }
            catch (Exception ex)
            {
                output["success"] = false;
                output["message"] = "(C)Upload data not successfully";

                log.apilog_status = "500";
                log.apilog_message = ex.ToString();
            }
            finally
            {
                objBpcOpr.doRecordLog(log);
            }

            return output.ToString(Formatting.None);
        }
        #endregion

        #region MTFAMILY
        public string getFamilyList(BasicRequest req)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "FML001.1";
            log.apilog_by = req.username;
            log.apilog_data = "all";

            try
            {
                var authHeader = WebOperationContext.Current.IncomingRequest.Headers["Authorization"];
                if (authHeader == null || !objBpcOpr.doVerify(authHeader))
                {
                    output["success"] = false;
                    output["message"] = BpcOpr.MessageNotAuthen;

                    log.apilog_status = "500";
                    log.apilog_message = BpcOpr.MessageNotAuthen;
                    objBpcOpr.doRecordLog(log);

                    return output.ToString(Formatting.None);
                }

                cls_ctMTFamily objFamily = new cls_ctMTFamily();
                List<cls_MTFamily> list = objFamily.getDataByFillter("");
                JArray array = new JArray();

                if (list.Count > 0)
                {
                    int index = 1;

                    foreach (cls_MTFamily model in list)
                    {
                        JObject json = new JObject();

                        json.Add("family_id", model.family_id);
                        json.Add("family_code", model.family_code);
                        json.Add("family_name_th", model.family_name_th);
                        json.Add("family_name_en", model.family_name_en);
                        json.Add("modified_by", model.modified_by);
                        json.Add("modified_date", model.modified_date);
                        json.Add("flag", model.flag);
                        json.Add("index", index++);
                        array.Add(json);
                    }

                    output["success"] = true;
                    output["message"] = "";
                    output["data"] = array;

                    log.apilog_status = "200";
                    log.apilog_message = "";
                }
                else
                {
                    output["success"] = false;
                    output["message"] = "Data not Found";
                    output["data"] = array;

                    log.apilog_status = "404";
                    log.apilog_message = "Data not Found";
                }

                objFamily.dispose();
            }
            catch (Exception ex)
            {
                output["success"] = false;
                output["message"] = "(C)Retrieved data not successfully";

                log.apilog_status = "500";
                log.apilog_message = ex.ToString();
            }
            finally
            {
                objBpcOpr.doRecordLog(log);
            }

            return output.ToString(Formatting.None);
        }
        public string doManageMTFamily(InputMTFamily input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);


            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "FML001.2";
            log.apilog_by = input.modified_by;
            log.apilog_data = tmp.ToString();

            try
            {
                var authHeader = WebOperationContext.Current.IncomingRequest.Headers["Authorization"];
                if (authHeader == null || !objBpcOpr.doVerify(authHeader))
                {
                    output["success"] = false;
                    output["message"] = BpcOpr.MessageNotAuthen;

                    log.apilog_status = "500";
                    log.apilog_message = BpcOpr.MessageNotAuthen;
                    objBpcOpr.doRecordLog(log);

                    return output.ToString(Formatting.None);
                }

                cls_ctMTFamily objFamily = new cls_ctMTFamily();
                cls_MTFamily model = new cls_MTFamily();

                model.family_id = Convert.ToInt32(input.family_id);
                model.family_code = input.family_code;
                model.family_name_th = input.family_name_th;
                model.family_name_en = input.family_name_en;
                model.created_by = input.created_by;
                model.modified_by = input.modified_by;

                string strID = objFamily.insert(model);

                if (!strID.Equals(""))
                {
                    output["success"] = true;
                    output["message"] = "Retrieved data successfully";
                    output["record_id"] = strID;

                    log.apilog_status = "200";
                    log.apilog_message = "";
                }
                else
                {
                    output["success"] = false;
                    output["message"] = "Retrieved data not successfully";

                    log.apilog_status = "500";
                    log.apilog_message = objFamily.getMessage();
                }

                objFamily.dispose();

            }
            catch (Exception ex)
            {
                output["success"] = false;
                output["message"] = "(C)Retrieved data not successfully";

                log.apilog_status = "500";
                log.apilog_message = ex.ToString();
            }
            finally
            {
                objBpcOpr.doRecordLog(log);
            }

            output["data"] = tmp;

            return output.ToString(Formatting.None);
        }
        public string doDeleteMTFamily(InputMTFamily input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "FML001.3";
            log.apilog_by = input.modified_by;
            log.apilog_data = tmp.ToString();

            try
            {
                var authHeader = WebOperationContext.Current.IncomingRequest.Headers["Authorization"];
                if (authHeader == null || !objBpcOpr.doVerify(authHeader))
                {
                    output["success"] = false;
                    output["message"] = BpcOpr.MessageNotAuthen;
                    log.apilog_status = "500";
                    log.apilog_message = BpcOpr.MessageNotAuthen;
                    objBpcOpr.doRecordLog(log);

                    return output.ToString(Formatting.None);
                }

                cls_ctMTFamily objFamily = new cls_ctMTFamily();

                if (objFamily.checkDataOld(input.family_id))
                {
                    bool blnResult = objFamily.delete(input.family_code);

                    if (blnResult)
                    {
                        output["success"] = true;
                        output["message"] = "Remove data successfully";

                        log.apilog_status = "200";
                        log.apilog_message = "";
                    }
                    else
                    {
                        output["success"] = false;
                        output["message"] = "Remove data not successfully";

                        log.apilog_status = "500";
                        log.apilog_message = objFamily.getMessage();
                    }

                }
                else
                {
                    string message = "Not Found Project code : " + input.family_code;
                    output["success"] = false;
                    output["message"] = message;

                    log.apilog_status = "404";
                    log.apilog_message = message;
                }

                objFamily.dispose();
            }
            catch (Exception ex)
            {
                output["success"] = false;
                output["message"] = "(C)Remove data not successfully";

                log.apilog_status = "500";
                log.apilog_message = ex.ToString();
            }
            finally
            {
                objBpcOpr.doRecordLog(log);
            }

            output["data"] = tmp;

            return output.ToString(Formatting.None);

        }
        public async Task<string> doUploadMTFamily(string token, string by, string fileName, Stream stream)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "FML001.4";
            log.apilog_by = by;
            log.apilog_data = "Stream";

            try
            {
                if (!objBpcOpr.doVerify(token))
                {
                    output["success"] = false;
                    output["message"] = BpcOpr.MessageNotAuthen;

                    log.apilog_status = "500";
                    log.apilog_message = BpcOpr.MessageNotAuthen;
                    objBpcOpr.doRecordLog(log);

                    return output.ToString(Formatting.None);
                }


                bool upload = await this.doUploadFile(fileName, stream);

                if (upload)
                {
                    cls_srvSystemImport srv_import = new cls_srvSystemImport();
                    string tmp = srv_import.doImportExcel("FAMILY", fileName, "TEST");

                    output["success"] = true;
                    output["message"] = tmp;

                    log.apilog_status = "200";
                    log.apilog_message = "";
                }
                else
                {
                    output["success"] = false;
                    output["message"] = "Upload data not successfully";

                    log.apilog_status = "500";
                    log.apilog_message = "Upload data not successfully";
                }

            }
            catch (Exception ex)
            {
                output["success"] = false;
                output["message"] = "(C)Upload data not successfully";

                log.apilog_status = "500";
                log.apilog_message = ex.ToString();
            }
            finally
            {
                objBpcOpr.doRecordLog(log);
            }

            return output.ToString(Formatting.None);
        }
        #endregion

        #region MTLEVEL
        public string getLevelList(BasicRequest req)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "LVL001.1";
            log.apilog_by = req.username;
            log.apilog_data = "all";

            try
            {
                var authHeader = WebOperationContext.Current.IncomingRequest.Headers["Authorization"];
                if (authHeader == null || !objBpcOpr.doVerify(authHeader))
                {
                    output["success"] = false;
                    output["message"] = BpcOpr.MessageNotAuthen;

                    log.apilog_status = "500";
                    log.apilog_message = BpcOpr.MessageNotAuthen;
                    objBpcOpr.doRecordLog(log);

                    return output.ToString(Formatting.None);
                }

                cls_ctMTLevel objLevel = new cls_ctMTLevel();
                List<cls_MTLevel> list = objLevel.getDataByFillter("");
                JArray array = new JArray();

                if (list.Count > 0)
                {
                    int index = 1;

                    foreach (cls_MTLevel model in list)
                    {
                        JObject json = new JObject();

                        json.Add("level_id", model.level_id);
                        json.Add("level_code", model.level_code);
                        json.Add("level_name_th", model.level_name_th);
                        json.Add("level_name_en", model.level_name_en);
                        json.Add("company_code", model.company_code);

                        json.Add("modified_by", model.modified_by);
                        json.Add("modified_date", model.modified_date);
                        json.Add("flag", model.flag);
                        json.Add("index", index++);
                        array.Add(json);
                    }

                    output["success"] = true;
                    output["message"] = "";
                    output["data"] = array;

                    log.apilog_status = "200";
                    log.apilog_message = "";
                }
                else
                {
                    output["success"] = false;
                    output["message"] = "Data not Found";
                    output["data"] = array;

                    log.apilog_status = "404";
                    log.apilog_message = "Data not Found";
                }

                objLevel.dispose();
            }
            catch (Exception ex)
            {
                output["success"] = false;
                output["message"] = "(C)Retrieved data not successfully";

                log.apilog_status = "500";
                log.apilog_message = ex.ToString();
            }
            finally
            {
                objBpcOpr.doRecordLog(log);
            }

            return output.ToString(Formatting.None);
        }
        public string doManageMTLevel(InputMTLevel input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);


            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "LVL001.2";
            log.apilog_by = input.modified_by;
            log.apilog_data = tmp.ToString();

            try
            {
                var authHeader = WebOperationContext.Current.IncomingRequest.Headers["Authorization"];
                if (authHeader == null || !objBpcOpr.doVerify(authHeader))
                {
                    output["success"] = false;
                    output["message"] = BpcOpr.MessageNotAuthen;

                    log.apilog_status = "500";
                    log.apilog_message = BpcOpr.MessageNotAuthen;
                    objBpcOpr.doRecordLog(log);

                    return output.ToString(Formatting.None);
                }

                cls_ctMTLevel objMTLevel = new cls_ctMTLevel();
                cls_MTLevel model = new cls_MTLevel();

                model.level_id = Convert.ToInt32(input.level_id);
                model.level_code = input.level_code;
                model.level_name_th = input.level_name_th;
                model.level_name_en = input.level_name_en;
                model.company_code = input.company_code;

                model.created_by = input.created_by;
                model.modified_by = input.modified_by;

                string strID = objMTLevel.insert(model);

                if (!strID.Equals(""))
                {
                    output["success"] = true;
                    output["message"] = "Retrieved data successfully";
                    output["record_id"] = strID;

                    log.apilog_status = "200";
                    log.apilog_message = "";
                }
                else
                {
                    output["success"] = false;
                    output["message"] = "Retrieved data not successfully";

                    log.apilog_status = "500";
                    log.apilog_message = objMTLevel.getMessage();
                }

                objMTLevel.dispose();

            }
            catch (Exception ex)
            {
                output["success"] = false;
                output["message"] = "(C)Retrieved data not successfully";

                log.apilog_status = "500";
                log.apilog_message = ex.ToString();
            }
            finally
            {
                objBpcOpr.doRecordLog(log);
            }

            output["data"] = tmp;

            return output.ToString(Formatting.None);
        }
        public string doDeleteMTLevel(InputMTLevel input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "LVL001.3";
            log.apilog_by = input.modified_by;
            log.apilog_data = tmp.ToString();

            try
            {
                var authHeader = WebOperationContext.Current.IncomingRequest.Headers["Authorization"];
                if (authHeader == null || !objBpcOpr.doVerify(authHeader))
                {
                    output["success"] = false;
                    output["message"] = BpcOpr.MessageNotAuthen;
                    log.apilog_status = "500";
                    log.apilog_message = BpcOpr.MessageNotAuthen;
                    objBpcOpr.doRecordLog(log);

                    return output.ToString(Formatting.None);
                }

                cls_ctMTLevel objLevel = new cls_ctMTLevel();

                if (objLevel.checkDataOld(input.level_code))
                {
                    bool blnResult = objLevel.delete(input.level_code);

                    if (blnResult)
                    {
                        output["success"] = true;
                        output["message"] = "Remove data successfully";

                        log.apilog_status = "200";
                        log.apilog_message = "";
                    }
                    else
                    {
                        output["success"] = false;
                        output["message"] = "Remove data not successfully";

                        log.apilog_status = "500";
                        log.apilog_message = objLevel.getMessage();
                    }

                }
                else
                {
                    string message = "Not Found Project code : " + input.level_code;
                    output["success"] = false;
                    output["message"] = message;

                    log.apilog_status = "404";
                    log.apilog_message = message;
                }

                objLevel.dispose();
            }
            catch (Exception ex)
            {
                output["success"] = false;
                output["message"] = "(C)Remove data not successfully";

                log.apilog_status = "500";
                log.apilog_message = ex.ToString();
            }
            finally
            {
                objBpcOpr.doRecordLog(log);
            }

            output["data"] = tmp;

            return output.ToString(Formatting.None);

        }
        public async Task<string> doUploadMTLevel(string token, string by, string fileName, Stream stream)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "LVL001.4";
            log.apilog_by = by;
            log.apilog_data = "Stream";

            try
            {
                if (!objBpcOpr.doVerify(token))
                {
                    output["success"] = false;
                    output["message"] = BpcOpr.MessageNotAuthen;

                    log.apilog_status = "500";
                    log.apilog_message = BpcOpr.MessageNotAuthen;
                    objBpcOpr.doRecordLog(log);

                    return output.ToString(Formatting.None);
                }


                bool upload = await this.doUploadFile(fileName, stream);

                if (upload)
                {
                    cls_srvSystemImport srv_import = new cls_srvSystemImport();
                    string tmp = srv_import.doImportExcel("LEVEL", fileName, "TEST");

                    output["success"] = true;
                    output["message"] = tmp;

                    log.apilog_status = "200";
                    log.apilog_message = "";
                }
                else
                {
                    output["success"] = false;
                    output["message"] = "Upload data not successfully";

                    log.apilog_status = "500";
                    log.apilog_message = "Upload data not successfully";
                }

            }
            catch (Exception ex)
            {
                output["success"] = false;
                output["message"] = "(C)Upload data not successfully";

                log.apilog_status = "500";
                log.apilog_message = ex.ToString();
            }
            finally
            {
                objBpcOpr.doRecordLog(log);
            }

            return output.ToString(Formatting.None);
        }


        #endregion

        #region MTLocation
        public string getMTLocationList(InputMTLocation input)
        {
            JObject output = new JObject();
            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "ATT001.1";
            log.apilog_by = input.username;
            log.apilog_data = "all";
            try
            {

                var authHeader = WebOperationContext.Current.IncomingRequest.Headers["Authorization"];
                if (authHeader == null || !objBpcOpr.doVerify(authHeader))
                {
                    output["success"] = false;
                    output["message"] = BpcOpr.MessageNotAuthen;

                    log.apilog_status = "500";
                    log.apilog_message = BpcOpr.MessageNotAuthen;
                    objBpcOpr.doRecordLog(log);

                    return output.ToString(Formatting.None);
                }
                cls_ctMTLocation objLocation = new cls_ctMTLocation();
                List<cls_MTLocation> listLocation = objLocation.getDataByFillter(input.location_id, input.location_code, input.company_code);

                JArray array = new JArray();

                if (listLocation.Count > 0)
                {

                    int index = 1;

                    foreach (cls_MTLocation model in listLocation)
                    {
                        JObject json = new JObject();
                        json.Add("company_code", model.company_code);
                        json.Add("location_id", model.location_id);
                        json.Add("location_code", model.location_code);
                        json.Add("location_name_th", model.location_name_th);
                        json.Add("location_name_en", model.location_name_en);
                        json.Add("location_detail", model.location_detail);
                        json.Add("location_lat", model.location_lat);
                        json.Add("location_long", model.location_long);
                        json.Add("modified_by", model.modified_by);
                        json.Add("modified_date", model.modified_date);
                        json.Add("flag", model.flag);

                        json.Add("index", index);

                        index++;

                        array.Add(json);
                    }

                    output["result"] = "1";
                    output["result_text"] = "1";
                    output["data"] = array;
                }
                else
                {
                    output["result"] = "0";
                    output["result_text"] = "Data not Found";
                    output["data"] = array;
                }
            }
            catch (Exception e)
            {
                return e.ToString();
            }
            return output.ToString(Formatting.None);
        }
        public string doManageMTLocation(InputMTLocation input)
        {
            JObject output = new JObject();
            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "ATT001.1";
            log.apilog_by = input.username;
            log.apilog_data = "all";
            try
            {

                var authHeader = WebOperationContext.Current.IncomingRequest.Headers["Authorization"];
                if (authHeader == null || !objBpcOpr.doVerify(authHeader))
                {
                    output["success"] = false;
                    output["message"] = BpcOpr.MessageNotAuthen;

                    log.apilog_status = "500";
                    log.apilog_message = BpcOpr.MessageNotAuthen;
                    objBpcOpr.doRecordLog(log);

                    return output.ToString(Formatting.None);
                }
                cls_ctMTLocation objLocation = new cls_ctMTLocation();
                cls_MTLocation model = new cls_MTLocation();
                model.company_code = input.company_code;
                model.location_id = input.location_id.Equals("") ? 0 : Convert.ToInt32(input.location_id);
                model.location_code = input.location_code;
                model.location_name_th = input.location_name_th;
                model.location_name_en = input.location_name_en;
                model.location_detail = input.location_detail;
                model.location_lat = input.location_lat;
                model.location_long = input.location_long;
                model.modified_by = input.modified_by;
                model.flag = input.flag;
                string strID = objLocation.insert(model);
                if (!strID.Equals(""))
                {
                    output["success"] = true;
                    output["message"] = "Retrieved data successfully";
                    output["record_id"] = strID;

                    log.apilog_status = "200";
                    log.apilog_message = "";
                }
                else
                {
                    output["success"] = false;
                    output["message"] = "Retrieved data not successfully";

                    log.apilog_status = "500";
                    log.apilog_message = objLocation.getMessage();
                }

                objLocation.dispose();
            }
            catch (Exception ex)
            {
                output["result"] = "0";
                output["result_text"] = ex.ToString();

            }

            return output.ToString(Formatting.None);

        }
        public string doDeleteMTLocation(InputMTLocation input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "ATT001.3";
            log.apilog_by = input.username;
            log.apilog_data = tmp.ToString();

            try
            {
                var authHeader = WebOperationContext.Current.IncomingRequest.Headers["Authorization"];
                if (authHeader == null || !objBpcOpr.doVerify(authHeader))
                {
                    output["success"] = false;
                    output["message"] = BpcOpr.MessageNotAuthen;
                    log.apilog_status = "500";
                    log.apilog_message = BpcOpr.MessageNotAuthen;
                    objBpcOpr.doRecordLog(log);

                    return output.ToString(Formatting.None);
                }

                cls_ctMTLocation controller = new cls_ctMTLocation();

                bool blnResult = controller.delete(input.location_id, input.company_code);

                if (blnResult)
                {
                    output["success"] = true;
                    output["message"] = "Remove data successfully";

                    log.apilog_status = "200";
                    log.apilog_message = "";
                }
                else
                {
                    output["success"] = false;
                    output["message"] = "Remove data not successfully";

                    log.apilog_status = "500";
                    log.apilog_message = controller.getMessage();
                }
                controller.dispose();
            }
            catch (Exception ex)
            {
                output["success"] = false;
                output["message"] = "(C)Remove data not successfully";

                log.apilog_status = "500";
                log.apilog_message = ex.ToString();
            }
            finally
            {
                objBpcOpr.doRecordLog(log);
            }

            output["data"] = tmp;

            return output.ToString(Formatting.None);

        }
        public async Task<string> doUploadMTLocation(string token, string by, string fileName, Stream stream)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "ATT001.4";
            log.apilog_by = by;
            log.apilog_data = "Stream";

            try
            {
                if (!objBpcOpr.doVerify(token))
                {
                    output["success"] = false;
                    output["message"] = BpcOpr.MessageNotAuthen;

                    log.apilog_status = "500";
                    log.apilog_message = BpcOpr.MessageNotAuthen;
                    objBpcOpr.doRecordLog(log);

                    return output.ToString(Formatting.None);
                }


                bool upload = await this.doUploadFile(fileName, stream);

                if (upload)
                {
                    cls_srvSystemImport srv_import = new cls_srvSystemImport();
                    string tmp = srv_import.doImportExcel("LOCATION", fileName, by);


                    output["success"] = true;
                    output["message"] = tmp;

                    log.apilog_status = "200";
                    log.apilog_message = "";
                }
                else
                {
                    output["success"] = false;
                    output["message"] = "Upload data not successfully";

                    log.apilog_status = "500";
                    log.apilog_message = "Upload data not successfully";
                }

            }
            catch (Exception ex)
            {
                output["success"] = false;
                output["message"] = "(C)Upload data not successfully";

                log.apilog_status = "500";
                log.apilog_message = ex.ToString();
            }
            finally
            {
                objBpcOpr.doRecordLog(log);
            }

            return output.ToString(Formatting.None);
        }
        #endregion

        #region MTYear
        public string getMTYearList(InputMTYear input)
        {
            JObject output = new JObject();
            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "ATT001.1";
            log.apilog_by = input.username;
            log.apilog_data = "all";
            try
            {

                var authHeader = WebOperationContext.Current.IncomingRequest.Headers["Authorization"];
                if (authHeader == null || !objBpcOpr.doVerify(authHeader))
                {
                    output["success"] = false;
                    output["message"] = BpcOpr.MessageNotAuthen;

                    log.apilog_status = "500";
                    log.apilog_message = BpcOpr.MessageNotAuthen;
                    objBpcOpr.doRecordLog(log);

                    return output.ToString(Formatting.None);
                }
                cls_ctMTYear objYear = new cls_ctMTYear();
                List<cls_MTYear> listYear = objYear.getDataByFillter(input.company_code, input.year_group, input.year_id, input.year_code);

                JArray array = new JArray();

                if (listYear.Count > 0)
                {
                    int index = 1;

                    foreach (cls_MTYear model in listYear)
                    {
                        JObject json = new JObject();

                        json.Add("year_id", model.year_id);
                        json.Add("year_code", model.year_code);
                        json.Add("year_name_th", model.year_name_th);
                        json.Add("year_name_en", model.year_name_en);

                        json.Add("year_fromdate", model.year_fromdate);
                        json.Add("year_todate", model.year_todate);
                        json.Add("year_group", model.year_group);

                        json.Add("company_code", model.company_code);

                        json.Add("modified_by", model.modified_by);
                        json.Add("modified_date", model.modified_date);
                        json.Add("flag", model.flag);

                        json.Add("index", index);

                        index++;

                        array.Add(json);
                    }

                    output["result"] = "1";
                    output["result_text"] = "1";
                    output["data"] = array;
                }
                else
                {
                    output["result"] = "0";
                    output["result_text"] = "Data not Found";
                    output["data"] = array;
                }
            }
            catch (Exception e)
            {
                return e.ToString();
            }
            return output.ToString(Formatting.None);
        }
        public string doManageMTYear(InputMTYear input)
        {
            JObject output = new JObject();
            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "ATT001.1";
            log.apilog_by = input.username;
            log.apilog_data = "all";
            try
            {

                var authHeader = WebOperationContext.Current.IncomingRequest.Headers["Authorization"];
                if (authHeader == null || !objBpcOpr.doVerify(authHeader))
                {
                    output["success"] = false;
                    output["message"] = BpcOpr.MessageNotAuthen;

                    log.apilog_status = "500";
                    log.apilog_message = BpcOpr.MessageNotAuthen;
                    objBpcOpr.doRecordLog(log);

                    return output.ToString(Formatting.None);
                }
                cls_ctMTYear objYear = new cls_ctMTYear();
                cls_MTYear model = new cls_MTYear();

                model.company_code = input.company_code;

                model.year_id = input.year_id;
                model.year_code = input.year_code;
                model.year_name_th = input.year_name_th;
                model.year_name_en = input.year_name_en;
                model.year_fromdate = Convert.ToDateTime(input.year_fromdate);
                model.year_todate = Convert.ToDateTime(input.year_todate);
                model.year_group = input.year_group;
                model.company_code = input.company_code;

                model.modified_by = input.modified_by;
                model.flag = input.flag;
                string strID = objYear.insert(model);
                if (!strID.Equals(""))
                {
                    output["success"] = true;
                    output["message"] = "Retrieved data successfully";
                    output["record_id"] = strID;

                    log.apilog_status = "200";  
                    log.apilog_message = "";
                }
                else
                {
                    output["success"] = false;
                    output["message"] = "Retrieved data not successfully";

                    log.apilog_status = "500";
                    log.apilog_message = objYear.getMessage();
                }

                objYear.dispose();
            }
            catch (Exception ex)
            {
                output["result"] = "0";
                output["result_text"] = ex.ToString();

            }

            return output.ToString(Formatting.None);

        }
        public string doDeleteMTYear(InputMTYear input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "ATT001.3";
            log.apilog_by = input.username;
            log.apilog_data = tmp.ToString();

            try
            {
                var authHeader = WebOperationContext.Current.IncomingRequest.Headers["Authorization"];
                if (authHeader == null || !objBpcOpr.doVerify(authHeader))
                {
                    output["success"] = false;
                    output["message"] = BpcOpr.MessageNotAuthen;
                    log.apilog_status = "500";
                    log.apilog_message = BpcOpr.MessageNotAuthen;
                    objBpcOpr.doRecordLog(log);

                    return output.ToString(Formatting.None);
                }

                cls_ctMTYear controller = new cls_ctMTYear();

                bool blnResult = controller.delete(input.year_id);

                if (blnResult)
                {
                    output["success"] = true;
                    output["message"] = "Remove data successfully";

                    log.apilog_status = "200";
                    log.apilog_message = "";
                }
                else
                {
                    output["success"] = false;
                    output["message"] = "Remove data not successfully";

                    log.apilog_status = "500";
                    log.apilog_message = controller.getMessage();
                }
                controller.dispose();
            }
            catch (Exception ex)
            {
                output["success"] = false;
                output["message"] = "(C)Remove data not successfully";

                log.apilog_status = "500";
                log.apilog_message = ex.ToString();
            }
            finally
            {
                objBpcOpr.doRecordLog(log);
            }

            output["data"] = tmp;

            return output.ToString(Formatting.None);

        }
        public async Task<string> doUploadYear(string token, string by, string fileName, Stream stream)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "ATT001.4";
            log.apilog_by = by;
            log.apilog_data = "Stream";

            try
            {
                if (!objBpcOpr.doVerify(token))
                {
                    output["success"] = false;
                    output["message"] = BpcOpr.MessageNotAuthen;

                    log.apilog_status = "500";
                    log.apilog_message = BpcOpr.MessageNotAuthen;
                    objBpcOpr.doRecordLog(log);

                    return output.ToString(Formatting.None);
                }


                bool upload = await this.doUploadFile(fileName, stream);

                if (upload)
                {
                    cls_srvSystemImport srv_import = new cls_srvSystemImport();
                    string tmp = srv_import.doImportExcel("YEAR", fileName, by);


                    output["success"] = true;
                    output["message"] = tmp;

                    log.apilog_status = "200";
                    log.apilog_message = "";
                }
                else
                {
                    output["success"] = false;
                    output["message"] = "Upload data not successfully";

                    log.apilog_status = "500";
                    log.apilog_message = "Upload data not successfully";
                }

            }
            catch (Exception ex)
            {
                output["success"] = false;
                output["message"] = "(C)Upload data not successfully";

                log.apilog_status = "500";
                log.apilog_message = ex.ToString();
            }
            finally
            {
                objBpcOpr.doRecordLog(log);
            }

            return output.ToString(Formatting.None);
        }
        #endregion

        #region MTREDUCE
        public string getReduceList(BasicRequest req)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "RED001.1";
            log.apilog_by = req.username;
            log.apilog_data = "all";

            try
            {
                var authHeader = WebOperationContext.Current.IncomingRequest.Headers["Authorization"];
                if (authHeader == null || !objBpcOpr.doVerify(authHeader))
                {
                    output["success"] = false;
                    output["message"] = BpcOpr.MessageNotAuthen;

                    log.apilog_status = "500";
                    log.apilog_message = BpcOpr.MessageNotAuthen;
                    objBpcOpr.doRecordLog(log);

                    return output.ToString(Formatting.None);
                }

                cls_ctMTReduce objReduce = new cls_ctMTReduce();
                List<cls_MTReduce> list = objReduce.getDataByFillter("","");
                JArray array = new JArray();

                if (list.Count > 0)
                {
                    int index = 1;

                    foreach (cls_MTReduce model in list)
                    {
                        JObject json = new JObject();

                        json.Add("reduce_id", model.reduce_id);
                        json.Add("reduce_code", model.reduce_code);
                        json.Add("reduce_name_th", model.reduce_name_th);
                        json.Add("reduce_name_en", model.reduce_name_en);

                        json.Add("reduce_amount", model.reduce_amount);
                        json.Add("reduce_percent", model.reduce_percent);
                        json.Add("reduce_percent_max", model.reduce_percent_max);

                        json.Add("modified_by", model.modified_by);
                        json.Add("modified_date", model.modified_date);
                        json.Add("flag", model.flag);

                        json.Add("index", index++);
                        array.Add(json);
                    }

                    output["success"] = true;
                    output["message"] = "";
                    output["data"] = array;

                    log.apilog_status = "200";
                    log.apilog_message = "";
                }
                else
                {
                    output["success"] = false;
                    output["message"] = "Data not Found";
                    output["data"] = array;

                    log.apilog_status = "404";
                    log.apilog_message = "Data not Found";
                }

                objReduce.dispose();
            }
            catch (Exception ex)
            {
                output["success"] = false;
                output["message"] = "(C)Retrieved data not successfully";

                log.apilog_status = "500";
                log.apilog_message = ex.ToString();
            }
            finally
            {
                objBpcOpr.doRecordLog(log);
            }

            return output.ToString(Formatting.None);
        }
        public string doManageMTReduce(InputMTReduce input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);


            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "RED001.2";
            log.apilog_by = input.modified_by;
            log.apilog_data = tmp.ToString();

            try
            {
                var authHeader = WebOperationContext.Current.IncomingRequest.Headers["Authorization"];
                if (authHeader == null || !objBpcOpr.doVerify(authHeader))
                {
                    output["success"] = false;
                    output["message"] = BpcOpr.MessageNotAuthen;

                    log.apilog_status = "500";
                    log.apilog_message = BpcOpr.MessageNotAuthen;
                    objBpcOpr.doRecordLog(log);

                    return output.ToString(Formatting.None);
                }

                cls_ctMTReduce objMTReduce = new cls_ctMTReduce();
                cls_MTReduce model = new cls_MTReduce();

                model.reduce_id = input.reduce_id;
                model.reduce_code = input.reduce_code;

                model.reduce_name_th = input.reduce_name_th;
                model.reduce_name_en = input.reduce_name_en;

                model.reduce_amount = input.reduce_amount;
                model.reduce_percent = input.reduce_percent;
                model.reduce_percent_max = input.reduce_percent_max;

                model.modified_by = input.modified_by;
                model.flag = model.flag;

                string strID = objMTReduce.insert(model);

                if (!strID.Equals(""))
                {
                    output["success"] = true;
                    output["message"] = "Retrieved data successfully";
                    output["record_id"] = strID;

                    log.apilog_status = "200";
                    log.apilog_message = "";
                }
                else
                {
                    output["success"] = false;
                    output["message"] = "Retrieved data not successfully";

                    log.apilog_status = "500";
                    log.apilog_message = objMTReduce.getMessage();
                }

                objMTReduce.dispose();

            }
            catch (Exception ex)
            {
                output["success"] = false;
                output["message"] = "(C)Retrieved data not successfully";

                log.apilog_status = "500";
                log.apilog_message = ex.ToString();
            }
            finally
            {
                objBpcOpr.doRecordLog(log);
            }

            output["data"] = tmp;

            return output.ToString(Formatting.None);
        }
        public string doDeleteMTReduce(InputMTReduce input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "RED001.3";
            log.apilog_by = input.modified_by;
            log.apilog_data = tmp.ToString();

            try
            {
                var authHeader = WebOperationContext.Current.IncomingRequest.Headers["Authorization"];
                if (authHeader == null || !objBpcOpr.doVerify(authHeader))
                {
                    output["success"] = false;
                    output["message"] = BpcOpr.MessageNotAuthen;
                    log.apilog_status = "500";
                    log.apilog_message = BpcOpr.MessageNotAuthen;
                    objBpcOpr.doRecordLog(log);

                    return output.ToString(Formatting.None);
                }

                cls_ctMTReduce objReduce = new cls_ctMTReduce();

                if (objReduce.checkDataOld(input.reduce_code))
                {
                    bool blnResult = objReduce.delete(input.reduce_code);

                    if (blnResult)
                    {
                        output["success"] = true;
                        output["message"] = "Remove data successfully";

                        log.apilog_status = "200";
                        log.apilog_message = "";
                    }
                    else
                    {
                        output["success"] = false;
                        output["message"] = "Remove data not successfully";

                        log.apilog_status = "500";
                        log.apilog_message = objReduce.getMessage();
                    }

                }
                else
                {
                    string message = "Not Found Project code : " + input.reduce_code;
                    output["success"] = false;
                    output["message"] = message;

                    log.apilog_status = "404";
                    log.apilog_message = message;
                }

                objReduce.dispose();
            }
            catch (Exception ex)
            {
                output["success"] = false;
                output["message"] = "(C)Remove data not successfully";

                log.apilog_status = "500";
                log.apilog_message = ex.ToString();
            }
            finally
            {
                objBpcOpr.doRecordLog(log);
            }

            output["data"] = tmp;

            return output.ToString(Formatting.None);

        }
        public async Task<string> doUploadMTReduce(string token, string by, string fileName, Stream stream)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "RED001.4";
            log.apilog_by = by;
            log.apilog_data = "Stream";

            try
            {
                if (!objBpcOpr.doVerify(token))
                {
                    output["success"] = false;
                    output["message"] = BpcOpr.MessageNotAuthen;

                    log.apilog_status = "500";
                    log.apilog_message = BpcOpr.MessageNotAuthen;
                    objBpcOpr.doRecordLog(log);

                    return output.ToString(Formatting.None);
                }


                bool upload = await this.doUploadFile(fileName, stream);

                if (upload)
                {
                    cls_srvSystemImport srv_import = new cls_srvSystemImport();
                    string tmp = srv_import.doImportExcel("REDUCE", fileName, "TEST");

                    output["success"] = true;
                    output["message"] = tmp;

                    log.apilog_status = "200";
                    log.apilog_message = "";
                }
                else
                {
                    output["success"] = false;
                    output["message"] = "Upload data not successfully";

                    log.apilog_status = "500";
                    log.apilog_message = "Upload data not successfully";
                }

            }
            catch (Exception ex)
            {
                output["success"] = false;
                output["message"] = "(C)Upload data not successfully";

                log.apilog_status = "500";
                log.apilog_message = ex.ToString();
            }
            finally
            {
                objBpcOpr.doRecordLog(log);
            }

            return output.ToString(Formatting.None);
        }
    


        #endregion

        #region MTEthnicity
        public string getEthnicityList(BasicRequest req)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "ENT001.1";
            log.apilog_by = req.username;
            log.apilog_data = "all";

            try
            {
                var authHeader = WebOperationContext.Current.IncomingRequest.Headers["Authorization"];
                if (authHeader == null || !objBpcOpr.doVerify(authHeader))
                {
                    output["success"] = false;
                    output["message"] = BpcOpr.MessageNotAuthen;

                    log.apilog_status = "500";
                    log.apilog_message = BpcOpr.MessageNotAuthen;
                    objBpcOpr.doRecordLog(log);

                    return output.ToString(Formatting.None);
                }

                cls_ctMTEthnicity controller = new cls_ctMTEthnicity();
                List<cls_MTEthnicity> list = controller.getDataByFillter("");
                JArray array = new JArray();

                if (list.Count > 0)
                {
                    int index = 1;

                    foreach (cls_MTEthnicity model in list)
                    {
                        JObject json = new JObject();
                        json.Add("ethnicity_id", model.ethnicity_id);
                        json.Add("ethnicity_code", model.ethnicity_code);
                        json.Add("ethnicity_name_th", model.ethnicity_name_th);
                        json.Add("ethnicity_name_en", model.ethnicity_name_en);
                        json.Add("modified_by", model.modified_by);
                        json.Add("modified_date", model.modified_date);
                        json.Add("index", index++);
                        array.Add(json);
                    }

                    output["success"] = true;
                    output["message"] = "";
                    output["data"] = array;

                    log.apilog_status = "200";
                    log.apilog_message = "";
                }
                else
                {
                    output["success"] = false;
                    output["message"] = "Data not Found";
                    output["data"] = array;

                    log.apilog_status = "404";
                    log.apilog_message = "Data not Found";
                }

                controller.dispose();
            }
            catch (Exception ex)
            {
                output["success"] = false;
                output["message"] = "(C)Retrieved data not successfully";

                log.apilog_status = "500";
                log.apilog_message = ex.ToString();
            }
            finally
            {
                objBpcOpr.doRecordLog(log);
            }

            return output.ToString(Formatting.None);
        }
        public string doManageMTEthnicity(InputMTEthnicity input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);


            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "ENT001.2";
            log.apilog_by = input.modified_by;
            log.apilog_data = tmp.ToString();

            try
            {
                var authHeader = WebOperationContext.Current.IncomingRequest.Headers["Authorization"];
                if (authHeader == null || !objBpcOpr.doVerify(authHeader))
                {
                    output["success"] = false;
                    output["message"] = BpcOpr.MessageNotAuthen;

                    log.apilog_status = "500";
                    log.apilog_message = BpcOpr.MessageNotAuthen;
                    objBpcOpr.doRecordLog(log);

                    return output.ToString(Formatting.None);
                }

                cls_ctMTEthnicity controller = new cls_ctMTEthnicity();
                cls_MTEthnicity model = new cls_MTEthnicity();

                model.ethnicity_id = Convert.ToInt32(input.ethnicity_id);
                model.ethnicity_code = input.ethnicity_code;
                model.ethnicity_name_th = input.ethnicity_name_th;
                model.ethnicity_name_en = input.ethnicity_name_en;
                model.modified_by = input.modified_by;

                string strID = controller.insert(model);

                if (!strID.Equals(""))
                {
                    output["success"] = true;
                    output["message"] = "Retrieved data successfully";
                    output["record_id"] = strID;

                    log.apilog_status = "200";
                    log.apilog_message = "";
                }
                else
                {
                    output["success"] = false;
                    output["message"] = "Retrieved data not successfully";

                    log.apilog_status = "500";
                    log.apilog_message = controller.getMessage();
                }

                controller.dispose();

            }
            catch (Exception ex)
            {
                output["success"] = false;
                output["message"] = "(C)Retrieved data not successfully";

                log.apilog_status = "500";
                log.apilog_message = ex.ToString();
            }
            finally
            {
                objBpcOpr.doRecordLog(log);
            }

            output["data"] = tmp;

            return output.ToString(Formatting.None);
        }
        public string doDeleteMTEthnicity(InputMTEthnicity input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "ENT001.3";
            log.apilog_by = input.modified_by;
            log.apilog_data = tmp.ToString();

            try
            {
                var authHeader = WebOperationContext.Current.IncomingRequest.Headers["Authorization"];
                if (authHeader == null || !objBpcOpr.doVerify(authHeader))
                {
                    output["success"] = false;
                    output["message"] = BpcOpr.MessageNotAuthen;
                    log.apilog_status = "500";
                    log.apilog_message = BpcOpr.MessageNotAuthen;
                    objBpcOpr.doRecordLog(log);

                    return output.ToString(Formatting.None);
                }

                cls_ctMTEthnicity controller = new cls_ctMTEthnicity();

                if (controller.checkDataOld(input.ethnicity_code))
                {
                    bool blnResult = controller.delete(input.ethnicity_code);

                    if (blnResult)
                    {
                        output["success"] = true;
                        output["message"] = "Remove data successfully";

                        log.apilog_status = "200";
                        log.apilog_message = "";
                    }
                    else
                    {
                        output["success"] = false;
                        output["message"] = "Remove data not successfully";

                        log.apilog_status = "500";
                        log.apilog_message = controller.getMessage();
                    }

                }
                else
                {
                    string message = "Not Found Project code : " + input.ethnicity_code;
                    output["success"] = false;
                    output["message"] = message;

                    log.apilog_status = "404";
                    log.apilog_message = message;
                }

                controller.dispose();
            }
            catch (Exception ex)
            {
                output["success"] = false;
                output["message"] = "(C)Remove data not successfully";

                log.apilog_status = "500";
                log.apilog_message = ex.ToString();
            }
            finally
            {
                objBpcOpr.doRecordLog(log);
            }

            output["data"] = tmp;

            return output.ToString(Formatting.None);

        }
        public async Task<string> doUploadMTEthnicity(string token, string by, string fileName, Stream stream)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "ENT001.4";
            log.apilog_by = by;
            log.apilog_data = "Stream";

            try
            {
                if (!objBpcOpr.doVerify(token))
                {
                    output["success"] = false;
                    output["message"] = BpcOpr.MessageNotAuthen;

                    log.apilog_status = "500";
                    log.apilog_message = BpcOpr.MessageNotAuthen;
                    objBpcOpr.doRecordLog(log);

                    return output.ToString(Formatting.None);
                }


                bool upload = await this.doUploadFile(fileName, stream);

                if (upload)
                {
                    cls_srvSystemImport srv_import = new cls_srvSystemImport();
                    string tmp = srv_import.doImportExcel("ETHNICITY", fileName, "TEST");

                    output["success"] = true;
                    output["message"] = tmp;

                    log.apilog_status = "200";
                    log.apilog_message = "";
                }
                else
                {
                    output["success"] = false;
                    output["message"] = "Upload data not successfully";

                    log.apilog_status = "500";
                    log.apilog_message = "Upload data not successfully";
                }

            }
            catch (Exception ex)
            {
                output["success"] = false;
                output["message"] = "(C)Upload data not successfully";

                log.apilog_status = "500";
                log.apilog_message = ex.ToString();
            }
            finally
            {
                objBpcOpr.doRecordLog(log);
            }

            return output.ToString(Formatting.None);
        }
        #endregion

        #region MTReligion
        public string getReligionList(BasicRequest req)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "RLG001.1";
            log.apilog_by = req.username;
            log.apilog_data = "all";

            try
            {
                var authHeader = WebOperationContext.Current.IncomingRequest.Headers["Authorization"];
                if (authHeader == null || !objBpcOpr.doVerify(authHeader))
                {
                    output["success"] = false;
                    output["message"] = BpcOpr.MessageNotAuthen;

                    log.apilog_status = "500";
                    log.apilog_message = BpcOpr.MessageNotAuthen;
                    objBpcOpr.doRecordLog(log);

                    return output.ToString(Formatting.None);
                }

                cls_ctMTReligion controller = new cls_ctMTReligion();
                List<cls_MTReligion> list = controller.getDataByFillter("");
                JArray array = new JArray();

                if (list.Count > 0)
                {
                    int index = 1;

                    foreach (cls_MTReligion model in list)
                    {
                        JObject json = new JObject();
                        json.Add("religion_id", model.religion_id);
                        json.Add("religion_code", model.religion_code);
                        json.Add("religion_name_th", model.religion_name_th);
                        json.Add("religion_name_en", model.religion_name_en);
                        json.Add("modified_by", model.modified_by);
                        json.Add("modified_date", model.modified_date);
                        json.Add("index", index++);
                        array.Add(json);
                    }

                    output["success"] = true;
                    output["message"] = "";
                    output["data"] = array;

                    log.apilog_status = "200";
                    log.apilog_message = "";
                }
                else
                {
                    output["success"] = false;
                    output["message"] = "Data not Found";
                    output["data"] = array;

                    log.apilog_status = "404";
                    log.apilog_message = "Data not Found";
                }

                controller.dispose();
            }
            catch (Exception ex)
            {
                output["success"] = false;
                output["message"] = "(C)Retrieved data not successfully";

                log.apilog_status = "500";
                log.apilog_message = ex.ToString();
            }
            finally
            {
                objBpcOpr.doRecordLog(log);
            }

            return output.ToString(Formatting.None);
        }
        public string doManageMTReligion(InputMTReligion input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);


            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "RLG001.2";
            log.apilog_by = input.modified_by;
            log.apilog_data = tmp.ToString();

            try
            {
                var authHeader = WebOperationContext.Current.IncomingRequest.Headers["Authorization"];
                if (authHeader == null || !objBpcOpr.doVerify(authHeader))
                {
                    output["success"] = false;
                    output["message"] = BpcOpr.MessageNotAuthen;

                    log.apilog_status = "500";
                    log.apilog_message = BpcOpr.MessageNotAuthen;
                    objBpcOpr.doRecordLog(log);

                    return output.ToString(Formatting.None);
                }

                cls_ctMTReligion controller = new cls_ctMTReligion();
                cls_MTReligion model = new cls_MTReligion();

                model.religion_id = Convert.ToInt32(input.religion_id);
                model.religion_code = input.religion_code;
                model.religion_name_th = input.religion_name_th;
                model.religion_name_en = input.religion_name_en;
                model.modified_by = input.modified_by;

                string strID = controller.insert(model);

                if (!strID.Equals(""))
                {
                    output["success"] = true;
                    output["message"] = "Retrieved data successfully";
                    output["record_id"] = strID;

                    log.apilog_status = "200";
                    log.apilog_message = "";
                }
                else
                {
                    output["success"] = false;
                    output["message"] = "Retrieved data not successfully";

                    log.apilog_status = "500";
                    log.apilog_message = controller.getMessage();
                }

                controller.dispose();

            }
            catch (Exception ex)
            {
                output["success"] = false;
                output["message"] = "(C)Retrieved data not successfully";

                log.apilog_status = "500";
                log.apilog_message = ex.ToString();
            }
            finally
            {
                objBpcOpr.doRecordLog(log);
            }

            output["data"] = tmp;

            return output.ToString(Formatting.None);
        }
        public string doDeleteMTReligion(InputMTReligion input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "RLG001.3";
            log.apilog_by = input.modified_by;
            log.apilog_data = tmp.ToString();

            try
            {
                var authHeader = WebOperationContext.Current.IncomingRequest.Headers["Authorization"];
                if (authHeader == null || !objBpcOpr.doVerify(authHeader))
                {
                    output["success"] = false;
                    output["message"] = BpcOpr.MessageNotAuthen;
                    log.apilog_status = "500";
                    log.apilog_message = BpcOpr.MessageNotAuthen;
                    objBpcOpr.doRecordLog(log);

                    return output.ToString(Formatting.None);
                }

                cls_ctMTReligion controller = new cls_ctMTReligion();

                if (controller.checkDataOld(input.religion_code))
                {
                    bool blnResult = controller.delete(input.religion_code);

                    if (blnResult)
                    {
                        output["success"] = true;
                        output["message"] = "Remove data successfully";

                        log.apilog_status = "200";
                        log.apilog_message = "";
                    }
                    else
                    {
                        output["success"] = false;
                        output["message"] = "Remove data not successfully";

                        log.apilog_status = "500";
                        log.apilog_message = controller.getMessage();
                    }

                }
                else
                {
                    string message = "Not Found Project code : " + input.religion_code;
                    output["success"] = false;
                    output["message"] = message;

                    log.apilog_status = "404";
                    log.apilog_message = message;
                }

                controller.dispose();
            }
            catch (Exception ex)
            {
                output["success"] = false;
                output["message"] = "(C)Remove data not successfully";

                log.apilog_status = "500";
                log.apilog_message = ex.ToString();
            }
            finally
            {
                objBpcOpr.doRecordLog(log);
            }

            output["data"] = tmp;

            return output.ToString(Formatting.None);

        }
        public async Task<string> doUploadMTReligion(string token, string by, string fileName, Stream stream)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "RLG001.4";
            log.apilog_by = by;
            log.apilog_data = "Stream";

            try
            {
                if (!objBpcOpr.doVerify(token))
                {
                    output["success"] = false;
                    output["message"] = BpcOpr.MessageNotAuthen;

                    log.apilog_status = "500";
                    log.apilog_message = BpcOpr.MessageNotAuthen;
                    objBpcOpr.doRecordLog(log);

                    return output.ToString(Formatting.None);
                }


                bool upload = await this.doUploadFile(fileName, stream);

                if (upload)
                {
                    cls_srvSystemImport srv_import = new cls_srvSystemImport();
                    string tmp = srv_import.doImportExcel("RELIGION", fileName, "TEST");

                    output["success"] = true;
                    output["message"] = tmp;

                    log.apilog_status = "200";
                    log.apilog_message = "";
                }
                else
                {
                    output["success"] = false;
                    output["message"] = "Upload data not successfully";

                    log.apilog_status = "500";
                    log.apilog_message = "Upload data not successfully";
                }

            }
            catch (Exception ex)
            {
                output["success"] = false;
                output["message"] = "(C)Upload data not successfully";

                log.apilog_status = "500";
                log.apilog_message = ex.ToString();
            }
            finally
            {
                objBpcOpr.doRecordLog(log);
            }

            return output.ToString(Formatting.None);
        }
    

        #endregion

        #region MTBloodtype
        public string getBloodtypeList(BasicRequest req)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "BLD001.1";
            log.apilog_by = req.username;
            log.apilog_data = "all";

            try
            {
                var authHeader = WebOperationContext.Current.IncomingRequest.Headers["Authorization"];
                if (authHeader == null || !objBpcOpr.doVerify(authHeader))
                {
                    output["success"] = false;
                    output["message"] = BpcOpr.MessageNotAuthen;

                    log.apilog_status = "500";
                    log.apilog_message = BpcOpr.MessageNotAuthen;
                    objBpcOpr.doRecordLog(log);

                    return output.ToString(Formatting.None);
                }

                cls_ctMTBloodtype controller = new cls_ctMTBloodtype();
                List<cls_MTBloodtype> list = controller.getDataByFillter("");
                JArray array = new JArray();

                if (list.Count > 0)
                {
                    int index = 1;

                    foreach (cls_MTBloodtype model in list)
                    {
                        JObject json = new JObject();
                        json.Add("bloodtype_id", model.bloodtype_id);
                        json.Add("bloodtype_code", model.bloodtype_code);
                        json.Add("bloodtype_name_th", model.bloodtype_name_th);
                        json.Add("bloodtype_name_en", model.bloodtype_name_en);
                        json.Add("modified_by", model.modified_by);
                        json.Add("modified_date", model.modified_date);
                        json.Add("index", index++);
                        array.Add(json);
                    }

                    output["success"] = true;
                    output["message"] = "";
                    output["data"] = array;

                    log.apilog_status = "200";
                    log.apilog_message = "";
                }
                else
                {
                    output["success"] = false;
                    output["message"] = "Data not Found";
                    output["data"] = array;

                    log.apilog_status = "404";
                    log.apilog_message = "Data not Found";
                }

                controller.dispose();
            }
            catch (Exception ex)
            {
                output["success"] = false;
                output["message"] = "(C)Retrieved data not successfully";

                log.apilog_status = "500";
                log.apilog_message = ex.ToString();
            }
            finally
            {
                objBpcOpr.doRecordLog(log);
            }

            return output.ToString(Formatting.None);
        }
        public string doManagemMTBloodtype(InputMTBloodtype input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);


            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "BLD001.2";
            log.apilog_by = input.modified_by;
            log.apilog_data = tmp.ToString();

            try
            {
                var authHeader = WebOperationContext.Current.IncomingRequest.Headers["Authorization"];
                if (authHeader == null || !objBpcOpr.doVerify(authHeader))
                {
                    output["success"] = false;
                    output["message"] = BpcOpr.MessageNotAuthen;

                    log.apilog_status = "500";
                    log.apilog_message = BpcOpr.MessageNotAuthen;
                    objBpcOpr.doRecordLog(log);

                    return output.ToString(Formatting.None);
                }

                cls_ctMTBloodtype controller = new cls_ctMTBloodtype();
                cls_MTBloodtype model = new cls_MTBloodtype();

                model.bloodtype_id = Convert.ToInt32(input.bloodtype_id);
                model.bloodtype_code = input.bloodtype_code;
                model.bloodtype_name_th = input.bloodtype_name_th;
                model.bloodtype_name_en = input.bloodtype_name_en;
                model.modified_by = input.modified_by;

                string strID = controller.insert(model);

                if (!strID.Equals(""))
                {
                    output["success"] = true;
                    output["message"] = "Retrieved data successfully";
                    output["record_id"] = strID;

                    log.apilog_status = "200";
                    log.apilog_message = "";
                }
                else
                {
                    output["success"] = false;
                    output["message"] = "Retrieved data not successfully";

                    log.apilog_status = "500";
                    log.apilog_message = controller.getMessage();
                }

                controller.dispose();

            }
            catch (Exception ex)
            {
                output["success"] = false;
                output["message"] = "(C)Retrieved data not successfully";

                log.apilog_status = "500";
                log.apilog_message = ex.ToString();
            }
            finally
            {
                objBpcOpr.doRecordLog(log);
            }

            output["data"] = tmp;

            return output.ToString(Formatting.None);
        }
        public string doDeleteMTBloodtype(InputMTBloodtype input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "BLD001.3";
            log.apilog_by = input.modified_by;
            log.apilog_data = tmp.ToString();

            try
            {
                var authHeader = WebOperationContext.Current.IncomingRequest.Headers["Authorization"];
                if (authHeader == null || !objBpcOpr.doVerify(authHeader))
                {
                    output["success"] = false;
                    output["message"] = BpcOpr.MessageNotAuthen;
                    log.apilog_status = "500";
                    log.apilog_message = BpcOpr.MessageNotAuthen;
                    objBpcOpr.doRecordLog(log);

                    return output.ToString(Formatting.None);
                }

                cls_ctMTBloodtype controller = new cls_ctMTBloodtype();

                if (controller.checkDataOld(input.bloodtype_code))
                {
                    bool blnResult = controller.delete(input.bloodtype_code);

                    if (blnResult)
                    {
                        output["success"] = true;
                        output["message"] = "Remove data successfully";

                        log.apilog_status = "200";
                        log.apilog_message = "";
                    }
                    else
                    {
                        output["success"] = false;
                        output["message"] = "Remove data not successfully";

                        log.apilog_status = "500";
                        log.apilog_message = controller.getMessage();
                    }

                }
                else
                {
                    string message = "Not Found Project code : " + input.bloodtype_code;
                    output["success"] = false;
                    output["message"] = message;

                    log.apilog_status = "404";
                    log.apilog_message = message;
                }

                controller.dispose();
            }
            catch (Exception ex)
            {
                output["success"] = false;
                output["message"] = "(C)Remove data not successfully";

                log.apilog_status = "500";
                log.apilog_message = ex.ToString();
            }
            finally
            {
                objBpcOpr.doRecordLog(log);
            }

            output["data"] = tmp;

            return output.ToString(Formatting.None);

        }
        public async Task<string> doUploadBloodtype(string token, string by, string fileName, Stream stream)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "BLD001.4";
            log.apilog_by = by;
            log.apilog_data = "Stream";

            try
            {
                if (!objBpcOpr.doVerify(token))
                {
                    output["success"] = false;
                    output["message"] = BpcOpr.MessageNotAuthen;

                    log.apilog_status = "500";
                    log.apilog_message = BpcOpr.MessageNotAuthen;
                    objBpcOpr.doRecordLog(log);

                    return output.ToString(Formatting.None);
                }


                bool upload = await this.doUploadFile(fileName, stream);

                if (upload)
                {
                    cls_srvSystemImport srv_import = new cls_srvSystemImport();
                    string tmp = srv_import.doImportExcel("BLOODTYPE", fileName, "TEST");

                    output["success"] = true;
                    output["message"] = tmp;

                    log.apilog_status = "200";
                    log.apilog_message = "";
                }
                else
                {
                    output["success"] = false;
                    output["message"] = "Upload data not successfully";

                    log.apilog_status = "500";
                    log.apilog_message = "Upload data not successfully";
                }

            }
            catch (Exception ex)
            {
                output["success"] = false;
                output["message"] = "(C)Upload data not successfully";

                log.apilog_status = "500";
                log.apilog_message = ex.ToString();
            }
            finally
            {
                objBpcOpr.doRecordLog(log);
            }

            return output.ToString(Formatting.None);
        }
    
        #endregion

        #region MTHospital
        public string getHospitalList(BasicRequest req)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "HPT001.1";
            log.apilog_by = req.username;
            log.apilog_data = "all";

            try
            {
                var authHeader = WebOperationContext.Current.IncomingRequest.Headers["Authorization"];
                if (authHeader == null || !objBpcOpr.doVerify(authHeader))
                {
                    output["success"] = false;
                    output["message"] = BpcOpr.MessageNotAuthen;

                    log.apilog_status = "500";
                    log.apilog_message = BpcOpr.MessageNotAuthen;
                    objBpcOpr.doRecordLog(log);

                    return output.ToString(Formatting.None);
                }

                cls_ctMTHospital controller = new cls_ctMTHospital();
                List<cls_MTHospital> list = controller.getDataByFillter("");
                JArray array = new JArray();

                if (list.Count > 0)
                {
                    int index = 1;

                    foreach (cls_MTHospital model in list)
                    {
                        JObject json = new JObject();
                        json.Add("hospital_id", model.hospital_id);
                        json.Add("hospital_code", model.hospital_code);
                        json.Add("hospital_name_th", model.hospital_name_th);
                        json.Add("hospital_name_en", model.hospital_name_en);
                        json.Add("modified_by", model.modified_by);
                        json.Add("modified_date", model.modified_date);
                        json.Add("index", index++);
                        array.Add(json);
                    }

                    output["success"] = true;
                    output["message"] = "";
                    output["data"] = array;

                    log.apilog_status = "200";
                    log.apilog_message = "";
                }
                else
                {
                    output["success"] = false;
                    output["message"] = "Data not Found";
                    output["data"] = array;

                    log.apilog_status = "404";
                    log.apilog_message = "Data not Found";
                }

                controller.dispose();
            }
            catch (Exception ex)
            {
                output["success"] = false;
                output["message"] = "(C)Retrieved data not successfully";

                log.apilog_status = "500";
                log.apilog_message = ex.ToString();
            }
            finally
            {
                objBpcOpr.doRecordLog(log);
            }

            return output.ToString(Formatting.None);
        }
        public string doManagemMTHospital(InputMTHospital input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);


            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "HPT001.2";
            log.apilog_by = input.modified_by;
            log.apilog_data = tmp.ToString();

            try
            {
                var authHeader = WebOperationContext.Current.IncomingRequest.Headers["Authorization"];
                if (authHeader == null || !objBpcOpr.doVerify(authHeader))
                {
                    output["success"] = false;
                    output["message"] = BpcOpr.MessageNotAuthen;

                    log.apilog_status = "500";
                    log.apilog_message = BpcOpr.MessageNotAuthen;
                    objBpcOpr.doRecordLog(log);

                    return output.ToString(Formatting.None);
                }

                cls_ctMTHospital controller = new cls_ctMTHospital();
                cls_MTHospital model = new cls_MTHospital();

                model.hospital_id = Convert.ToInt32(input.hospital_id);
                model.hospital_code = input.hospital_code;
                model.hospital_name_th = input.hospital_name_th;
                model.hospital_name_en = input.hospital_name_en;
                model.modified_by = input.modified_by;

                string strID = controller.insert(model);

                if (!strID.Equals(""))
                {
                    output["success"] = true;
                    output["message"] = "Retrieved data successfully";
                    output["record_id"] = strID;

                    log.apilog_status = "200";
                    log.apilog_message = "";
                }
                else
                {
                    output["success"] = false;
                    output["message"] = "Retrieved data not successfully";

                    log.apilog_status = "500";
                    log.apilog_message = controller.getMessage();
                }

                controller.dispose();

            }
            catch (Exception ex)
            {
                output["success"] = false;
                output["message"] = "(C)Retrieved data not successfully";

                log.apilog_status = "500";
                log.apilog_message = ex.ToString();
            }
            finally
            {
                objBpcOpr.doRecordLog(log);
            }

            output["data"] = tmp;

            return output.ToString(Formatting.None);
        }
        public string doDeleteMTHospital(InputMTHospital input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "HPT001.3";
            log.apilog_by = input.modified_by;
            log.apilog_data = tmp.ToString();

            try
            {
                var authHeader = WebOperationContext.Current.IncomingRequest.Headers["Authorization"];
                if (authHeader == null || !objBpcOpr.doVerify(authHeader))
                {
                    output["success"] = false;
                    output["message"] = BpcOpr.MessageNotAuthen;
                    log.apilog_status = "500";
                    log.apilog_message = BpcOpr.MessageNotAuthen;
                    objBpcOpr.doRecordLog(log);

                    return output.ToString(Formatting.None);
                }

                cls_ctMTHospital controller = new cls_ctMTHospital();

                if (controller.checkDataOld(input.hospital_code))
                {
                    bool blnResult = controller.delete(input.hospital_code);

                    if (blnResult)
                    {
                        output["success"] = true;
                        output["message"] = "Remove data successfully";

                        log.apilog_status = "200";
                        log.apilog_message = "";
                    }
                    else
                    {
                        output["success"] = false;
                        output["message"] = "Remove data not successfully";

                        log.apilog_status = "500";
                        log.apilog_message = controller.getMessage();
                    }

                }
                else
                {
                    string message = "Not Found Project code : " + input.hospital_code;
                    output["success"] = false;
                    output["message"] = message;

                    log.apilog_status = "404";
                    log.apilog_message = message;
                }

                controller.dispose();
            }
            catch (Exception ex)
            {
                output["success"] = false;
                output["message"] = "(C)Remove data not successfully";

                log.apilog_status = "500";
                log.apilog_message = ex.ToString();
            }
            finally
            {
                objBpcOpr.doRecordLog(log);
            }

            output["data"] = tmp;

            return output.ToString(Formatting.None);

        }
        public async Task<string> doUploadHospital(string token, string by, string fileName, Stream stream)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "HPT001.4";
            log.apilog_by = by;
            log.apilog_data = "Stream";

            try
            {
                if (!objBpcOpr.doVerify(token))
                {
                    output["success"] = false;
                    output["message"] = BpcOpr.MessageNotAuthen;

                    log.apilog_status = "500";
                    log.apilog_message = BpcOpr.MessageNotAuthen;
                    objBpcOpr.doRecordLog(log);

                    return output.ToString(Formatting.None);
                }


                bool upload = await this.doUploadFile(fileName, stream);

                if (upload)
                {
                    cls_srvSystemImport srv_import = new cls_srvSystemImport();
                    string tmp = srv_import.doImportExcel("HOSPITAL", fileName, "TEST");

                    output["success"] = true;
                    output["message"] = tmp;

                    log.apilog_status = "200";
                    log.apilog_message = "";
                }
                else
                {
                    output["success"] = false;
                    output["message"] = "Upload data not successfully";

                    log.apilog_status = "500";
                    log.apilog_message = "Upload data not successfully";
                }

            }
            catch (Exception ex)
            {
                output["success"] = false;
                output["message"] = "(C)Upload data not successfully";

                log.apilog_status = "500";
                log.apilog_message = ex.ToString();
            }
            finally
            {
                objBpcOpr.doRecordLog(log);
            }

            return output.ToString(Formatting.None);
        }
        #endregion

        #region MTProvince
        public string getProvinceList(BasicRequest req)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "PRV001.1";
            log.apilog_by = req.username;
            log.apilog_data = "all";

            try
            {
                var authHeader = WebOperationContext.Current.IncomingRequest.Headers["Authorization"];
                if (authHeader == null || !objBpcOpr.doVerify(authHeader))
                {
                    output["success"] = false;
                    output["message"] = BpcOpr.MessageNotAuthen;

                    log.apilog_status = "500";
                    log.apilog_message = BpcOpr.MessageNotAuthen;
                    objBpcOpr.doRecordLog(log);

                    return output.ToString(Formatting.None);
                }

                cls_ctMTProvince contprovince = new cls_ctMTProvince();
                List<cls_MTProvince> list = contprovince.getDataByFillter("");
                JArray array = new JArray();

                if (list.Count > 0)
                {
                    int index = 1;

                    foreach (cls_MTProvince model in list)
                    {
                        JObject json = new JObject();
                        json.Add("province_id", model.province_id);
                        json.Add("province_code", model.province_code);
                        json.Add("province_name_th", model.province_name_th);
                        json.Add("province_name_en", model.province_name_en);
                        json.Add("modified_by", model.modified_by);
                        json.Add("modified_date", model.modified_date);
                        json.Add("index", index++);
                        array.Add(json);
                    }

                    output["success"] = true;
                    output["message"] = "";
                    output["data"] = array;

                    log.apilog_status = "200";
                    log.apilog_message = "";
                }
                else
                {
                    output["success"] = false;
                    output["message"] = "Data not Found";
                    output["data"] = array;

                    log.apilog_status = "404";
                    log.apilog_message = "Data not Found";
                }

                contprovince.dispose();
            }
            catch (Exception ex)
            {
                output["success"] = false;
                output["message"] = "(C)Retrieved data not successfully";

                log.apilog_status = "500";
                log.apilog_message = ex.ToString();
            }
            finally
            {
                objBpcOpr.doRecordLog(log);
            }

            return output.ToString(Formatting.None);
        }
        public string doManageMTProvince(InputMTProvince input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);


            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "PRV001.2";
            log.apilog_by = input.modified_by;
            log.apilog_data = tmp.ToString();

            try
            {
                var authHeader = WebOperationContext.Current.IncomingRequest.Headers["Authorization"];
                if (authHeader == null || !objBpcOpr.doVerify(authHeader))
                {
                    output["success"] = false;
                    output["message"] = BpcOpr.MessageNotAuthen;

                    log.apilog_status = "500";
                    log.apilog_message = BpcOpr.MessageNotAuthen;
                    objBpcOpr.doRecordLog(log);

                    return output.ToString(Formatting.None);
                }

                cls_ctMTProvince controller = new cls_ctMTProvince();
                cls_MTProvince model = new cls_MTProvince();

                model.province_id = Convert.ToInt32(input.province_id);
                model.province_code = input.province_code;
                model.province_name_th = input.province_name_th;
                model.province_name_en = input.province_name_en;
                model.modified_by = input.modified_by;

                string strID = controller.insert(model);

                if (!strID.Equals(""))
                {
                    output["success"] = true;
                    output["message"] = "Retrieved data successfully";
                    output["record_id"] = strID;

                    log.apilog_status = "200";
                    log.apilog_message = "";
                }
                else
                {
                    output["success"] = false;
                    output["message"] = "Retrieved data not successfully";

                    log.apilog_status = "500";
                    log.apilog_message = controller.getMessage();
                }

                controller.dispose();

            }
            catch (Exception ex)
            {
                output["success"] = false;
                output["message"] = "(C)Retrieved data not successfully";

                log.apilog_status = "500";
                log.apilog_message = ex.ToString();
            }
            finally
            {
                objBpcOpr.doRecordLog(log);
            }

            output["data"] = tmp;

            return output.ToString(Formatting.None);
        }
        public string doDeleteMTProvince(InputMTProvince input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "PRV001.3";
            log.apilog_by = input.modified_by;
            log.apilog_data = tmp.ToString();

            try
            {
                var authHeader = WebOperationContext.Current.IncomingRequest.Headers["Authorization"];
                if (authHeader == null || !objBpcOpr.doVerify(authHeader))
                {
                    output["success"] = false;
                    output["message"] = BpcOpr.MessageNotAuthen;
                    log.apilog_status = "500";
                    log.apilog_message = BpcOpr.MessageNotAuthen;
                    objBpcOpr.doRecordLog(log);

                    return output.ToString(Formatting.None);
                }

                cls_ctMTProvince controller = new cls_ctMTProvince();

                if (controller.checkDataOld(input.province_code))
                {
                    bool blnResult = controller.delete(input.province_code);

                    if (blnResult)
                    {
                        output["success"] = true;
                        output["message"] = "Remove data successfully";

                        log.apilog_status = "200";
                        log.apilog_message = "";
                    }
                    else
                    {
                        output["success"] = false;
                        output["message"] = "Remove data not successfully";

                        log.apilog_status = "500";
                        log.apilog_message = controller.getMessage();
                    }

                }
                else
                {
                    string message = "Not Found Project code : " + input.province_code;
                    output["success"] = false;
                    output["message"] = message;

                    log.apilog_status = "404";
                    log.apilog_message = message;
                }

                controller.dispose();
            }
            catch (Exception ex)
            {
                output["success"] = false;
                output["message"] = "(C)Remove data not successfully";

                log.apilog_status = "500";
                log.apilog_message = ex.ToString();
            }
            finally
            {
                objBpcOpr.doRecordLog(log);
            }

            output["data"] = tmp;

            return output.ToString(Formatting.None);

        }
        public async Task<string> doUploadMTProvince(string token, string by, string fileName, Stream stream)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "PRV001.4";
            log.apilog_by = by;
            log.apilog_data = "Stream";

            try
            {
                if (!objBpcOpr.doVerify(token))
                {
                    output["success"] = false;
                    output["message"] = BpcOpr.MessageNotAuthen;

                    log.apilog_status = "500";
                    log.apilog_message = BpcOpr.MessageNotAuthen;
                    objBpcOpr.doRecordLog(log);

                    return output.ToString(Formatting.None);
                }


                bool upload = await this.doUploadFile(fileName, stream);

                if (upload)
                {
                    cls_srvSystemImport srv_import = new cls_srvSystemImport();
                    string tmp = srv_import.doImportExcel("PROVINCE", fileName, "TEST");

                    output["success"] = true;
                    output["message"] = tmp;

                    log.apilog_status = "200";
                    log.apilog_message = "";
                }
                else
                {
                    output["success"] = false;
                    output["message"] = "Upload data not successfully";

                    log.apilog_status = "500";
                    log.apilog_message = "Upload data not successfully";
                }

            }
            catch (Exception ex)
            {
                output["success"] = false;
                output["message"] = "(C)Upload data not successfully";

                log.apilog_status = "500";
                log.apilog_message = ex.ToString();
            }
            finally
            {
                objBpcOpr.doRecordLog(log);
            }

            return output.ToString(Formatting.None);
        }
    
        #endregion

        #region combranch
        public string getCombranchList(FillterCompany req)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "CBR001.1";
            log.apilog_by = req.username;
            log.apilog_data = "all";

            try
            {
                var authHeader = WebOperationContext.Current.IncomingRequest.Headers["Authorization"];
                if (authHeader == null || !objBpcOpr.doVerify(authHeader))
                {
                    output["success"] = false;
                    output["message"] = BpcOpr.MessageNotAuthen;

                    log.apilog_status = "500";
                    log.apilog_message = BpcOpr.MessageNotAuthen;
                    objBpcOpr.doRecordLog(log);

                    return output.ToString(Formatting.None);
                }

                cls_ctMTCombranch controller = new cls_ctMTCombranch();
                List<cls_MTCombranch> list = controller.getDataByFillter(req.company_code, req.combranch_code, req.combranch_id);
                JArray array = new JArray();

                if (list.Count > 0)
                {
                    int index = 1;

                    foreach (cls_MTCombranch model in list)
                    {
                        JObject json = new JObject();
                        json.Add("combranch_id", model.combranch_id);
                        json.Add("combranch_code", model.combranch_code);
                        json.Add("combranch_name_th", model.combranch_name_th);
                        json.Add("combranch_name_en", model.combranch_name_en);
                        json.Add("company_code", model.company_code);
                        json.Add("modified_by", model.modified_by);
                        json.Add("modified_date", model.modified_date);
                        json.Add("flag", model.flag);

                        json.Add("index", index);

                        index++;

                        array.Add(json);
                    }

                    output["success"] = true;
                    output["message"] = "";
                    output["data"] = array;

                    log.apilog_status = "200";
                    log.apilog_message = "";
                }
                else
                {
                    output["success"] = false;
                    output["message"] = "Data not Found";
                    output["data"] = array;

                    log.apilog_status = "404";
                    log.apilog_message = "Data not Found";
                }

                controller.dispose();
            }
            catch (Exception ex)
            {
                output["success"] = false;
                output["message"] = "(C)Retrieved data not successfully";

                log.apilog_status = "500";
                log.apilog_message = ex.ToString();
            }
            finally
            {
                objBpcOpr.doRecordLog(log);
            }

            return output.ToString(Formatting.None);
        }
        public string doManageMTCombranch(InputMTCombranch input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);


            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "CBR001.2";
            log.apilog_by = input.modified_by;
            log.apilog_data = tmp.ToString();

            try
            {
                var authHeader = WebOperationContext.Current.IncomingRequest.Headers["Authorization"];
                if (authHeader == null || !objBpcOpr.doVerify(authHeader))
                {
                    output["success"] = false;
                    output["message"] = BpcOpr.MessageNotAuthen;

                    log.apilog_status = "500";
                    log.apilog_message = BpcOpr.MessageNotAuthen;
                    objBpcOpr.doRecordLog(log);

                    return output.ToString(Formatting.None);
                }

                cls_ctMTCombranch controller = new cls_ctMTCombranch();
                cls_MTCombranch model = new cls_MTCombranch();

                model.combranch_id = input.combranch_id;
                model.combranch_code = input.combranch_code;
                model.combranch_name_th = input.combranch_name_th;
                model.combranch_name_en = input.combranch_name_en;
                model.company_code = input.company_code;
                model.modified_by = input.modified_by;
                model.flag = model.flag;

                string strID = controller.insert(model);

                if (!strID.Equals(""))
                {
                    output["success"] = true;
                    output["message"] = "Retrieved data successfully";
                    output["record_id"] = strID;

                    log.apilog_status = "200";
                    log.apilog_message = "";
                }
                else
                {
                    output["success"] = false;
                    output["message"] = "Retrieved data not successfully";

                    log.apilog_status = "500";
                    log.apilog_message = controller.getMessage();
                }

                controller.dispose();

            }
            catch (Exception ex)
            {
                output["success"] = false;
                output["message"] = "(C)Retrieved data not successfully";

                log.apilog_status = "500";
                log.apilog_message = ex.ToString();
            }
            finally
            {
                objBpcOpr.doRecordLog(log);
            }

            output["data"] = tmp;

            return output.ToString(Formatting.None);
        }
        public string doDeleteMTCombranch(InputMTCombranch input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "CBR001.3";
            log.apilog_by = input.modified_by;
            log.apilog_data = tmp.ToString();

            try
            {
                var authHeader = WebOperationContext.Current.IncomingRequest.Headers["Authorization"];
                if (authHeader == null || !objBpcOpr.doVerify(authHeader))
                {
                    output["success"] = false;
                    output["message"] = BpcOpr.MessageNotAuthen;
                    log.apilog_status = "500";
                    log.apilog_message = BpcOpr.MessageNotAuthen;
                    objBpcOpr.doRecordLog(log);

                    return output.ToString(Formatting.None);
                }

                cls_ctMTCombranch controller = new cls_ctMTCombranch();

                if (controller.checkDataOld(input.combranch_code))
                {
                    bool blnResult = controller.delete(input.combranch_id.ToString());

                    if (blnResult)
                    {
                        output["success"] = true;
                        output["message"] = "Remove data successfully";

                        log.apilog_status = "200";
                        log.apilog_message = "";
                    }
                    else
                    {
                        output["success"] = false;
                        output["message"] = "Remove data not successfully";

                        log.apilog_status = "500";
                        log.apilog_message = controller.getMessage();
                    }

                }
                else
                {
                    string message = "Not Found Project code : " + input.combranch_code;
                    output["success"] = false;
                    output["message"] = message;

                    log.apilog_status = "404";
                    log.apilog_message = message;
                }

                controller.dispose();
            }
            catch (Exception ex)
            {
                output["success"] = false;
                output["message"] = "(C)Remove data not successfully";

                log.apilog_status = "500";
                log.apilog_message = ex.ToString();
            }
            finally
            {
                objBpcOpr.doRecordLog(log);
            }

            output["data"] = tmp;

            return output.ToString(Formatting.None);

        }
        public async Task<string> doUploadMTCombranch(string token, string by, string fileName, Stream stream)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "CBR001.4";
            log.apilog_by = by;
            log.apilog_data = "Stream";

            try
            {
                if (!objBpcOpr.doVerify(token))
                {
                    output["success"] = false;
                    output["message"] = BpcOpr.MessageNotAuthen;

                    log.apilog_status = "500";
                    log.apilog_message = BpcOpr.MessageNotAuthen;
                    objBpcOpr.doRecordLog(log);

                    return output.ToString(Formatting.None);
                }


                bool upload = await this.doUploadFile(fileName, stream);

                if (upload)
                {
                    cls_srvSystemImport srv_import = new cls_srvSystemImport();
                    string tmp = srv_import.doImportExcel("Combranch", fileName, "TEST");

                    output["success"] = true;
                    output["message"] = tmp;

                    log.apilog_status = "200";
                    log.apilog_message = "";
                }
                else
                {
                    output["success"] = false;
                    output["message"] = "Upload data not successfully";

                    log.apilog_status = "500";
                    log.apilog_message = "Upload data not successfully";
                }

            }
            catch (Exception ex)
            {
                output["success"] = false;
                output["message"] = "(C)Upload data not successfully";

                log.apilog_status = "500";
                log.apilog_message = ex.ToString();
            }
            finally
            {
                objBpcOpr.doRecordLog(log);
            }

            return output.ToString(Formatting.None);
        }

        #endregion

        #region company
        public string getCompanyList(FillterCompany req) 
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "CPN001.1";
            log.apilog_by = req.username;
            log.apilog_data = "all";

            try
            {
                var authHeader = WebOperationContext.Current.IncomingRequest.Headers["Authorization"];
                if (authHeader == null || !objBpcOpr.doVerify(authHeader))
                {
                    output["success"] = false;
                    output["message"] = BpcOpr.MessageNotAuthen;

                    log.apilog_status = "500";
                    log.apilog_message = BpcOpr.MessageNotAuthen;
                    objBpcOpr.doRecordLog(log);

                    return output.ToString(Formatting.None);
                }

                cls_ctMTCompany controller = new cls_ctMTCompany();
                List<cls_MTCompany> list = controller.getDataByFillter(req.company_id,req.company_code);
                JArray array = new JArray();

                if (list.Count > 0)
                {
                    int index = 1;

                    foreach (cls_MTCompany model in list)
                    {
                        JObject json = new JObject();
                        json.Add("company_id", model.company_id);
                        json.Add("company_code", model.company_code);
                        json.Add("company_initials", model.company_initials);
                        json.Add("company_name_th", model.company_name_th);
                        json.Add("company_name_en", model.company_name_en);
                        json.Add("hrs_perday", model.hrs_perday);

                        json.Add("sso_com_rate", model.sso_com_rate);
                        json.Add("sso_emp_rate", model.sso_emp_rate);
                        json.Add("sso_min_wage", model.sso_min_wage);
                        json.Add("sso_max_wage", model.sso_max_wage);
                        json.Add("sso_min_age", model.sso_min_age);
                        json.Add("sso_max_age", model.sso_max_age);

                        json.Add("modified_by", model.modified_by);
                        json.Add("modified_date", model.modified_date);
                        json.Add("flag", model.flag);

                        json.Add("index", index);

                        index++;

                        array.Add(json);
                    }

                    output["success"] = true;
                    output["message"] = "";
                    output["data"] = array;

                    log.apilog_status = "200";
                    log.apilog_message = "";
                }
                else
                {
                    output["success"] = false;
                    output["message"] = "Data not Found";
                    output["data"] = array;

                    log.apilog_status = "404";
                    log.apilog_message = "Data not Found";
                }

                controller.dispose();
            }
            catch (Exception ex)
            {
                output["success"] = false;
                output["message"] = "(C)Retrieved data not successfully";

                log.apilog_status = "500";
                log.apilog_message = ex.ToString();
            }
            finally
            {
                objBpcOpr.doRecordLog(log);
            }

            return output.ToString(Formatting.None);
        }
        public string doManageMTCompany(InputMTCompany input) 
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);


            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "CPN001.2";
            log.apilog_by = input.modified_by;
            log.apilog_data = tmp.ToString();

            try
            {
                var authHeader = WebOperationContext.Current.IncomingRequest.Headers["Authorization"];
                if (authHeader == null || !objBpcOpr.doVerify(authHeader))
                {
                    output["success"] = false;
                    output["message"] = BpcOpr.MessageNotAuthen;

                    log.apilog_status = "500";
                    log.apilog_message = BpcOpr.MessageNotAuthen;
                    objBpcOpr.doRecordLog(log);

                    return output.ToString(Formatting.None);
                }

                cls_ctMTCompany controller = new cls_ctMTCompany();
                cls_MTCompany model = new cls_MTCompany();

                model.company_id = input.company_id;
                model.company_code = input.company_code;
                model.company_initials = input.company_initials;

                model.company_name_th = input.company_name_th;
                model.company_name_en = input.company_name_en;
                model.hrs_perday = input.hrs_perday;
                model.sso_com_rate = input.sso_com_rate;
                model.sso_emp_rate = input.sso_emp_rate;
                model.sso_min_wage = input.sso_min_wage;
                model.sso_max_wage = input.sso_max_wage;
                model.sso_min_age = input.sso_min_age;
                model.sso_max_age = input.sso_max_age;

                model.modified_by = input.modified_by;
                model.flag = model.flag;

                string strID = controller.insert(model);

                if (!strID.Equals(""))
                {
                    output["success"] = true;
                    output["message"] = "Retrieved data successfully";
                    output["record_id"] = strID;

                    log.apilog_status = "200";
                    log.apilog_message = "";
                }
                else
                {
                    output["success"] = false;
                    output["message"] = "Retrieved data not successfully";

                    log.apilog_status = "500";
                    log.apilog_message = controller.getMessage();
                }

                controller.dispose();

            }
            catch (Exception ex)
            {
                output["success"] = false;
                output["message"] = "(C)Retrieved data not successfully";

                log.apilog_status = "500";
                log.apilog_message = ex.ToString();
            }
            finally
            {
                objBpcOpr.doRecordLog(log);
            }

            output["data"] = tmp;

            return output.ToString(Formatting.None);
        }
        public string doDeleteMTCompany(InputMTCompany input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "CPN001.3";
            log.apilog_by = input.modified_by;
            log.apilog_data = tmp.ToString();

            try
            {
                var authHeader = WebOperationContext.Current.IncomingRequest.Headers["Authorization"];
                if (authHeader == null || !objBpcOpr.doVerify(authHeader))
                {
                    output["success"] = false;
                    output["message"] = BpcOpr.MessageNotAuthen;
                    log.apilog_status = "500";
                    log.apilog_message = BpcOpr.MessageNotAuthen;
                    objBpcOpr.doRecordLog(log);

                    return output.ToString(Formatting.None);
                }

                cls_ctMTCompany controller = new cls_ctMTCompany();

                if (controller.checkDataOld(input.company_code))
                {
                    bool blnResult = controller.delete(input.company_id.ToString());

                    if (blnResult)
                    {
                        output["success"] = true;
                        output["message"] = "Remove data successfully";

                        log.apilog_status = "200";
                        log.apilog_message = "";
                    }
                    else
                    {
                        output["success"] = false;
                        output["message"] = "Remove data not successfully";

                        log.apilog_status = "500";
                        log.apilog_message = controller.getMessage();
                    }

                }
                else
                {
                    string message = "Not Found Project code : " + input.company_code;
                    output["success"] = false;
                    output["message"] = message;

                    log.apilog_status = "404";
                    log.apilog_message = message;
                }

                controller.dispose();
            }
            catch (Exception ex)
            {
                output["success"] = false;
                output["message"] = "(C)Remove data not successfully";

                log.apilog_status = "500";
                log.apilog_message = ex.ToString();
            }
            finally
            {
                objBpcOpr.doRecordLog(log);
            }

            output["data"] = tmp;

            return output.ToString(Formatting.None);

        }
        public async Task<string> doUploadMTCompany(string token, string by, string fileName, Stream stream)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "CPN001.4";
            log.apilog_by = by;
            log.apilog_data = "Stream";

            try
            {
                if (!objBpcOpr.doVerify(token))
                {
                    output["success"] = false;
                    output["message"] = BpcOpr.MessageNotAuthen;

                    log.apilog_status = "500";
                    log.apilog_message = BpcOpr.MessageNotAuthen;
                    objBpcOpr.doRecordLog(log);

                    return output.ToString(Formatting.None);
                }


                bool upload = await this.doUploadFile(fileName, stream);

                if (upload)
                {
                    cls_srvSystemImport srv_import = new cls_srvSystemImport(); 
                    string tmp = srv_import.doImportExcel("COMPANY", fileName, "TEST");

                    output["success"] = true;
                    output["message"] = tmp;

                    log.apilog_status = "200";
                    log.apilog_message = "";
                }
                else
                {
                    output["success"] = false;
                    output["message"] = "Upload data not successfully";

                    log.apilog_status = "500";
                    log.apilog_message = "Upload data not successfully";
                }

            }
            catch (Exception ex)
            {
                output["success"] = false;
                output["message"] = "(C)Upload data not successfully";

                log.apilog_status = "500";
                log.apilog_message = ex.ToString();
            }
            finally
            {
                objBpcOpr.doRecordLog(log);
            }

            return output.ToString(Formatting.None);
        }

        #endregion


        #region comBank
        public string getCombankList(FillterCompany req)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "CBK001.1";
            log.apilog_by = req.username;
            log.apilog_data = "all";

            try
            {
                var authHeader = WebOperationContext.Current.IncomingRequest.Headers["Authorization"];
                if (authHeader == null || !objBpcOpr.doVerify(authHeader))
                {
                    output["success"] = false;
                    output["message"] = BpcOpr.MessageNotAuthen;

                    log.apilog_status = "500";
                    log.apilog_message = BpcOpr.MessageNotAuthen;
                    objBpcOpr.doRecordLog(log);

                    return output.ToString(Formatting.None);
                }

                cls_ctMTCombank controller = new cls_ctMTCombank();
                List<cls_MTCombank> list = controller.getDataByFillter(req.company_code);
                JArray array = new JArray();

                if (list.Count > 0)
                {
                    int index = 1;

                    foreach (cls_MTCombank model in list)
                    {
                        JObject json = new JObject();
                        json.Add("company_code", model.combank_bankaccount);

                        json.Add("combank_id", model.combank_id);
                        json.Add("combank_bankcode", model.combank_bankcode);
                        json.Add("combank_bankaccount", model.combank_bankaccount);

                        json.Add("modified_by", model.modified_by);
                        json.Add("modified_date", model.modified_date);
                        json.Add("flag", model.flag);

                        json.Add("change", false);

                        json.Add("index", index);
                        array.Add(json);
                    }

                    output["success"] = true;
                    output["message"] = "";
                    output["data"] = array;

                    log.apilog_status = "200";
                    log.apilog_message = "";
                }
                else
                {
                    output["success"] = false;
                    output["message"] = "Data not Found";
                    output["data"] = array;

                    log.apilog_status = "404";
                    log.apilog_message = "Data not Found";
                }

                controller.dispose();
            }
            catch (Exception ex)
            {
                output["success"] = false;
                output["message"] = "(C)Retrieved data not successfully";

                log.apilog_status = "500";
                log.apilog_message = ex.ToString();
            }
            finally
            {
                objBpcOpr.doRecordLog(log);
            }

            return output.ToString(Formatting.None);
        }

        public string doManageMTCombank(InputComTransaction input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);


            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "CBK001.2";
            log.apilog_by = input.modified_by;
            log.apilog_data = tmp.ToString();

            try
            {
                var authHeader = WebOperationContext.Current.IncomingRequest.Headers["Authorization"];
                if (authHeader == null || !objBpcOpr.doVerify(authHeader))
                {
                    output["success"] = false;
                    output["message"] = BpcOpr.MessageNotAuthen;

                    log.apilog_status = "500";
                    log.apilog_message = BpcOpr.MessageNotAuthen;
                    objBpcOpr.doRecordLog(log);

                    return output.ToString(Formatting.None);
                }

                cls_ctMTCombank controller = new cls_ctMTCombank();

                JObject jsonObject = new JObject();
                var jsonArray = JsonConvert.DeserializeObject<List<cls_MTCombank>>(input.transaction_data);

                int success = 0;
                int error = 0;
                StringBuilder obj_error = new StringBuilder();

                bool clear = controller.delete(input.company_code);

                if (clear)
                {
                    foreach (cls_MTCombank model in jsonArray)
                    {

                        model.modified_by = input.modified_by;

                        bool blnResult = controller.insert(model);

                        if (blnResult)
                            success++;
                        else
                        {
                            var json = new JavaScriptSerializer().Serialize(model);
                            var tmp2 = JToken.Parse(json);
                            obj_error.Append(tmp2);
                        }

                    }
                }
                else
                {
                    error = 1;
                }


                if (error == 0)
                {
                    output["success"] = true;
                    output["message"] = "Retrieved data successfully";
                    //output["record_id"] = strID;

                    log.apilog_status = "200";
                    log.apilog_message = "";
                }
                else
                {

                    output["success"] = false;
                    output["message"] = "Retrieved data not successfully";

                    output["error"] = obj_error.ToString();

                    log.apilog_status = "500";
                    log.apilog_message = controller.getMessage();
                }


                controller.dispose();

            }
            catch (Exception ex)
            {
                output["success"] = false;
                output["message"] = "(C)Retrieved data not successfully";

                log.apilog_status = "500";
                log.apilog_message = ex.ToString();
            }
            finally
            {
                objBpcOpr.doRecordLog(log);
            }

            output["data"] = tmp;

            return output.ToString(Formatting.None);
        }

        public string doDeleteMTCombank(InputMTCombank input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "CBK001.3";
            log.apilog_by = input.modified_by;
            log.apilog_data = tmp.ToString();

            try
            {
                var authHeader = WebOperationContext.Current.IncomingRequest.Headers["Authorization"];
                if (authHeader == null || !objBpcOpr.doVerify(authHeader))
                {
                    output["success"] = false;
                    output["message"] = BpcOpr.MessageNotAuthen;
                    log.apilog_status = "500";
                    log.apilog_message = BpcOpr.MessageNotAuthen;
                    objBpcOpr.doRecordLog(log);

                    return output.ToString(Formatting.None);
                }

                cls_ctMTCombank controller = new cls_ctMTCombank();

                if (controller.checkDataOld(input.company_code, input.combank_bankcode))
                {
                    bool blnResult = controller.delete(input.company_code);

                    if (blnResult)
                    {
                        output["success"] = true;
                        output["message"] = "Remove data successfully";

                        log.apilog_status = "200";
                        log.apilog_message = "";
                    }
                    else
                    {
                        output["success"] = false;
                        output["message"] = "Remove data not successfully";

                        log.apilog_status = "500";
                        log.apilog_message = controller.getMessage();
                    }

                }
                else
                {
                    string message = "Not Found Project code : " + input.combank_id;
                    output["success"] = false;
                    output["message"] = message;

                    log.apilog_status = "404";
                    log.apilog_message = message;
                }

                controller.dispose();
            }
            catch (Exception ex)
            {
                output["success"] = false;
                output["message"] = "(C)Remove data not successfully";

                log.apilog_status = "500";
                log.apilog_message = ex.ToString();
            }
            finally
            {
                objBpcOpr.doRecordLog(log);
            }

            output["data"] = tmp;

            return output.ToString(Formatting.None);

        }

        public async Task<string> doUploadCombank(string token, string by, string fileName, Stream stream)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "CBK001.4";
            log.apilog_by = by;
            log.apilog_data = "Stream";

            try
            {
                if (!objBpcOpr.doVerify(token))
                {
                    output["success"] = false;
                    output["message"] = BpcOpr.MessageNotAuthen;

                    log.apilog_status = "500";
                    log.apilog_message = BpcOpr.MessageNotAuthen;
                    objBpcOpr.doRecordLog(log);

                    return output.ToString(Formatting.None);
                }


                bool upload = await this.doUploadFile(fileName, stream);

                if (upload)
                {
                    cls_srvSystemImport srv_import = new cls_srvSystemImport();
                    string tmp = srv_import.doImportExcel("COMBANK", fileName, "TEST");

                    output["success"] = true;
                    output["message"] = tmp;

                    log.apilog_status = "200";
                    log.apilog_message = "";
                }
                else
                {
                    output["success"] = false;
                    output["message"] = "Upload data not successfully";

                    log.apilog_status = "500";
                    log.apilog_message = "Upload data not successfully";
                }

            }
            catch (Exception ex)
            {
                output["success"] = false;
                output["message"] = "(C)Upload data not successfully";

                log.apilog_status = "500";
                log.apilog_message = ex.ToString();
            }
            finally
            {
                objBpcOpr.doRecordLog(log);
            }

            return output.ToString(Formatting.None);
        }
        #endregion


        //#region comBank
        //public string getCombankList(FillterCompany req)
        //{
        //    JObject output = new JObject();

        //    cls_SYSApilog log = new cls_SYSApilog();
        //    log.apilog_code = "CBK001.1";
        //    log.apilog_by = req.username;
        //    log.apilog_data = "all";

        //    try
        //    {
        //        var authHeader = WebOperationContext.Current.IncomingRequest.Headers["Authorization"];
        //        if (authHeader == null || !objBpcOpr.doVerify(authHeader))
        //        {
        //            output["success"] = false;
        //            output["message"] = BpcOpr.MessageNotAuthen;

        //            log.apilog_status = "500";
        //            log.apilog_message = BpcOpr.MessageNotAuthen;
        //            objBpcOpr.doRecordLog(log);

        //            return output.ToString(Formatting.None);
        //        }

        //        cls_ctMTCombank controller = new cls_ctMTCombank();
        //        List<cls_MTCombank> list = controller.getDataByFillter(req.company_code);
        //        JArray array = new JArray();

        //        if (list.Count > 0)
        //        {
        //            int index = 1;

        //            foreach (cls_MTCombank model in list)
        //            {
        //                JObject json = new JObject();
        //                json.Add("company_code", model.combank_bankaccount);

        //                json.Add("combank_id", model.combank_id);
        //                json.Add("combank_bankcode", model.combank_bankcode);
        //                json.Add("combank_bankaccount", model.combank_bankaccount);

        //                json.Add("modified_by", model.modified_by);
        //                json.Add("modified_date", model.modified_date);
        //                json.Add("flag", model.flag);

        //                json.Add("change", false);

        //                json.Add("index", index);

        //                index++;

        //                array.Add(json);
        //            }

        //            output["success"] = true;
        //            output["message"] = "";
        //            output["data"] = array;

        //            log.apilog_status = "200";
        //            log.apilog_message = "";
        //        }
        //        else
        //        {
        //            output["success"] = false;
        //            output["message"] = "Data not Found";
        //            output["data"] = array;

        //            log.apilog_status = "404";
        //            log.apilog_message = "Data not Found";
        //        }

        //        controller.dispose();
        //    }
        //    catch (Exception ex)
        //    {
        //        output["success"] = false;
        //        output["message"] = "(C)Retrieved data not successfully";

        //        log.apilog_status = "500";
        //        log.apilog_message = ex.ToString();
        //    }
        //    finally
        //    {
        //        objBpcOpr.doRecordLog(log);
        //    }

        //    return output.ToString(Formatting.None);
        //}

        //public string doManageMTCombank(InputComTransaction input)
        //{
        //    JObject output = new JObject();

        //    var json_data = new JavaScriptSerializer().Serialize(input);
        //    var tmp = JToken.Parse(json_data);


        //    cls_SYSApilog log = new cls_SYSApilog();
        //    log.apilog_code = "CBK001.2";
        //    log.apilog_by = input.modified_by;
        //    log.apilog_data = tmp.ToString();

        //    try
        //    {
        //        var authHeader = WebOperationContext.Current.IncomingRequest.Headers["Authorization"];
        //        if (authHeader == null || !objBpcOpr.doVerify(authHeader))
        //        {
        //            output["success"] = false;
        //            output["message"] = BpcOpr.MessageNotAuthen;

        //            log.apilog_status = "500";
        //            log.apilog_message = BpcOpr.MessageNotAuthen;
        //            objBpcOpr.doRecordLog(log);

        //            return output.ToString(Formatting.None);
        //        }

        //        cls_ctMTCombank controller = new cls_ctMTCombank();

        //        JObject jsonObject = new JObject();
        //        var jsonArray = JsonConvert.DeserializeObject<List<cls_MTCombank>>(input.transaction_data);

        //        int success = 0;
        //        int error = 0;
        //        StringBuilder obj_error = new StringBuilder();

        //        bool clear = controller.delete(input.company_code);

        //        if (clear)
        //        {
        //            foreach (cls_MTCombank model in jsonArray)
        //            {

        //                model.modified_by = input.modified_by;

        //                bool blnResult = controller.insert(model);

        //                if (blnResult)
        //                    success++;
        //                else
        //                {
        //                    var json = new JavaScriptSerializer().Serialize(model);
        //                    var tmp2 = JToken.Parse(json);
        //                    obj_error.Append(tmp2);
        //                }

        //            }
        //        }
        //        else
        //        {
        //            error = 1;
        //        }


        //        if (error == 0)
        //        {
        //            output["success"] = true;
        //            output["message"] = "Retrieved data successfully";
        //            //output["record_id"] = strID;

        //            log.apilog_status = "200";
        //            log.apilog_message = "";
        //        }
        //        else
        //        {

        //            output["success"] = false;
        //            output["message"] = "Retrieved data not successfully";

        //            output["error"] = obj_error.ToString();

        //            log.apilog_status = "500";
        //            log.apilog_message = controller.getMessage();
        //        }


        //        controller.dispose();

        //    }
        //    catch (Exception ex)
        //    {
        //        output["success"] = false;
        //        output["message"] = "(C)Retrieved data not successfully";

        //        log.apilog_status = "500";
        //        log.apilog_message = ex.ToString();
        //    }
        //    finally
        //    {
        //        objBpcOpr.doRecordLog(log);
        //    }

        //    output["data"] = tmp;

        //    return output.ToString(Formatting.None);
        //}

        //public string doDeleteMTCombank(InputMTCombank input)
        //{
        //    JObject output = new JObject();

        //    var json_data = new JavaScriptSerializer().Serialize(input);
        //    var tmp = JToken.Parse(json_data);

        //    cls_SYSApilog log = new cls_SYSApilog();
        //    log.apilog_code = "CBK001.3";
        //    log.apilog_by = input.modified_by;
        //    log.apilog_data = tmp.ToString();

        //    try
        //    {
        //        var authHeader = WebOperationContext.Current.IncomingRequest.Headers["Authorization"];
        //        if (authHeader == null || !objBpcOpr.doVerify(authHeader))
        //        {
        //            output["success"] = false;
        //            output["message"] = BpcOpr.MessageNotAuthen;
        //            log.apilog_status = "500";
        //            log.apilog_message = BpcOpr.MessageNotAuthen;
        //            objBpcOpr.doRecordLog(log);

        //            return output.ToString(Formatting.None);
        //        }

        //        cls_ctMTCombank controller = new cls_ctMTCombank();

        //        if (controller.checkDataOld(input.company_code, input.worker_code))
        //        {
        //            bool blnResult = controller.delete(input.company_code);

        //            if (blnResult)
        //            {
        //                output["success"] = true;
        //                output["message"] = "Remove data successfully";

        //                log.apilog_status = "200";
        //                log.apilog_message = "";
        //            }
        //            else
        //            {
        //                output["success"] = false;
        //                output["message"] = "Remove data not successfully";

        //                log.apilog_status = "500";
        //                log.apilog_message = controller.getMessage();
        //            }

        //        }
        //        else
        //        {
        //            string message = "Not Found Project code : " + input.combank_id;
        //            output["success"] = false;
        //            output["message"] = message;

        //            log.apilog_status = "404";
        //            log.apilog_message = message;
        //        }

        //        controller.dispose();
        //    }
        //    catch (Exception ex)
        //    {
        //        output["success"] = false;
        //        output["message"] = "(C)Remove data not successfully";

        //        log.apilog_status = "500";
        //        log.apilog_message = ex.ToString();
        //    }
        //    finally
        //    {
        //        objBpcOpr.doRecordLog(log);
        //    }

        //    output["data"] = tmp;

        //    return output.ToString(Formatting.None);

        //}

        //public async Task<string> doUploadCombank(string token, string by, string fileName, Stream stream)
        //{
        //    JObject output = new JObject();

        //    cls_SYSApilog log = new cls_SYSApilog();
        //    log.apilog_code = "CBK001.4";
        //    log.apilog_by = by;
        //    log.apilog_data = "Stream";

        //    try
        //    {
        //        if (!objBpcOpr.doVerify(token))
        //        {
        //            output["success"] = false;
        //            output["message"] = BpcOpr.MessageNotAuthen;

        //            log.apilog_status = "500";
        //            log.apilog_message = BpcOpr.MessageNotAuthen;
        //            objBpcOpr.doRecordLog(log);

        //            return output.ToString(Formatting.None);
        //        }


        //        bool upload = await this.doUploadFile(fileName, stream);

        //        if (upload)
        //        {
        //            cls_srvSystemImport srv_import = new cls_srvSystemImport();
        //            string tmp = srv_import.doImportExcel("COMBANK", fileName, "TEST");

        //            output["success"] = true;
        //            output["message"] = tmp;

        //            log.apilog_status = "200";
        //            log.apilog_message = "";
        //        }
        //        else
        //        {
        //            output["success"] = false;
        //            output["message"] = "Upload data not successfully";

        //            log.apilog_status = "500";
        //            log.apilog_message = "Upload data not successfully";
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        output["success"] = false;
        //        output["message"] = "(C)Upload data not successfully";

        //        log.apilog_status = "500";
        //        log.apilog_message = ex.ToString();
        //    }
        //    finally
        //    {
        //        objBpcOpr.doRecordLog(log);
        //    }

        //    return output.ToString(Formatting.None);
        //}
        //#endregion

        #region MTcomcard
        public string getComcardList (FillterCompany req)

        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "CDD001.1";
            log.apilog_by = req.username;
            log.apilog_data = "all";

            try
            {
                var authHeader = WebOperationContext.Current.IncomingRequest.Headers["Authorization"];
                if (authHeader == null || !objBpcOpr.doVerify(authHeader))
                {
                    output["success"] = false;
                    output["message"] = BpcOpr.MessageNotAuthen;

                    log.apilog_status = "500";
                    log.apilog_message = BpcOpr.MessageNotAuthen;
                    objBpcOpr.doRecordLog(log);

                    return output.ToString(Formatting.None);
                }

                cls_ctMTComcard objComcard = new cls_ctMTComcard();
                List<cls_MTComcard> list = objComcard.getDataByFillter(req.company_code, req.card_type, req.comcard_id,  req.comcard_code,req.combranch_code);
                JArray array = new JArray();

                if (list.Count > 0)
                {
                    int index = 1;

                    foreach (cls_MTComcard model in list)
                    {
                        JObject json = new JObject();
                        json.Add("comcard_id", model.comcard_id);
                        json.Add("card_type", model.card_type);

                        json.Add("comcard_code", model.comcard_code);

                        json.Add("comcard_issue", model.comcard_issue);
                        json.Add("comcard_expire", model.comcard_expire);

                        json.Add("company_code", model.company_code);
                        json.Add("combranch_code", model.combranch_code);
                        json.Add("change", false);

                        json.Add("modified_by", model.modified_by);
                        json.Add("modified_date", model.modified_date);
                        json.Add("flag", model.flag);

                        json.Add("index", index);
                        array.Add(json);
                    }

                    output["success"] = true;
                    output["message"] = "";
                    output["data"] = array;

                    log.apilog_status = "200";
                    log.apilog_message = "";
                }
                else
                {
                    output["success"] = false;
                    output["message"] = "Data not Found";
                    output["data"] = array;

                    log.apilog_status = "404";
                    log.apilog_message = "Data not Found";
                }

                objComcard.dispose();
            }
            catch (Exception ex)
            {
                output["success"] = false;
                output["message"] = "(C)Retrieved data not successfully";

                log.apilog_status = "500";
                log.apilog_message = ex.ToString();
            }
            finally
            {
                objBpcOpr.doRecordLog(log);
            }

            return output.ToString(Formatting.None);
        }
        public string doManageMTComcard(InputComTransaction input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);


            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "CDD001.2";
            log.apilog_by = input.modified_by;
            log.apilog_data = tmp.ToString();

            try
            {
                var authHeader = WebOperationContext.Current.IncomingRequest.Headers["Authorization"];
                if (authHeader == null || !objBpcOpr.doVerify(authHeader))
                {
                    output["success"] = false;
                    output["message"] = BpcOpr.MessageNotAuthen;

                    log.apilog_status = "500";
                    log.apilog_message = BpcOpr.MessageNotAuthen;
                    objBpcOpr.doRecordLog(log);

                    return output.ToString(Formatting.None);
                }

                cls_ctMTComcard controller = new cls_ctMTComcard();

                JObject jsonObject = new JObject();
                var jsonArray = JsonConvert.DeserializeObject<List<cls_MTComcard>>(input.transaction_data);

                int success = 0;
                int error = 0;
                StringBuilder obj_error = new StringBuilder();

                bool clear = controller.delete(input.comcard_id);

                if (clear)
                {
                    foreach (cls_MTComcard model in jsonArray)
                    {

                        model.modified_by = input.modified_by;

                        bool blnResult = controller.insert(model);

                        if (blnResult)
                            success++;
                        else
                        {
                            var json = new JavaScriptSerializer().Serialize(model);
                            var tmp2 = JToken.Parse(json);
                            obj_error.Append(tmp2);
                        }

                    }
                }
                else
                {
                    error = 1;
                }


                if (error == 0)
                {
                    output["success"] = true;
                    output["message"] = "Retrieved data successfully";
                    //output["record_id"] = strID;

                    log.apilog_status = "200";
                    log.apilog_message = "";
                }
                else
                {

                    output["success"] = false;
                    output["message"] = "Retrieved data not successfully";

                    output["error"] = obj_error.ToString();

                    log.apilog_status = "500";
                    log.apilog_message = controller.getMessage();
                }

                controller.dispose();

            }
            catch (Exception ex)
            {
                output["success"] = false;
                output["message"] = "(C)Retrieved data not successfully";

                log.apilog_status = "500";
                log.apilog_message = ex.ToString();
            }
            finally
            {
                objBpcOpr.doRecordLog(log);
            }

            output["data"] = tmp;

            return output.ToString(Formatting.None);
        }
        public string doDeleteMTComcard(InputMTComcard input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "CDD001.3";
            log.apilog_by = input.modified_by;
            log.apilog_data = tmp.ToString();

            try
            {
                var authHeader = WebOperationContext.Current.IncomingRequest.Headers["Authorization"];
                if (authHeader == null || !objBpcOpr.doVerify(authHeader))
                {
                    output["success"] = false;
                    output["message"] = BpcOpr.MessageNotAuthen;
                    log.apilog_status = "500";
                    log.apilog_message = BpcOpr.MessageNotAuthen;
                    objBpcOpr.doRecordLog(log);

                    return output.ToString(Formatting.None);
                }

                cls_ctMTComcard objComcard = new cls_ctMTComcard();

                if (objComcard.checkDataOld(input.company_code,input.card_type,input.comcard_code,input.combranch_code))
                {
                    bool blnResult = objComcard.delete(input.comcard_id.ToString());

                    if (blnResult)
                    {
                        output["success"] = true;
                        output["message"] = "Remove data successfully";

                        log.apilog_status = "200";
                        log.apilog_message = "";
                    }
                    else
                    {
                        output["success"] = false;
                        output["message"] = "Remove data not successfully";

                        log.apilog_status = "500";
                        log.apilog_message = objComcard.getMessage();
                    }

                }
                else
                {
                    string message = "Not Found Project code : " + input.company_code;
                    output["success"] = false;
                    output["message"] = message;

                    log.apilog_status = "404";
                    log.apilog_message = message;
                }

                objComcard.dispose();
            }
            catch (Exception ex)
            {
                output["success"] = false;
                output["message"] = "(C)Remove data not successfully";

                log.apilog_status = "500";
                log.apilog_message = ex.ToString();
            }
            finally
            {
                objBpcOpr.doRecordLog(log);
            }

            output["data"] = tmp;

            return output.ToString(Formatting.None);

        }
        public async Task<string> doUploadComcard(string token, string by, string fileName, Stream stream)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "CDD001.4";
            log.apilog_by = by;
            log.apilog_data = "Stream";

            try
            {
                if (!objBpcOpr.doVerify(token))
                {
                    output["success"] = false;
                    output["message"] = BpcOpr.MessageNotAuthen;

                    log.apilog_status = "500";
                    log.apilog_message = BpcOpr.MessageNotAuthen;
                    objBpcOpr.doRecordLog(log);

                    return output.ToString(Formatting.None);
                }


                bool upload = await this.doUploadFile(fileName, stream);

                if (upload)
                {
                    cls_srvSystemImport srv_import = new cls_srvSystemImport();
                    string tmp = srv_import.doImportExcel("Comcard", fileName, "TEST");

                    output["success"] = true;
                    output["message"] = tmp;

                    log.apilog_status = "200";
                    log.apilog_message = "";
                }
                else
                {
                    output["success"] = false;
                    output["message"] = "Upload data not successfully";

                    log.apilog_status = "500";
                    log.apilog_message = "Upload data not successfully";
                }

            }
            catch (Exception ex)
            {
                output["success"] = false;
                output["message"] = "(C)Upload data not successfully";

                log.apilog_status = "500";
                log.apilog_message = ex.ToString();
            }
            finally
            {
                objBpcOpr.doRecordLog(log);
            }

            return output.ToString(Formatting.None);
        }
        #endregion


        

        #region MTcomAddress
        public string getComAddressList(FillterCompany req)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "CAD001.1";
            log.apilog_by = req.username;
            log.apilog_data = "all";

            try
            {
                var authHeader = WebOperationContext.Current.IncomingRequest.Headers["Authorization"];
                if (authHeader == null || !objBpcOpr.doVerify(authHeader))
                {
                    output["success"] = false;
                    output["message"] = BpcOpr.MessageNotAuthen;

                    log.apilog_status = "500";
                    log.apilog_message = BpcOpr.MessageNotAuthen;
                    objBpcOpr.doRecordLog(log);

                    return output.ToString(Formatting.None);
                }

                cls_ctMTComaddress contcomaddress = new cls_ctMTComaddress();
                List<cls_MTComaddress> list = contcomaddress.getDataByFillter(req.company_code , req.combranch_code, req.comaddress_type);
                JArray array = new JArray();

                if (list.Count > 0)
                {
                    int index = 1;

                    foreach (cls_MTComaddress model in list)
                    {
                        JObject json = new JObject();
                        json.Add("comaddress_type", model.comaddress_type);
                        json.Add("comaddress_no", model.comaddress_no);
                        json.Add("comaddress_moo", model.comaddress_moo);
                        json.Add("comaddress_soi", model.comaddress_soi);
                        json.Add("comaddress_road", model.comaddress_road);
                        json.Add("comaddress_tambon", model.comaddress_tambon);
                        json.Add("comaddress_amphur", model.comaddress_amphur);
                        json.Add("comaddress_zipcode", model.comaddress_zipcode);
                        json.Add("comaddress_tel", model.comaddress_tel);
                        json.Add("comaddress_email", model.comaddress_email);
                        json.Add("comaddress_line", model.comaddress_line);
                        json.Add("comaddress_facebook", model.comaddress_facebook);
                        json.Add("province_code", model.province_code);

                        json.Add("company_code", model.company_code);
                        json.Add("combranch_code", model.combranch_code);

                        json.Add("modified_by", model.modified_by);
                        json.Add("modified_date", model.modified_date);
                        json.Add("flag", model.flag);
                        json.Add("change", false);
                        json.Add("index", index);
                        index++;
                        array.Add(json);
                    }

                    output["success"] = true;
                    output["message"] = "";
                    output["data"] = array;

                    log.apilog_status = "200";
                    log.apilog_message = "";
                }
                else
                {
                    output["success"] = false;
                    output["message"] = "Data not Found";
                    output["data"] = array;

                    log.apilog_status = "404";
                    log.apilog_message = "Data not Found";
                }

                contcomaddress.dispose();
            }
            catch (Exception ex)
            {
                output["success"] = false;
                output["message"] = "(C)Retrieved data not successfully";

                log.apilog_status = "500";
                log.apilog_message = ex.ToString();
            }
            finally
            {
                objBpcOpr.doRecordLog(log);
            }

            return output.ToString(Formatting.None);
        }
        public string doManageMTComAddress(InputComTransaction input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);


            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "CAD001.2";
            log.apilog_by = input.modified_by;
            log.apilog_data = tmp.ToString();

            try
            {
                var authHeader = WebOperationContext.Current.IncomingRequest.Headers["Authorization"];
                if (authHeader == null || !objBpcOpr.doVerify(authHeader))
                {
                    output["success"] = false;
                    output["message"] = BpcOpr.MessageNotAuthen;

                    log.apilog_status = "500";
                    log.apilog_message = BpcOpr.MessageNotAuthen;
                    objBpcOpr.doRecordLog(log);

                    return output.ToString(Formatting.None);
                }

                cls_ctMTComaddress controller = new cls_ctMTComaddress();

                JObject jsonObject = new JObject();
                var jsonArray = JsonConvert.DeserializeObject<List<cls_MTComaddress>>(input.transaction_data);

                int success = 0;
                int error = 0;
                StringBuilder obj_error = new StringBuilder();

                //bool clear = controller.delete(input.company_code, input.worker_code);
                bool clear = controller.delete(input.company_code, input.combranch_code, input.comaddress_type);

                if (clear)
                {
                    foreach (cls_MTComaddress model in jsonArray)
                    {

                        model.modified_by = input.modified_by;

                        bool blnResult = controller.insert(model);

                        if (blnResult)
                            success++;
                        else
                        {
                            var json = new JavaScriptSerializer().Serialize(model);
                            var tmp2 = JToken.Parse(json);
                            obj_error.Append(tmp2);
                        }

                    }
                }
                else
                {
                    error = 1;
                }


                if (error == 0)
                {
                    output["success"] = true;
                    output["message"] = "Retrieved data successfully";
                    //output["record_id"] = strID;

                    log.apilog_status = "200";
                    log.apilog_message = "";
                }
                else
                {

                    output["success"] = false;
                    output["message"] = "Retrieved data not successfully";

                    output["error"] = obj_error.ToString();

                    log.apilog_status = "500";
                    log.apilog_message = controller.getMessage();
                }

                controller.dispose();

            }
            catch (Exception ex)
            {
                output["success"] = false;
                output["message"] = "(C)Retrieved data not successfully";

                log.apilog_status = "500";
                log.apilog_message = ex.ToString();
            }
            finally
            {
                objBpcOpr.doRecordLog(log);
            }

            output["data"] = tmp;

            return output.ToString(Formatting.None);
        }
        public string doDeleteMTComAddress(InputMTComaddress input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "CAD001.3";
            log.apilog_by = input.modified_by;
            log.apilog_data = tmp.ToString();

            try
            {
                var authHeader = WebOperationContext.Current.IncomingRequest.Headers["Authorization"];
                if (authHeader == null || !objBpcOpr.doVerify(authHeader))
                {
                    output["success"] = false;
                    output["message"] = BpcOpr.MessageNotAuthen;
                    log.apilog_status = "500";
                    log.apilog_message = BpcOpr.MessageNotAuthen;
                    objBpcOpr.doRecordLog(log);

                    return output.ToString(Formatting.None);
                }

                cls_ctMTComaddress controller = new cls_ctMTComaddress();

                if (controller.checkDataOld(input.company_code, input.combranch_code, input.comaddress_type))
                {
                    bool blnResult = controller.delete(input.company_code, input.combranch_code, input.comaddress_type);

                    if (blnResult)
                    {
                        output["success"] = true;
                        output["message"] = "Remove data successfully";

                        log.apilog_status = "200";
                        log.apilog_message = "";
                    }
                    else
                    {
                        output["success"] = false;
                        output["message"] = "Remove data not successfully";

                        log.apilog_status = "500";
                        log.apilog_message = controller.getMessage();
                    }

                }
                else
                {
                    string message = "Not Found Project code : " + input.comaddress_type;
                    output["success"] = false;
                    output["message"] = message;

                    log.apilog_status = "404";
                    log.apilog_message = message;
                }

                controller.dispose();
            }
            catch (Exception ex)
            {
                output["success"] = false;
                output["message"] = "(C)Remove data not successfully";

                log.apilog_status = "500";
                log.apilog_message = ex.ToString();
            }
            finally
            {
                objBpcOpr.doRecordLog(log);
            }

            output["data"] = tmp;

            return output.ToString(Formatting.None);

        }
        public async Task<string> doUploadMTComAddress(string token, string by, string fileName, Stream stream)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "CAD001.4";
            log.apilog_by = by;
            log.apilog_data = "Stream";

            try
            {
                if (!objBpcOpr.doVerify(token))
                {
                    output["success"] = false;
                    output["message"] = BpcOpr.MessageNotAuthen;

                    log.apilog_status = "500";
                    log.apilog_message = BpcOpr.MessageNotAuthen;
                    objBpcOpr.doRecordLog(log);

                    return output.ToString(Formatting.None);
                }


                bool upload = await this.doUploadFile(fileName, stream);

                if (upload)
                {
                    cls_srvSystemImport srv_import = new cls_srvSystemImport();
                    string tmp = srv_import.doImportExcel("COMADDRESS", fileName, "TEST");

                    output["success"] = true;
                    output["message"] = tmp;

                    log.apilog_status = "200";
                    log.apilog_message = "";
                }
                else
                {
                    output["success"] = false;
                    output["message"] = "Upload data not successfully";

                    log.apilog_status = "500";
                    log.apilog_message = "Upload data not successfully";
                }

            }
            catch (Exception ex)
            {
                output["success"] = false;
                output["message"] = "(C)Upload data not successfully";

                log.apilog_status = "500";
                log.apilog_message = ex.ToString();
            }
            finally
            {
                objBpcOpr.doRecordLog(log);
            }

            return output.ToString(Formatting.None);
        }

        #endregion

        #region MTAddresstype
        public string getAddresstypeList(BasicRequest req)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "ADD001.1";
            log.apilog_by = req.username;
            log.apilog_data = "all";

            try
            {
                var authHeader = WebOperationContext.Current.IncomingRequest.Headers["Authorization"];
                if (authHeader == null || !objBpcOpr.doVerify(authHeader))
                {
                    output["success"] = false;
                    output["message"] = BpcOpr.MessageNotAuthen;

                    log.apilog_status = "500";
                    log.apilog_message = BpcOpr.MessageNotAuthen;
                    objBpcOpr.doRecordLog(log);

                    return output.ToString(Formatting.None);
                }

                cls_ctMTAddresstype contaddresstype = new cls_ctMTAddresstype();
                List<cls_MTAddresstype> list = contaddresstype.getDataByFillter("");
                JArray array = new JArray();

                if (list.Count > 0)
                {
                    int index = 1;

                    foreach (cls_MTAddresstype model in list)
                    {
                        JObject json = new JObject();
                        json.Add("addresstype_id", model.addresstype_id);
                        json.Add("addresstype_code", model.addresstype_code);
                        json.Add("addresstype_name_th", model.addresstype_name_th);
                        json.Add("addresstype_name_en", model.addresstype_name_en);
                        json.Add("modified_by", model.modified_by);
                        json.Add("modified_date", model.modified_date);
                        json.Add("index", index++);
                        array.Add(json);
                    }

                    output["success"] = true;
                    output["message"] = "";
                    output["data"] = array;

                    log.apilog_status = "200";
                    log.apilog_message = "";
                }
                else
                {
                    output["success"] = false;
                    output["message"] = "Data not Found";
                    output["data"] = array;

                    log.apilog_status = "404";
                    log.apilog_message = "Data not Found";
                }

                contaddresstype.dispose();
            }
            catch (Exception ex)
            {
                output["success"] = false;
                output["message"] = "(C)Retrieved data not successfully";

                log.apilog_status = "500";
                log.apilog_message = ex.ToString();
            }
            finally
            {
                objBpcOpr.doRecordLog(log);
            }

            return output.ToString(Formatting.None);
        }
        public string doManageMTAddresstype(InputMTAddresstype input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);


            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "ADD001.2";
            log.apilog_by = input.modified_by;
            log.apilog_data = tmp.ToString();

            try
            {
                var authHeader = WebOperationContext.Current.IncomingRequest.Headers["Authorization"];
                if (authHeader == null || !objBpcOpr.doVerify(authHeader))
                {
                    output["success"] = false;
                    output["message"] = BpcOpr.MessageNotAuthen;

                    log.apilog_status = "500";
                    log.apilog_message = BpcOpr.MessageNotAuthen;
                    objBpcOpr.doRecordLog(log);

                    return output.ToString(Formatting.None);
                }

                cls_ctMTAddresstype controller = new cls_ctMTAddresstype();
                cls_MTAddresstype model = new cls_MTAddresstype();

                model.addresstype_id = Convert.ToInt32(input.addresstype_id);
                model.addresstype_code = input.addresstype_code;
                model.addresstype_name_th = input.addresstype_name_th;
                model.addresstype_name_en = input.addresstype_name_en;
                model.modified_by = input.modified_by;

                string strID = controller.insert(model);

                if (!strID.Equals(""))
                {
                    output["success"] = true;
                    output["message"] = "Retrieved data successfully";
                    output["record_id"] = strID;

                    log.apilog_status = "200";
                    log.apilog_message = "";
                }
                else
                {
                    output["success"] = false;
                    output["message"] = "Retrieved data not successfully";

                    log.apilog_status = "500";
                    log.apilog_message = controller.getMessage();
                }

                controller.dispose();

            }
            catch (Exception ex)
            {
                output["success"] = false;
                output["message"] = "(C)Retrieved data not successfully";

                log.apilog_status = "500";
                log.apilog_message = ex.ToString();
            }
            finally
            {
                objBpcOpr.doRecordLog(log);
            }

            output["data"] = tmp;

            return output.ToString(Formatting.None);
        }
        public string doDeleteMTAddresstype(InputMTAddresstype input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "ADD001.3";
            log.apilog_by = input.modified_by;
            log.apilog_data = tmp.ToString();

            try
            {
                var authHeader = WebOperationContext.Current.IncomingRequest.Headers["Authorization"];
                if (authHeader == null || !objBpcOpr.doVerify(authHeader))
                {
                    output["success"] = false;
                    output["message"] = BpcOpr.MessageNotAuthen;
                    log.apilog_status = "500";
                    log.apilog_message = BpcOpr.MessageNotAuthen;
                    objBpcOpr.doRecordLog(log);

                    return output.ToString(Formatting.None);
                }

                cls_ctMTAddresstype controller = new cls_ctMTAddresstype();

                if (controller.checkDataOld(input.addresstype_code))
                {
                    bool blnResult = controller.delete(input.addresstype_code);

                    if (blnResult)
                    {
                        output["success"] = true;
                        output["message"] = "Remove data successfully";

                        log.apilog_status = "200";
                        log.apilog_message = "";
                    }
                    else
                    {
                        output["success"] = false;
                        output["message"] = "Remove data not successfully";

                        log.apilog_status = "500";
                        log.apilog_message = controller.getMessage();
                    }

                }
                else
                {
                    string message = "Not Found Project code : " + input.addresstype_code;
                    output["success"] = false;
                    output["message"] = message;

                    log.apilog_status = "404";
                    log.apilog_message = message;
                }

                controller.dispose();
            }
            catch (Exception ex)
            {
                output["success"] = false;
                output["message"] = "(C)Remove data not successfully";

                log.apilog_status = "500";
                log.apilog_message = ex.ToString();
            }
            finally
            {
                objBpcOpr.doRecordLog(log);
            }

            output["data"] = tmp;

            return output.ToString(Formatting.None);

        }
        public async Task<string> doUploadMTAddresstype(string token, string by, string fileName, Stream stream)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "ADD001.4";
            log.apilog_by = by;
            log.apilog_data = "Stream";

            try
            {
                if (!objBpcOpr.doVerify(token))
                {
                    output["success"] = false;
                    output["message"] = BpcOpr.MessageNotAuthen;

                    log.apilog_status = "500";
                    log.apilog_message = BpcOpr.MessageNotAuthen;
                    objBpcOpr.doRecordLog(log);

                    return output.ToString(Formatting.None);
                }


                bool upload = await this.doUploadFile(fileName, stream);

                if (upload)
                {
                    cls_srvSystemImport srv_import = new cls_srvSystemImport();
                    string tmp = srv_import.doImportExcel("ADDRESSTYPE", fileName, "TEST");

                    output["success"] = true;
                    output["message"] = tmp;

                    log.apilog_status = "200";
                    log.apilog_message = "";
                }
                else
                {
                    output["success"] = false;
                    output["message"] = "Upload data not successfully";

                    log.apilog_status = "500";
                    log.apilog_message = "Upload data not successfully";
                }

            }
            catch (Exception ex)
            {
                output["success"] = false;
                output["message"] = "(C)Upload data not successfully";

                log.apilog_status = "500";
                log.apilog_message = ex.ToString();
            }
            finally
            {
                objBpcOpr.doRecordLog(log);
            }

            return output.ToString(Formatting.None);
        }
    

        #endregion

        #region MTRequest
        public string getRequestList(BasicRequest req)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "REQ001.1";
            log.apilog_by = req.username;
            log.apilog_data = "all";

            try
            {
                var authHeader = WebOperationContext.Current.IncomingRequest.Headers["Authorization"];
                if (authHeader == null || !objBpcOpr.doVerify(authHeader))
                {
                    output["success"] = false;
                    output["message"] = BpcOpr.MessageNotAuthen;

                    log.apilog_status = "500";
                    log.apilog_message = BpcOpr.MessageNotAuthen;
                    objBpcOpr.doRecordLog(log);

                    return output.ToString(Formatting.None);
                }

                cls_ctMTRequest contrequest = new cls_ctMTRequest();
                List<cls_MTRequest> list = contrequest.getDataByFillter("", "");
                JArray array = new JArray();

                if (list.Count > 0)
                {
                    int index = 1;

                    foreach (cls_MTRequest model in list)
                    {
                        JObject json = new JObject();
                        json.Add("request_id", model.request_id);
                        json.Add("request_code", model.request_code);
                        json.Add("request_date", model.request_date);
                        json.Add("request_agency", model.request_agency);
                        json.Add("request_work", model.request_work);
                        json.Add("request_job_type", model.request_job_type);
                        json.Add("request_employee_type", model.request_employee_type);
                        json.Add("request_quantity", model.request_quantity);
                        json.Add("request_urgency", model.request_urgency);
                        json.Add("request_wage_rate", model.request_wage_rate);
                        json.Add("request_overtime", model.request_overtime);
                        json.Add("request_another", model.request_another);

                        //json.Add("created_by", model.modified_by);
                        //json.Add("created_date", model.modified_date);

                        json.Add("modified_by", model.modified_by);
                        json.Add("modified_date", model.modified_date);
                        json.Add("index", index++);
                        array.Add(json);
                    }

                    output["success"] = true;
                    output["message"] = "";
                    output["data"] = array;

                    log.apilog_status = "200";
                    log.apilog_message = "";
                }
                else
                {
                    output["success"] = false;
                    output["message"] = "Data not Found";
                    output["data"] = array;

                    log.apilog_status = "404";
                    log.apilog_message = "Data not Found";
                }

                contrequest.dispose();
            }
            catch (Exception ex)
            {
                output["success"] = false;
                output["message"] = "(C)Retrieved data not successfully";

                log.apilog_status = "500";
                log.apilog_message = ex.ToString();
            }
            finally
            {
                objBpcOpr.doRecordLog(log);
            }

            return output.ToString(Formatting.None);
        }
        public string doManageMTRequest(InputMTRequest input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);


            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "REQ001.2";
            log.apilog_by = input.modified_by;
            log.apilog_data = tmp.ToString();

            try
            {
                var authHeader = WebOperationContext.Current.IncomingRequest.Headers["Authorization"];
                if (authHeader == null || !objBpcOpr.doVerify(authHeader))
                {
                    output["success"] = false;
                    output["message"] = BpcOpr.MessageNotAuthen;

                    log.apilog_status = "500";
                    log.apilog_message = BpcOpr.MessageNotAuthen;
                    objBpcOpr.doRecordLog(log);

                    return output.ToString(Formatting.None);
                }

                cls_ctMTRequest controller = new cls_ctMTRequest();
                cls_MTRequest model = new cls_MTRequest();

                model.request_id = Convert.ToInt32(input.request_id);
                model.request_code = input.request_code;
                model.request_date = Convert.ToDateTime(input.request_date);
                model.request_agency = input.request_agency;
                model.request_work = input.request_work;
                model.request_job_type = input.request_job_type;
                model.request_employee_type = input.request_employee_type;
                model.request_quantity = input.request_quantity;
                model.request_urgency = input.request_urgency;
                model.request_wage_rate = input.request_wage_rate;
                model.request_overtime = input.request_overtime;
                model.request_another = input.request_another;

                //model.created_by = input.created_by;
                //model.created_date = Convert.ToDateTime(input.created_date);

                model.modified_by = input.modified_by;
                string strID = controller.insert(model);

                if (!strID.Equals(""))
                {
                    output["success"] = true;
                    output["message"] = "Retrieved data successfully";
                    output["record_id"] = strID;

                    log.apilog_status = "200";
                    log.apilog_message = "";
                }
                else
                {
                    output["success"] = false;
                    output["message"] = "Retrieved data not successfully";

                    log.apilog_status = "500";
                    log.apilog_message = controller.getMessage();
                }

                controller.dispose();

            }
            catch (Exception ex)
            {
                output["success"] = false;
                output["message"] = "(C)Retrieved data not successfully";

                log.apilog_status = "500";
                log.apilog_message = ex.ToString();
            }
            finally
            {
                objBpcOpr.doRecordLog(log);
            }

            output["data"] = tmp;

            return output.ToString(Formatting.None);
        }
        public string doDeleteMTRequest(InputMTRequest input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "REQ001.3";
            log.apilog_by = input.modified_by;
            log.apilog_data = tmp.ToString();

            try
            {
                var authHeader = WebOperationContext.Current.IncomingRequest.Headers["Authorization"];
                if (authHeader == null || !objBpcOpr.doVerify(authHeader))
                {
                    output["success"] = false;
                    output["message"] = BpcOpr.MessageNotAuthen;
                    log.apilog_status = "500";
                    log.apilog_message = BpcOpr.MessageNotAuthen;
                    objBpcOpr.doRecordLog(log);

                    return output.ToString(Formatting.None);
                }

                cls_ctMTRequest controller = new cls_ctMTRequest();

                if (controller.checkDataOld(input.request_code))
                {
                    bool blnResult = controller.delete(input.request_code);

                    if (blnResult)
                    {
                        output["success"] = true;
                        output["message"] = "Remove data successfully";

                        log.apilog_status = "200";
                        log.apilog_message = "";
                    }
                    else
                    {
                        output["success"] = false;
                        output["message"] = "Remove data not successfully";

                        log.apilog_status = "500";
                        log.apilog_message = controller.getMessage();
                    }

                }
                else
                {
                    string message = "Not Found Project code : " + input.request_code;
                    output["success"] = false;
                    output["message"] = message;

                    log.apilog_status = "404";
                    log.apilog_message = message;
                }

                controller.dispose();
            }
            catch (Exception ex)
            {
                output["success"] = false;
                output["message"] = "(C)Remove data not successfully";

                log.apilog_status = "500";
                log.apilog_message = ex.ToString();
            }
            finally
            {
                objBpcOpr.doRecordLog(log);
            }

            output["data"] = tmp;

            return output.ToString(Formatting.None);

        }
        public async Task<string> doUploadMTRequest(string token, string by, string fileName, Stream stream)
        {
            JObject output = new JObject();

            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "REQ001.4";
            log.apilog_by = by;
            log.apilog_data = "Stream";

            try
            {
                if (!objBpcOpr.doVerify(token))
                {
                    output["success"] = false;
                    output["message"] = BpcOpr.MessageNotAuthen;

                    log.apilog_status = "500";
                    log.apilog_message = BpcOpr.MessageNotAuthen;
                    objBpcOpr.doRecordLog(log);

                    return output.ToString(Formatting.None);
                }


                bool upload = await this.doUploadFile(fileName, stream);

                if (upload)
                {
                    cls_srvSystemImport srv_import = new cls_srvSystemImport();
                    string tmp = srv_import.doImportExcel("request", fileName, "TEST");

                    output["success"] = true;
                    output["message"] = tmp;

                    log.apilog_status = "200";
                    log.apilog_message = "";
                }
                else
                {
                    output["success"] = false;
                    output["message"] = "Upload data not successfully";

                    log.apilog_status = "500";
                    log.apilog_message = "Upload data not successfully";
                }

            }
            catch (Exception ex)
            {
                output["success"] = false;
                output["message"] = "(C)Upload data not successfully";

                log.apilog_status = "500";
                log.apilog_message = ex.ToString();
            }
            finally
            {
                objBpcOpr.doRecordLog(log);
            }

            return output.ToString(Formatting.None);
        }
    }
}
        #endregion