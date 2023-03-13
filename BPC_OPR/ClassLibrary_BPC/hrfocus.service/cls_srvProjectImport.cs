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

        public string doImportExcel(string type, string filename, string by)
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
