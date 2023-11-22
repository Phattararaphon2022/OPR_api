using ClassLibrary_BPC.hrfocus.controller;
using ClassLibrary_BPC.hrfocus.controller.Project;
using ClassLibrary_BPC.hrfocus.model;
using ClassLibrary_BPC.hrfocus.model.Project;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary_BPC.hrfocus.service
{
    public class cls_srvProjectImport
    {
        public DataTable doReadExcel(string fileName)
        {
            DataTable dt = new DataTable();

            string filePath = Path.Combine(ClassLibrary_BPC.Config.PathFileImport + "\\Imports\\", fileName);
            string xlConnStr = @"Provider=Microsoft.ACE.OLEDB.12.0; Data Source=" + filePath + ";Extended Properties='Excel 8.0;HDR=Yes;';";
            var xlConn = new OleDbConnection(xlConnStr);

            try
            {

                var da = new OleDbDataAdapter("SELECT * FROM [Sheet1$]", xlConn);
                da.Fill(dt);

            }
            catch (Exception ex)
            {

            }
            finally
            {
                xlConn.Close();
            }

            return dt;
        }

        public string doImportExcel(string type, string filename, string by, string com)
        {
            string strResult = "";
            
            try
            {              
                int success = 0;
                StringBuilder objStr = new StringBuilder();
                DataTable dt = new DataTable();

                switch (type)
                {
                    case "PROBUSINESS":

                        dt = doReadExcel(filename);
                        if (dt.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dt.Rows)
                            {
                                cls_ctMTProbusiness controller = new cls_ctMTProbusiness();
                                cls_MTProbusiness model = new cls_MTProbusiness();
                                if (!com.Equals(dr["company_code"].ToString()))
                                {
                                    continue;
                                }
                                model.company_code = dr["company_code"].ToString();

                                model.probusiness_code = dr["probusiness_code"].ToString();
                                model.probusiness_name_th = dr["probusiness_name_th"].ToString();
                                model.probusiness_name_en = dr["probusiness_name_en"].ToString();
                                model.modified_by = by;
                                string strID = controller.insert(model);

                                if (!strID.Equals("")){
                                    success++;
                                }
                                else{
                                    objStr.Append(model.probusiness_code);
                                }
                            }

                            strResult = "";
                            if (success > 0)
                                strResult += "Success : " + success.ToString();

                            if (objStr.Length > 0)
                                strResult += " Fail : " + objStr.ToString();
                        }

                        break;

                    case "PROTYPE":

                        dt = doReadExcel(filename);
                        if (dt.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dt.Rows)
                            {
                                cls_ctMTProtype controller = new cls_ctMTProtype();
                                cls_MTProtype model = new cls_MTProtype();
                                if (!com.Equals(dr["company_code"].ToString()))
                                {
                                    continue;
                                }
                                model.company_code = dr["company_code"].ToString();

                                model.protype_code = dr["protype_code"].ToString();
                                model.protype_name_th = dr["protype_name_th"].ToString();
                                model.protype_name_en = dr["protype_name_en"].ToString();
                                model.modified_by = by;
                                string strID = controller.insert(model);

                                if (!strID.Equals(""))
                                {
                                    success++;
                                }
                                else
                                {
                                    objStr.Append(model.protype_code);
                                }
                            }

                            strResult = "";
                            if (success > 0)
                                strResult += "Success : " + success.ToString();

                            if (objStr.Length > 0)
                                strResult += " Fail : " + objStr.ToString();
                        }

                        break;

                    case "PROUNIFORM":

                        dt = doReadExcel(filename);
                        if (dt.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dt.Rows)
                            {
                                cls_ctMTProuniform controller = new cls_ctMTProuniform();
                                cls_MTProuniform model = new cls_MTProuniform();
                                if (!com.Equals(dr["company_code"].ToString()))
                                {
                                    continue;
                                }
                                model.company_code = dr["company_code"].ToString();

                                model.prouniform_code = dr["prouniform_code"].ToString();
                                model.prouniform_name_th = dr["prouniform_name_th"].ToString();
                                model.prouniform_name_en = dr["prouniform_name_en"].ToString();
                                model.modified_by = by;
                                string strID = controller.insert(model);

                                if (!strID.Equals(""))
                                {
                                    success++;
                                }
                                else
                                {
                                    objStr.Append(model.prouniform_code);
                                }
                            }

                            strResult = "";
                            if (success > 0)
                                strResult += "Success : " + success.ToString();

                            if (objStr.Length > 0)
                                strResult += " Fail : " + objStr.ToString();
                        }

                        break;

                    case "PROSLIP":

                        dt = doReadExcel(filename);
                        if (dt.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dt.Rows)
                            {
                                cls_ctMTProslip controller = new cls_ctMTProslip();
                                cls_MTProslip model = new cls_MTProslip();
                                if (!com.Equals(dr["company_code"].ToString()))
                                {
                                    continue;
                                }
                                model.company_code = dr["company_code"].ToString();

                                model.proslip_code = dr["proslip_code"].ToString();
                                model.proslip_name_th = dr["proslip_name_th"].ToString();
                                model.proslip_name_en = dr["proslip_name_en"].ToString();
                                model.modified_by = by;
                                string strID = controller.insert(model);

                                if (!strID.Equals(""))
                                {
                                    success++;
                                }
                                else
                                {
                                    objStr.Append(model.proslip_code);
                                }
                            }

                            strResult = "";
                            if (success > 0)
                                strResult += "Success : " + success.ToString();

                            if (objStr.Length > 0)
                                strResult += " Fail : " + objStr.ToString();
                        }

                        break;


                    case "PROGROUP":

                        dt = doReadExcel(filename);
                        if (dt.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dt.Rows)
                            {
                                cls_ctMTProgroup controller = new cls_ctMTProgroup();
                                cls_MTProgroup model = new cls_MTProgroup();
                                if (!com.Equals(dr["company_code"].ToString()))
                                {
                                    continue;
                                }
                                model.company_code = dr["company_code"].ToString();

                                 model.progroup_code = dr["progroup_code"].ToString();
                                model.progroup_name_th = dr["progroup_name_th"].ToString();
                                model.progroup_name_en = dr["progroup_name_en"].ToString();

                                model.modified_by = by;
                                string strID = controller.insert(model);

                                if (!strID.Equals(""))
                                {
                                    success++;
                                }
                                else
                                {
                                    objStr.Append(model.progroup_code);
                                }
                            }

                            strResult = "";
                            if (success > 0)
                                strResult += "Success : " + success.ToString();

                            if (objStr.Length > 0)
                                strResult += " Fail : " + objStr.ToString();
                        }

                        break;

                    case "PROAREA":

                        dt = doReadExcel(filename);
                        if (dt.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dt.Rows)
                            {
                                cls_ctMTProarea controller = new cls_ctMTProarea();
                                cls_MTProarea model = new cls_MTProarea();
                                if (!com.Equals(dr["company_code"].ToString()))
                                {
                                    continue;
                                }
                                model.company_code = dr["company_code"].ToString();
                                model.proarea_code = dr["proarea_code"].ToString();
                                model.proarea_name_th = dr["proarea_name_th"].ToString();
                                model.proarea_name_en = dr["proarea_name_en"].ToString();

                                model.modified_by = by;
                                string strID = controller.insert(model);

                                if (!strID.Equals(""))
                                {
                                    success++;
                                }
                                else
                                {
                                    objStr.Append(model.proarea_code);
                                }
                            }

                            strResult = "";
                            if (success > 0)
                                strResult += "Success : " + success.ToString();

                            if (objStr.Length > 0)
                                strResult += " Fail : " + objStr.ToString();
                        }

                        break;

                    case "PROCOST":

                        dt = doReadExcel(filename);
                        if (dt.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dt.Rows)
                            {
                                cls_ctMTProcost controller = new cls_ctMTProcost();
                                cls_MTProcost model = new cls_MTProcost();
                                if (!com.Equals(dr["company_code"].ToString()))
                                {
                                    continue;
                                }
                                model.company_code = dr["company_code"].ToString();
                                model.procost_code = dr["procost_code"].ToString();
                                model.procost_name_th = dr["procost_name_th"].ToString();
                                model.procost_name_en = dr["procost_name_en"].ToString();

                                model.procost_type = dr["procost_type"].ToString();
                                model.procost_itemcode = dr["procost_itemcode"].ToString();

                                model.procost_auto = dr["procost_auto"].ToString().Equals("1") ? true : false;


                                model.procost_itemcode = dr["procost_itemcode"].ToString();

                                model.modified_by = by;
                                string strID = controller.insert(model);

                                if (!strID.Equals(""))
                                {
                                    success++;
                                }
                                else
                                {
                                    objStr.Append(model.procost_code);
                                }
                            }

                            strResult = "";
                            if (success > 0)
                                strResult += "Success : " + success.ToString();

                            if (objStr.Length > 0)
                                strResult += " Fail : " + objStr.ToString();
                        }

                        break;

                    case "PROJECT":

                        dt = doReadExcel(filename);
                        if (dt.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dt.Rows)
                            {
                                cls_ctMTProject controller = new cls_ctMTProject();
                                cls_MTProject model = new cls_MTProject();
                                if (!com.Equals(dr["company_code"].ToString()))
                                {
                                    continue;
                                }
                                model.company_code = dr["company_code"].ToString();
                                model.project_code = dr["project_code"].ToString();
                                model.project_name_th = dr["project_name_th"].ToString();
                                model.project_name_en = dr["project_name_en"].ToString();

                                model.project_name_sub = dr["project_name_sub"].ToString();
                                model.project_codecentral = dr["project_codecentral"].ToString();
                                model.project_protype = dr["project_protype"].ToString();
                                model.project_proarea = dr["project_proarea"].ToString();

                      
                                model.project_progroup = dr["project_progroup"].ToString();
                                model.project_probusiness = dr["project_probusiness"].ToString();
                                model.project_roundtime = dr["project_roundtime"].ToString();
                                model.project_roundmoney = dr["project_roundmoney"].ToString();

                                model.project_proholiday = dr["project_proholiday"].ToString();
                                model.project_status = dr["project_status"].ToString();
                  

                                model.modified_by = by;
                                string strID = controller.insert(model);

                                if (!strID.Equals(""))
                                {
                                    success++;
                                }
                                else
                                {
                                    objStr.Append(model.project_code);
                                }
                            }

                            strResult = "";
                            if (success > 0)
                                strResult += "Success : " + success.ToString();

                            if (objStr.Length > 0)
                                strResult += " Fail : " + objStr.ToString();
                        }

                        break;

                    case "PROJECT_CONTACT":

                        dt = doReadExcel(filename);
                        if (dt.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dt.Rows)
                            {
                                cls_ctTRProcontact controller = new cls_ctTRProcontact();
                                cls_TRProcontact model = new cls_TRProcontact();

                                model.procontact_ref = dr["procontact_ref"].ToString();
                                model.procontact_firstname_th = dr["procontact_firstname_th"].ToString();
                                model.procontact_lastname_th = dr["procontact_lastname_th"].ToString();

                                model.procontact_firstname_en = dr["procontact_firstname_en"].ToString();
                                model.procontact_lastname_en = dr["procontact_lastname_en"].ToString();
                                model.procontact_tel = dr["procontact_tel"].ToString();
                                model.procontact_email = dr["procontact_email"].ToString();


                                model.position_code = dr["position_code"].ToString();
                                model.initial_code = dr["initial_code"].ToString();
                                model.project_code = dr["project_code"].ToString();
                                


                                model.modified_by = by;
                                bool strID = controller.insert(model);

                                if (!strID.Equals(""))
                                {
                                    success++;
                                }
                                else
                                {
                                    objStr.Append(model.project_code);
                                }
                            }

                            strResult = "";
                            if (success > 0)
                                strResult += "Success : " + success.ToString();

                            if (objStr.Length > 0)
                                strResult += " Fail : " + objStr.ToString();
                        }

                        break;

                    case "PROJECT_CONTRACT":

                        dt = doReadExcel(filename);
                        if (dt.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dt.Rows)
                            {
                                cls_ctTRProcontract controller = new cls_ctTRProcontract();
                                cls_TRProcontract model = new cls_TRProcontract();

                                model.procontract_ref = dr["procontract_ref"].ToString();
                                model.procontract_date = Convert.ToDateTime(dr["procontract_date"]);
                                model.procontract_amount = Convert.ToDecimal(dr["procontract_amount"]);
                                model.procontract_fromdate = Convert.ToDateTime(dr["procontract_fromdate"]);
                                model.procontract_todate = Convert.ToDateTime(dr["procontract_todate"]);
                                model.procontract_customer = dr["procontract_customer"].ToString();
                                model.procontract_bidder = dr["procontract_bidder"].ToString();
                                model.project_code = dr["project_code"].ToString();
                                model.procontract_type = dr["procontract_type"].ToString();
 
                                model.modified_by = by;
                                bool strID = controller.insert(model);

                                if (!strID.Equals(""))
                                {
                                    success++;
                                }
                                else
                                {
                                    objStr.Append(model.project_code);
                                }
                            }

                            strResult = "";
                            if (success > 0)
                                strResult += "Success : " + success.ToString();

                            if (objStr.Length > 0)
                                strResult += " Fail : " + objStr.ToString();
                        }

                        break;

                    case "PROJECT_DOCATT":
                        dt = doReadExcel(filename);
                        if (dt.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dt.Rows)
                            {

                                cls_ctTRProDocatt objReqworker = new cls_ctTRProDocatt();
                                cls_TRProDocatt model = new cls_TRProDocatt();
                                if (!com.Equals(dr["company_code"].ToString()))
                                {
                                    continue;
                                }
                                model.company_code = dr["company_code"].ToString();
                                model.project_code = dr["project_code"].ToString();

                                model.job_type = dr["job_type"].ToString();
                                model.document_name = dr["document_name"].ToString();
                                model.document_type = dr["document_type"].ToString();
                                model.document_path = dr["document_path"].ToString();

                                model.modified_by = by;

                                bool strID = objReqworker.insert(model);

                                if (strID)
                                {
                                    success++;
                                }
                                else
                                {
                                    objStr.Append(model.project_code);
                                }

                            }

                            strResult = "";
                            if (success > 0)
                                strResult += "Success : " + success.ToString();

                            if (objStr.Length > 0)
                                strResult += " Fail : " + objStr.ToString();
                        }
                        break;



                }

            }
            catch (Exception ex)
            {
                strResult = ex.ToString();
            }
               
            return strResult;
        }
    }
}
