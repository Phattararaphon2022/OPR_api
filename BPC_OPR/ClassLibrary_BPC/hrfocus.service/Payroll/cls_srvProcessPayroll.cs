using ClassLibrary_BPC.hrfocus.controller;
using ClassLibrary_BPC.hrfocus.controller.Payroll;
using ClassLibrary_BPC.hrfocus.model;
using ClassLibrary_BPC.hrfocus.model.Payroll;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary_BPC.hrfocus.service.Payroll
{
  public  class cls_srvProcessPayroll
     {

        //private void addTimeManual(ref List<cls_TRTimeinput> listTimeinput, DateTime date, string time)
        //{
        //    cls_TRTimeinput model = new cls_TRTimeinput();
        //    model.timeinput_hhmm = time;
        //    model.timeinput_compare = "N";
        //    model.timeinput_function = "RECORD";
        //    model.timeinput_date = date;

        //    listTimeinput.Add(model);
        //}

        //private int doConvertTime2Int(string value)
        //{
        //    int intResult = 0;
        //    try
        //    {
        //        intResult = Convert.ToInt32(value);
        //    }
        //    catch { }

        //    return intResult;
        //}




         //SSO test 
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
                 //cls_ctTREmpcard objEmpcard = new cls_ctTREmpcard();
                 //List<cls_TREmpcard> list_empcard = objEmpcard.getDataTaxMultipleEmp(com, strEmp);

                 //-- Step 7 Get Company card
                 cls_ctMTComcard objComcard = new cls_ctMTComcard();
                 List<cls_MTComcard> list_comcard = objComcard.getDataByFillter(com, "NTID", "", "", "");
                 cls_MTComcard comcard = list_comcard[0];

                 cls_ctMTProvince objProvince = new cls_ctMTProvince();
                 List<cls_MTProvince> list_province = objProvince.getDataByFillter("");



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
                         cls_MTProvince obj_card = new cls_MTProvince();

                         //cls_TREmpcard obj_card = new cls_TREmpcard();

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

                         //foreach (cls_TREmpcard card in list_empcard)
                         //{
                         //    if (paytran.worker_code.Equals(card.worker_code))
                         //    {
                         //        obj_card = card;
                         //        break;
                         //    }
                         //}

                         foreach (cls_MTProvince province in list_province)
                         {
                             if (obj_address.province_code.Equals(province.province_code))
                             {
                                 obj_province = province;
                                 break;
                             }
                         }
                         //"Header Record  (ส่วนที่ 1)"     

                         if (empname.Equals("") || obj_card.province_code.Equals(""))
                             continue;


                         if (paytran.paytran_income_401 > 0)
                         {

                             //1.ประเภทข้อมูล
                             bkData = obj_card.province_id + "|";

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
                         if (empname.Equals("") || obj_card.province_code.Equals(""))
                             continue;


                         if (paytran.paytran_income_401 > 0)
                         {

                             //1.ประเภทข้อมูล
                             bkData = "2|";


                             //2.เลขที่บัตรประชาชน
                             if (comcard.comcard_code.Length == 13)
                                 bkData += comcard.comcard_code + "|";
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
                         string filename = "TRN_SSO_" + DateTime.Now.ToString("yyMMddHHmm") + "." + "xls";
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
                         dataTable.Columns.AddRange(new DataColumn[12] { new DataColumn("ลำดับที่"), new DataColumn("เดือน / ปี"), new DataColumn("เลขประจำตัวประชาชน"), new DataColumn("ชื่อ-นามสกุล"), new DataColumn("กยศ."), new DataColumn("กรอ."), new DataColumn("จำนวนเงิน"), new DataColumn("ยอดยืนยันนำส่ง"), new DataColumn("วันที่หักเงินเดือน"), new DataColumn("ไม่ได้นำส่งเงิน"), new DataColumn("รหัสสาเหตุ"), new DataColumn("ไฟล์แนบ") });
                         foreach (var i in data)
                         {
                             if (i.Equals(""))
                                 continue;
                             string[] array = i.Split('|');
                             dataTable.Rows.Add(array[0], array[1], array[2], array[3], array[4], array[5], array[6], array[7], array[8], array[9], array[10], array[11]);
                         }
                         ExcelLibrary.DataSetHelper.CreateWorkbook(filepath, ds);

                         // Create a new file     
                         //using (FileStream fs = File.Create(filepath))
                         //{
                         //    // Add some text to file    
                         //    Byte[] Table = new UTF8Encoding(true).GetBytes(tmpData);
                         //    fs.Write(Table, 0, Table.Length);


                         //}

                         strResult = filename;

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

             }
             else
             {

             }

             return strResult;
         }


     //TRN_BANK เริ่ม
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
              List<cls_TRPaytran> list_paytran = objPay.getDataMultipleEmp("TH", com, datePay, datePay, strEmp);



              //-- Step 3 Get Company acc
              cls_ctMTCombank objCombank = new cls_ctMTCombank();
              List<cls_MTCombank> list_combank = objCombank.getDataByFillter(com);
              cls_MTCombank combank = list_combank[0];

              //-- Step 4 Get Company detail
              cls_ctMTCompany objCom = new cls_ctMTCompany();
              List<cls_MTCompany> list_com = objCom.getDataByFillter("", com);
              cls_MTCompany comdetail = list_com[0];



              //-- Step 5 Get Emp acc
              //cls_ctTREmpbank objEmpbank = new cls_ctTREmpbank();
              //List<cls_TREmpbank> list_empbank = objEmpbank.getDataMultipleEmp(com, strEmp);

              //-- Step 6 Get pay bank
              //cls_ctTRPaybank objPaybank = new cls_ctTRPaybank();
              //List<cls_TRPaybank> list_paybank = objPaybank.getDataByFillter(com, strEmp);
              //cls_TRPaybank paybank = list_paybank[0];


              string tmpData = "";



              if (list_paytran.Count > 0)
              {
                  //-- Head

                  if (comdetail.company_name_en.Length > 25)
                      comdetail.company_name_en = comdetail.company_name_en.Remove(25, comdetail.company_name_en.Length - 25);
                  if (comdetail.company_name_en.Length < 25)
                      comdetail.company_name_en = comdetail.company_name_en.PadRight(25, ' ');
                  tmpData = combank.combank_bankcode + "|" + comdetail.company_name_en + "|";
                  //tmpData = tmpData.PadRight(128, '0') + '\r' + '\n';




                  double douTotal = 0;

                  int index = 0;

                  string sequence;
                  string amount;
                  string bkData;

                  foreach (cls_TRPaytran paytran in list_paytran)
                  {
                      string empacc = "";
                      string empname = "";


                      foreach (cls_MTWorker worker in list_worker)
                      {
                          if (paytran.worker_code.Equals(worker.worker_code))
                          {
                              empname = " " + worker.initial_name_en + " " + worker.worker_fname_en + " " + worker.worker_lname_en + "|" + datePay.ToString("ddMMyy", DateTimeFormatInfo.CurrentInfo) + "|";

                              break;
                          }
                      }


                      //foreach (cls_TREmpbank worker in list_empbank)
                      //{
                      //    if (paytran.worker_code.Equals(worker.worker_code))
                      //    {
                      //        empacc = worker.empbank_bankaccount.Replace("-", "");
                      //        break;
                      //    }
                      //}

                      //foreach (cls_TREmpbank worker in list_empbank)
                      //{
                      //    if (paytran.worker_code.Equals(worker.worker_code))
                      //    {
                      //        empacc = worker.empbank_bankaccount.Replace("-", "");
                      //        break;
                      //    }
                      //}

                      //foreach (cls_TRPaybank paybanks in list_paybank)
                      //{
                      //    if (paytran.worker_code.Equals(paybanks.worker_code))
                      //    {
                      //        empacc = paybanks.paybank_bankcode.Replace("-", "");
                      //        break;
                      //    }
                      //}

                      if (empname.Equals("") || empacc.Equals(""))
                          continue;

                      sequence = Convert.ToString(index + 2).ToString().PadLeft(6, '0');

                      decimal temp = (decimal)paytran.paytran_netpay_b;

                      amount = temp.ToString("#.#0").Trim().Replace(".", "").PadLeft(10, '0');
                      //bkData = "D" + sequence + "002" + empacc + "C" + amount + "029";
                      bkData = " " + "D" + "|" + combank.combank_bankaccount + "|" + "C" + "|";
                      bkData = bkData.PadRight('0');

                      //bkData = "|" + amount + "|";
                      //bkData = "D" + "|r" + paybank.paybank_bankaccount + "r|" + "d|" + paybank.paybank_bankamount+ "d|"  ;
                      //bkData = " " + combank.combank_bankaccount + " " + "|";
                      bkData = bkData.PadRight('0') + "|";

                      if (empname.Length > 35)
                          empname = empname.Substring(0, 35);

                      bkData = bkData + empname.ToUpper();

                      tmpData += bkData.PadRight(128, ' ') + '\r' + '\n';

                      douTotal += paytran.paytran_netpay_b;

                      index++;
                  }
                  //int record = list_paybank.Count;
                  //-- Total
                  //sequence = Convert.ToString(record + 2).ToString().PadLeft(6, '0') + "|";
                  //bkData = "T" + "|" + sequence + "|" + "002" + "|" + combank.combank_bankaccount;
                  //bkData = bkData.PadRight(40, '0') + "|";
                  //sequence = Convert.ToString(index + 2).ToString().PadLeft(6, '0');
                  //bkData = "T" + "|" + paybank.paybank_bankamount + "|" + paybank.paybank_bankaccount + "|";
                  //bkData = bkData + record.ToString().PadLeft('0') + "|" + combank.combank_id + "|";
                  //tmpData += bkData.PadRight('0') + "1" + "|";

                  //amount = douTotal.ToString("#.#0").Replace(".", "").PadLeft(13, '0');
                  //bkData = bkData + record.ToString().PadLeft('0') + "|";

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
        ////TRN_BANK
        // public string doExportSSF(string com, string taskid)
        //{
        //    string strResult = "";

        //    cls_ctMTTask objMTTask = new cls_ctMTTask();
        //    List<cls_MTTask> listMTTask = objMTTask.getDataByFillter(com, taskid, "TRN_SSF", "");
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

        //        StringBuilder objStr = new StringBuilder();
        //        foreach (cls_TRTaskwhose whose in listWhose)
        //        {
        //            objStr.Append("'" + whose.worker_code + "',");
        //        }

        //        string strEmp = objStr.ToString().Substring(0, objStr.ToString().Length - 1);



        //        //-- Get worker
        //        cls_ctMTWorker objWorker = new cls_ctMTWorker();
        //        List<cls_MTWorker> list_worker = objWorker.getDataMultipleEmp(com, strEmp);

        //        //-- Step 2 Get Paytran
        //        cls_ctTRPaytran objPay = new cls_ctTRPaytran();
        //        List<cls_TRPaytran> list_paytran = objPay.getDataMultipleEmp("TH", com, datePay, datePay, strEmp);



        //        //-- Step 3 Get Company acc
        //        cls_ctTRCombank objCombank = new cls_ctTRCombank();
        //        List<cls_TRCombank> list_combank = objCombank.getDataByFillter(com);
        //        cls_TRCombank combank = list_combank[0];

        //        //-- Step 4 Get Company detail
        //        cls_ctMTCompany objCom = new cls_ctMTCompany();
        //        List<cls_MTCompany> list_com = objCom.getDataByFillter("", com);
        //        cls_MTCompany comdetail = list_com[0];

        //        //-- Step 5 Get Emp address
        //        cls_ctTREmpaddress objEmpadd = new cls_ctTREmpaddress();
        //        List<cls_TREmpaddress> list_empaddress = objEmpadd.getDataMultipleEmp(com, strEmp);

        //        //-- Step 6 Get Emp card
        //        cls_ctTREmpcard objEmpcard = new cls_ctTREmpcard();
        //        List<cls_TREmpcard> list_empcard = objEmpcard.getDataTaxMultipleEmp(com, strEmp);

        //        //-- Step 7 Get Company card
        //        cls_ctTRComcard objComcard = new cls_ctTRComcard();
        //        List<cls_TRComcard> list_comcard = objComcard.getDataByFillter(com, "NTID", "", "", "");
        //        cls_TRComcard comcard = list_comcard[0];

        //        //-- Step 8 Get Payitem

        //        cls_ctTRPayitem objPayitem = new cls_ctTRPayitem();
        //        List<cls_TRPayitem> list_payitem = objPayitem.getDataByFillter("TH", com, strEmp, datePay, "SLF1", "");
        //        cls_TRPayitem payitem = list_payitem[0];
        //        if (list_payitem.Count > 0)
        //        {
        //            cls_TRPayitem Payitem = list_payitem[0];
        //        }

        //        //-- Step 9  Get Payitemm

        //        cls_ctTRPayitem objPayitemm = new cls_ctTRPayitem();
        //        List<cls_TRPayitem> list_payitemm = objPayitemm.getDataByFillter("TH", com, strEmp, datePay, "SLF2", "");
        //        cls_TRPayitem payitemm = list_payitemm[0];
        //        if (list_payitemm.Count > 0)
        //        {
        //            cls_TRPayitem Payitemm = list_payitemm[0];
        //        }
             

           
                
                



               

        //        if (list_comcard.Count > 0)
        //            comcard = list_comcard[0];

        //        cls_ctMTPeriod objPeriod = new cls_ctMTPeriod();
        //        List<cls_MTPeriod> list_period = objPeriod.getDataByFillter("", com, "PAY", datePay.Year.ToString(), "M");


        //        cls_ctTREmpdep objEmpdep = new cls_ctTREmpdep();
        //        List<cls_TREmpdep> list_empdep = objEmpdep.getDataTaxMultipleEmp("", "", datePay);




        //        cls_ctMTProvince objProvince = new cls_ctMTProvince();
        //        List<cls_MTProvince> list_province = objProvince.getDataByFillter("", "");

        //        cls_MTPeriod period = new cls_MTPeriod();
        //        //cls_TRPayitem payitem = new cls_TRPayitem();

        //        foreach (cls_MTPeriod tmp in list_period)
        //        {
        //            if (tmp.period_payment.Equals(datePay))
        //            {
        //                period = tmp;
        //                break;
        //            }
        //        }


        //        string tmpData = "";

        //        if (list_paytran.Count > 0)
        //        {

        //            double douTotal = 0;

        //            int index = 1;
        //            string bkData;

        //            foreach (cls_TRPaytran paytran in list_paytran)
        //            {

        //                string empname = "";

        //                cls_MTWorker obj_worker = new cls_MTWorker();
        //                cls_TREmpaddress obj_address = new cls_TREmpaddress();
        //                cls_MTProvince obj_province = new cls_MTProvince();
        //                cls_TREmpcard obj_card = new cls_TREmpcard();
        //                cls_TRPayitem obj_Payitem = new cls_TRPayitem();

        //                foreach (cls_MTWorker worker in list_worker)
        //                {
        //                    if (paytran.worker_code.Equals(worker.worker_code))
        //                    {
        //                        empname = worker.initial_name_en + " " + worker.worker_fname_en + " " + worker.worker_lname_en;
        //                        obj_worker = worker;
        //                        break;
        //                    }
        //                }

        //                foreach (cls_TREmpaddress address in list_empaddress)
        //                {
        //                    if (paytran.worker_code.Equals(address.worker_code))
        //                    {
        //                        obj_address = address;
        //                        break;
        //                    }
        //                }

        //                foreach (cls_TREmpcard card in list_empcard)
        //                {
        //                    if (paytran.worker_code.Equals(card.worker_code))
        //                    {
        //                        obj_card = card;
        //                        break;
        //                    }
        //                }

        //                foreach (cls_MTProvince province in list_province)
        //                {
        //                    if (obj_worker.worker_code.Equals(province.province_code))
        //                    {
        //                        obj_province = province;
        //                        break;
        //                    }
        //                }


        //                //foreach (cls_TRPayitem payitem in list_payitem)
        //                //{
        //                //    if (paytran.worker_code.Equals(payitem.item_code))
        //                //    {
        //                //        obj_Payitem = payitem;
        //                //        break;
        //                //    }
        //                //}



        //                if (empname.Equals("") || obj_card.empcard_code.Equals(""))
        //                    continue;


        //                if (paytran.paytran_income_401 > 0)
        //                {
        //                    //1.ลำดับที่
        //                    bkData = index++ + "|";

        //                    //2.เดือน / ปี
        //                    bkData += period.period_name_th + "/" + datePay.Year.ToString() + "|";

                            

        //                    //3.เลขประจำตัวประชาชน
        //                    if (comcard.comcard_code.Length == 13)
        //                        bkData += comcard.comcard_code + "|";
        //                    else
        //                        bkData += "0000000000000|";

        //                    //4.ชื่อ-นามสกุล
        //                    bkData += obj_worker.initial_name_th + " " + obj_worker.worker_fname_th + " " + obj_worker.worker_lname_th + "|";

        //                    //5.กยศ	

        //                    if (payitem.item_code.Length == 5)
        //                        bkData += payitem.payitem_amount.ToString("0000") + "|";
        //                    else
        //                        bkData += payitem.payitem_amount.ToString("0,00").Trim() + "|";


        //                    //6.กรอ
        //                    if (payitemm.item_code.Length == 5)
        //                        bkData += payitemm.payitem_amount.ToString("0000") + "|";
        //                    else
        //                        bkData += payitemm.payitem_amount.ToString("0,00").Trim() + "|";



        //                    //7.จำนวนเงิน

        //                    bkData += payitem.payitem_amount.ToString(".").Trim() + payitemm.payitem_amount.ToString(".").Trim() + "|"; ;
        //                    //bkData += payitemm.payitem_amount.ToString("0,00").Trim() + "|";
                          

                            

        //                    //8.ยอดยืนยันนำส่ง			
        //                    bkData += payitem.payitem_amount.ToString(".").Trim() + payitemm.payitem_amount.ToString(".").Trim() + "|"; ;
                       
                          


        //                    //9.วันที่หักเงินเดือน
        //                    bkData += datePay.ToString("dd/MM/yyyy") + "|";

        //                    //10.ไม่ได้นำส่งเงิน
        //                     bkData += "|";

        //                    //11.รหัสสาเหตุ
        //                    bkData += "|";

        //                    //12.ไฟล์แนบ
        //                    bkData += "|";


        //                    tmpData += bkData + '\r' + '\n';
        //                }





        //                douTotal += paytran.paytran_netpay_b;

        //                index++;
        //            }

        //            int record = list_paytran.Count;


        //            try
            //        {
            //            //-- Step 1 create file
            //            string filename = "TRN_SSF_" + DateTime.Now.ToString("yyMMddHHmm") + "." + "xls";
            //            string filepath = Path.Combine
            //           (ClassLibrary_BPC.Config.PathFileExport, filename);



            //            // Check if file already exists. If yes, delete it.     
            //            if (File.Exists(filepath))
            //            {
            //                File.Delete(filepath);
            //            }
            //            DataSet ds = new DataSet();
            //            string str = tmpData.Replace("\r\n", "]");
            //            string[] data = str.Split(']');
            //            DataTable dataTable = ds.Tables.Add();
            //            dataTable.Columns.AddRange(new DataColumn[12] { new DataColumn("ลำดับที่"), new DataColumn("เดือน / ปี"), new DataColumn("เลขประจำตัวประชาชน"), new DataColumn("ชื่อ-นามสกุล"), new DataColumn("กยศ."), new DataColumn("กรอ."), new DataColumn("จำนวนเงิน"), new DataColumn("ยอดยืนยันนำส่ง"), new DataColumn("วันที่หักเงินเดือน"), new DataColumn("ไม่ได้นำส่งเงิน"), new DataColumn("รหัสสาเหตุ"), new DataColumn("ไฟล์แนบ") });
            //            foreach (var i in data)
            //            {
            //                if (i.Equals(""))
            //                    continue;
            //                string[] array = i.Split('|');
            //                dataTable.Rows.Add(array[0], array[1], array[2], array[3], array[4], array[5], array[6], array[7], array[8], array[9], array[10], array[11]);
            //            }
            //            ExcelLibrary.DataSetHelper.CreateWorkbook(filepath, ds);

            //            // Create a new file     
            //            //using (FileStream fs = File.Create(filepath))
            //            //{
            //            //    // Add some text to file    
            //            //    Byte[] Table = new UTF8Encoding(true).GetBytes(tmpData);
            //            //    fs.Write(Table, 0, Table.Length);


            //            //}

            //            strResult = filename;

            //        }
            //    catch (Exception ex)
            //    {
            //        strResult = ex.ToString();
            //    }

            //    }


            //    task.task_end = DateTime.Now;
            //    task.task_status = "F";
            //    task.task_note = strResult;
            //    objMTTask.updateStatus(task);

            //}
            //else
        //    {

        //    }

        //    return strResult;
        //}
        //TRN_SSF

    }
}
 
 