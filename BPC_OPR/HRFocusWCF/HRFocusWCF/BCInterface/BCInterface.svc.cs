using AntsCode.Util;
using ClassLibrary_BPC.hrfocus.controller;
using ClassLibrary_BPC.hrfocus.controller.Project;
using ClassLibrary_BPC.hrfocus.model;
using ClassLibrary_BPC.hrfocus.model.Project;
using ClassLibrary_BPC.hrfocus.service;
using ClassLibrary_BPC.hrfocus.service.Payroll;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace BPC_OPR
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "ModuleProject" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select ModuleProject.svc or ModuleProject.svc.cs at the Solution Explorer and start debugging.

    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]

    public class BCInterface : IBCInterface
    {
       
        BpcOpr objBpcOpr = new BpcOpr();
        private DateTime doConvertDate(string date)
        {
            DateTime result = DateTime.Now;
            try
            {
                result = Convert.ToDateTime(date);
            }
            catch { }

            return result;
        }

        private string doCheckDateTimeEmpty(DateTime date)
        {
            if (date.Date.ToString("dd/MM/yyyy").Equals("01/01/1900"))
                return "-";
            else
                return date.ToString("dd/MM/yyyy HH:mm:ss");
        }

        #region APIHRJob

        public string doManageAPIHRJob(APIHRJob input)
        {
            JObject output = new JObject();

            var json_data = new JavaScriptSerializer().Serialize(input);
            var tmp = JToken.Parse(json_data);


            cls_SYSApilog log = new cls_SYSApilog();
            log.apilog_code = "BCO001.2";
            log.apilog_by = input.PostBy;
            log.apilog_data = tmp.ToString();

            try
            {
                var authHeader = WebOperationContext.Current.IncomingRequest.Headers["Authorization"];
                var TransactionId = WebOperationContext.Current.IncomingRequest.Headers["TransactionId"];
                var RequestDate = WebOperationContext.Current.IncomingRequest.Headers["RequestDate"];
                var OldTransactionId = WebOperationContext.Current.IncomingRequest.Headers["OldTransactionId"];
                
                //if (authHeader == null || !objBpcOpr.doVerify(authHeader))
                //{
                //    output["success"] = false;
                //    output["message"] = BpcOpr.MessageNotAuthen;

                //    log.apilog_status = "500";
                //    log.apilog_message = BpcOpr.MessageNotAuthen;
                //    objBpcOpr.doRecordLog(log);

                //    return output.ToString(Formatting.None);
                //}

                //-- Step 1 Validate
                //-- Shift


                cls_ctMTProjobversion ctVersion = new cls_ctMTProjobversion();
                cls_ctMTShift ctShift = new cls_ctMTShift();

                //-- Get shift master
                List<cls_MTShift> shift_list = ctShift.getDataByFillter("", "", "", true);

                List<JobPlaningLines> JobPlaningLines = input.JobPlaningLines;
                StringBuilder sbNotfound = new StringBuilder();

                foreach (JobPlaningLines job in JobPlaningLines)
                {

                    foreach (JobTaskShift job_shift in job.JobTaskShift){

                        bool found = false;
                        foreach (cls_MTShift shift in shift_list)
                        {
                            if (job_shift.TimeIN.Equals(shift.shift_ch3) && job_shift.TimeOUT.Equals(shift.shift_ch4))
                            {
                                found = true;
                                break;
                            }
                        }

                        if (!found)
                            sbNotfound.Append(job.JabTaskNo + " not found shift " + job_shift.TimeIN + "-" + job_shift.TimeOUT);
                    }
                }

                if (sbNotfound.Length > 0)
                {
                    output["success"] = false;
                    output["message"] = sbNotfound.ToString();                    
                    output["data"] = tmp;

                    return output.ToString(Formatting.None);                    
                }

                //-- Transaction
                cls_MTProjobversion version = ctVersion.getDataTransaction(TransactionId);
                if (version != null)
                {
                    output["success"] = false;
                    output["message"] = "Transaction id is dupilcate";
                    output["data"] = tmp;

                    return output.ToString(Formatting.None);    
                }

                //-- Step 2 Record version
                
                version = new cls_MTProjobversion();
                version.projobversion_id = 0;
                version.transaction_id = TransactionId;
                version.version = input.Version;
                version.fromdate = Convert.ToDateTime(input.StartDate);
                version.todate = Convert.ToDateTime(input.EndDate);
                version.project_code = input.JobNo;
                version.modified_by = input.PostBy;

                bool blnVersion = ctVersion.insert(version);

                //-- Step 3 Record Job




                bool blnResult = true;

                if (blnResult)
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

                    log.apilog_status = "500";
                    //log.apilog_message = controller.getMessage();
                    log.apilog_message = "";
                }

                //controller.dispose();

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
        
        #endregion
        
    }
}
