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
    public class cls_srvEmpImport
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
                    case "LOCATION":

                        DataTable dt = doReadExcel(filename);
                        if (dt.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dt.Rows)
                            {

                                cls_ctMTLocation objWorker = new cls_ctMTLocation();
                                cls_MTLocation model = new cls_MTLocation();

                                model.location_code = dr["location_code"].ToString();
                                model.location_name_th = dr["location_name_th"].ToString();
                                model.location_name_en = dr["location_name_en"].ToString();
                                model.location_detail = dr["location_detail"].ToString();
                                model.modified_by = by;

                                string strID = objWorker.insert(model);

                                if (!strID.Equals(""))
                                {
                                    success++;
                                }
                                else
                                {
                                    objStr.Append(model.location_code);
                                }

                            }

                            strResult = "";

                            if (success > 0)
                                strResult += "Success : " + success.ToString();

                            if (objStr.Length > 0)
                                strResult += " Fail : " + objStr.ToString();

                        }

                        break;

                    case "DEP":
                        DataTable dtdep = doReadExcel(filename);
                        if (dtdep.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dtdep.Rows)
                            {

                                cls_ctMTDep objWorker = new cls_ctMTDep();
                                cls_MTDep model = new cls_MTDep();

                                model.dep_code = dr["dep_code"].ToString();
                                model.dep_name_th = dr["dep_name_th"].ToString();
                                model.dep_name_en = dr["dep_name_en"].ToString();
                                model.dep_parent = dr["dep_parent"].ToString();
                                model.dep_level = dr["dep_level"].ToString();
                                model.company_code = dr["company_code"].ToString();
                                model.modified_by = by;

                                string strID = objWorker.insert(model);

                                if (!strID.Equals(""))
                                {
                                    success++;
                                }
                                else
                                {
                                    objStr.Append(model.dep_code);
                                }

                            }

                            strResult = "";

                            if (success > 0)
                                strResult += "Success : " + success.ToString();

                            if (objStr.Length > 0)
                                strResult += " Fail : " + objStr.ToString();

                        }
                        break;

                    case "POSITION":
                        DataTable dtpo = doReadExcel(filename);
                        if (dtpo.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dtpo.Rows)
                            {

                                cls_ctMTPosition objWorker = new cls_ctMTPosition();
                                cls_MTPosition model = new cls_MTPosition();

                                model.position_code = dr["position_code"].ToString();
                                model.position_name_th = dr["position_name_th"].ToString();
                                model.position_name_en = dr["position_name_en"].ToString();
                                model.company_code = dr["company_code"].ToString();
                                model.modified_by = by;

                                string strID = objWorker.insert(model);

                                if (!strID.Equals(""))
                                {
                                    success++;
                                }
                                else
                                {
                                    objStr.Append(model.position_code);
                                }

                            }

                            strResult = "";

                            if (success > 0)
                                strResult += "Success : " + success.ToString();

                            if (objStr.Length > 0)
                                strResult += " Fail : " + objStr.ToString();

                        }
                        break;

                    case "GROUP":
                        DataTable dtgr = doReadExcel(filename);
                        if (dtgr.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dtgr.Rows)
                            {

                                cls_ctMTGroup objWorker = new cls_ctMTGroup();
                                cls_MTGroup model = new cls_MTGroup();

                                model.group_code = dr["group_code"].ToString();
                                model.group_name_th = dr["group_name_th"].ToString();
                                model.group_name_en = dr["group_name_en"].ToString();
                                model.company_code = dr["company_code"].ToString();
                                model.modified_by = by;

                                string strID = objWorker.insert(model);

                                if (!strID.Equals(""))
                                {
                                    success++;
                                }
                                else
                                {
                                    objStr.Append(model.group_code);
                                }

                            }

                            strResult = "";

                            if (success > 0)
                                strResult += "Success : " + success.ToString();

                            if (objStr.Length > 0)
                                strResult += " Fail : " + objStr.ToString();

                        }
                        break;

                    case "INITIAL":
                        DataTable dtin = doReadExcel(filename);
                        if (dtin.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dtin.Rows)
                            {

                                cls_ctMTInitial objWorker = new cls_ctMTInitial();
                                cls_MTInitial model = new cls_MTInitial();

                                model.initial_code = dr["initial_code"].ToString();
                                model.initial_name_th = dr["initial_name_th"].ToString();
                                model.initial_name_en = dr["initial_name_en"].ToString();
                                model.modified_by = by;

                                string strID = objWorker.insert(model);

                                if (!strID.Equals(""))
                                {
                                    success++;
                                }
                                else
                                {
                                    objStr.Append(model.initial_code);
                                }

                            }

                            strResult = "";

                            if (success > 0)
                                strResult += "Success : " + success.ToString();

                            if (objStr.Length > 0)
                                strResult += " Fail : " + objStr.ToString();

                        }
                        break;

                    case "TYPE":
                        DataTable dtty = doReadExcel(filename);
                        if (dtty.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dtty.Rows)
                            {

                                cls_ctMTType objWorker = new cls_ctMTType();
                                cls_MTType model = new cls_MTType();

                                model.type_code = dr["type_code"].ToString();
                                model.type_name_th = dr["type_name_th"].ToString();
                                model.type_name_en = dr["type_name_en"].ToString();
                                model.modified_by = by;

                                string strID = objWorker.insert(model);

                                if (!strID.Equals(""))
                                {
                                    success++;
                                }
                                else
                                {
                                    objStr.Append(model.type_code);
                                }

                            }

                            strResult = "";

                            if (success > 0)
                                strResult += "Success : " + success.ToString();

                            if (objStr.Length > 0)
                                strResult += " Fail : " + objStr.ToString();

                        }
                        break;

                    case "STATUS":
                        DataTable dtst = doReadExcel(filename);
                        if (dtst.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dtst.Rows)
                            {

                                cls_ctMTStatus objWorker = new cls_ctMTStatus();
                                cls_MTStatus model = new cls_MTStatus();

                                model.status_code = dr["status_code"].ToString();
                                model.status_name_th = dr["status_name_th"].ToString();
                                model.status_name_en = dr["status_name_en"].ToString();
                                model.modified_by = by;

                                string strID = objWorker.insert(model);

                                if (!strID.Equals(""))
                                {
                                    success++;
                                }
                                else
                                {
                                    objStr.Append(model.status_code);
                                }

                            }

                            strResult = "";

                            if (success > 0)
                                strResult += "Success : " + success.ToString();

                            if (objStr.Length > 0)
                                strResult += " Fail : " + objStr.ToString();

                        }
                        break;

                    case "WORKER":

                        DataTable dtworker = doReadExcel(filename);
                        if (dtworker.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dtworker.Rows)
                            {

                                cls_ctMTWorker objWorker = new cls_ctMTWorker();
                                cls_MTWorker model = new cls_MTWorker();

                                model.company_code = dr["company_code"].ToString();
                                model.worker_code = dr["worker_code"].ToString();
                                model.worker_card = dr["worker_code"].ToString();
                                model.worker_initial = dr["worker_initial"].ToString();
                                model.worker_fname_th = dr["worker_fname_th"].ToString();
                                model.worker_lname_th = dr["worker_lname_th"].ToString();
                                model.worker_fname_en = dr["worker_fname_en"].ToString();
                                model.worker_lname_en = dr["worker_lname_en"].ToString();
                                model.worker_type = dr["worker_type"].ToString();
                                model.worker_gender = dr["worker_gender"].ToString();
                                model.worker_birthdate = Convert.ToDateTime(dr["worker_birthdate"]);
                                model.worker_hiredate = Convert.ToDateTime(dr["worker_hiredate"]);
                                model.worker_status = dr["worker_status"].ToString();
                                model.religion_code = dr["religion_code"].ToString();
                                model.blood_code = dr["blood_code"].ToString();
                                if (dr["worker_resignstatus"].ToString().Equals("1"))
                                {
                                    model.worker_resigndate = Convert.ToDateTime(dr["worker_resigndate"]);
                                    model.worker_resignstatus = true;
                                    model.worker_resignreason = dr["worker_resignreason"].ToString();
                                }
                                else
                                {
                                    model.worker_resignstatus = false;
                                }
                                model.worker_probationday = Convert.ToDouble(dr["worker_probationday"]);
                                model.worker_probationdate = Convert.ToDateTime(dr["worker_probationdate"]);
                                model.worker_probationenddate = Convert.ToDateTime(dr["worker_probationenddate"]);
                                model.hrs_perday = Convert.ToDouble(dr["hrs_perday"]);
                                model.worker_taxmethod = dr["worker_taxmethod"].ToString();
                                model.self_admin = false;

                                model.modified_by = by;

                                string strID = objWorker.insert(model);

                                if (!strID.Equals(""))
                                {
                                    success++;
                                }
                                else
                                {
                                    objStr.Append(model.worker_code);
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
