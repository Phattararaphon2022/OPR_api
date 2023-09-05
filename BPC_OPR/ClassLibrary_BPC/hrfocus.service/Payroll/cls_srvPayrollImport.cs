using ClassLibrary_BPC.hrfocus.controller.Payroll;
using ClassLibrary_BPC.hrfocus.model.Payroll;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary_BPC.hrfocus.service.Payroll
{
    public class cls_srvPayrollImport
    {
        public string Error = "";
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
                Error = ex.ToString();
            }
            finally
            {
                xlConn.Close();
            }

            return dt;
        }
        public string checkshiftemty(string val)
        {
            string result = "00:00";
            if (val.Equals(""))
            {
                return result;
            }
            try
            {
                var resultcal = val.Split(' ')[1].Split(':');
                result = resultcal[0] + ":" + resultcal[1];
            }
            catch
            {
                result = val;
            }
            return result;
        }
        public string doImportExcel(string type, string filename, string by)
        {
            string strResult = "";

            try
            {

                int success = 0;
                StringBuilder objStr = new StringBuilder();
                DataTable dt = doReadExcel(filename);
                switch (type)
                {
                    #region PERIOD
                    case "PERIOD":
                        if (dt.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dt.Rows)
                            {

                                cls_ctMTPeriods controller = new cls_ctMTPeriods();
                                cls_MTPeriods model = new cls_MTPeriods();

                                model.company_code = dr["company_code"].ToString();
                                model.period_id = dr["period_id"].ToString().Equals("") ? 0 : Convert.ToInt32(dr["period_id"].ToString());
                                model.period_type = dr["period_type"].ToString();
                                model.emptype_code = dr["emptype_code"].ToString();
                                model.year_code = dr["year_code"].ToString();
                                model.period_no = dr["period_no"].ToString();
                                model.period_name_th = dr["period_name_th"].ToString();
                                model.period_name_en = dr["period_name_en"].ToString();
                                model.period_from = Convert.ToDateTime(dr["period_from"].ToString());
                                model.period_to = Convert.ToDateTime(dr["period_to"].ToString());
                                model.period_payment = Convert.ToDateTime(dr["period_payment"].ToString());
                                model.period_dayonperiod = dr["period_dayonperiod"].ToString().Equals("1") ? true : false;
                                model.modified_by = by;
                                model.flag = false;
                                string strID = controller.insert(model);
                                if (!strID.Equals(""))
                                {
                                    success++;
                                }
                                else
                                {
                                    objStr.Append(model.period_id + " " + model.period_no);
                                }

                            }

                            strResult = "";

                            if (success > 0)
                                strResult += "Success : " + success.ToString();

                            if (objStr.Length > 0)
                                strResult += " Fail : " + objStr.ToString();

                        }
                        break;
                    #endregion

                    #region taxrate
                    case "Taxrate":
                        if (dt.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dt.Rows)
                            {

                                cls_ctTRTaxrate controller = new cls_ctTRTaxrate();
                                cls_TRTaxrate model = new cls_TRTaxrate();
                                model.company_code = dr["company_code"].ToString();
                                model.taxrate_id = dr["taxrate_id"].ToString().Equals("") ? 0 : Convert.ToInt32(dr["taxrate_id"].ToString());
                                model.taxrate_from = Convert.ToDouble(dr["taxrate_from"]);
                                model.taxrate_to = Convert.ToDouble(dr["taxrate_to"]);
                                model.taxrate_tax = Convert.ToDouble(dr["taxrate_tax"]);

                                model.modified_by = by;
                                model.flag = false;
                                string strID = controller.insert(model);
                                if (!strID.Equals(""))
                                {
                                    success++;
                                }
                                else
                                {
                                    objStr.Append(model.company_code);
                           
                                }

                            }

                            strResult = "";

                            if (success > 0)
                                strResult += "Success : " + success.ToString();

                            if (objStr.Length > 0)
                                strResult += " Fail : " + objStr.ToString();

                        }
                        break;

                    #endregion

                    #region MTItem
                    case "MTItem":
                        if (dt.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dt.Rows)
                            {

                                cls_ctMTItem controller = new cls_ctMTItem();
                                cls_MTItem model = new cls_MTItem();

                                model.company_code = dr["company_code"].ToString();
                                model.item_id = dr["item_id"].ToString().Equals("") ? 0 : Convert.ToInt32(dr["item_id"].ToString());

                                model.item_code = dr["item_code"].ToString();
                                model.item_name_th = dr["item_name_th"].ToString();
                                model.item_name_en = dr["item_name_en"].ToString();

                                model.item_type = dr["item_type"].ToString();
                                model.item_regular = dr["item_regular"].ToString();

                                model.item_caltax = dr["item_caltax"].ToString().Equals("") ? "N" : dr["item_caltax"].ToString();
                                model.item_calpf = dr["item_calpf"].ToString().Equals("") ? "N" : dr["item_calpf"].ToString();
                                model.item_calsso = dr["item_calsso"].ToString().Equals("") ? "N" : dr["item_calsso"].ToString();
                                model.item_calot = dr["item_calot"].ToString().Equals("") ? "N" : dr["item_calot"].ToString();
                                model.item_allowance = dr["item_allowance"].ToString().Equals("") ? "N" : dr["item_allowance"].ToString();
                                model.item_contax = dr["item_contax"].ToString().Equals("") ? "N" : dr["item_contax"].ToString();
                                model.item_section = dr["item_section"].ToString();
                                model.item_rate = dr["item_rate"].ToString().Equals("") ? 0 : Convert.ToDouble(dr["item_rate"].ToString());

                                model.modified_by = by;
                                model.flag = false;
                                string strID = controller.insert(model);
                                if (!strID.Equals(""))
                                {
                                    success++;
                                }
                                else
                                {
                                    objStr.Append(model.item_id );
                                }

                            }

                            strResult = "";

                            if (success > 0)
                                strResult += "Success : " + success.ToString();

                            if (objStr.Length > 0)
                                strResult += " Fail : " + objStr.ToString();

                        }
                        break;
                    #endregion

                    #region Provident
                    case "Provident":
                        if (dt.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dt.Rows)
                            {

                                cls_ctMTProvident controller = new cls_ctMTProvident();
                                cls_MTProvident model = new cls_MTProvident();

                                model.company_code = Convert.ToString(dr["company_code"]);
                                model.provident_id = dr["provident_id"].ToString().Equals("") ? 0 : Convert.ToInt32(dr["provident_id"].ToString());

                                model.provident_code = Convert.ToString(dr["provident_code"]);
                                model.provident_name_th = Convert.ToString(dr["provident_name_th"]);
                                model.provident_name_en = Convert.ToString(dr["provident_name_en"]);

 
                                model.modified_by = by;
                                model.flag = false;
                                bool strID = controller.insert(model);
                                if (!strID.Equals(""))
                                {
                                    success++;
                                }
                                else
                                {
                                    objStr.Append(model.provident_id);
                                }

                            }

                            strResult = "";

                            if (success > 0)
                                strResult += "Success : " + success.ToString();

                            if (objStr.Length > 0)
                                strResult += " Fail : " + objStr.ToString();

                        }
                        break;
                    #endregion

                    #region Bonus
                    case "Bonus":
                        if (dt.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dt.Rows)
                            {

                                cls_ctMTBonus controller = new cls_ctMTBonus();
                                cls_MTBonus model = new cls_MTBonus();

                                model.bonus_id = dr["bonus_id"].ToString().Equals("") ? 0 : Convert.ToInt32(dr["bonus_id"].ToString());


                                model.company_code = Convert.ToString(dr["company_code"]);
                                model.bonus_code = Convert.ToString(dr["bonus_code"]);
                                model.bonus_name_th = Convert.ToString(dr["bonus_name_th"]);
                                model.bonus_name_en = Convert.ToString(dr["bonus_name_en"]);

                                model.item_code = Convert.ToString(dr["item_code"]);

                                


                                model.modified_by = by;
                                model.flag = false;
                                string strID = controller.insert(model);
                                if (!strID.Equals(""))
                                {
                                    success++;
                                }
                                else
                                {
                                    objStr.Append(model.bonus_id);
                                }

                            }

                            strResult = "";

                            if (success > 0)
                                strResult += "Success : " + success.ToString();

                            if (objStr.Length > 0)
                                strResult += " Fail : " + objStr.ToString();

                        }
                        break;
                    #endregion

                    #region itemsplan
                    case "PLANITEMS":
                        if (dt.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dt.Rows)
                            {

                                cls_ctMTPlanitems controller = new cls_ctMTPlanitems();
                                cls_MTPlanitems model = new cls_MTPlanitems();

                                model.company_code = dr["company_code"].ToString();
                                model.planitems_id = dr["planitems_id"].ToString().Equals("") ? 0 : Convert.ToInt32(dr["planitems_id"].ToString());
                                model.planitems_code = dr["planitems_code"].ToString();
                                model.planitems_name_th = dr["planitems_name_th"].ToString();
                                model.planitems_name_en = dr["planitems_name_en"].ToString();
                                model.modified_by = by;
                                model.flag = false;
                                string strID = controller.insert(model);
                                if (!strID.Equals(""))
                                {
                                    success++;
                                }
                                else
                                {
                                    objStr.Append(model.planitems_id + " " + model.planitems_code);
                                }

                            }

                            strResult = "";

                            if (success > 0)
                                strResult += "Success : " + success.ToString();

                            if (objStr.Length > 0)
                                strResult += " Fail : " + objStr.ToString();

                        }
                        break;
                    #endregion

                    #region planreduce
                    case "PLANREDUCE":
                        if (dt.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dt.Rows)
                            {

                                cls_ctMTPlanreduce controller = new cls_ctMTPlanreduce();
                                cls_MTPlanreduce model = new cls_MTPlanreduce();

                                model.company_code = dr["company_code"].ToString();
                                model.planreduce_id = dr["planreduce_id"].ToString().Equals("") ? 0 : Convert.ToInt32(dr["planreduce_id"].ToString());
                                model.planreduce_code = dr["planreduce_code"].ToString();
                                model.planreduce_name_th = dr["planreduce_name_th"].ToString();
                                model.planreduce_name_en = dr["planreduce_name_en"].ToString();
                                model.modified_by = by;
                                model.flag = false;
                                string strID = controller.insert(model);
                                if (!strID.Equals(""))
                                {
                                    success++;
                                }
                                else
                                {
                                    objStr.Append(model.planreduce_id + " " + model.planreduce_code);
                                }

                            }

                            strResult = "";

                            if (success > 0)
                                strResult += "Success : " + success.ToString();

                            if (objStr.Length > 0)
                                strResult += " Fail : " + objStr.ToString();

                        }
                        break;
                    #endregion

                    #region  TRPayitem
                    case "PAYITEM":
                        if (dt.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dt.Rows)
                            {

                                cls_ctTRPayitem controller = new cls_ctTRPayitem();
                                cls_TRPayitem model = new cls_TRPayitem();

                                model.company_code = dr["company_code"].ToString();
                                model.worker_code = dr["worker_code"].ToString();
                                model.payitem_date = Convert.ToDateTime(dr["payitem_date"].ToString());

                                 model.payitem_amount = dr["payitem_amount"].ToString().Equals("") ? 0 : Convert.ToDouble(dr["payitem_amount"].ToString());
                                 model.payitem_quantity = dr["payitem_quantity"].ToString().Equals("") ? 0 : Convert.ToDouble(dr["payitem_quantity"].ToString());
                                 model.item_code = dr["item_code"].ToString();

                                 model.payitem_paytype = dr["payitem_paytype"].ToString();

 
                                model.payitem_note = dr["payitem_note"].ToString();

                                model.modified_by = by;
                                model.flag = false;
                                bool strID = controller.insert(model);
                                if (!strID.Equals(""))
                                {
                                    success++;
                                }
                                else
                                {
                                    objStr.Append(model.worker_code + " " + model.worker_code);
                                }

                            }

                            strResult = "";

                            if (success > 0)
                                strResult += "Success : " + success.ToString();

                            if (objStr.Length > 0)
                                strResult += " Fail : " + objStr.ToString();

                        }
                        break;
                    #endregion



                    case "REASONs":
                        break;

                }

            }
            catch (Exception ex)
            {
                strResult = ex.ToString();
            }
            if (!Error.Equals(""))
            {
                strResult = Error;
            }
            return strResult;
        }
    }
}
