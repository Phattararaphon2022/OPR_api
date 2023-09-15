using ClassLibrary_BPC.hrfocus.controller;
using ClassLibrary_BPC.hrfocus.controller.Payroll;
using ClassLibrary_BPC.hrfocus.model;
using ClassLibrary_BPC.hrfocus.model.Payroll;
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
  public  class cls_srvProcessPayroll
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

        #region SSO
        //SSO   
         public string doExportSso(string com,string taskid)
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
                 cls_TRPaytran paybank = list_paytran[0];


                 //-- Step 3 Get Company acc
                 cls_ctMTCombank objCombank = new cls_ctMTCombank();
                 List<cls_MTCombank> list_combank = objCombank.getDataByFillter(com);
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
                 List<cls_MTProvince> list_province = objProvince.getDataByFillter( "");



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

                             //7.อัตราเงินสมทบ
                             if (paybank.paytran_ssocom == 4)
                                 bkData += paybank.paytran_ssocom;
                             else
                                 bkData += paybank.paytran_ssocom.ToString("0000") + "|";

                             //bkData += paybank.paytran_ssocom + "|";




                             //8.จำนวนผู้ประกันตน			
                             //bkData += paytran.paytran_tax_401.ToString("0.00") + "|";
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

                             //7.จำนวนเงินสมทบ
                             if (paytran.paytran_pfemp == 12)
                                 bkData += paytran.paytran_pfemp + "|";
                             else
                                 bkData += paytran.paytran_pfemp.ToString("000000000000") + "|";


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
                            // Add some text to file    
                            Byte[] title = new UTF8Encoding(true).GetBytes(tmpData);
                            fs.Write(title, 0, title.Length);
                        }



                        strResult = filename;

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
                 List<cls_MTCombank> list_combank = objCombank.getDataByFillter(com);
                 cls_MTCombank combank = list_combank[0];

                 //-- Step 4 Get Company detail
                 cls_ctMTCompany objCom = new cls_ctMTCompany();
                 List<cls_MTCompany> list_com = objCom.getDataByFillter("", com);
                 cls_MTCompany comdetail = list_com[0];



                 //-- Step 5 Get Emp acc
                 cls_ctTRBank objEmpbank = new cls_ctTRBank();
                 List<cls_TRBank> list_empbank = objEmpbank.getDataMultipleEmp(com, strEmp);

                 //-- Step 6 Get pay bank
                
                 cls_ctTRPaybank objPaybank = new cls_ctTRPaybank();
                 List<cls_TRPaybank> list_paybank = objPaybank.getDataByFillter(com, strEmp);
                 cls_TRPaybank paybank = list_paybank[0];

                 

                 string tmpData = "";


                 if (list_paytran.Count > 0)
                 {
                     // ตรวจสอบความยาวของ comdetail.company_name_en หากมากกว่า 25 ตัวอักษรให้ตัดทิ้งเหลือเพียง 25 ตัวอักษร
                     if (comdetail.company_name_en.Length > 25)
                         comdetail.company_name_en = comdetail.company_name_en.Remove(25, comdetail.company_name_en.Length - 25);
                     // ตรวจสอบความยาวของ comdetail.company_name_en หากน้อยกว่า 25 ตัวอักษรให้เติมด้วยช่องว่างจนครบ 25 ตัวอักษร
                     if (comdetail.company_name_en.Length < 25)
                         comdetail.company_name_en = comdetail.company_name_en.PadRight(25, ' ');

                     // กำหนดค่า tmpData โดยรวมข้อมูล combank.combank_bankcode และ comdetail.company_name_en
                     tmpData = combank.combank_bankcode + "|" + comdetail.company_name_en + "|";

                     double douTotal = 0;
                     int index = 0;
                     string sequence;
                     string amount;
                     string bkData;
                  

                

                    foreach (cls_TRPaytran paytran in list_paytran)
                    {
                        string empacc = "";
                        string empname = "";

                        // วนลูปเพื่อค้นหาข้อมูล worker ใน list_worker ที่ตรงกับ worker_code ใน paytran
                        foreach (cls_MTWorker worker in list_worker)
                        {
                            if (paytran.worker_code.Equals(worker.worker_code))
                            {
                                // กำหนดค่า empname โดยรวมข้อมูลจาก worker
                                empname = " " + worker.initial_name_en + " " + worker.worker_fname_en + " " + worker.worker_lname_en + "|" + datePay.ToString("ddMMyy", DateTimeFormatInfo.CurrentInfo) + "|";

                                break;
                            }
                        }

                        // วนลูปเพื่อค้นหาข้อมูล worker ใน list_empbank ที่ตรงกับ worker_code ใน paytran
                        foreach (cls_TRBank worker in list_empbank)
                        {
                            if (paytran.worker_code.Equals(worker.worker_code))
                            {
                                // กำหนดค่า empacc โดยรับประจำที่ bank_account และลบเครื่องหมาย -
                                empacc = worker.bank_account.Replace("-", "");
                                break;
                            }
                        }

                        // วนลูปเพื่อค้นหาข้อมูล paybanks ใน list_paybank ที่ตรงกับ worker_code ใน paytran
                        foreach (cls_TRPaybank paybanks in list_paybank)
                        {
                            if (paytran.worker_code.Equals(paybanks.worker_code))
                            {
                                // กำหนดค่า empacc โดยรับประจำที่ bank_code และลบเครื่องหมาย -
                                empacc = paybanks.paybank_code.Replace("-", "");
                                break;
                            }
                        }

                        // ตรวจสอบว่ามีค่าว่างใน empname หรือ empacc หากมีให้ข้ามไปทำงานกับข้อมูลรอบถัดไป

                        //"Header Record  (ส่วนที่ 1)"     
                        
                        if (empname.Equals("") || empacc.Equals(""))
                            empname += paytran.worker_detail;
                        else
                            empname +=   empacc.Equals("") + "|";
 
                        //if (empname.Equals("") || empacc.Equals(""))
                        //    continue;

                        // กำหนดค่า sequence โดยแปลงตัวเลข index+2 เป็นสตริงและเติมเต็มด้วยศูนย์ด้านหน้าเพื่อให้ครบ 6 หลัก
                        sequence = Convert.ToString(index + 2).ToString().PadLeft(6, '0');

                        // แปลงค่า paytran.paytran_netpay_b เป็น decimal และเก็บในตัวแปร temp
                        decimal temp = (decimal)paytran.paytran_netpay_b;

                        // แปลงค่า temp เป็นสตริงโดยใช้รูปแบบที่ต้องการและเติมเต็มด้วยศูนย์ด้านหน้าเพื่อให้ครบ 10 หลัก
                        amount = temp.ToString("#.#0").Trim().Replace(".", "").PadLeft(10, '0');

                        // กำหนดค่า bkData โดยรวมข้อมูลตามรูปแบบที่ต้องการ
                        //bkData = "D" + sequence + "002" + empacc + "C" + amount + "029";

                        // กำหนดค่า bkData อีกครั้ง
                        bkData = " " + "D" + "|" + combank.combank_bankaccount + "|" + "C" + "|";
                        bkData = bkData.PadRight('0');

                        // กำหนดค่า bkData อีกครั้ง
                        bkData = bkData.PadRight('0') + "|";

                        // ตรวจสอบความยาวของ empname หากมีมากกว่า 35 ตัวอักษรให้ตัดทิ้งเหลือเพียง 35 ตัวอักษร
                        if (empname.Length > 35)
                            empname = empname.Substring(0, 35);

                        // กำหนดค่า bkData โดยรวมข้อมูล empname และแนวขวางสุดด้านขวาให้ครบ 128 ตัวอักษรด้วยช่องว่าง
                        bkData = bkData + empname.ToUpper();

                        // เพิ่มค่า bkData ลงในตัวแปร tmpData และเติมด้วยตัวขึ้นบรรทัดใหม่
                        tmpData += bkData.PadRight(128, ' ') + '\r' + '\n';

                        // บวกค่า paytran.paytran_netpay_b เข้ากับตัวแปร douTotal
                        douTotal += paytran.paytran_netpay_b;

                        index++;



                     }
                    int record = list_paybank.Count;
                    sequence = Convert.ToString(index + 2).ToString().PadLeft(6, '0');
                    bkData = "T" + "|" + paybank.paybank_bankamount + "|" + paybank.paybank_bankaccount + "|";
                    bkData = bkData + record.ToString().PadLeft('0') + "|" + combank.combank_id + "|";
                    tmpData += bkData.PadRight('0') + "1" + "|";

                  

                     try
                     {
                         //-- Step 1 create file
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

                         strResult = filename;

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
                 List<cls_MTCombank> list_combank = objCombank.getDataByFillter(com);
                 cls_MTCombank combank = list_combank[0];

                 //-- Step 4 Get Company detail
                 cls_ctMTCompany objCom = new cls_ctMTCompany();
                 List<cls_MTCompany> list_com = objCom.getDataByFillter("", com);
                 cls_MTCompany comdetail = list_com[0];

                 //-- Step 5 Get Emp address

                 cls_ctTRAddress objEmpadd = new cls_ctTRAddress();
                 List<cls_TRAddress> list_empaddress = objEmpadd.getDataByFillter(com, strEmp);

                 //-- Step 6 Get Emp card
                 cls_ctTRCard objEmpcard = new cls_ctTRCard();
                 List<cls_TRCard> list_empcard = objEmpcard.getDataTaxMultipleEmp(com, strEmp);

      

                 //-- Step 7 Get Company card
                 cls_ctTRCard objComcard = new cls_ctTRCard();
                 List<cls_TRCard> list_comcard = objComcard.getDataByFillter(com, "NTID", "", "", "");
                 cls_TRCard comcard = list_comcard[0];


                 cls_ctMTProvince objProvince = new cls_ctMTProvince();
                 List<cls_MTProvince> list_province = objProvince.getDataByFillter( "");
 
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
                         //foreach (cls_MTProvince province in list_province)
                         //{
                         //    if (obj_address.province_code.Equals(province.province_code))
                         //    {
                         //        obj_province = province;
                         //        break;
                         //    }
                         //}
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
        
                             bkData = "00|";
                             //bkData += "00|";
                             // 2.เลขประจำตัวประชาชนผู้มี่หน้าที่หัก ณ ที่จ่าย<CardNo>
                             bkData += comdetail.citizen_no.Length == 13 ? comdetail.citizen_no : "0000000000000";
                             bkData += "|";
                             // 3.เลขประจำตัวผู้เสียภาษีอากรผู้มีหน้าที่หัก ณ ที่จ่าย<TaxNo>
                             bkData += comdetail.sso_tax_no.Length == 10 ? comdetail.sso_tax_no : "0000000000";
                             bkData += "|";
                             // 4.เลขที่สาขา ผู้มีหน้าที่หักภาษี ณ ที่จ่าย<BranchID>
                             bkData += combank.company_code.Length == 4 ? combank.company_code : "00000";
                             bkData += "|";
                             // 5.เลขประจำตัวประชาชนผู้มีเงินได้<CardNo>	
                             bkData += comcard.card_code.Length == 13 ? comcard.card_code : "0000000000000";
                             bkData += "|";
                             // 6.เลขประจำตัวผู้เสียภาษีอากรผู้มีเงินได้ <TaxNo>
                             bkData += comcard.card_code.Length == 13 ? comcard.card_code : "0000000000";
                             bkData += "|";
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
                             bkData += obj_address.address_zipcode + "|";
                             // 13.เดือนภาษี<TaxMonth>
                             bkData += datePay.Month.ToString().PadLeft(2, '0') + "|";
                             // 14.ปีภาษี<TaxYear>
                             int n = Convert.ToInt32(datePay.Year);
                             if (n < 2400)
                                 n += 543;
                             bkData += n.ToString() + "|";
                             // 15.รหัสเงินได้<AllwonceCode>
                             bkData += "1|";
                             // 16.วันที่จ่ายเงินได้ <TaxDate>+<TaxMonth>+<TaxYear>
                             bkData += datePay.ToString("ddMM") + n.ToString() + "|";
                             // 17.อัตราภาษีร้อยละ
                             bkData += "0|";
                             // 18.จำนวนเงินที่จ่าย<PayMent>
                             bkData += paytran.paytran_income_401.ToString("0.00") + "|";
                             // 19.จำนวนเงินภาษีที่หักและนำส่ง<Tax>
                             bkData += paytran.paytran_tax_401.ToString("0.00") + "|";
                             // 20.เงื่อนไขการหักภาษี ณ จ่าย <TaxCondition>
                             bkData += "1";

                         
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

                         strResult = filename;

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
                 List<cls_MTCombank> list_combank = objCombank.getDataByFillter(com);
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
                 List<cls_MTProvince> list_province = objProvince.getDataByFillter( "");

                 //-- Step 9 Get Company card
                 cls_ctTRPaybonus objPaybonus= new cls_ctTRPaybonus();
                 List<cls_TRPaybonus> list_paybonus = objPaybonus.getDataByFillter("", com, datePay ,  "");
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
                         cls_TRPaybonus obj_paybonus= new cls_TRPaybonus();


 
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



                        strResult = filename;

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

     }
}
 
 