using ClassLibrary_BPC.hrfocus.controller;
using ClassLibrary_BPC.hrfocus.controller.Payroll;
using ClassLibrary_BPC.hrfocus.model;
using ClassLibrary_BPC.hrfocus.model.Payroll;
using ClassLibrary_BPC.hrfocus.model.System;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary_BPC.hrfocus.service.Payroll
{
    public class cls_srvProcessPayroll
    {

        private void addTimeManual(ref List<cls_TRTimeinput> listTimeinput, DateTime date, string time)
        {
            cls_TRTimeinput model = new cls_TRTimeinput();
            model.timeinput_hhmm = time;
            model.timeinput_compare = "N";
            model.timeinput_function = "RECORD";
            model.timeinput_date = date;

            listTimeinput.Add(model);
        }

        private int doConvertTime2Int(string value)
        {
            int intResult = 0;
            try
            {
                intResult = Convert.ToInt32(value);
            }
            catch { }

            return intResult;
        }

        public string doCalculateTax(string com, string taskid)
        {
            string strResult = "";

            cls_ctConnection obj_conn = new cls_ctConnection();

            try
            {

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_conn.doConnect();

                obj_str.Append(" EXEC [dbo].[PAY_PRO_JOBTAX] '" + com + "', '" + taskid + "' ");

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());
                obj_cmd.CommandType = CommandType.Text;

                int intCountSuccess = obj_cmd.ExecuteNonQuery();

                if (intCountSuccess > 0)
                {
                    //obj_conn.doCommit();
                    strResult = "Success::" + intCountSuccess.ToString();
                }

            }
            catch (Exception ex)
            {

            }

            return strResult;
        }

        public string doCalculateIncomeDeduct(string com, string taskid)
        {
            string strResult = "";

            cls_ctConnection obj_conn = new cls_ctConnection();

            try
            {

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_conn.doConnect();

                obj_str.Append(" EXEC [dbo].[PAY_PRO_JOBINDE] '" + com + "', '" + taskid + "' ");

                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());
                obj_cmd.CommandType = CommandType.Text;

                int intCountSuccess = obj_cmd.ExecuteNonQuery();

                if (intCountSuccess > 0)
                {
                    //obj_conn.doCommit();
                    strResult = "Success::" + intCountSuccess.ToString();
                }

            }
            catch (Exception ex)
            {

            }

            return strResult;
        }

        public string doCalculateBonus(string com, string taskid)
        {
            string strResult = "";

            cls_ctConnection obj_conn = new cls_ctConnection();

            try
            {
                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_conn.doConnect();

                obj_str.Append(" EXEC [dbo].[PAY_PRO_CALBONUS] '" + com + "', '" + taskid + "' ");
                SqlCommand obj_cmd = new SqlCommand(obj_str.ToString(), obj_conn.getConnection());

                obj_cmd.CommandType = CommandType.Text;

                int intCountSuccess = obj_cmd.ExecuteNonQuery();

                if (intCountSuccess > 0)
                {
                    strResult = "Success::" + intCountSuccess.ToString();
                }
            }
            catch (Exception ex)
            {

            }

            return strResult;
        }

        #region SSO
        //SSO   
        public string doExportSso(string com, string taskid)
        {
            string strResult = "";

            cls_ctMTTask objMTTask = new cls_ctMTTask();
            List<cls_MTTask> listMTTask = objMTTask.getDataByFillter(com, taskid, "TRN_SSO", "");
            List<string> listError = new List<string>();

            if (listMTTask.Count > 0)
            {
                cls_MTTask task = listMTTask[0];

                task.task_start = DateTime.Now;

                cls_ctMTTask objTaskDetail = new cls_ctMTTask();
                cls_TRTaskdetail task_detail = objTaskDetail.getTaskDetail(task.task_id.ToString());

                cls_ctMTTask objTaskWhose = new cls_ctMTTask();
                List<cls_TRTaskwhose> listWhose = objTaskWhose.getTaskWhose(task.task_id.ToString());

                DateTime dateEff = task_detail.taskdetail_fromdate;
                DateTime datePay = task_detail.taskdetail_paydate;

                StringBuilder objStr = new StringBuilder();
                foreach (cls_TRTaskwhose whose in listWhose)
                {
                    objStr.Append("'" + whose.worker_code + "',");
                }

                string strEmp = objStr.ToString().Substring(0, objStr.ToString().Length - 1);



                //-- Get worker
                cls_ctMTWorker objWorker = new cls_ctMTWorker();
                List<cls_MTWorker> list_worker = objWorker.getDataMultipleEmp(com, strEmp);

                //-- Step 2 Get Paytran cls_ctTRPaytran
                cls_ctTRPaytran objPay = new cls_ctTRPaytran();
                List<cls_TRPaytran> list_paytran = objPay.getDataMultipleEmp("TH", com, datePay, datePay, strEmp);
                cls_TRPaytran Paytran = list_paytran[0];


                //-- Step 3 Get Company acc
                cls_ctMTCombank objCombank = new cls_ctMTCombank();
                List<cls_MTCombank> list_combank = objCombank.getDataByFillter("", com);
                cls_MTCombank combank = list_combank[0];

                //-- Step 4 Get Company detail
                cls_ctMTCompany objCom = new cls_ctMTCompany();
                List<cls_MTCompany> list_com = objCom.getDataByFillter("", com);
                cls_MTCompany comdetail = list_com[0];

                //-- Step 5 Get Emp address
                cls_ctTRAddress objEmpadd = new cls_ctTRAddress();
                List<cls_TRAddress> list_empaddress = objEmpadd.getDataByFillter(com, strEmp);

                //-- Step 6 GetCompany detail
                cls_ctTRCard objEmpcard = new cls_ctTRCard();
                List<cls_TRCard> list_empcard = objEmpcard.getDataTaxMultipleEmp(com, strEmp);

                //-- Step 7 Get Company card
                cls_ctTRCard objComcard = new cls_ctTRCard();
                List<cls_TRCard> list_comcard = objComcard.getDataByFillter(com, "NTID", "", "", "");
                cls_TRCard comcard = list_comcard[0];

                //cls_ctMTCompany objComcard = new cls_ctMTCompany();
                //List<cls_MTCompany> list_comcard = objComcard.getDataByFillter(com, strEmp);
                //cls_MTCompany comcard = list_comcard[0];

                cls_ctMTProvince objProvince = new cls_ctMTProvince();
                List<cls_MTProvince> list_province = objProvince.getDataByFillter("");

                cls_ctTREmpbranch objEmpbranch = new cls_ctTREmpbranch();
                List<cls_TREmpbranch> list_Empbranch = objEmpbranch.getDataTaxMultipleEmp(com, strEmp);
                cls_TREmpbranch comEmpbranch = list_Empbranch[0];


                string tmpData = "";
                string bkData = "";
                string tmpDatas = "";
                string sequence = "000001";


                if (list_paytran.Count > 0)
                {

                    double douTotal = 0;

                    double totalIncome = 0;
                    double totalIncome2 = 0;
                    double totalIncome3 = 0;
                    double totalIncome4 = 0;

                    int index = 0;
                    foreach (cls_TRPaytran paytran in list_paytran)
                    {

                        string empname = "";

                        cls_MTWorker obj_worker = new cls_MTWorker();
                        cls_TRAddress obj_address = new cls_TRAddress();
                        cls_MTProvince obj_province = new cls_MTProvince();
                        cls_TRCard obj_card = new cls_TRCard();
                        cls_TREmpbranch obj_empbranch = new cls_TREmpbranch();

                        foreach (cls_MTWorker worker in list_worker)
                        {
                            if (paytran.worker_code.Equals(worker.worker_code))
                            {
                                empname = worker.initial_name_en + " " + worker.worker_fname_en + " " + worker.worker_lname_en;
                                obj_worker = worker;
                                break;
                            }
                        }

                        foreach (cls_TRAddress address in list_empaddress)
                        {
                            if (paytran.worker_code.Equals(address.worker_code))
                            {
                                obj_address = address;
                                break;
                            }
                        }

                        foreach (cls_TRCard card in list_empcard)
                        {
                            if (paytran.worker_code.Equals(card.worker_code))
                            {
                                obj_card = card;
                                break;
                            }
                        }

                        string spare1 = "";
                        if (comdetail.company_name_en.Length > 45)
                            comdetail.company_name_en = comdetail.company_name_en.Substring(0, 45);
                        else if (comdetail.company_name_en.Length < 45)
                            comdetail.company_name_en = comdetail.company_name_en.PadRight(45, ' ');
                        if (spare1.Length < 45)
                            spare1 = spare1.PadRight(45, '0');


                        int record = list_paytran.Count;
                        sequence = (index + 2).ToString().PadLeft(6, '0');




                        //foreach (cls_TRPaytran paytrans in list_paytran)
                        //{
                        //    totalIncome += paytran.paytran_income_total;
                        //}








                        //"Header Record  (ส่วนที่ 1)"     
                        if (empname.Equals("") || obj_worker.worker_cardno.Equals(""))
                            continue;
                        if (paytran.paytran_income_401 > 0)
                        {

                            //1.ประเภทข้อมูล กำหนดให้เป็น " 1  " เสมอ
                            tmpDatas = "1";

                            //2.เลขที่บัญชีนายจ้าง
                            if (combank.combank_bankaccount.Length == 10)
                                tmpDatas += combank.combank_bankaccount;
                            else
                                tmpDatas += combank.combank_bankaccount;

                            //3.ลำดับที่สาขา ตามที่ สปส.กำหนด
                            if (comEmpbranch.branch_code.Length == 6)
                                tmpDatas += comEmpbranch.branch_code;
                            else
                                tmpDatas += comEmpbranch.branch_code;

                            //4.วันที่ชำระเงิน <>PAYTRAN_PAYDATE
                            if (Paytran.paytran_date.ToString("ddMMyy").Length == 6)
                                tmpDatas += Paytran.paytran_date.ToString("ddMMyy");
                            else
                                tmpDatas += Paytran.paytran_date.ToString("ddMMyy");

                            //5.งวดค่าจ้าง<>	
                            tmpDatas += datePay.ToString("MM") + dateEff.ToString("yy");

                            //6.ชื่อสถานประกอบการ 
                            tmpDatas += comdetail.company_name_en;

                            //7.อัตราเงินสมทบ
                            if (Paytran.paytran_ssocom == 4)
                                tmpDatas += Paytran.paytran_ssocom;
                            else
                                tmpDatas += Paytran.paytran_ssocom.ToString("0000");

                            //bkData += paybank.paytran_ssocom + "|";

                            //8.จำนวนผู้ประกันตน			
                            if (record == 6)
                                tmpDatas += record;
                            else
                                tmpDatas += record.ToString("000000");

                            //9.ค่าจ้างรวม PAYTRAN_INCOME_TOTAL

                            totalIncome += paytran.paytran_income_total;
                            if (totalIncome == 15)
                                tmpDatas += totalIncome;
                            else
                                tmpDatas += totalIncome.ToString("000000000000000");

                            //10.เงินสมทบรวม
                            totalIncome2 += paytran.paytran_pfcom;

                            if (totalIncome2 == 14)
                                tmpDatas += totalIncome2;
                            else
                                tmpDatas += totalIncome2.ToString("00000000000000");



                            //if (paytran.paytran_pfcom == 14)
                            //    tmpDatas += paytran.paytran_pfcom + " ";
                            //else
                            //    tmpDatas += paytran.paytran_pfcom.ToString("00000000000000") + " ";

                            //11.เงินสมทบรวมส่วนผปต. 
                            totalIncome3 += paytran.paytran_income_notax;
                            if (totalIncome3 == 12)
                                tmpDatas += totalIncome3;
                            else
                                tmpDatas += totalIncome3.ToString("000000000000");



                            //if (paytran.paytran_income_notax == 12)
                            //    tmpDatas += paytran.paytran_income_notax + " ";
                            //else
                            //    tmpDatas += paytran.paytran_income_notax.ToString("000000000000") + " ";

                            //12.เงินสมทบส่วนนายจ้าง <Poscod>
                            totalIncome4 += paytran.paytran_tax_401;
                            if (totalIncome4 == 12)
                                tmpDatas += totalIncome4;
                            else
                                tmpDatas += totalIncome4.ToString("000000000000");



                            //if (paytran.paytran_tax_401 == 12)
                            //    tmpDatas += paytran.paytran_tax_401 + " ";
                            //else
                            //    tmpDatas += paytran.paytran_tax_401.ToString("000000000000") + " ";

                            //tmpData += tmpDatas.PadRight(130, ' ') + "\r\n";

                            //tmpData += tmpDatas.PadRight(130, ' ') + "\r\n";

                            douTotal += paytran.paytran_netpay_b;

                            //int record = list_paytran.Count;

                        }


                        string spare = "";



                        // ตรวจสอบและปรับขนาด worker_fname_en
                        if (obj_worker.worker_fname_en.Length > 25)
                        {
                            obj_worker.worker_fname_en = obj_worker.worker_fname_en.Substring(0, 25);
                        }
                        else if (obj_worker.worker_fname_en.Length < 25)
                        {
                            obj_worker.worker_fname_en = obj_worker.worker_fname_en.PadRight(25, ' ');
                        }

                        // ตรวจสอบและปรับขนาด worker_lname_en
                        if (obj_worker.worker_lname_en.Length > 25)
                        {
                            obj_worker.worker_lname_en = obj_worker.worker_lname_en.Substring(0, 25);
                        }
                        else if (obj_worker.worker_lname_en.Length < 25)
                        {
                            obj_worker.worker_lname_en = obj_worker.worker_lname_en.PadRight(25, ' ');
                        }

                        // ตรวจสอบและปรับขนาด spare
                        if (spare.Length < 65)
                        {
                            spare = spare.PadRight(65, '0');
                        }
                        //////////////////////////////////
                        //Detail Record 2
                        if (empname.Equals("") || obj_worker.worker_cardno.Equals(""))
                            continue;

                        if (paytran.paytran_income_401 > 0)
                        {
                            //1.ประเภทข้อมูล
                            bkData = "2";


                            //2.เลขที่บัตรประชาชนobj_worker

                            if (obj_worker.worker_cardno.Length == 13)
                                bkData += obj_worker.worker_cardno;
                            else
                                bkData += "0000000000000";

                            //3.คำนำหน้าชื่อ
                            if (obj_worker.worker_initial.Length == 3)
                                bkData += obj_worker.worker_initial;
                            else
                                bkData += "000";

                            //4.ชื่อผู้ประกันตน 
                            bkData += obj_worker.worker_fname_en;

                            //5.นามสกุลผู้ประกันตน

                            bkData += obj_worker.worker_lname_en;

                            //6.ค่าจ้าง
                            if (paytran.paytran_income_total == 14)
                                bkData += paytran.paytran_income_total;
                            else
                                bkData += paytran.paytran_income_total.ToString("00000000000000");

                            //7.จำนวนเงินสมทบ
                            if (paytran.paytran_pfemp == 12)
                                bkData += paytran.paytran_pfemp;
                            else
                                bkData += paytran.paytran_pfemp.ToString("000000000000");

                            //8.คอลัมน์ว่าง			                          
                            bkData += " ";

                            Console.WriteLine("bkData: " + bkData);
                            Console.WriteLine("tmpDatas: " + tmpDatas);


                            tmpData += bkData.PadRight(135, ' ') + "\r\n";
                        }
                        //douTotal += paytran.paytran_netpay_b;

                        //index++;
                        //int record = list_paytran.Count;
                    }

                    try
                    {
                        //-- Step 1 create file
                        //string filename = "UPAYDAT.DAT";

                        string filename = "TRN_SSO_" + DateTime.Now.ToString("yyMMddHHmm") + "." + "txt";
                        string filepath = Path.Combine
                       (ClassLibrary_BPC.Config.PathFileExport, filename);

                        // Check if file already exists. If yes, delete it.     
                        if (File.Exists(filepath))
                        {
                            File.Delete(filepath);
                        }

                        // Create a new file     
                        using (FileStream fs = File.Create(filepath))
                        {
                            // Add some text to file (ชุดแรก)
                            Byte[] title = new UTF8Encoding(true).GetBytes(tmpDatas.PadRight(130, ' ') + "\r\n");
                            fs.Write(title, 0, title.Length);

                            // Add some text to file (ชุดที่สอง)
                            Byte[] dataBytes = new UTF8Encoding(true).GetBytes(tmpData);
                            fs.Write(dataBytes, 0, dataBytes.Length);
                        }


                        //using (FileStream fs = File.Create(filepath))
                        //{
                        //    // Add some text to file    
                        //    Byte[] title = new UTF8Encoding(true).GetBytes(tmpData);
                        //    fs.Write(title, 0, title.Length);

                        //}


                        strResult = filepath;

                    }
                    catch
                    {
                        strResult = "";
                    }
                }

                task.task_end = DateTime.Now;
                task.task_status = "F";
                task.task_note = strResult;
                objMTTask.updateStatus(task);

            }
            else
            {

            }
            return strResult;
        }
        //            try
        //            {
        //                //-- Step 1 create file
        //                string filename = "TRN_SSO_" + DateTime.Now.ToString("yyMMddHHmm") + "." + "xls";
        //                string filepath = Path.Combine
        //               (ClassLibrary_BPC.Config.PathFileExport, filename);



        //                // Check if file already exists. If yes, delete it.     
        //                if (File.Exists(filepath))
        //                {
        //                    File.Delete(filepath);
        //                }
        //                DataSet ds = new DataSet();
        //                string str = tmpData.Replace("\r\n", "]");
        //                string[] data = str.Split(']');
        //                DataTable dataTable = ds.Tables.Add();
        //                dataTable.Columns.AddRange(new DataColumn[12] { new DataColumn("ลำดับที่"), new DataColumn("เดือน / ปี"), new DataColumn("เลขประจำตัวประชาชน"), new DataColumn("ชื่อ-นามสกุล"), new DataColumn("กยศ."), new DataColumn("กรอ."), new DataColumn("จำนวนเงิน"), new DataColumn("ยอดยืนยันนำส่ง"), new DataColumn("วันที่หักเงินเดือน"), new DataColumn("ไม่ได้นำส่งเงิน"), new DataColumn("รหัสสาเหตุ"), new DataColumn("ไฟล์แนบ") });
        //                foreach (var i in data)
        //                {
        //                    if (i.Equals(""))
        //                        continue;
        //                    string[] array = i.Split('|');
        //                    dataTable.Rows.Add(array[0], array[1], array[2], array[3], array[4], array[5], array[6], array[7], array[8], array[9], array[10], array[11]);
        //                }
        //                ExcelLibrary.DataSetHelper.CreateWorkbook(filepath, ds);

        //                // Create a new file     
        //                //using (FileStream fs = File.Create(filepath))
        //                //{
        //                //    // Add some text to file    
        //                //    Byte[] Table = new UTF8Encoding(true).GetBytes(tmpData);
        //                //    fs.Write(Table, 0, Table.Length);


        //                //}

        //                strResult = filepath;

        //            }
        //            catch (Exception ex)
        //            {
        //                strResult = ex.ToString();
        //            }

        //        }


        //        task.task_end = DateTime.Now;
        //        task.task_status = "F";
        //        task.task_note = strResult;
        //        objMTTask.updateStatus(task);

        //    }
        //    else
        //    {

        //    }

        //    return strResult;
        //}
        #endregion

        #region TRN_BANK

        public string doExportBank(string com, string taskid)
        {

            string strResult = "";

            cls_ctMTTask objMTTask = new cls_ctMTTask();
            List<cls_MTTask> listMTTask = objMTTask.getDataByFillter(com, taskid, "TRN_BANK", "");
            List<string> listError = new List<string>();

            if (listMTTask.Count > 0)
            {
                cls_MTTask task = listMTTask[0];

                task.task_start = DateTime.Now;

                cls_ctMTTask objTaskDetail = new cls_ctMTTask();
                cls_TRTaskdetail task_detail = objTaskDetail.getTaskDetail(task.task_id.ToString());

                cls_ctMTTask objTaskWhose = new cls_ctMTTask();
                List<cls_TRTaskwhose> listWhose = objTaskWhose.getTaskWhose(task.task_id.ToString());

                DateTime dateEff = task_detail.taskdetail_fromdate;
                DateTime datePay = task_detail.taskdetail_paydate;
                //ตัวเช็คธนาคาร
                string[] task_bank = task_detail.taskdetail_process.Split('|');

                StringBuilder objStr = new StringBuilder();
                foreach (cls_TRTaskwhose whose in listWhose)
                {
                    objStr.Append("'" + whose.worker_code + "',");
                }

                string strEmp = objStr.ToString().Substring(0, objStr.ToString().Length - 1);

                //-- Get worker
                cls_ctMTWorker objWorker = new cls_ctMTWorker();
                List<cls_MTWorker> list_worker = objWorker.getDataMultipleEmp(com, strEmp);

                //-- Step 2 Get Paytran
                cls_ctTRPaytran objPay = new cls_ctTRPaytran();
                List<cls_TRPaytran> list_paytran = objPay.getDataMultipleEmp("TH", com,  datePay,dateEff, strEmp);

                //-- Step 3 Get Company acc
                cls_ctMTCombank objCombank = new cls_ctMTCombank();
                List<cls_MTCombank> list_combank = objCombank.getDataByFillter("", com);
                cls_MTCombank combank = list_combank[0];

                //-- Step 4 Get Company detail
                cls_ctMTCompany objCom = new cls_ctMTCompany();
                List<cls_MTCompany> list_com = objCom.getDataByFillter("", com);
                cls_MTCompany comdetail = list_com[0];


                //-- Step 5 Get Emp acc
                cls_ctTRBank objEmpbank = new cls_ctTRBank();
                List<cls_TRBank> list_empbank = objEmpbank.getDataMultipleEmp(com, strEmp);
                cls_TRBank bank = list_empbank[0];

                //-- Step 6 Get Branch
                cls_ctMTCombranch objcombranch = new cls_ctMTCombranch();
                List<cls_MTCombranch> list_combranch = objcombranch.getDataTaxMultipleCombranch(com);
                cls_MTCombranch combranch = list_combranch[0];

                string tmpData = "";
                string sequence = "000001";
                //string beneref = "001";
                string sequence2 = "000001";
                string sequence1 = "000001";
                string spare = "";

                string spare1 = "";

                string departmentcode = "";
                string user = "";
                string spare2 = "";
                string spare3 = "";
                 string empacc1 = "";
                string empaccbank = "";
                if (list_paytran.Count > 0)
                {

                    // ตรวจสอบความยาวของ comdetail.company_name_en หากมากกว่า 25 ตัวอักษรให้ตัดทิ้งเหลือเพียง 25 ตัวอักษร
                    if (comdetail.company_name_en.Length > 25)
                        comdetail.company_name_en = comdetail.company_name_en.Substring(0, 25);
                    // ตรวจสอบความยาวของ comdetail.company_name_en หากน้อยกว่า 25 ตัวอักษรให้เติมด้วยช่องว่างจนครบ 25 ตัวอักษร
                    else if (comdetail.company_name_en.Length < 25)
                        comdetail.company_name_en = comdetail.company_name_en.PadRight(25, ' ');
                    if (spare.Length < 77)
                        spare = spare.PadRight(77, '0');



                    if (spare1.Length < 32)
                        spare1 = spare1.PadRight(32, '0');


                    //task_detail.taskdetail_process.Split('|');
                    if (task_bank.Length > 0)
                    {
                        switch (task_detail.taskdetail_process )
                        {
                            case "002":/// ธนาคารกรุงเทพ
                               {
                                   foreach (cls_TRBank workerlist in list_empbank)
                                   {
                                       if (combank.combank_bankcode.Equals(workerlist.bank_code))
                                       {
                                           {
                                               //กำหนดค่า tmpData โดยรวมข้อมูล combank.combank_bankcode และ comdetail.company_name_en
                                               tmpData = "H" + "000001" + combank.combank_bankcode + combank.combank_bankaccount + comdetail.company_name_en + datePay.ToString("ddMMyy", DateTimeFormatInfo.CurrentInfo) + spare + "\r\n";

                                               double douTotal = 0;
                                               double douTotal2 = 0;

                                               double douTotal3 = 0;

                                               int index = 0;
                                               string amount;
                                               string bkData;

                                               foreach (cls_TRPaytran paytran in list_paytran)
                                               {

                                                   string empacc = "";
                                                   string empname = "";
                                                   string empname2 = "";

                                                   cls_TRBank obj_Empbank = new cls_TRBank();

                                                   foreach (cls_MTWorker worker in list_worker)
                                                   {
                                                       if (paytran.worker_code.Equals(worker.worker_code))
                                                       {
                                                           foreach (cls_TRBank workerlistt in list_empbank)
                                                           {
                                                               if (worker.worker_code.Equals(workerlistt.worker_code) && workerlistt.bank_code.Equals(combank.combank_bankcode))
                                                               {
                                                                   // ทำงานตามที่ต้องการเมื่อพบข้อมูลที่ตรง
                                                                   empname = " " + worker.initial_name_en + " " + worker.worker_fname_en + " " + worker.worker_lname_en + " " + datePay.ToString("ddMMyy", DateTimeFormatInfo.CurrentInfo) + " ";
                                                                   empname2 = " " + worker.initial_name_en + " " + worker.worker_fname_en + " " + worker.worker_lname_en + " ";

                                                                   break;
                                                               }
                                                           }
                                                       }
                                                   }

                                                   foreach (cls_TRBank worker in list_empbank)
                                                   {
                                                       if (paytran.worker_code.Equals(worker.worker_code))
                                                       {
                                                           empacc = worker.bank_account.Replace("-", "");
                                                           break;
                                                       }
                                                   }



                                                   //"Header Record  (ส่วนที่ 1)"   
                                                   if (empname.Equals("") || empacc.Equals(""))
                                                       continue;


                                                   sequence = Convert.ToString(index + 1).PadLeft(6, '0');

                                                   decimal temp = (decimal)paytran.paytran_netpay_b;
                                                   amount = temp.ToString("#.#0").Trim().Replace(".", "").PadLeft(10, '0');

                                                   if (spare1.Length < 32)
                                                       spare1 = spare1.PadRight(32, '0');

                                                   if (departmentcode.Length < 4)
                                                       departmentcode = departmentcode.PadRight(4, '0');

                                                   if (user.Length < 10)
                                                       user = user.PadRight(10, '0');

                                                   if (spare2.Length < 13)
                                                       spare2 = spare2.PadRight(13, '0');


                                                   if (spare1.Length < 32)
                                                       spare1 = spare1.PadRight(32, '0');


                                                   foreach (cls_TRBank worker in list_empbank)
                                                   {
                                                       if (paytran.worker_code.Equals(worker.worker_code))
                                                       {
                                                           if (worker.bank_percent != 0)
                                                           {
                                                               empaccbank = "C";
                                                               break; // เพิ่ม break เมื่อพบเงื่อนไขที่ตรง
                                                           }
                                                           else if (worker.bank_cashpercent != 0)
                                                           {
                                                               empaccbank = "D";
                                                               break; // เพิ่ม break เมื่อพบเงื่อนไขที่ตรง
                                                           }
                                                       }
                                                   }

                                                  


                                                   bkData = "D" + sequence + combank.combank_bankcode + empacc + empaccbank + amount + "02" + "9" + spare1 + departmentcode + user + spare2;
                                                   bkData = bkData.PadRight(32, '0');

                                                   if (empname2.Length > 35)
                                                       empname2 = empname2.Substring(0, 35);

                                                   bkData = bkData + empname2.ToUpper();
                                                   tmpData += bkData.PadRight(128, ' ') + "\r\n";

                                                   douTotal += paytran.paytran_netpay_b;
                                                   douTotal2 += paytran.paytran_netpay_c;

                                                   index++;
                                               }



                                               int record = list_paytran.Count;
                                               sequence = (index + 1).ToString().PadLeft(6, '0');

                                               int countdouTotalRecord1 = list_empbank.Count(worker => worker.bank_cashpercent != 0 && worker.bank_code == combank.combank_bankcode);

                                               string total1record1 = "";

                                               if (douTotal > 0)
                                               {
                                                   decimal numericDouTotal = decimal.Parse(douTotal.ToString("#.#0").Trim().Replace(".", "").PadLeft(13, '0'));
                                                   long roundedDouTotal = (long)Math.Floor(numericDouTotal * 100);
                                                   total1record1 = (roundedDouTotal).ToString("D13");
                                               }
                                               else
                                               {
                                                   total1record1 = "0000000000000";
                                               }


                                               ////
                                               int countdouTotalRecord2 = list_empbank.Count(worker => worker.bank_cashpercent != 0 && worker.bank_code == combank.combank_bankcode);
                                               string total1record2 = "";
                                               if (douTotal > 0)
                                               {
                                                   decimal numericDouTotal2 = decimal.Parse(douTotal2.ToString("#.#0").Trim().Replace(".", "").PadLeft(13, '0'));
                                                   long roundedDouTotal2 = (long)Math.Floor(numericDouTotal2 * 100);
                                                   total1record2 = (roundedDouTotal2).ToString("D13");
                                               }
                                               else
                                               {
                                                   total1record2 = "0000000000000";
                                               }

                                               //
                                               string formattedAmount = "";
                                               if (record > 1)
                                               {
                                                   formattedAmount = (record).ToString().PadLeft(6, '0');
                                               }
                                               else
                                               {
                                                   formattedAmount = (index).ToString().PadLeft(6, '0');
                                               }

                                               if (spare3.Length < 68)
                                                   spare3 = spare3.PadRight(68, '0');

                                               //
                                               int countEmpbankRecord1 = list_empbank.Count(worker => worker.bank_percent != 0 && worker.bank_code == combank.combank_bankcode);
                                               int countCashEmpbankRecord1 = list_empbank.Count(worker => worker.bank_cashpercent != 0 && worker.bank_code == combank.combank_bankcode);

                                               // sequence1
                                               sequence1 = (douTotal).ToString().PadLeft(7, '0');
                                               string empbankrecord1 = "";
                                               if (countEmpbankRecord1 < 0)
                                               {
                                                   empbankrecord1 = (countEmpbankRecord1  ).ToString().PadLeft(7, '0');
                                               }
                                               else
                                               {
                                                   if (countEmpbankRecord1 > 0)
                                                   {
                                                       empbankrecord1 = (countEmpbankRecord1 -1).ToString().PadLeft(7, '0');
                                                   }
                                                   else
                                                   {
                                                       empbankrecord1 = "000000";
                                                   }
                                               }



                                               // sequence2
                                               sequence2 = (douTotal2).ToString().PadLeft(7, '0');
                                               string empbankrecord2 = "";
                                               if (countCashEmpbankRecord1 < 0)
                                               {
                                                    empbankrecord2 = (countCashEmpbankRecord1).ToString().PadLeft(7, '0');
                                               }
                                               else
                                               {
                                                   if (countCashEmpbankRecord1 > 0)
                                                   {
                                                        empbankrecord2 = (countCashEmpbankRecord1 - 1).ToString().PadLeft(7, '0');
                                                   }
                                                   else
                                                   {
                                                        empbankrecord2 += "000000";
                                                   }
                                               }




                                               if (spare3.Length < 68)
                                                   spare3 = spare3.PadRight(68, '0');








                                               bkData = "T" + sequence + combank.combank_bankcode + combank.combank_bankaccount + empbankrecord1 + total1record1 + empbankrecord2 + total1record2 + spare3;

                                               bkData = bkData.PadRight(81, '0');



                                               tmpData += bkData;
                                           }
                                       }
                                       else
                                       {
                                           continue;
                                       }

                                   }
                                }
                                break;

                            case "014": //scb
                                {
                                    string companyid = "";
                                    string combankid1 = "";
                                    string combankid2 = "";

                                    string filedate = "";
                                    string filetime = "";
                                    string batchreference = "";

                                    string space2 = ""; //ค่าว่าง
                                    string space3 = ""; //ค่าว่าง
                                    string space4 = ""; //ค่าว่าง
                                    string space8 = ""; //ค่าว่าง
                                    string space9 = ""; //ค่าว่าง
                                    string space10 = ""; //ค่าว่าง
                                    string space14 = ""; //ค่าว่าง
                                    string space18 = ""; //ค่าว่าง
                                    string space20 = ""; //ค่าว่าง
                                    string space35 = ""; //ค่าว่าง
                                    string space40 = ""; //ค่าว่าง
                                    string space70 = ""; //ค่าว่าง
                                    string space100 = ""; //ค่าว่าง

                                    if (space4.Length < 4)
                                    {
                                        space4 = space4.PadLeft(4);
                                    }
                                    if (space14.Length < 14)
                                    {
                                        space14 = space14.PadLeft(14);
                                    }

                                    if (space40.Length < 40)
                                    {
                                        space40 = space40.PadLeft(40);
                                    }
                                    if (space8.Length < 8)
                                    {
                                        space8 = space8.PadLeft(8);
                                    }
                                    if (space35.Length < 35)
                                    {
                                        space35 = space35.PadLeft(35);
                                    }
                                    if (space20.Length < 20)
                                    {
                                        space20 = space20.PadLeft(20);
                                    }
                                    if (space3.Length < 3)
                                    {
                                        space3 = space3.PadLeft(3);
                                    }
                                    if (space2.Length < 2)
                                    {
                                        space2 = space2.PadLeft(2);
                                    }
                                    if (space18.Length < 18)
                                    {
                                        space18 = space18.PadLeft(18);
                                    }

                                    if (space70.Length < 70)
                                    {
                                        space70 = space18.PadLeft(70);
                                    }
                                    if (space10.Length < 10)
                                    {
                                        space10 = space10.PadLeft(10);
                                    }

                                    if (combank.company_code.Length < 12)
                                        companyid = combank.company_code.PadRight(12, ' ');

                                    string firstFourDigits = combank.company_code.PadRight(4, ' '); // เพิ่มเติมค่าว่างเพื่อให้มีความยาว 4 ตัวอักษร
                                    //string firstFourDigits = combank.company_code.Substring(0, 4);   
                                    string datePart1 = datePay.ToString("yyyyMMdd", DateTimeFormatInfo.CurrentInfo);
                                    string datePart2 = datePay.ToString("HHmmss", DateTimeFormatInfo.CurrentInfo);
                                    string description = firstFourDigits + datePart1;

                                    if (description.Length <= 32)
                                        description = description.PadRight(32, ' ');

                                    if (datePart1.Length <= 8)
                                        filedate = datePart1.PadRight(8, '0');

                                    if (datePart2.Length <= 6)
                                        filetime = datePart2.PadRight(6, '0');

                                    if (firstFourDigits.Length <= 32)
                                        batchreference = firstFourDigits.PadRight(32, ' ');
                                    double douTotal = 0;
                                    int index = 0;
                                    string bkData;
                                    foreach (cls_TRPaytran paytran in list_paytran)
                                    {

                                        string empacc = "";
                                        string debitsccountno = "";
                                        string accounttype1 = "";
                                        string accounttype2 = "";

                                        if (empacc.Length <= 25)
                                            debitsccountno = empacc.PadRight(25, ' ');

                                        string accountNumber1 = combank.combank_bankaccount;
                                        char fourthDigit = accountNumber1[3];
                                        string result1 = "0" + fourthDigit;
                                        if (result1.Length <= 2)
                                            accounttype1 = result1.PadRight(2, '0');

                                        string accountNumber2 = combank.combank_bankaccount;
                                        string firstThreeDigits = accountNumber2.Substring(0, 3);
                                        string result2 = "0" + firstThreeDigits;
                                        if (result2.Length <= 4)
                                            accounttype2 = result2.PadRight(4, '0');


                                        double myNumber = paytran.paytran_netpay_b;
                                        string myStringNumber = myNumber.ToString("F2").Replace(".", "").Replace(",", "");

                                        if (myStringNumber.Length < 16)
                                        {
                                            myStringNumber = myStringNumber.PadLeft(16, '0');
                                        }

                                        if (space9.Length < 9)
                                        {
                                            space9 = space9.PadLeft(9);
                                        }


                                        double total = 0;
                                        if (paytran.paytran_netpay_b > 1)
                                        {
                                            total += paytran.paytran_netpay_b;
                                        }
                                        if (combank.combank_bankaccount.Length < 25)
                                            combankid1 = combank.combank_bankaccount.PadRight(25, ' ');

                                        if (combank.combank_bankaccount.Length < 15)
                                            combankid2 = combank.combank_bankaccount.PadRight(15, ' ');

                                         
                                        //string total2 = total.ToString().PadLeft(16, '0');
                                        string total2 = "";
                                        total2 = total.ToString("#.#0").Trim().Replace(".", "").PadLeft(16, '0');


                                        //              //เลขบัญชีธนาคาร  //Customer Reference  //Date of generate/extract data  //Time of generate/extract data //เลขอ้างอิง
                                        tmpData = "001" + companyid + description + filedate + filetime + "BMC" + batchreference + "\r\n";

                                        int totalData = list_paytran.Count;
                                        string totalDataString = totalData.ToString().PadLeft(6, '0'); // แปลงจำนวนข้อมูลให้เป็น string และใส่ 0 ด้านหน้าให้ครบ 7 หลัก

                                        ///                             " วันที่จ่ายเงินให้บริษัท" "บัญชีที่หักเงินต้น"                      "ระบุ จำนวนเงินรวมที่ต้องจ่ายให้บริษัท"ระบุ จำนวนบริษัทที่ทำการจ่าย"
                                        bkData = "002" + "PAY" + filedate + combankid1 + accounttype1 + accounttype2 + "THB" + total2 + "00000001" + totalDataString + combankid2 + space9 + " " + accounttype1 + accounttype2;

                                        bkData = bkData.PadRight(109, '0');
                                        tmpData += bkData.PadRight(109, ' ') + "\r\n";
                                    }

                                    //003
                                    foreach (cls_TRPaytran paytran in list_paytran)
                                    {

                                        string empacc = "";
                                        string empname = "";
                                        string empname2 = "";



                                        foreach (cls_MTWorker worker in list_worker)
                                        {
                                            if (paytran.worker_code.Equals(worker.worker_code))
                                            {
                                                empname = " " + worker.initial_name_en + " " + worker.worker_fname_en + " " + worker.worker_lname_en + " " + datePay.ToString("ddMMyy", DateTimeFormatInfo.CurrentInfo) + " ";
                                                empname2 = " " + worker.initial_name_en + " " + worker.worker_fname_en + " " + worker.worker_lname_en + " ";

                                                break;
                                            }
                                        }


                                        foreach (cls_TRBank worker in list_empbank)
                                        {
                                            if (paytran.worker_code.Equals(worker.worker_code))
                                            {
                                                empacc = worker.bank_account.Replace("-", "");
                                                break;
                                            }
                                        }

                                        if (empname.Equals("") || empacc.Equals(""))
                                            continue;
                                        sequence = Convert.ToString(index + 1).PadLeft(6, '0');
                                        string crediaccount = "";
                                        if (empacc.Length < 25)
                                            crediaccount = empacc.PadRight(25, ' ');

                                        string receivingbankname = "SIAM COMMERCIAL BANK";
                                        if (receivingbankname.Length < 35)
                                        {
                                            receivingbankname = receivingbankname.PadRight(35, ' ');
                                        }

                                        string receivingbranchcode = empacc;
                                        string accountNumber = empacc; // เลขที่บัญชีพนักงาน
                                        string modifiedAccountNumber = "0" + accountNumber.Substring(0, 3); // เพิ่ม "0" ไว้ด้านหน้า 3 ตัวแรกของเลขที่บัญชี
                                        if (modifiedAccountNumber.Length <= 4)
                                        {
                                            receivingbranchcode = modifiedAccountNumber.PadLeft(4);
                                        }

                                        // "ระบุ ยอดเงินจ่ายให้บริษัท"
                                        double myNumber = paytran.paytran_netpay_b;
                                        string myStringNumber = Math.Floor(myNumber).ToString(); // ตัดทศนิยมออก
                                        if (myStringNumber.Length < 16) // ตรวจสอบความยาวของ string
                                        {
                                            myStringNumber = myStringNumber.PadLeft(16, '0'); // ใส่เลข 0 ด้านหน้าให้ครบ 16 หลัก
                                        }

                                        //                                 " วันที่ระบุ เลขที่บัญชีของบริษัท"                                                                                                                                                                                                                                                                                                                                                                                                                                                                         จะเป็นข้อความในการส่งให้ลุกค้า   
                                        bkData = "003" + sequence + crediaccount + myStringNumber + "THB" + "00000001" + "N" + "N" + "Y" + "S" + space4 + "00 " + space14 + "000000" + "00" + " 0000000000000000" + " 000000" + " 0000000000000000" + "0" + space40 + space8 + "014" + receivingbankname + receivingbranchcode + space35 + " " + "N " + space20 + " " + space3 + space2 + " " + space18 + space2;
                                        bkData = bkData.PadRight(32, '0');

                                        tmpData += bkData.PadRight(128, ' ') + "\r\n";

                                        if (empname2.Length > 100)
                                            empname2 = empname2.Substring(0, 100);

                                        bkData = "004" + "00000001" + sequence + "000000000000000" + empname2 + space70 + space70 + space70 + space10 + space70 + space100 + space70 + space70 + space70;

                                        bkData = bkData.PadRight(32, '0');
                                        tmpData += bkData.PadRight(128, ' ') + "\r\n";
                                        douTotal += paytran.paytran_netpay_b;
                                        index++;
                                    }
                                    //999
                                    int record = list_paytran.Count;
                                    double totals = 0;
                                    foreach (cls_TRPaytran paytran in list_paytran)
                                    {
                                        if (paytran.paytran_netpay_b > 1)
                                        {
                                            totals += paytran.paytran_netpay_b;
                                        }
                                    }
                                    string totals2 = totals.ToString().PadLeft(16, '0');
                                    sequence = (index).ToString().PadLeft(6, '0');
                                    bkData = "999" + "000001" + sequence + totals2;
                                    bkData = bkData.PadRight(31, '0');
                                    tmpData += bkData;
                                }
                                break;

                            case "004": // กสิกรไทย 
                                {
                                    string companyid = "";
                                    string filedate = "";
                                    string filetime = "";
                                    string batchreference = "";
                                    string branchid = "";
                                    string bankaccount = "";
 

                                    string space14 = ""; //ค่าว่าง
                                    string space1 = ""; //ค่าว่าง
                                    string space25 = ""; //ค่าว่าง
                                    string space8 = ""; //ค่าว่าง
                                    string free9 = ""; //ค่าว่าง
                                    string free10 = ""; //ค่าว่าง
                                    string free14 = ""; //ค่าว่าง
                                    string free18 = ""; //ค่าว่าง
                                    string free20 = ""; //ค่าว่าง
                                    string free35 = ""; //ค่าว่าง
                                    string free40 = ""; //ค่าว่าง
                                    string free70 = ""; //ค่าว่าง
                                    string free100 = ""; //ค่าว่าง
                                    string amount = "";
                                    if (space25.Length < 25)
                                    {
                                        space25 = space25.PadLeft(25);
                                    }
                                    if (free14.Length < 14)
                                    {
                                        free14 = free14.PadLeft(14);
                                    }

                                    if (free40.Length < 40)
                                    {
                                        free40 = free40.PadLeft(40);
                                    }
                                    if (space8.Length < 8)
                                    {
                                        space8 = space8.PadLeft(8);
                                    }
                                    if (free35.Length < 35)
                                    {
                                        free35 = free35.PadLeft(35);
                                    }
                                    if (free20.Length < 20)
                                    {
                                        free20 = free20.PadLeft(20);
                                    }
                                    if (space1.Length < 1)
                                    {
                                        space1 = space1.PadLeft(1);
                                    }
                                    if (space14.Length < 14)
                                    {
                                        space14 = space14.PadLeft(14);
                                    }
                                    if (free18.Length < 18)
                                    {
                                        free18 = free18.PadLeft(18);
                                    }

                                    if (free70.Length < 70)
                                    {
                                        free70 = free18.PadLeft(70);
                                    }
                                    if (free10.Length < 10)
                                    {
                                        free10 = free10.PadLeft(10);
                                    }


                                    foreach (var branch in list_combranch)
                                    {
                                         if (branch.combranch_code.Length <= 5)
                                            {
                                                branchid = branch.combranch_code.PadRight(5, ' ');
                                            }
                                        


                                    foreach (var data in list_combank)
                                    {
                                        if (data.combank_bankcode.StartsWith("004"))
                                        {


                                            if (data.company_code.Length < 50)
                                            {
                                                companyid = data.company_code.PadRight(50, ' ');
                                            }


                                            //if (data.combank_branch.Length < 5)
                                            //{
                                            //    branchid = data.combank_branch.PadRight(5, ' '); 
                                            //}
                                                


                                            if (data.combank_bankaccount.Length <= 10)
                                            {
                                                bankaccount = data.combank_bankaccount.PadRight(10, ' ');
                                            }
 

                                    string firstFourDigits = combank.company_code.PadRight(4, ' '); // เพิ่มเติมค่าว่างเพื่อให้มีความยาว 4 ตัวอักษร
                                    //string firstFourDigits = combank.company_code.Substring(0, 4); 

                                     string  batchref = "TRAN" + "-" + DateTime.Now.ToString("yyyyMMdd");
 

                                    string datePart1 = datePay.ToString("yyMMdd", DateTimeFormatInfo.CurrentInfo);
                                    string datePart2 = datePay.ToString("HHmmss", DateTimeFormatInfo.CurrentInfo);
                                    string description = firstFourDigits + datePart1;

                                    if (batchref.Length <= 16)
                                        batchref = batchref.PadRight(16, ' ');

                                    if (description.Length <= 32)
                                        description = description.PadRight(32, ' ');

                                    if (datePart1.Length <= 6)
                                        filedate = datePart1.PadRight(6, '0');

                                    if (datePart2.Length <= 6)
                                        filetime = datePart2.PadRight(6, '0');

                                    if (firstFourDigits.Length <= 50)
                                        batchreference = firstFourDigits.PadRight(50, ' ');
                                   
                                    double douTotal = 0;
                                    int index = 0;
                                    string bkData;
                                    foreach (cls_TRPaytran paytran in list_paytran)
                                    {

                                        string empacc = "";
                                        string debitsccountno = "";
                                        string accounttype1 = "";
                                        string accounttype2 = "";

                                        if (empacc.Length <= 25)
                                            debitsccountno = empacc.PadRight(25, ' ');

                                        string accountNumber1 = combank.combank_bankaccount;
                                        if (accountNumber1.Length <= 10)
                                            bankaccount = accountNumber1.PadRight(10, ' ');



                                        char fourthDigit = accountNumber1[3];
                                        string result1 = "0" + fourthDigit;
                                        if (result1.Length <= 2)
                                            accounttype1 = result1.PadRight(2, '0');

                                        string accountNumber2 = combank.combank_bankaccount;
                                        string firstThreeDigits = accountNumber2.Substring(0, 3);
                                        string result2 = "0" + firstThreeDigits;
                                        if (result2.Length <= 4)
                                            accounttype2 = result2.PadRight(4, '0');


                                        double myNumber = paytran.paytran_netpay_b;
                                        string myStringNumber = myNumber.ToString("F2").Replace(".", "").Replace(",", "");

                                        if (myStringNumber.Length < 16)
                                        {
                                            myStringNumber = myStringNumber.PadLeft(16, '0');
                                        }

                                        if (free9.Length < 9)
                                        {
                                            free9 = free9.PadLeft(9);
                                        }

                                        double total = 0;
                                        if (paytran.paytran_netpay_b > 1)
                                        {
                                            total += paytran.paytran_netpay_b;
                                        }
                                        //string total2 = total.ToString().PadLeft(16, '0');
                                        string total2 = "";
                                        total2 = total.ToString("#.#0").Trim().Replace(".", "").PadLeft(16, '0');




                                        decimal temp = (decimal)paytran.paytran_netpay_b;
                                        amount = temp.ToString("#.#0").Trim().Replace(".", "").PadLeft(15, '0');


                                        int totalData = list_paytran.Count;
                                        string totalDataString = totalData.ToString().PadLeft(18, '0');



                                        //Header
                                        tmpData = "H" + "|" + "PCT" + "|" + batchref + "|" + "000000" + "|" + space14 + "|"
                                            + bankaccount + "|" + space1 + "|" + amount + "|" + space1 + "|"
                                            + datePart1 + "|" + space25 + "|" + companyid + "|" + datePart1 + "|" + totalDataString  + "|" + "N" + "|" + branchid + "|" + "\r\n";

                                     }

                                    //Detail
                                    foreach (cls_TRPaytran paytran in list_paytran)
                                    {

                                        string empacc = "";
                                        string empname = "";
                                        string empname2 = "";
                                        string amounts = "";
                                        string tax = "";//หักณที่จ่าย
                                        string payee_tax_id = "";//บัตรประชาชน
                                        string payee_personal_id = "";
 

                                        foreach (cls_MTWorker worker in list_worker)
                                        {
                                            if (paytran.worker_code.Equals(worker.worker_code))
                                            {
                                                empname = " " + worker.initial_name_en + " " + worker.worker_fname_en + " " + worker.worker_lname_en + " " + datePay.ToString("ddMMyy", DateTimeFormatInfo.CurrentInfo) + " ";
                                                empname2 = " " + worker.initial_name_en + " " + worker.worker_fname_en + " " + worker.worker_lname_en + " ";

                                                break;
                                            }
                                        }

                                        foreach (cls_TRBank worker in list_empbank)
                                        {
                                            if (paytran.worker_code.Equals(worker.worker_code))
                                            {
                                                empacc = worker.bank_account.Replace("-", "");
                                                break;
                                            }
                                        }

                                        if (empname.Equals("") || empacc.Equals(""))
                                            continue;
                                        string crediaccount = "";
                                        if (empacc.Length <= 10)
                                            crediaccount = empacc.PadRight(10, ' ');

                                        ///
                                        sequence = Convert.ToString(index + 1).PadLeft(6, '0');

                                        string beneref = (index + 1).ToString("000").PadRight(16);

                                        decimal temp = (decimal)paytran.paytran_netpay_b;
                                        amounts = temp.ToString("#.#0").Trim().Replace(".", "").PadLeft(15, '0');
                                        string total_inv_amt_bef_vat = "";
                                        string total_tax_deducted_amt = "";
                                        string total_Inv_amt_After_vat = "";

                                        //กรณีที่มีการหักภาษีณ ที่จ่ายควรใส่ข้อมูลให้สอดคล้องกัน
                                        if (paytran.paytran_tax_401 != null && Convert.ToDecimal(paytran.paytran_tax_401) > 0.00m)
                                        {
                                            total_inv_amt_bef_vat += paytran.paytran_tax_401;
                                            if (total_inv_amt_bef_vat.Length <= 10.2)
                                            {
                                                total_inv_amt_bef_vat = total_inv_amt_bef_vat.PadLeft(10);
                                            }

                                            total_tax_deducted_amt += paytran.paytran_tax_4012;
                                            if (total_tax_deducted_amt.Length <= 10.2)
                                            {
                                                total_tax_deducted_amt = total_tax_deducted_amt.PadLeft(10);
                                            }
                                            total_Inv_amt_After_vat += paytran.paytran_tax_4013;
                                            if (total_Inv_amt_After_vat.Length <= 10.2)
                                            {
                                                total_Inv_amt_After_vat = total_Inv_amt_After_vat.PadLeft(10);
                                            }


                                            foreach (cls_MTWorker worker in list_worker)
                                            {
                                                if (worker.worker_cardno.Equals(worker.worker_cardno))
                                                {
                                                     payee_tax_id =  "".PadRight(10);

                                                    break;
                                                }
                                                if (worker.worker_cardno.Equals(worker.worker_cardno))
                                                {
                                                      payee_personal_id = "".PadRight(13);
                                                    break;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            total_inv_amt_bef_vat = "".PadRight(10);
                                        }
                                        {
                                            total_tax_deducted_amt = "".PadRight(10);
                                        }
                                        {
                                            total_Inv_amt_After_vat = "".PadRight(10);
                                        }
                                        {
                                            payee_tax_id = "".PadRight(10);
                                        }
                                        {
                                            payee_personal_id = "".PadRight(13);
                                        }
                                           
                                        //"ชื่อไฟล์เอกสาร" 
                                        string attachment = "".PadRight(50);
                                        // " รูปแบบการแจ้งเตือน" 
                                        string advicemode = "".PadRight(1);
                                        //" หมายเลขโทรสาร" 
                                        string  faxno = "".PadRight(50);
                                        //" E"  
                                        string emailid  = "".PadRight(50);
                                        //  ayee Address1 
                                        string address1 = "".PadRight(30);
                                        //  ayee Address2
                                        string address2 = "".PadRight(30);
                                        //  ayee Address3
                                        string address3 = "".PadRight(30);
                                        //  ayee Address4
                                        string address4 = "".PadRight(30);

                                      



                                        bkData = "D" + "|" + sequence + "|" + space8 + "|" + crediaccount + "|" + space1 + "|" + amounts + "จํานวนเงินที่ต้องการโอนเข้าในแต่ละบัญชี" + "|" + space1 + "|" + datePart1 + "วันที่" + "|" + space25 + "|" + empname2 + "ชื่อผู้รับเงิน" + "|" + datePart1 + "วันที่ " + "|" + "000" + "จำนวนรายการทั้งหมด" + "|" + beneref + "หมายเลขอ้างอิง" + "|" + attachment + "|" + advicemode + "|" + faxno + "|" + emailid + "|" + total_inv_amt_bef_vat + "|" + total_tax_deducted_amt + "|" + total_Inv_amt_After_vat + "|" + payee_tax_id + "|" + payee_personal_id + "|" + address1 + "|" + address2 + "|" + address3 + "|" + address4;
                                       
                                        bkData = bkData.PadRight(32, '0');
                                        tmpData += bkData.PadRight(128, ' ') + "\r\n";
                                        douTotal += paytran.paytran_netpay_b;
                                        index++;
                                    }
                                        }
                                    }
                                    }
                                }
                                break;
                            default:
                                break;
                        }
                    }
                    try
                    {
                        //-- Step 1 create file
                        //string filename = "UPAYDAT.DAT";
                        string filename = "TRN_BANK_" + DateTime.Now.ToString("yyMMddHHmm") + "." + "txt";
                        string filepath = Path.Combine
                       (ClassLibrary_BPC.Config.PathFileExport, filename);

                        // Check if file already exists. If yes, delete it.     
                        if (File.Exists(filepath))
                        {
                            File.Delete(filepath);
                        }

                        // Create a new file     
                        using (FileStream fs = File.Create(filepath))
                        {
                            // Add some text to file    
                            Byte[] title = new UTF8Encoding(true).GetBytes(tmpData);
                            fs.Write(title, 0, title.Length);
                        }

                        strResult = filepath;

                    }
                    catch
                    {
                        strResult = "";
                    }

                }


                task.task_end = DateTime.Now;
                task.task_status = "F";
                task.task_note = strResult;
                objMTTask.updateStatus(task);

            }
            else
            {

            }

            return strResult;
        }




        #endregion

        #region TAX
        public string doExportTax(string com, string taskid)
        {
            string strResult = "";

            cls_ctMTTask objMTTask = new cls_ctMTTask();
            List<cls_MTTask> listMTTask = objMTTask.getDataByFillter(com, taskid, "TRN_TAX", "");
            List<string> listError = new List<string>();

            if (listMTTask.Count > 0)
            {
                cls_MTTask task = listMTTask[0];

                task.task_start = DateTime.Now;

                cls_ctMTTask objTaskDetail = new cls_ctMTTask();
                cls_TRTaskdetail task_detail = objTaskDetail.getTaskDetail(task.task_id.ToString());

                cls_ctMTTask objTaskWhose = new cls_ctMTTask();
                List<cls_TRTaskwhose> listWhose = objTaskWhose.getTaskWhose(task.task_id.ToString());

                DateTime dateEff = task_detail.taskdetail_fromdate;
                DateTime datePay = task_detail.taskdetail_paydate;

                StringBuilder objStr = new StringBuilder();
                foreach (cls_TRTaskwhose whose in listWhose)
                {
                    objStr.Append("'" + whose.worker_code + "',");
                }

                string strEmp = objStr.ToString().Substring(0, objStr.ToString().Length - 1);



                //-- Get worker
                cls_ctMTWorker objWorker = new cls_ctMTWorker();
                List<cls_MTWorker> list_worker = objWorker.getDataMultipleEmp(com, strEmp);

                //-- Step 2 Get Paytran
                cls_ctTRPaytran objPay = new cls_ctTRPaytran();
                List<cls_TRPaytran> list_paytran = objPay.getDataMultipleEmp("TH", com, dateEff, datePay, strEmp);



                //-- Step 3 Get Company acc
                cls_ctMTCombank objCombank = new cls_ctMTCombank();
                List<cls_MTCombank> list_combank = objCombank.getDataByFillter("", com);
                cls_MTCombank combank = list_combank[0];

                //-- Step 4 Get Company detail
                cls_ctMTCompany objCom = new cls_ctMTCompany();
                List<cls_MTCompany> list_com = objCom.getDataByFillter("", com);
                cls_MTCompany comdetail = list_com[0];

                //-- Step 5 Get Emp address

                cls_ctTRAddress objEmpadd = new cls_ctTRAddress();
                List<cls_TRAddress> list_empaddress = objEmpadd.getDataByFillter2(com, strEmp);

                //-- Step 6 Get Emp card
                cls_ctTRCard objEmpcard = new cls_ctTRCard();
                List<cls_TRCard> list_empcard = objEmpcard.getDataTaxMultipleEmp(com, strEmp);



                //-- Step 7 Get Company card
                cls_ctTRCard objComcard = new cls_ctTRCard();
                List<cls_TRCard> list_comcard = objComcard.getDataByFillter(com, "NTID", "", "", "");
                cls_TRCard comcard = list_comcard[0];


                cls_ctMTProvince objProvince = new cls_ctMTProvince();
                List<cls_MTProvince> list_province = objProvince.getDataByFillter("");

                cls_ctTREmpbranch objEmpbranch = new cls_ctTREmpbranch();
                List<cls_TREmpbranch> list_Empbranch = objEmpbranch.getDataTaxMultipleEmp(com, strEmp);
                cls_TREmpbranch comEmpbranch = list_Empbranch[0];

                cls_ctMTCombranch objcombranch = new cls_ctMTCombranch();
                List<cls_MTCombranch> list_combranch = objcombranch.getDataTaxMultipleCombranch(com);
                cls_MTCombranch combranch = list_combranch[0];


                string tmpData = "";


                if (list_paytran.Count > 0)
                {

                    double douTotal = 0;

                    int index = 0;
                    string bkData;

                    foreach (cls_TRPaytran paytran in list_paytran)
                    {
                        string empname = "";

                        cls_MTWorker obj_worker = new cls_MTWorker();
                        cls_TRAddress obj_address = new cls_TRAddress();
                        cls_MTProvince obj_province = new cls_MTProvince();
                        cls_TRCard obj_card = new cls_TRCard();



                        // วนลูปเพื่อค้นหาข้อมูล worker ใน list_worker ที่ตรงกับ worker_code ใน paytran
                        foreach (cls_MTWorker worker in list_worker)
                        {
                            if (paytran.worker_code.Equals(worker.worker_code))
                            {
                                empname = worker.initial_name_en + " " + worker.worker_fname_en + " " + worker.worker_lname_en;
                                obj_worker = worker;
                                break;
                            }
                        }


                        // วนลูปเพื่อค้นหาข้อมูล address ใน list_empaddress ที่ตรงกับ worker_code ใน paytran
                        foreach (cls_TRAddress address in list_empaddress)
                        {
                            if (paytran.worker_code.Equals(address.worker_code))
                            {
                                obj_address = address;
                                break;
                            }
                        }

                        // วนลูปเพื่อค้นหาข้อมูล card ใน list_empcard ที่ตรงกับ worker_code ใน paytran
                        foreach (cls_TRCard card in list_empcard)
                        {
                            if (paytran.worker_code.Equals(card.worker_code))
                            {
                                obj_card = card;
                                break;
                            }
                        }


                        // วนลูปเพื่อค้นหาข้อมูล province ใน list_province ที่ตรงกับ province_code ใน obj_address
                        foreach (cls_MTProvince province in list_province)
                        {
                            if (obj_address.province_code.Equals(province.province_code))
                            {
                                obj_province = province;
                                break;
                            }
                        }



                        //if (empname.Equals("") || obj_card.card_code.Equals(""))
                        //    continue;
                        if (empname.Equals("") || empname.Equals(""))
                            empname += paytran.worker_detail;
                        else
                            empname += empname.Equals("") + "|";


                        if (paytran.paytran_income_401 > 0)
                        {
                            // กำหนดค่า bkData ตามเงื่อนไขที่กำหนด
                            // 1.ลักษณะการยื่นแบบปกติ
                            bkData = "00" + "  ";
                            //bkData += "00|";
                            // 2.เลขประจำตัวประชาชนผู้มี่หน้าที่หัก ณ ที่จ่าย<CardNo>citizen_no
                            if (comdetail.citizen_no.Length == 13)
                                bkData += comdetail.citizen_no + "  ";

                                //bkData += comcard.comcard_code + "|";
                            else
                                bkData += "0000000000000" + "  ";

                            //bkData += comdetail.citizen_no.Length == 13 ? comdetail.citizen_no : "0000000000000";
                            //bkData += "|";

                            // 3.เลขประจำตัวผู้เสียภาษีอากรผู้มีหน้าที่หัก ณ ที่จ่าย<TaxNo>
                            if (comdetail.sso_tax_no.Length == 10)
                                bkData += comdetail.sso_tax_no + "  ";
                            else
                                bkData += "0000000000" + "  ";

                            //bkData += comdetail.sso_tax_no.Length == 10 ? comdetail.sso_tax_no : "0000000000";
                            //bkData += "|";

                            // 4.เลขที่สาขา ผู้มีหน้าที่หักภาษี ณ ที่จ่าย<BranchID>

                            if (combranch.combranch_code.Length == 4)
                                bkData += combranch.combranch_code + "  ";
                            else
                                bkData += "0000" + "  ";


                            //if (combranch.combranch_code.Length == 4)
                            //    bkData += combranch.combranch_code + "  ";
                            //else
                            //    bkData += "0000" + "  ";


                            //bkData += combank.company_code.Length == 4 ? combank.company_code : "00000";
                            //bkData += "|";

                            // 5.เลขประจำตัวประชาชนผู้มีเงินได้<CardNo>	
                            if (obj_worker.worker_cardno.Length == 13)
                                bkData += obj_worker.worker_cardno + " ";

                                //bkData += comcard.comcard_code + "|";
                            else
                                bkData += "0000000000000" + "  ";

                            //bkData += comcard.card_code.Length == 13 ? comcard.card_code : "0000000000000";
                            //bkData += "|";

                            // 6.เลขประจำตัวผู้เสียภาษีอากรผู้มีเงินได้ <TaxNo>
                            //if (obj_worker.worker_cardno.Length == 10)
                            //    bkData += obj_worker.worker_cardno + " ";

                            //  //bkData += comcard.comcard_code + "|";
                            //else
                            bkData += "0000000000000" + "  ";
                            //bkData += comcard.card_code.Length == 13 ? comcard.card_code : "0000000000";
                            //bkData += "|";


                            // 7.คำนำหน้าชื่อผู้มีเงินได้<InitialNameT>
                            bkData += obj_worker.initial_name_en + "|";
                            // 8.ชื่อผู้มีเงินได้<EmpFNameT>				
                            bkData += obj_worker.worker_fname_en + "|";
                            // 9.นามสกุลผู้มีเงินได้<EmpLNameT>
                            bkData += obj_worker.worker_lname_en + "|";
                            // 10.ที่อยู่ 1<Address>
                            string temp = obj_address.address_no + obj_address.address_soi + " " + obj_address.address_road + " " + obj_address.address_tambon + " " + obj_address.address_amphur + " " + obj_province.province_name_en;
                            bkData += temp + "|";
                            // 11.ที่อยู่2
                            bkData += "|";
                            // 12.รหัสไปรษณีย์ <Poscod>
                            bkData += obj_address.address_zipcode + "  ";
                            // 13.เดือนภาษี<TaxMonth>
                            bkData += datePay.Month.ToString().PadLeft(2, '0') + "  ";
                            // 14.ปีภาษี<TaxYear>
                            int n = Convert.ToInt32(datePay.Year);
                            if (n < 2400)
                                n += 543;
                            bkData += n.ToString() + "  ";
                            // 15.รหัสเงินได้<AllwonceCode>
                            bkData += "1" + "  ";
                            // 16.วันที่จ่ายเงินได้ <TaxDate>+<TaxMonth>+<TaxYear>
                            bkData += datePay.ToString("ddMM") + n.ToString() + "  ";
                            //bkData += "0" + "  ";
                            // 18.จำนวนเงินที่จ่าย<PayMent>
                            bkData += paytran.paytran_income_401.ToString("0.00") + "  ";
                            // 19.จำนวนเงินภาษีที่หักและนำส่ง<Tax>
                            bkData += paytran.paytran_tax_401.ToString("0.00") + "  ";
                            // 20.เงื่อนไขการหักภาษี ณ จ่าย <TaxCondition>
                            bkData += "1" + "  ";


                            tmpData += bkData + '\r' + '\n';
                        }

                        douTotal += paytran.paytran_netpay_b;
                        index++;
                    }

                    int record = list_paytran.Count;



                    try
                    {
                        //-- Step 1 create file
                        string filename = "TRN_TAX_" + DateTime.Now.ToString("yyMMddHHmm") + "." + "txt";
                        string filepath = Path.Combine
                       (ClassLibrary_BPC.Config.PathFileExport, filename);

                        // Check if file already exists. If yes, delete it.     
                        if (File.Exists(filepath))
                        {
                            File.Delete(filepath);
                        }

                        // Create a new file     
                        using (FileStream fs = File.Create(filepath))
                        {
                            // Add some text to file    
                            Byte[] title = new UTF8Encoding(true).GetBytes(tmpData);
                            fs.Write(title, 0, title.Length);
                        }

                        strResult = filepath;

                    }
                    catch
                    {
                        strResult = "";
                    }

                }


                task.task_end = DateTime.Now;
                task.task_status = "F";
                task.task_note = strResult;
                objMTTask.updateStatus(task);

            }
            else
            {

            }

            return strResult;
        }
        //TAX 
        #endregion

        #region TRN_bonus
        public string doExportBonus(string com, string taskid)
        {
            string strResult = "";

            cls_ctMTTask objMTTask = new cls_ctMTTask();
            List<cls_MTTask> listMTTask = objMTTask.getDataByFillter(com, taskid, "TRN_BONUS", "");
            List<string> listError = new List<string>();

            if (listMTTask.Count > 0)
            {
                cls_MTTask task = listMTTask[0];

                task.task_start = DateTime.Now;

                cls_ctMTTask objTaskDetail = new cls_ctMTTask();
                cls_TRTaskdetail task_detail = objTaskDetail.getTaskDetail(task.task_id.ToString());

                cls_ctMTTask objTaskWhose = new cls_ctMTTask();
                List<cls_TRTaskwhose> listWhose = objTaskWhose.getTaskWhose(task.task_id.ToString());

                DateTime dateEff = task_detail.taskdetail_fromdate;
                DateTime datePay = task_detail.taskdetail_paydate;

                StringBuilder objStr = new StringBuilder();
                foreach (cls_TRTaskwhose whose in listWhose)
                {
                    objStr.Append("'" + whose.worker_code + "',");
                }

                string strEmp = objStr.ToString().Substring(0, objStr.ToString().Length - 1);



                //-- Get worker
                cls_ctMTWorker objWorker = new cls_ctMTWorker();
                List<cls_MTWorker> list_worker = objWorker.getDataMultipleEmp(com, strEmp);

                //-- Step 2 Get Paytran
                cls_ctTRPaytran objPay = new cls_ctTRPaytran();
                List<cls_TRPaytran> list_paytran = objPay.getDataMultipleEmp("TH", com, datePay, datePay, strEmp);
                cls_TRPaytran paybank = list_paytran[0];


                //-- Step 3 Get Company acc
                cls_ctMTCombank objCombank = new cls_ctMTCombank();
                List<cls_MTCombank> list_combank = objCombank.getDataByFillter("", com);
                cls_MTCombank combank = list_combank[0];

                //-- Step 4 Get Company detail
                cls_ctMTCompany objCom = new cls_ctMTCompany();
                List<cls_MTCompany> list_com = objCom.getDataByFillter("", com);
                cls_MTCompany comdetail = list_com[0];

                //-- Step 5 Get Emp address
                cls_ctTRAddress objEmpadd = new cls_ctTRAddress();
                List<cls_TRAddress> list_empaddress = objEmpadd.getDataByFillter(com, strEmp);

                //-- Step 6 GetCompany detail
                cls_ctTRCard objEmpcard = new cls_ctTRCard();
                List<cls_TRCard> list_empcard = objEmpcard.getDataTaxMultipleEmp(com, strEmp);

                //-- Step 7 Get Company card
                cls_ctTRCard objComcard = new cls_ctTRCard();
                List<cls_TRCard> list_comcard = objComcard.getDataByFillter(com, "NTID", "", "", "");
                cls_TRCard comcard = list_comcard[0];


                //-- Step 8 Get Company card
                cls_ctMTProvince objProvince = new cls_ctMTProvince();
                List<cls_MTProvince> list_province = objProvince.getDataByFillter("");

                //-- Step 9 Get Company card
                cls_ctTRPaybonus objPaybonus = new cls_ctTRPaybonus();
                List<cls_TRPaybonus> list_paybonus = objPaybonus.getDataByFillter("", com, datePay, "");
                cls_TRPaybonus paybonus = list_paybonus[0];



                string tmpData = "";


                if (list_paytran.Count > 0)
                {

                    double douTotal = 0;

                    int index = 0;
                    string bkData;

                    foreach (cls_TRPaytran paytran in list_paytran)
                    {

                        string empname = "";

                        cls_MTWorker obj_worker = new cls_MTWorker();
                        cls_TRAddress obj_address = new cls_TRAddress();
                        cls_MTProvince obj_province = new cls_MTProvince();
                        cls_TRCard obj_card = new cls_TRCard();
                        cls_TRPaybonus obj_paybonus = new cls_TRPaybonus();



                        foreach (cls_MTWorker worker in list_worker)
                        {
                            if (paytran.worker_code.Equals(worker.worker_code))
                            {
                                empname = worker.initial_name_en + " " + worker.worker_fname_en + " " + worker.worker_lname_en;
                                obj_worker = worker;
                                break;
                            }
                        }

                        foreach (cls_TRAddress address in list_empaddress)
                        {
                            if (paytran.worker_code.Equals(address.worker_code))
                            {
                                obj_address = address;
                                break;
                            }
                        }

                        foreach (cls_TRCard card in list_empcard)
                        {
                            if (paytran.worker_code.Equals(card.worker_code))
                            {
                                obj_card = card;
                                break;
                            }
                        }

                        foreach (cls_MTProvince province in list_province)
                        {
                            if (obj_address.province_code.Equals(province.province_code))
                            {
                                obj_province = province;
                                break;
                            }
                        }

                        //"Header Record  (ส่วนที่ 1)"     
                        if (empname.Equals("") || obj_card.card_code.Equals(""))
                            empname += obj_card.card_code;
                        else
                            empname += obj_card.card_code.Equals("") + "|";



                        if (paytran.paytran_income_401 > 0)
                        {

                            //1.ประเภทข้อมูล
                            bkData = obj_card.card_id + "|";

                            //2.เลขที่บัญชีนายจ้าง
                            if (combank.combank_bankaccount.Length == 10)
                                bkData += combank.combank_bankaccount + "|";
                            else
                                bkData += combank.combank_bankaccount + "|";



                            //3.ลำดับที่สาขา
                            if (combank.combank_bankcode.Length == 6)
                                bkData += combank.combank_bankcode + "|";
                            else
                                bkData += combank.combank_bankcode + "|";

                            //4.วันที่ชำระเงิน <>

                            bkData += datePay.ToString("ddMMyy") + "|";

                            //5.งวดค่าจ้าง<>	
                            bkData += datePay.ToString("MM") + dateEff.ToString("yy") + "|";


                            //6.ชื่อสถานประกอบการ 

                            bkData += comdetail.company_name_en + "|";

                            //7.โบนัส 
                            if (paybonus.paybonus_netpay == 4)
                                bkData += paybonus.paybonus_netpay;
                            else
                                bkData += paybonus.paybonus_netpay.ToString("0000") + "|";

                            //8.จำนวนผู้ประกันตน			
                            if (paytran.paytran_tax_401 == 6)
                                bkData += paytran.paytran_tax_401;
                            else
                                bkData += paytran.paytran_tax_401.ToString("000000") + "|";


                            //9.ค่าจ้างรวม PAYTRAN_INCOME_TOTAL
                            if (paytran.paytran_income_total == 15)
                                bkData += paytran.paytran_income_total + "|";
                            else
                                bkData += paytran.paytran_income_total.ToString("000000000000000") + "|";

                            //10.เงินสมทบรวม 
                            if (paytran.paytran_pfcom == 14)
                                bkData += paytran.paytran_pfcom + "|";
                            else
                                bkData += paytran.paytran_pfcom.ToString("00000000000000") + "|";

                            //11.เงินสมทบรวมส่วนผปต. 
                            if (paytran.paytran_income_notax == 12)
                                bkData += paytran.paytran_income_notax + "|";
                            else
                                bkData += paytran.paytran_income_notax.ToString("000000000000") + "|";

                            //12.เงินสมทบส่วนนายจ้าง <Poscod>
                            if (paytran.paytran_tax_401 == 12)
                                bkData += paytran.paytran_tax_401 + "|";
                            else
                                bkData += paytran.paytran_tax_401.ToString("000000000000") + "|";





                            tmpData += bkData + '\r' + '\n';
                        }

                        //Detail Record 2
                        if (empname.Equals("") || obj_card.card_code.Equals(""))
                            continue;


                        if (paytran.paytran_income_401 > 0)
                        {

                            //1.ประเภทข้อมูล
                            bkData = "2|";


                            //2.เลขที่บัตรประชาชน
                            if (comcard.card_code.Length == 13)
                                bkData += comcard.card_code + "|";
                            else
                                bkData += "0000000000000|";


                            //3.คำนำหน้าชื่อ
                            bkData += obj_worker.initial_name_th + "|";

                            //4.ชื่อผู้ประกันตน 

                            bkData += obj_worker.worker_fname_th + "|";


                            //5.นามสกุลผู้ประกันตน
                            bkData += obj_worker.worker_lname_th + "|";


                            //6.ค่าจ้าง

                            if (paytran.paytran_income_total == 14)
                                bkData += paytran.paytran_income_total + "|";
                            else
                                bkData += paytran.paytran_income_total.ToString("00000000000000") + "|";

                            //7.โบนัส
                            if (paybonus.paybonus_netpay == 12)
                                bkData += paybonus.paybonus_netpay + "|";
                            else
                                bkData += paybonus.paybonus_netpay.ToString("000000000000") + "|";


                            //8.คอลัมน์ว่าง			                          
                            bkData += "|";


                            tmpData += bkData + '\r' + '\n';
                        }





                        douTotal += paytran.paytran_netpay_b;

                        index++;
                    }

                    int record = list_paytran.Count;

                    try
                    {
                        //-- Step 1 create file
                        string filename = "TRN_BONUS_" + DateTime.Now.ToString("yyMMddHHmm") + "." + "txt";
                        string filepath = Path.Combine
                       (ClassLibrary_BPC.Config.PathFileExport, filename);

                        // Check if file already exists. If yes, delete it.     
                        if (File.Exists(filepath))
                        {
                            File.Delete(filepath);
                        }

                        // Create a new file     
                        using (FileStream fs = File.Create(filepath))
                        {
                            // Add some text to file    
                            Byte[] title = new UTF8Encoding(true).GetBytes(tmpData);
                            fs.Write(title, 0, title.Length);
                        }



                        strResult = filepath;

                    }
                    catch
                    {
                        strResult = "";
                    }

                }


                task.task_end = DateTime.Now;
                task.task_status = "F";
                task.task_note = strResult;
                objMTTask.updateStatus(task);

            }
            else
            {

            }

            return strResult;
        }
        //TRN_bonus
        #endregion

        #region pf

        public string doExportPF(string com, string taskid)
        {
            string strResult = "";

            cls_ctMTTask objMTTask = new cls_ctMTTask();
            List<cls_MTTask> listMTTask = objMTTask.getDataByFillter(com, taskid, "TRN_PF", "");
            List<string> listError = new List<string>();

            if (listMTTask.Count > 0)
            {
                cls_MTTask task = listMTTask[0];

                task.task_start = DateTime.Now;

                cls_ctMTTask objTaskDetail = new cls_ctMTTask();
                cls_TRTaskdetail task_detail = objTaskDetail.getTaskDetail(task.task_id.ToString());
                cls_ctMTTask objTaskWhose = new cls_ctMTTask();
                List<cls_TRTaskwhose> listWhose = objTaskWhose.getTaskWhose(task.task_id.ToString());

                DateTime dateEff = task_detail.taskdetail_fromdate;
                DateTime datePay = task_detail.taskdetail_paydate;
                //ตัวเช็คธนาคาร
                //string[] task_bank = task_detail.taskdetail_process.Split('|');
                string[] task_details = task_detail.taskdetail_process.Split('|');
                string[] task_bank = task_details;
                string[] task_pf_detail = task_details;


                StringBuilder objStr = new StringBuilder();
                foreach (cls_TRTaskwhose whose in listWhose)
                {
                    objStr.Append("'" + whose.worker_code + "',");
                }

                string strEmp = objStr.ToString().Substring(0, objStr.ToString().Length - 1);


                //   -- Get worker
                cls_ctMTWorker objWorker = new cls_ctMTWorker();
                List<cls_MTWorker> list_worker = objWorker.getDataMultipleEmp(com, strEmp);

                //-- Step 2 Get Paypf
                cls_ctTRPaypf objPay = new cls_ctTRPaypf();
                List<cls_TRPaypf> list_pf = objPay.getDataMultipleEmp("TH", com, datePay, datePay, strEmp);


                // -- Step 3 Get Company acc
                cls_ctMTCombank objCombank = new cls_ctMTCombank();
                List<cls_MTCombank> list_combank = objCombank.getDataByFillter("", com);
                cls_MTCombank combank = list_combank[0];

                // -- Step 5 Get Emp acc
                cls_ctTRBank objEmpbank = new cls_ctTRBank();
                List<cls_TRBank> list_empbank = objEmpbank.getDataMultipleEmp(com, strEmp);
                cls_TRBank bank = list_empbank[0];

                //-- Step 6 Get Emp card
                cls_ctTRCard objEmpcard = new cls_ctTRCard();
                List<cls_TRCard> list_empcard = objEmpcard.getDataTaxMultipleEmp(com, strEmp);



                //-- Step 7 Get Company card
                cls_ctTRCard objComcard = new cls_ctTRCard();
                List<cls_TRCard> list_comcard = objComcard.getDataByFillter(com, "PVF", "", "", "");
                //cls_TRCard comcard = new cls_TRCard();

                //-- Step 8 Get Company detail
                cls_ctMTCompany objCom = new cls_ctMTCompany();
                List<cls_MTCompany> list_com = objCom.getDataByFillter("", com);
                cls_MTCompany Company = list_com[0];


                cls_TRCard comcard = list_comcard[0];

                if (list_comcard.Count > 0)
                    comcard = list_comcard[0];

                cls_ctMTPeriod objPeriod = new cls_ctMTPeriod();
                List<cls_MTPeriod> list_period = objPeriod.getDataByFillter("", com, "PAY", datePay.Year.ToString(), "M");

                cls_ctTRDep objEmpdep = new cls_ctTRDep();
                List<cls_TRDep> list_empdep = objEmpdep.getDataTaxMultipleEmp(com, strEmp, datePay);

                cls_MTPeriod period = new cls_MTPeriod();

                foreach (cls_MTPeriod tmp in list_period)
                {
                    if (tmp.period_payment.Equals(datePay))
                    {
                        period = tmp;
                        break;
                    }
                }


                string tmpData = "";
                //string[] task_pf_detail = task_detail.taskdetail_process.Split('|');


                if (list_pf.Count > 0)
                {
                    int index = 0;
                    string bkData = "";
                    string sequence = "1";
                    int TotalRecord = 0;
                    double Amount;
                    double TotalAmountEmp = 0;
                    double TotalAmountComp = 0;

                    foreach (cls_TRPaypf pf in list_pf)
                    {

                        string empnameen = "";
                        string empnameth = "";
                        string Header = "";

                        cls_MTWorker obj_worker = new cls_MTWorker();
                        cls_TRCard obj_pfcard = new cls_TRCard();
                        cls_TRCard obj_taxcard = new cls_TRCard();
                        cls_TRDep obj_empdep = new cls_TRDep();
                        cls_ctMTCompany obj_company = new cls_ctMTCompany();
                        cls_MTDep obj_mtempdep = new cls_MTDep();

                        foreach (cls_MTWorker worker in list_worker)
                        {
                            if (pf.worker_code.Equals(worker.worker_code))
                            {
                                empnameen = worker.initial_name_en + " " + worker.worker_fname_en + " " + worker.worker_lname_en;
                                obj_worker = worker;
                                break;
                            }
                        }

                        foreach (cls_MTWorker worker in list_worker)
                        {
                            if (pf.worker_code.Equals(worker.worker_code))
                            {
                                empnameth = worker.initial_name_th + " " + worker.worker_fname_th + " " + worker.worker_lname_th;
                                obj_worker = worker;
                                break;
                            }
                        }


                        foreach (cls_TRCard card in list_empcard)
                        {
                            if (pf.worker_code.Equals(card.worker_code) && card.card_type.Equals("PVF"))
                            {
                                obj_pfcard = card;
                                break;
                            }
                        }

                        foreach (cls_TRCard card in list_empcard)
                        {
                            if (pf.worker_code.Equals(card.worker_code) && card.card_type.Equals("NTID"))
                            {
                                obj_taxcard = card;
                                break;
                            }
                        }

                        foreach (cls_TRDep dep in list_empdep)
                        {
                            if (pf.worker_code.Equals(dep.worker_code))
                            {
                                obj_empdep = dep;
                                break;
                            }
                        }


                        double TotalAmount = TotalAmountEmp + TotalAmountComp;

                        //
                        if (task_bank.Length > 0 || task_pf_detail.Length > 0)
                        {
                            switch (bank.bank_code)
                            {
                                case "":///  SCB MasterFund
                                    {
                                        if (empnameen.Equals("") || obj_taxcard.card_code.Equals(""))
                                            continue;
                                        sequence = Convert.ToString(index + 1).PadLeft(6, '0');

                                        //if (empname.Equals("") || obj_taxcard.card_code.Equals("") || obj_pfcard.card_code.Equals(""))
                                        //    continue;
                                        ////
                                        bkData = bkData + '\r' + '\n';

                                        // 1, ลำดับ
                                        bkData += sequence;

                                        //2. รหัสนายจ้าง
                                        bkData += Company.company_code;

                                        // 3. ชื่อนายจ้าง
                                        bkData += Company.company_name_en;


                                        // 4. ชื่อฝ่าย
                                        bkData += obj_mtempdep.dep_name_en ;

                                        // 5.รหัสสมาชิก
                                        bkData += obj_worker.worker_code + ",";

                                        // 6. คำนำหน้าชื่อ
                                        bkData += obj_worker.worker_initial + ",";

                                        // 7. ชื่อสมาชิก th
                                        bkData += obj_worker.worker_fname_th + ",";


                                        // 8. นามสกุลสมาชิก en
                                        bkData += obj_worker.worker_fname_en + ",";


                                        // 9. เงินสะสม ( Emp Cont.)
                                        Amount = pf.paypf_emp_amount;
                                        bkData += Amount.ToString("0.00").Trim() + ",";
                                        TotalAmountEmp += Amount;

                                        // 10. เงินสมทบ ( Com Cont.)
                                        Amount = pf.paypf_com_amount;
                                        bkData += Amount.ToString("0.00").Trim();
                                        TotalAmountComp += Amount;
                                        
                                        
                                        // 11. เลขที่บัตรประชาชน
                                        bkData += obj_worker.worker_cardno + ",";

                                        // 12. รหัสแผนการลงทุน
                                        //bkData += obj_taxcard.card_code + ",";
                                        bkData += "01,";


                                        // 13. SF - 1
                                        bkData += "";

                                        // 14. SF - 2
                                        bkData += "";
                                        // 15. SF - 3
                                        bkData += "";
                                        // 16. SF - 4
                                        bkData += "";
                                        // 17. SF - 5
                                        bkData += "";
                                        // 18. SF - 6
                                        bkData += "";
                                        // 19. SF - 7
                                        bkData += "";
                                        // 20. SF - 8
                                        bkData += "";
                                        // 21. SF - 9
                                        bkData += "";
                                        // 22. SF - 10
                                        bkData += "";
                                        // 23. SF - 11
                                        bkData += "";
                            
                                        // 24. SF - 12
                                        bkData += "";
                                        // 25. SF - 13
                                        bkData += "";
                                        // 26. SF - 14
                                        bkData += "";
                                        // 27. SF - 15
                                        bkData += "";
                                        // 28. SF - 16
                                        bkData += "";
                                        // 29. SF - 17
                                        bkData += "";
                                        // 30. SF - 18
                                        bkData += "";
                                        // 31. SF - 19
                                        bkData += "";
                                        // 32. SF - 20
                                        bkData += "";

                                        // 33. ผลรวมของสัดส่วนการลงทุนของสมาชิก Total %
                                        bkData += TotalAmount.ToString("0.00") + "," + "|";  

                                        // 34. โทรศัพท์
                          
                                        bkData += obj_worker.worker_tel + ",";
                                        // 35. โทรศัพท์มือถือ
                                        bkData +=   " ";
                                        // 36. อีเมล
                                        bkData += obj_worker.worker_email + ",";
                                        // 37. ต้องการให้ระบบ Gen Password ผ่านทางอีเมล
                                        bkData += "";

                                        TotalRecord++;
                                        index++;
                                    }
                                    break;

                                case "014": //scb
                                    {
                                        // Header 

                                       
                                        // 1. รหัสกองทัน Fund Code
                                        Header += task_pf_detail[2] + "|";

                                        // 2. รหัสบริษัท Company Code
                                        Header += task_pf_detail[1] + "|";


                                        // 3. MEM_FUND (Employee Code)
                                        bkData += obj_worker.worker_code + "|";

                                        // 4. NAME_THAI (Name th) 
                                        bkData += empnameth + "|";

                                        // 5.  NAME_ENG (Name en) 
                                        bkData += empnameen + "|";

                                        // 6. M_AMT(เงินสะสม (EMP CONT.))  
                                        Amount = pf.paypf_emp_amount;
                                        bkData += Amount.ToString("0.00").Trim() + "|";
                                        TotalAmountEmp += Amount;

                                        // 7. เงินสมทบ (COMP CONT.)  
                                        Amount = pf.paypf_com_amount;
                                        bkData += Amount.ToString("0.00").Trim() + "|";
                                        TotalAmountComp += Amount;

                                        tmpData += Header  ;
                                        tmpData += bkData;
                                    }
                                    break;

                                case "004": //scb
                                    {
                                        // Header 
                                        // 1. A
                                        Header += "A" + "|";

                                        // 2. D
                                        Header += "D" + "|";

                                        // 3. รหัสกองทัน Fund Code
                                        Header += task_pf_detail[1] + "|";

                                        // 4. วันที่
                                        string datePart1 = datePay.ToString("ddMMyy", DateTimeFormatInfo.CurrentInfo);
                                        Header += datePart1 + "|";
                                        // 5. วันที่
                                        string datePart2 = datePay.ToString("ddMMyy", DateTimeFormatInfo.CurrentInfo);
                                        Header += datePart2 + "|";

                                        // 6. รหัสบริษัท Company Code
                                        Header += task_pf_detail[2] + "|";


                                        ////
                                        string initial = "";
                                        string fname = "";
                                        string lname = "";

                                        foreach (cls_MTWorker worker in list_worker)
                                        {
                                            if (pf.worker_code.Equals(worker.worker_code))
                                            {
                                                initial = worker.initial_name_th;
                                                fname = worker.worker_fname_th;
                                                lname = worker.worker_lname_th;

                                                
                                                
                                                
                                                obj_worker = worker;
                                                break;
                                            }
                                        }


                                        ///
                                        // 7.  
                                        bkData += "B" + "|";

                                        // 8. 
                                        bkData += "A" + "|";

                                        // 9. MEM_FUND (Employee Code)
                                        bkData += obj_worker.worker_code + "|";


                                        // 10. MEM_FUND (Employee Code)
                                        bkData += initial + "|";

                                        // 11. NAME_THAI (Name th) 
                                        bkData += fname + "|";
                                        

                                        // 12.  NAME_ENG (Name en) 
                                        bkData += lname + "|";

                                        //13. M_AMT(เงินสะสม (EMP CONT.))  
                                        Amount = pf.paypf_emp_amount;
                                        bkData += Amount.ToString("0.00").Trim() + "|";
                                        TotalAmountEmp += Amount;

                                        // 14. เงินสมทบ (COMP CONT.)  
                                        Amount = pf.paypf_com_amount;
                                        bkData += Amount.ToString("0.00").Trim() + "|";
                                        TotalAmountComp += Amount;

                                        // 15. เงินสมทบ (COMP CONT.)  
                                        bkData += "" + "|";

                                        // 16. เงินสมทบ (COMP CONT.)  
                                        bkData += "" + "|";

                                        // 17. เงินสมทบ (COMP CONT.)  
                                        bkData += "" + "|";

                                        // 18. รหัสบัตรประชาชน
                                        if (obj_worker.worker_cardno.Length == 13)
                                            bkData += obj_worker.worker_cardno + "|";
                                        else
                                            bkData += "0000000000000" + "|";


                                        Console.WriteLine( Header);
                                        Console.WriteLine( bkData);

                                        tmpData += Header + '\r' + '\n'+ bkData;
                                    }
                                    break;


                                default:
                                    break;
                            }
                        }
                        try
                        {
                            //-- Step 1 create file
                            string filename = "TRN_PF_" + DateTime.Now.ToString("yyMMddHHmm") + "." + "xls";
                            string filepath = Path.Combine
                           (ClassLibrary_BPC.Config.PathFileExport, filename);

                            // Check if file already exists. If yes, delete it.     
                            if (File.Exists(filepath))
                            {
                                File.Delete(filepath);
                            }
                            DataSet ds = new DataSet();
                            string str = tmpData.Replace("\r\n", "]");
                            string[] data = str.Split(']');
                            DataTable dataTable = ds.Tables.Add();

                            bool bank_code014 = (bank.bank_code == "014"); // scb
                            //bool bank_code  = (bank.bank_code == ""); // SCB-MasterFund
                            bool bank_code004 = (bank.bank_code == "004"); // กสิกรไทย


                            //int columnsCount = Math.Min(dataTable.Columns.Count, 6);

                            if (bank_code014)
                            {
                                dataTable.Columns.AddRange(new DataColumn[7] { new DataColumn("FUND_CODE"), new DataColumn("COMP_CODE"), new DataColumn("MEM_FUND"), new DataColumn("NAME_THAI"), new DataColumn("NAME_ENG"), new DataColumn("M_AMT"), new DataColumn("F_AMT") });
                            }
                            else if (bank_code004)
                            {
                                // เพิ่ม DataColumn 6 คอลัมน์แรก
                                dataTable.Columns.AddRange(new DataColumn[18] { new DataColumn("A"), new DataColumn("B"), new DataColumn("C"), new DataColumn("D"), new DataColumn("E"), new DataColumn("F") ,new DataColumn("A1"), new DataColumn("B1"), new DataColumn("C1"), new DataColumn("D1"), new DataColumn("E1"), new DataColumn("F1"), new DataColumn("G1"), new DataColumn("H1"), new DataColumn("I1"), new DataColumn("J1"), new DataColumn("K1"),  new DataColumn("L1") });
                            }
                            //else if (bank_code002)
                            //{
                            //    dataTable.Columns.AddRange(new DataColumn[37] { new DataColumn("No."), new DataColumn("Company Code"), new DataColumn("Company Name"), new DataColumn("Department Code"), new DataColumn("Employee Code"), new DataColumn("Title Name"), new DataColumn("First Name"), new DataColumn("Last Name"), new DataColumn("เงินสะสม (Emp Cont.)"), new DataColumn("เงินสมทบ (Com Cont.)"), new DataColumn("ID. No"), new DataColumn("MENU"), new DataColumn("PVDMPFMM"), new DataColumn("PVDMPFFI"), new DataColumn("PVDMPFEQ"), new DataColumn("PVDMGLDH"), new DataColumn("SF-5"), new DataColumn("SF-6"), new DataColumn("SF-7"), new DataColumn("SF-8"), new DataColumn("SF-9"), new DataColumn("SF-10"), new DataColumn("SF-11"), new DataColumn("SF-12"), new DataColumn("SF-13"), new DataColumn("SF-14"), new DataColumn("SF-15"), new DataColumn("SF-16"), new DataColumn("SF-17"), new DataColumn("SF-18"), new DataColumn("SF-19"), new DataColumn("SF-20"), new DataColumn("Total %"), new DataColumn("เบอร์โทรศัพท์"), new DataColumn("เบอร์โทรศัพท์มือถือ"), new DataColumn("อีเมล"), new DataColumn("ต้องการให้ระบบ  Gen Password ผ่านทางอีเมล") });
                            // }
 
                            foreach (var i in data)
                            {
                                if (!string.IsNullOrEmpty(i))
                                {
                                    string[] array = i.Split('|');

                                    if (bank_code014)
                                    {
                                        dataTable.Rows.Add(array[0], array[1], array[2], array[3], array[4], array[5], array[6]);
                                    }
                                    else if (bank_code004)
                                    {
                                        
                                        dataTable.Rows.Add(array[0], array[1], array[2], array[3], array[4], array[5] , array[6], array[7], array[8], array[9], array[10], array[11], array[12] ,  array[13], array[14], array[15], array[16], array[17] );

                                    }
                                    //else if (bank_code)
                                    //{
                                    //    dataTable.Rows.Add(array[0], array[1], array[2], array[3], array[4], array[5], array[6], array[7], array[8], array[9], array[10], array[11], array[12], array[13], array[14], array[15], array[16], array[17], array[18], array[19], array[20], array[21], array[22], array[23], array[24], array[25], array[26], array[27], array[28], array[29], array[30], array[31], array[32], array[33], array[34], array[35], array[36], array[37]);

                                    //}
                                }
                            }




                            ExcelLibrary.DataSetHelper.CreateWorkbook(filepath, ds);

                            strResult = filepath;

                        }
                        catch (Exception ex)
                        {
                            strResult = ex.ToString();
                        }
                    }

                
                    task.task_end = DateTime.Now;
                    task.task_status = "F";
                    task.task_note = strResult;
                    objMTTask.updateStatus(task);

                }}
                else
                {

                }

                return strResult;
            }

        #endregion



        //#region doExportPFf
        //public string doExportPFf(string com, string taskid)
        //{
        //    string strResult = "";

        //    cls_ctMTTask objMTTask = new cls_ctMTTask();
        //    List<cls_MTTask> listMTTask = objMTTask.getDataByFillter(com, taskid, "TRN_PF", "");
        //    List<string> listError = new List<string>();

        //    if (listMTTask.Count > 0)
        //    {
        //        cls_MTTask task = listMTTask[0];

        //        task.task_start = DateTime.Now;

        //        cls_ctMTTask objTaskDetail = new cls_ctMTTask();
        //        cls_TRTaskdetail task_detail = objTaskDetail.getTaskDetail(task.task_id.ToString());
        //        cls_ctMTTask objTaskWhose = new cls_ctMTTask();
        //        List<cls_TRTaskwhose> listWhose = objTaskWhose.getTaskWhose(task.task_id.ToString());

        //        DateTime dateEff = task_detail.taskdetail_fromdate;
        //        DateTime datePay = task_detail.taskdetail_paydate;
        //        //ตัวเช็คธนาคาร
        //        //string[] task_bank = task_detail.taskdetail_process.Split('|');
        //        string[] task_details = task_detail.taskdetail_process.Split('|');
        //        string[] task_bank = task_details;
        //        string[] task_pf_detail = task_details;


        //        StringBuilder objStr = new StringBuilder();
        //        foreach (cls_TRTaskwhose whose in listWhose)
        //        {
        //            objStr.Append("'" + whose.worker_code + "',");
        //        }

        //        string strEmp = objStr.ToString().Substring(0, objStr.ToString().Length - 1);


        //        //   -- Get worker
        //        cls_ctMTWorker objWorker = new cls_ctMTWorker();
        //        List<cls_MTWorker> list_worker = objWorker.getDataMultipleEmp(com, strEmp);

        //        //-- Step 2 Get Paypf
        //        cls_ctTRPaypf objPay = new cls_ctTRPaypf();
        //        List<cls_TRPaypf> list_pf = objPay.getDataMultipleEmp("TH", com, datePay, datePay, strEmp);


        //        // -- Step 3 Get Company acc
        //        cls_ctMTCombank objCombank = new cls_ctMTCombank();
        //        List<cls_MTCombank> list_combank = objCombank.getDataByFillter("", com);
        //        cls_MTCombank combank = list_combank[0];

        //        // -- Step 5 Get Emp acc
        //        cls_ctTRBank objEmpbank = new cls_ctTRBank();
        //        List<cls_TRBank> list_empbank = objEmpbank.getDataMultipleEmp(com, strEmp);
        //        cls_TRBank bank = list_empbank[0];

        //        //-- Step 6 Get Emp card
        //        cls_ctTRCard objEmpcard = new cls_ctTRCard();
        //        List<cls_TRCard> list_empcard = objEmpcard.getDataTaxMultipleEmp(com, strEmp);



        //        //-- Step 7 Get Company card
        //        cls_ctTRCard objComcard = new cls_ctTRCard();
        //        List<cls_TRCard> list_comcard = objComcard.getDataByFillter(com, "PVF", "", "", "");
        //        //cls_TRCard comcard = new cls_TRCard();

        //        //-- Step 8 Get Company detail
        //        cls_ctMTCompany objCom = new cls_ctMTCompany();
        //        List<cls_MTCompany> list_com = objCom.getDataByFillter("", com);
        //        cls_MTCompany Company = list_com[0];


        //        cls_TRCard comcard = list_comcard[0];

        //        if (list_comcard.Count > 0)
        //            comcard = list_comcard[0];

        //        cls_ctMTPeriod objPeriod = new cls_ctMTPeriod();
        //        List<cls_MTPeriod> list_period = objPeriod.getDataByFillter("", com, "PAY", datePay.Year.ToString(), "M");

        //        cls_ctTRDep objEmpdep = new cls_ctTRDep();
        //        List<cls_TRDep> list_empdep = objEmpdep.getDataTaxMultipleEmp(com, strEmp, datePay);

        //        cls_MTPeriod period = new cls_MTPeriod();

        //        foreach (cls_MTPeriod tmp in list_period)
        //        {
        //            if (tmp.period_payment.Equals(datePay))
        //            {
        //                period = tmp;
        //                break;
        //            }
        //        }


        //        string tmpData = "";
        //        string tmpData004 = "";                //string[] task_pf_detail = task_detail.taskdetail_process.Split('|');


        //        if (list_pf.Count > 0)
        //        {
        //            int index = 0;
        //            string bkData = "";

        //            string sequence = "1";
        //            int TotalRecord = 0;
        //            double Amount;
        //            double TotalAmountEmp = 0;
        //            double TotalAmountComp = 0;

        //            string bkData004 = "";


        //            foreach (cls_TRPaypf pf in list_pf)
        //            {

        //                string empnameen = "";
        //                string empnameth = "";
        //                string Header = "";

        //                string Header004 = "";


        //                cls_MTWorker obj_worker = new cls_MTWorker();
        //                cls_TRCard obj_pfcard = new cls_TRCard();
        //                cls_TRCard obj_taxcard = new cls_TRCard();
        //                cls_TRDep obj_empdep = new cls_TRDep();
        //                cls_ctMTCompany obj_company = new cls_ctMTCompany();
        //                cls_MTDep obj_mtempdep = new cls_MTDep();

        //                foreach (cls_MTWorker worker in list_worker)
        //                {
        //                    if (pf.worker_code.Equals(worker.worker_code))
        //                    {
        //                        empnameen = worker.initial_name_en + " " + worker.worker_fname_en + " " + worker.worker_lname_en;
        //                        obj_worker = worker;
        //                        break;
        //                    }
        //                }

        //                foreach (cls_MTWorker worker in list_worker)
        //                {
        //                    if (pf.worker_code.Equals(worker.worker_code))
        //                    {
        //                        empnameth = worker.initial_name_th + " " + worker.worker_fname_th + " " + worker.worker_lname_th;
        //                        obj_worker = worker;
        //                        break;
        //                    }
        //                }


        //                foreach (cls_TRCard card in list_empcard)
        //                {
        //                    if (pf.worker_code.Equals(card.worker_code) && card.card_type.Equals("PVF"))
        //                    {
        //                        obj_pfcard = card;
        //                        break;
        //                    }
        //                }

        //                foreach (cls_TRCard card in list_empcard)
        //                {
        //                    if (pf.worker_code.Equals(card.worker_code) && card.card_type.Equals("NTID"))
        //                    {
        //                        obj_taxcard = card;
        //                        break;
        //                    }
        //                }

        //                foreach (cls_TRDep dep in list_empdep)
        //                {
        //                    if (pf.worker_code.Equals(dep.worker_code))
        //                    {
        //                        obj_empdep = dep;
        //                        break;
        //                    }
        //                }


        //                double TotalAmount = TotalAmountEmp + TotalAmountComp;

        //                //
        //                if (task_bank.Length > 0 || task_pf_detail.Length > 0)
        //                {
        //                    switch (bank.bank_code)
        //                    {
        //                        case "004":/// ธนาคารกรุงเทพ
        //                            {

        //                                string comcode = " ";
        //                                string PFcode = " ";



        //                                // Header 


        //                                // 3. รหัสกองทัน Fund Code
        //                                comcode += task_pf_detail[2];
        //                                // 4. รหัสบริษัท Company Code
        //                                PFcode += task_pf_detail[1];

        //                                // 4. วันที่
        //                                string datePart1 = datePay.ToString("ddMMyy", DateTimeFormatInfo.CurrentInfo);

        //                                string datePart2 = datePay.ToString("yyMMdd", DateTimeFormatInfo.CurrentInfo);
        //                                datePart1 += datePart1 + "|";

        //                                //กำหนดค่า tmpData โดยรวมข้อมูล combank.combank_bankcode และ comdetail.company_name_en
        //                                tmpData = "A" + " " + "D" + " " + comcode + " " + datePart1 + " " + datePart1 + " " + PFcode + " " + "\r\n";


        //                                double douTotal = 0;
        //                                //int index = 0;
        //                                string amount;
        //                                //string bkData;

        //                                foreach (cls_TRPaypf pf1 in list_pf)
        //                                {

        //                                    string empacc = "";
        //                                    string empname = "";
        //                                    string empname2 = "";

        //                                    foreach (cls_MTWorker worker in list_worker)
        //                                    {
        //                                        if (pf1.worker_code.Equals(worker.worker_code))
        //                                        {
        //                                            empname = " " + worker.initial_name_en + " " + worker.worker_fname_en + " " + worker.worker_lname_en + " " + datePay.ToString("ddMMyy", DateTimeFormatInfo.CurrentInfo) + " ";
        //                                            empname2 = " " + worker.initial_name_en + " " + worker.worker_fname_en + " " + worker.worker_lname_en + " ";

        //                                            break;
        //                                        }
        //                                    }

        //                                    foreach (cls_TRBank worker in list_empbank)
        //                                    {
        //                                        if (pf1.worker_code.Equals(worker.worker_code))
        //                                        {
        //                                            empacc = worker.bank_account.Replace("-", "");
        //                                            break;
        //                                        }
        //                                    }




        //                                    //"Header Record  (ส่วนที่ 1)"   
        //                                    if (empname.Equals("") || empacc.Equals(""))
        //                                        continue;


        //                                    sequence = Convert.ToString(index + 2).PadLeft(6, '0');

        //                                    //amount = temp.ToString("#.#0").Trim().Replace(".", "").PadLeft(10, '0');





        //                                    bkData = "B" + "|" + "A" + "|" + "รหัสพนักงาน" + "|" + "คำนำหน้า" + "|" + "ชื่อ" + "|" + "นามสกุล" + "|" + " " + "|" + " " + "|" + " " + "|" + "บัตรประชาชน" + "|";
        //                                    bkData = bkData.PadRight(32, '0');

        //                                    if (empname2.Length > 35)
        //                                        empname2 = empname2.Substring(0, 35);

        //                                    bkData = bkData + empname2.ToUpper();
        //                                    tmpData += bkData.PadRight(128, ' ') + "\r\n";

        //                                    //douTotal += pf.paytran_netpay_b;
        //                                    index++;
        //                                }

        //                                tmpData += bkData;

        //                            }
        //                            break;

        //                        default:
        //                            break;
        //                    }
        //                }
        //                try
        //                {
        //                    //-- Step 1 create file
        //                    //string filename = "UPAYDAT.DAT";
        //                    string filename = "TRN_BANK_" + DateTime.Now.ToString("yyMMddHHmm") + "." + "xls";
        //                    string filepath = Path.Combine
        //                   (ClassLibrary_BPC.Config.PathFileExport, filename);

        //                    // Check if file already exists. If yes, delete it.     
        //                    if (File.Exists(filepath))
        //                    {
        //                        File.Delete(filepath);
        //                    }

        //                    // Create a new file     
        //                    using (FileStream fs = File.Create(filepath))
        //                    {
        //                        // Add some text to file    
        //                        Byte[] title = new UTF8Encoding(true).GetBytes(tmpData);
        //                        fs.Write(title, 0, title.Length);
        //                    }

        //                    strResult = filepath;

        //                }
        //                catch
        //                {
        //                    strResult = "";
        //                }

        //            }
        //        }

        //        task.task_end = DateTime.Now;
        //        task.task_status = "F";
        //        task.task_note = strResult;
        //        objMTTask.updateStatus(task);

        //    }
        //    else
        //    {

        //    }

        //    return strResult;
        //}




        //#endregion


    }
}
 
 