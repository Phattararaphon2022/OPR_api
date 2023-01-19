using ClassLibrary_BPC.hrfocus.controller;
using ClassLibrary_BPC.hrfocus.model;
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
    public class cls_srvSystemImport
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

        public string doImportExcel(string type, string filename, string by)
        {
            string strResult = "";
            
            try
            {
              
                int success = 0;
                StringBuilder objStr = new StringBuilder();

                switch (type)
                {
                    case "BANK":

                        DataTable dt = doReadExcel(filename);
                        if (dt.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dt.Rows)
                            {
                                
                                cls_ctMTBank objWorker = new cls_ctMTBank();
                                cls_MTBank model = new cls_MTBank();
                                
                                model.bank_code = dr["bank_code"].ToString();
                                model.bank_name_th = dr["bank_name_th"].ToString();
                                model.bank_name_en = dr["bank_name_en"].ToString();
                                model.modified_by = by;
                                
                                string strID = objWorker.insert(model);

                                if (!strID.Equals(""))
                                {
                                    success++;
                                }
                                else
                                {
                                    objStr.Append(model.bank_code);
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
                //REASON
                switch (type)
                {
                    case "REASON":

                        DataTable dt = doReadExcel(filename);
                        if (dt.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dt.Rows)
                            {

                                cls_ctSYSReason objReason = new cls_ctSYSReason();
                                cls_SYSReason model = new cls_SYSReason();

                                model.reason_code = dr["reason_code"].ToString();
                                model.reason_name_th = dr["reason_name_th"].ToString();
                                model.reason_name_en = dr["reason_name_th"].ToString();
                                model.modified_by = by;

                                string strID = objReason.insert(model);

                                if (!strID.Equals(""))
                                {
                                    success++;
                                }
                                else
                                {
                                    objStr.Append(model.reason_code);
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
                //Level
                switch (type)
                {
                    case "LEVEL":

                        DataTable dt = doReadExcel(filename);
                        if (dt.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dt.Rows)
                            {

                                cls_ctSYSReason objReason = new cls_ctSYSReason();
                                cls_SYSReason model = new cls_SYSReason();

                                model.reason_code = dr["level_code"].ToString();
                                model.reason_name_th = dr["level_name_th"].ToString();
                                model.reason_name_en = dr["level_name_en"].ToString();
                                model.modified_by = by;

                                string strID = objReason.insert(model);

                                if (!strID.Equals(""))
                                {
                                    success++;
                                }
                                else
                                {
                                    objStr.Append(model.reason_code);
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
                
                //Fmily

                switch (type)
                {
                    case "FAMILY":

                        DataTable dt = doReadExcel(filename);
                        if (dt.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dt.Rows)
                            {

                                cls_ctSYSReason objReason = new cls_ctSYSReason();
                                cls_SYSReason model = new cls_SYSReason();

                                model.reason_code = dr["family_code"].ToString();
                                model.reason_name_th = dr["family_name_th"].ToString();
                                model.reason_name_en = dr["family_name_en"].ToString();
                                model.modified_by = by;

                                string strID = objReason.insert(model);

                                if (!strID.Equals(""))
                                {
                                    success++;
                                }
                                else
                                {
                                    objStr.Append(model.reason_code);
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
                //Addresstype
                switch (type)
                {
                    case "ADDRESSTYPE":

                        DataTable dt = doReadExcel(filename);
                        if (dt.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dt.Rows)
                            {

                                cls_ctSYSReason objReason = new cls_ctSYSReason();
                                cls_SYSReason model = new cls_SYSReason();

                                model.reason_code = dr["addresstype_code"].ToString();
                                model.reason_name_th = dr["addresstype_name_th"].ToString();
                                model.reason_name_en = dr["addresstype_name_en"].ToString();
                                model.modified_by = by;

                                string strID = objReason.insert(model);

                                if (!strID.Equals(""))
                                {
                                    success++;
                                }
                                else
                                {
                                    objStr.Append(model.reason_code);
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
                //Ethnicity

                switch (type)
                {
                    case "ETHNICITY":

                        DataTable dt = doReadExcel(filename);
                        if (dt.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dt.Rows)
                            {

                                cls_ctSYSReason objReason = new cls_ctSYSReason();
                                cls_SYSReason model = new cls_SYSReason();

                                model.reason_code = dr["ethnicity_code"].ToString();
                                model.reason_name_th = dr["ethnicity_name_th"].ToString();
                                model.reason_name_en = dr["ethnicity_name_en"].ToString();
                                model.modified_by = by;

                                string strID = objReason.insert(model);

                                if (!strID.Equals(""))
                                {
                                    success++;
                                }
                                else
                                {
                                    objStr.Append(model.reason_code);
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
                //Bloodtype
                switch (type)
                {
                    case "BLOODTYPE":

                        DataTable dt = doReadExcel(filename);
                        if (dt.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dt.Rows)
                            {

                                cls_ctSYSReason objReason = new cls_ctSYSReason();
                                cls_SYSReason model = new cls_SYSReason();

                                model.reason_code = dr["bloodtype_code"].ToString();
                                model.reason_name_th = dr["bloodtype_name_th"].ToString();
                                model.reason_name_en = dr["bloodtype_name_en"].ToString();
                                model.modified_by = by;

                                string strID = objReason.insert(model);

                                if (!strID.Equals(""))
                                {
                                    success++;
                                }
                                else
                                {
                                    objStr.Append(model.reason_code);
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
                //Hospital

                switch (type)
                {
                    case "HOSPITAL":

                        DataTable dt = doReadExcel(filename);
                        if (dt.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dt.Rows)
                            {

                                cls_ctSYSReason objReason = new cls_ctSYSReason();
                                cls_SYSReason model = new cls_SYSReason();

                                model.reason_code = dr["hospital_code"].ToString();
                                model.reason_name_th = dr["hospital_name_th"].ToString();
                                model.reason_name_en = dr["hospital_name_en"].ToString();
                                model.modified_by = by;

                                string strID = objReason.insert(model);

                                if (!strID.Equals(""))
                                {
                                    success++;
                                }
                                else
                                {
                                    objStr.Append(model.reason_code);
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
                //Location


                switch (type)
                {
                    case "LOCATION":

                        DataTable dt = doReadExcel(filename);
                        if (dt.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dt.Rows)
                            {

                                cls_ctSYSReason objReason = new cls_ctSYSReason();
                                cls_SYSReason model = new cls_SYSReason();

                                model.reason_code = dr["location_code"].ToString();
                                model.reason_name_th = dr["location_name_th"].ToString();
                                model.reason_name_en = dr["location_name_en"].ToString();
                                model.reason_name_en = dr["location_detail"].ToString();
                                model.modified_by = by;

                                string strID = objReason.insert(model);

                                if (!strID.Equals(""))
                                {
                                    success++;
                                }
                                else
                                {
                                    objStr.Append(model.reason_code);
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
                 //Province
                switch (type)
                {
                    case "PROVINCE":

                        DataTable dt = doReadExcel(filename);
                        if (dt.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dt.Rows)
                            {

                                cls_ctSYSReason objReason = new cls_ctSYSReason();
                                cls_SYSReason model = new cls_SYSReason();

                                model.reason_code = dr["province_code"].ToString();
                                model.reason_name_th = dr["province_name_th"].ToString();
                                model.reason_name_en = dr["province_name_en"].ToString();
                                model.modified_by = by;

                                string strID = objReason.insert(model);

                                if (!strID.Equals(""))
                                {
                                    success++;
                                }
                                else
                                {
                                    objStr.Append(model.reason_code);
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
                //Religion
                switch (type)
                {
                    case "RELIGION":

                        DataTable dt = doReadExcel(filename);
                        if (dt.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dt.Rows)
                            {

                                cls_ctSYSReason objReason = new cls_ctSYSReason();
                                cls_SYSReason model = new cls_SYSReason();

                                model.reason_code = dr["religion_code"].ToString();
                                model.reason_name_th = dr["religion_name_th"].ToString();
                                model.reason_name_en = dr["religion_name_en"].ToString();
                                model.modified_by = by;

                                string strID = objReason.insert(model);

                                if (!strID.Equals(""))
                                {
                                    success++;
                                }
                                else
                                {
                                    objStr.Append(model.reason_code);
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
                //REDUCE
                switch (type)
                {
                    case "REDUCE":

                        DataTable dt = doReadExcel(filename);
                        if (dt.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dt.Rows)
                            {

                                cls_ctSYSReason objReason = new cls_ctSYSReason();
                                cls_SYSReason model = new cls_SYSReason();

                                model.reason_code = dr["reduce_code"].ToString();
                                model.reason_name_th = dr["reduce_name_th"].ToString();
                                model.reason_name_en = dr["reduce_name_en"].ToString();
                                model.reason_name_en = dr["reduce_amount"].ToString();
                                model.reason_name_en = dr["reduce_percent"].ToString();
                                model.reason_name_en = dr["reduce_percent_max"].ToString();
                                model.modified_by = by;

                                string strID = objReason.insert(model);

                                if (!strID.Equals(""))
                                {
                                    success++;
                                }
                                else
                                {
                                    objStr.Append(model.reason_code);
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
                //EmpID Structure
                switch (type)
                {
                    case "EmpID":

                        DataTable dt = doReadExcel(filename);
                        if (dt.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dt.Rows)
                            {

                                cls_ctSYSReason objReason = new cls_ctSYSReason();
                                cls_SYSReason model = new cls_SYSReason();

                                model.reason_code = dr["empid_code"].ToString();
                                model.reason_name_th = dr["empid_name_th"].ToString();
                                model.reason_name_en = dr["empid_name_en"].ToString();
                                model.modified_by = by;

                                string strID = objReason.insert(model);

                                if (!strID.Equals(""))
                                {
                                    success++;
                                }
                                else
                                {
                                    objStr.Append(model.reason_code);
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

                //cartype
                switch (type)
                {
                    case "CARTYPE":

                        DataTable dt = doReadExcel(filename);
                        if (dt.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dt.Rows)
                            {

                                cls_ctMTCardtype objCardtype = new cls_ctMTCardtype();
                                cls_MTCardtype model = new cls_MTCardtype();

                                model.cardtype_code = dr["cardtype_code"].ToString();
                                model.cardtype_name_th = dr["cardtype_name_th"].ToString();
                                model.cardtype_name_en = dr["cardtype_name_en"].ToString();
                                model.modified_by = by;

                                string strID = objCardtype.insert(model);

                                if (!strID.Equals(""))
                                {
                                    success++;
                                }
                                else
                                {
                                    objStr.Append(model.cardtype_code);
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

