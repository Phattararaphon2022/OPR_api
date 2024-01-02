using ClassLibrary_BPC.hrfocus.controller;
using ClassLibrary_BPC.hrfocus.controller.Attendance;
using ClassLibrary_BPC.hrfocus.model;
using ClassLibrary_BPC.hrfocus.model.Attendance;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary_BPC.hrfocus.service
{
    public class cls_srvProcessTime
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

        private cls_Timecompare doGetTimeInputIN(ref List<cls_TRTimeinput> listTimeinput, DateTime dateFrom, DateTime dateTo)
        {
            cls_Timecompare compareResult = new cls_Timecompare();

            for (int i = 0; i < listTimeinput.Count; i++)
            {
                cls_TRTimeinput timeinput = listTimeinput[i];

                if (timeinput.timeinput_compare.Equals("Y"))
                    continue;

                TimeSpan ts = TimeSpan.Parse(timeinput.timeinput_hhmm);
                DateTime dateinput = timeinput.timeinput_date;
                dateinput = dateinput.AddHours(ts.Hours).AddMinutes(ts.Minutes);

                if (dateinput >= dateFrom && dateinput <= dateTo)
                {
                    compareResult.date = dateinput;
                    compareResult.found = true;
                    listTimeinput[i].timeinput_compare = "Y";
                    break;
                }
            }

            return compareResult;
        }

        private cls_Timecompare doGetTimeInputOUT(ref List<cls_TRTimeinput> listTimeinput, DateTime dateFrom, DateTime dateTo)
        {
            cls_Timecompare compareResult = new cls_Timecompare();

            for (int i = listTimeinput.Count - 1; i >= 0; i--)
            {
                cls_TRTimeinput timeinput = listTimeinput[i];

                if (timeinput.timeinput_compare.Equals("Y"))
                    continue;

                TimeSpan ts = TimeSpan.Parse(timeinput.timeinput_hhmm);
                DateTime dateinput = timeinput.timeinput_date;
                dateinput = dateinput.AddHours(ts.Hours).AddMinutes(ts.Minutes);

                if (dateinput >= dateFrom && dateinput <= dateTo)
                {
                    compareResult.date = dateinput;
                    compareResult.found = true;
                    listTimeinput[i].timeinput_compare = "Y";
                    break;
                }
            }

            return compareResult;
        }
        
        public string doSummarizeTime_BK(string com, string taskid)
        {
            string strResult = "";

            cls_ctMTTask objMTTask = new cls_ctMTTask();
            List<cls_MTTask> listMTTask = objMTTask.getDataByFillter(com, taskid, "SUM_TIME", "");
            List<string> listError = new List<string>();

            if (listMTTask.Count > 0)
            {
                cls_MTTask task = listMTTask[0];

                task.task_start = DateTime.Now;

                cls_ctMTTask objTaskDetail = new cls_ctMTTask();
                cls_TRTaskdetail task_detail = objTaskDetail.getTaskDetail(task.task_id.ToString());

                cls_ctMTTask objTaskWhose = new cls_ctMTTask();
                List<cls_TRTaskwhose> listWhose = objTaskWhose.getTaskWhose(task.task_id.ToString());

                DateTime dateFrom = task_detail.taskdetail_fromdate;
                DateTime dateTo = task_detail.taskdetail_todate;

                //-- get shitf
                cls_ctMTShift objShift = new cls_ctMTShift();
                List<cls_MTShift> listShift = objShift.getDataByFillter(com, "", "",false);

                //-- get shitf
                cls_ctTRShiftbreak objBreak = new cls_ctTRShiftbreak();
                List<cls_TRShiftbreak> listBreak = objBreak.getDataByFillter(com, "");

                if (listShift.Count == 0)
                {
                    //-- Not set timecard
                    return "Not Found shift policy";
                }

                //-- Get worker
                cls_ctMTWorker objWorker = new cls_ctMTWorker();
                List<cls_MTWorker> listWorker = objWorker.getDataByCompanyCode(com);


                //-- Controller
                //cls_ctTRTimeleave objTimeleave = new cls_ctTRTimeleave();
                //cls_ctTRTimeshift objTimeshift = new cls_ctTRTimeshift();
                //cls_ctTRTimedaytype objTimedaytype = new cls_ctTRTimedaytype();
                //cls_ctTRTimeot objTimeot = new cls_ctTRTimeot();
                //cls_ctTRTimeonsite objTimeonsite = new cls_ctTRTimeonsite();

                //cls_ctTRPlanschedule objTimeschedule = new cls_ctTRPlanschedule();

                string[] process = task_detail.taskdetail_process.Split('|');


                bool fillauto = false;
                try
                {
                    if (process[1].Equals("AUTO"))
                        fillauto = true;
                }
                catch { }


                //-- Loop emp
                int intCountSuccess = 0;
                foreach (cls_TRTaskwhose whose in listWhose)
                {
                    cls_ctTRTimeinput objTimeinput = new cls_ctTRTimeinput();

                    //-- Get time card
                    cls_ctTRTimecard objTimecard = new cls_ctTRTimecard();
                    List<cls_TRTimecard> listTimecard = objTimecard.getDataByFillter(com, task.project_code, whose.worker_code, dateFrom, dateTo);

                    //-- Get doc request
                    //List<cls_TRTimeshift> listTimeshift = objTimeshift.getDataByFillter("EN", com, whose.worker_code, dateFrom, dateTo);
                    //List<cls_TRTimedaytype> listTimedaytype = objTimedaytype.getDataByFillter("EN", com, whose.worker_code, dateFrom, dateTo);
                    //List<cls_TRTimeleave> listTimeleave = objTimeleave.getDataByFillter("EN", com, whose.worker_code, dateFrom, dateTo);
                    //List<cls_TRTimeot> listTimeot = objTimeot.getDataByFillter("EN", com, whose.worker_code, dateFrom, dateTo);
                    //List<cls_TRTimeonsite> listTimeonsite = objTimeonsite.getDataByFillter("EN", com, whose.worker_code, dateFrom, dateTo);

                    //-- Flexible time
                    //cls_ctTRShiftflexible objShiftflexible = new cls_ctTRShiftflexible();
                    //List<cls_TRShiftflexible> listShiftflexible = objShiftflexible.getDataByWorker(com, whose.worker_code);
                    //--

                    cls_ctTRHoliday ctTRHoliday = new cls_ctTRHoliday();
                    List<cls_TRHoliday> listHoliday = ctTRHoliday.getDataByWorker(com, whose.worker_code);

                    //-- Get worker detail;
                    cls_MTWorker worker = null;
                    foreach (cls_MTWorker model in listWorker)
                    {
                        if (whose.worker_code.Equals(model.worker_code))
                        {
                            worker = model;
                            break;
                        }
                    }

                    if (worker == null)
                    {
                        //-- Not found worker
                        continue;
                    }

                    //-- Clear status compare time input
                    if (!fillauto)
                        objTimeinput.clear_compare(worker.worker_card, dateFrom, dateTo);

                    if (listTimecard.Count == 0)
                    {
                        //-- Not set timecard
                        continue;
                    }

                    //-- Loop date                    
                    for (DateTime date = dateFrom.Date; date.Date <= dateTo.Date; date = date.AddDays(1))
                    {
                        int hrs, min;

                        try
                        {


                            //--******************
                            //-- Step 1 Get timecard
                            //--******************
                            cls_TRTimecard timecard = null;
                            foreach (cls_TRTimecard mdTime in listTimecard)
                            {
                                if (mdTime.timecard_workdate.Date == date)
                                {
                                    timecard = mdTime;
                                    break;
                                }
                            }
                            if (timecard == null || timecard.timecard_lock)
                            {
                                continue;
                            }
                            else
                            {
                                timecard.modified_by = task.modified_by;
                                objTimecard.clearCH(timecard);
                            }


                            //-- Get daytype OLD
                            string daytype = "N";
                            //string daytype = timecard.timecard_daytype_plan;
                            //foreach (cls_TRHoliday holiday in listHoliday)
                            //{
                            //    if (holiday.holiday_date == date)
                            //    {
                            //        daytype = holiday.holiday_daytype;
                            //        break;
                            //    }
                            //}
                            timecard.timecard_daytype = daytype;


                            #region Request document
                            //-- Get request doc
                            //-- Change shift
                            //cls_TRTimeshift req_shift = null;
                            //foreach (cls_TRTimeshift model in listTimeshift)
                            //{
                            //    if (model.timeshift_workdate.Date == date)
                            //    {
                            //        req_shift = model;
                            //        break;
                            //    }
                            //}

                            //-- Change daytype
                            //cls_TRTimedaytype req_daytype = null;
                            //foreach (cls_TRTimedaytype model in listTimedaytype)
                            //{
                            //    if (model.timedaytype_workdate.Date == date)
                            //    {
                            //        req_daytype = model;
                            //        break;
                            //    }
                            //}

                            //-- Record time
                            //cls_TRTimeonsite req_onsite = null;
                            //foreach (cls_TRTimeonsite model in listTimeonsite)
                            //{
                            //    if (model.timeonsite_workdate.Date == date)
                            //    {
                            //        req_onsite = model;
                            //        break;
                            //    }
                            //}

                            //-- Request overtime
                            //cls_TRTimeot req_ot = null;
                            //foreach (cls_TRTimeot model in listTimeot)
                            //{
                            //    if (model.timeot_workdate.Date == date)
                            //    {
                            //        req_ot = model;
                            //        break;
                            //    }
                            //}

                            //-- Request leave                                 
                            int intLeaveMin = 0;
                            bool blnLeaveFullday = false;


                            int intLeaveMin_deduct = 0;

                            //foreach (cls_TRTimeleave model in listTimeleave)
                            //{
                            //    if (model.timeleave_fromdate.Date == date || model.timeleave_todate.Date == date)
                            //    {
                            //        if (model.timeleave_type.Equals("F"))
                            //        {
                            //            intLeaveMin = (int)worker.hrs_perday * 60;
                            //            blnLeaveFullday = true;

                            //            if (model.timeleave_deduct)
                            //                intLeaveMin_deduct = (int)worker.hrs_perday * 60;

                            //            break;
                            //        }
                            //        else
                            //        {
                            //            intLeaveMin += model.timeleave_min;

                            //            if (model.timeleave_deduct)
                            //                intLeaveMin_deduct += model.timeleave_min;
                            //        }
                            //    }
                            //    else
                            //    {
                            //        if (date < model.timeleave_todate.Date && date > model.timeleave_fromdate.Date)
                            //        {
                            //            if (model.timeleave_type.Equals("F"))
                            //            {
                            //                intLeaveMin = (int)worker.hrs_perday * 60;
                            //                blnLeaveFullday = true;

                            //                if (model.timeleave_deduct)
                            //                    intLeaveMin_deduct = (int)worker.hrs_perday * 60;

                            //                break;
                            //            }
                            //            else
                            //            {
                            //                intLeaveMin += model.timeleave_min;

                            //                if (model.timeleave_deduct)
                            //                    intLeaveMin_deduct += model.timeleave_min;
                            //            }
                            //        }
                            //    }
                            //}

                            #endregion

                            //--******************
                            //-- Step 2 Get timeinput       
                            //--******************
                            List<cls_TRTimeinput> listTimeinput = new List<cls_TRTimeinput>();

                            if (!fillauto)
                            {
                                listTimeinput = objTimeinput.getDataByFillter(com, whose.worker_code, date.AddDays(-1), date.AddDays(1), true);
                            }


                            //-- Request onsite
                            //if (req_onsite != null)
                            //{
                            //    if (!req_onsite.timeonsite_in.Equals("00:00"))
                            //    {
                            //        cls_TRTimeinput time = new cls_TRTimeinput();
                            //        time.timeinput_hhmm = req_onsite.timeonsite_in;
                            //        time.timeinput_compare = "N";
                            //        time.timeinput_function = "RECORD";
                            //        time.timeinput_date = date;

                            //        listTimeinput.Add(time);
                            //    }

                            //    if (!req_onsite.timeonsite_out.Equals("00:00"))
                            //    {
                            //        cls_TRTimeinput time = new cls_TRTimeinput();
                            //        time.timeinput_hhmm = req_onsite.timeonsite_out;
                            //        time.timeinput_compare = "N";
                            //        time.timeinput_function = "RECORD";
                            //        time.timeinput_date = date;

                            //        listTimeinput.Add(time);
                            //    }
                            //}

                            //--******************
                            //-- Step 3 get shift policy
                            //--******************
                            List<cls_Timechannel> listTimechannel = new List<cls_Timechannel>();
                            bool[] blnCH = new bool[11];
                            for (int i = 1; i <= 10; i++)
                            {
                                blnCH[i] = false;
                            }

                            //-- Request change shift
                            //if (req_shift != null)
                            //{
                            //    timecard.shift_code = req_shift.timeshift_new;
                            //}

                        FLEXIBLESHIFT:

                            cls_MTShift shift = null;
                            foreach (cls_MTShift mdShift in listShift)
                            {
                                if (mdShift.shift_code == timecard.shift_code)
                                {
                                    shift = mdShift;
                                    break;
                                }
                            }
                            if (shift == null)
                            {
                                continue;
                            }
                            else
                            {
                                #region Time channel
                                if (!shift.shift_ch1.Equals("00:00"))
                                {
                                    blnCH[1] = true;

                                    if (fillauto)
                                        this.addTimeManual(ref listTimeinput, date, shift.shift_ch1);
                                }

                                if (!shift.shift_ch3.Equals("00:00"))
                                {
                                    blnCH[3] = true;

                                    if (fillauto)
                                        this.addTimeManual(ref listTimeinput, date, shift.shift_ch3);
                                }

                                if (!shift.shift_ch4.Equals("00:00"))
                                {
                                    blnCH[4] = true;
                                    if (fillauto)
                                        this.addTimeManual(ref listTimeinput, date, shift.shift_ch4);
                                }

                                if (!shift.shift_ch9.Equals("00:00"))
                                {
                                    blnCH[9] = true;
                                    if (fillauto)
                                        this.addTimeManual(ref listTimeinput, date, shift.shift_ch9);
                                }

                                if (!shift.shift_ch10.Equals("00:00"))
                                {
                                    blnCH[10] = true;
                                    if (fillauto)
                                        this.addTimeManual(ref listTimeinput, date, shift.shift_ch10);
                                }

                                #endregion
                            }

                            //-- Break
                            cls_TRShiftbreak shift_break = null;
                            foreach (cls_TRShiftbreak mdBreak in listBreak)
                            {
                                if (mdBreak.shift_code == timecard.shift_code)
                                {
                                    shift_break = mdBreak;
                                    break;
                                }
                            }

                            int round = 1;

                            if (listTimeinput.Count == 0)
                                goto CALCULATE;

                            //-- Step 4 
                            #region Match time

                            if (blnCH[3] && blnCH[7])
                                round++;

                            TimeSpan ts;

                            //-- *********************** IN *************************

                            //-- OT IN
                            bool ot_in = false;
                            if (blnCH[1])
                            {

                                ts = TimeSpan.Parse(shift.shift_ch1);
                                DateTime dateShiftFrom = date.AddHours(ts.Hours).AddMinutes(ts.Minutes);

                                ts = TimeSpan.Parse(shift.shift_ch3);
                                DateTime dateShiftTo = date.AddHours(ts.Hours).AddMinutes(ts.Minutes);

                                if (dateShiftFrom.CompareTo(dateShiftTo) > 0)
                                    dateShiftFrom = dateShiftFrom.AddDays(-1);

                                cls_Timecompare compare = this.doGetTimeInputIN(ref listTimeinput, dateShiftFrom, dateShiftTo);

                                if (compare.found)
                                {
                                    timecard.timecard_ch1 = compare.date;
                                    timecard.timecard_ch2 = dateShiftTo;
                                    timecard.timecard_ch3 = dateShiftTo;

                                    timecard.timecard_ch1_scan = true;
                                    timecard.timecard_ch2_scan = true;
                                    timecard.timecard_ch3_scan = true;

                                    //-- F edit 18/05/2023
                                    //blnCH[3] = false;
                                    ot_in = true;
                                }
                            }

                            //-- Time normal IN
                            //-- F edit 18/05/2023
                            //if (blnCH[3])
                            if (!ot_in)
                            {
                                ts = TimeSpan.Parse(shift.shift_ch3);
                                DateTime dateShift = date.AddHours(ts.Hours).AddMinutes(ts.Minutes);

                                string[] ch3 = shift.shift_ch3.Split(':');
                                string[] ch3_from = shift.shift_ch3_from.Split(':');
                                string[] ch3_to = shift.shift_ch3_to.Split(':');

                                int min_ch3 = this.doConvertTime2Int(ch3[0]) * 60 + this.doConvertTime2Int(ch3[1]);
                                int min_ch3_from = this.doConvertTime2Int(ch3_from[0]) * 60 + this.doConvertTime2Int(ch3_from[1]);
                                int min_ch3_to = this.doConvertTime2Int(ch3_to[0]) * 60 + this.doConvertTime2Int(ch3_to[1]);

                                min_ch3_from = min_ch3 - min_ch3_from;
                                hrs = min_ch3_from / 60;
                                min = min_ch3_from % 60;

                                DateTime dateStart = dateShift.AddHours(-hrs).AddMinutes(-min);

                                min_ch3_to = min_ch3_to - min_ch3;
                                hrs = min_ch3_to / 60;
                                min = min_ch3_to % 60;

                                DateTime dateEnd = dateShift.AddHours(hrs).AddMinutes(min);

                                if (dateEnd.CompareTo(dateStart) < 0)
                                    dateEnd = dateEnd.AddDays(1);

                                cls_Timecompare compare = this.doGetTimeInputIN(ref listTimeinput, dateStart, dateEnd);

                                if (compare.found)
                                {
                                    timecard.timecard_ch3 = compare.date;
                                    timecard.timecard_ch3_scan = true;

                                    //-- F add 22/02/2022
                                    //-- Flexible time
                                    //string shift_new = objShiftflexible.getShiftFlexibel(com, whose.worker_code, compare.date.ToString("HH:mm"), "");
                                    //if (!shift_new.Equals(""))
                                    //{
                                    //    timecard.shift_code = shift_new;
                                    //    goto FLEXIBLESHIFT;
                                    //}
                                    //--


                                }
                            }

                            //-- *********************** OUT *************************

                            //-- OT OUT
                            if (blnCH[10])
                            {
                                ts = TimeSpan.Parse(shift.shift_ch9);
                                DateTime dateShiftFrom = date.AddHours(ts.Hours).AddMinutes(ts.Minutes);

                                ts = TimeSpan.Parse(shift.shift_ch10);
                                DateTime dateShiftTo = date.AddHours(ts.Hours).AddMinutes(ts.Minutes);

                                if (dateShiftTo.CompareTo(dateShiftFrom) < 0)
                                    dateShiftTo = dateShiftTo.AddDays(1);

                                cls_Timecompare compare = this.doGetTimeInputOUT(ref listTimeinput, dateShiftFrom, dateShiftTo);

                                if (compare.found)
                                {
                                    timecard.timecard_ch10 = compare.date;
                                    timecard.timecard_ch9 = dateShiftFrom;
                                    timecard.timecard_ch4 = dateShiftFrom;

                                    timecard.timecard_ch10_scan = true;
                                    timecard.timecard_ch9_scan = true;
                                    timecard.timecard_ch4_scan = true;
                                }
                            }

                            //-- OT IN
                            if (blnCH[9])
                            {
                                ts = TimeSpan.Parse(shift.shift_ch9);
                                DateTime dateShiftFrom = date.AddHours(ts.Hours).AddMinutes(ts.Minutes);

                                ts = TimeSpan.Parse(shift.shift_ch10);
                                DateTime dateShiftTo = date.AddHours(ts.Hours).AddMinutes(ts.Minutes);

                                if (dateShiftTo.CompareTo(dateShiftFrom) < 0)
                                    dateShiftTo = dateShiftTo.AddDays(1);

                                cls_Timecompare compare = this.doGetTimeInputIN(ref listTimeinput, dateShiftFrom, dateShiftTo);

                                if (compare.found)
                                {
                                    timecard.timecard_ch9 = compare.date;
                                    timecard.timecard_ch4 = dateShiftFrom;

                                    timecard.timecard_ch9_scan = true;
                                    timecard.timecard_ch4_scan = true;
                                }
                            }

                            //-- Normal OUT
                            if (blnCH[4])
                            {

                                ts = TimeSpan.Parse(shift.shift_ch4);
                                DateTime dateShift = date.AddHours(ts.Hours).AddMinutes(ts.Minutes);

                                string[] ch4 = shift.shift_ch4.Split(':');
                                string[] ch4_from = shift.shift_ch4_from.Split(':');
                                string[] ch4_to = shift.shift_ch4_to.Split(':');

                                int min_ch4 = this.doConvertTime2Int(ch4[0]) * 60 + this.doConvertTime2Int(ch4[1]);
                                int min_ch4_from = this.doConvertTime2Int(ch4_from[0]) * 60 + this.doConvertTime2Int(ch4_from[1]);
                                int min_ch4_to = this.doConvertTime2Int(ch4_to[0]) * 60 + this.doConvertTime2Int(ch4_to[1]);

                                min_ch4_from = min_ch4 - min_ch4_from;
                                hrs = min_ch4_from / 60;
                                min = min_ch4_from % 60;

                                DateTime dateStart = dateShift.AddHours(-hrs).AddMinutes(-min);

                                if (min_ch4_to < min_ch4)
                                {
                                    min_ch4_to = (24 * 60) - min_ch4 + min_ch4_to;
                                }
                                else
                                {
                                    min_ch4_to = min_ch4_to - min_ch4;
                                }


                                hrs = min_ch4_to / 60;
                                min = min_ch4_to % 60;


                                DateTime dateEnd = dateShift.AddHours(hrs).AddMinutes(min);

                                if (dateEnd.CompareTo(dateStart) < 0)
                                    dateEnd = dateEnd.AddDays(1);

                                cls_Timecompare compare = this.doGetTimeInputOUT(ref listTimeinput, dateStart, dateEnd);

                                if (compare.found)
                                {
                                    timecard.timecard_ch4 = compare.date;
                                    timecard.timecard_ch4_scan = true;
                                }
                            }
                            #endregion

                        CALCULATE:

                            //-- Step 5 
                            #region Calculate time
                            string strDaytype = timecard.timecard_daytype;

                            timecard.timecard_work1_min = 0;
                            timecard.timecard_work2_min = 0;
                            timecard.timecard_work1_min_app = 0;
                            timecard.timecard_work2_min_app = 0;
                            timecard.timecard_before_min = 0;
                            timecard.timecard_before_min_app = 0;
                            timecard.timecard_after_min = 0;
                            timecard.timecard_after_min_app = 0;


                            //-- Request change daytype
                            //if (req_daytype != null)
                            //{
                            //    strDaytype = req_daytype.timedaytype_new;
                            //}


                            #region Calculate Late

                            timecard.timecard_late_min = 0;


                            if (blnCH[3])
                            {
                                if (timecard.timecard_ch3 != null && timecard.timecard_ch3_scan)
                                {
                                    ts = TimeSpan.Parse(shift.shift_ch3);
                                    DateTime dateShift = date.AddHours(ts.Hours).AddMinutes(ts.Minutes);
                                    ts = timecard.timecard_ch3.Subtract(dateShift);

                                    int intLate = (ts.Hours * 60) + ts.Minutes;
                                    if (intLate < 0)
                                        intLate = 0;

                                    timecard.timecard_late_min = intLate;
                                }

                                if (timecard.timecard_ch4 != null && timecard.timecard_ch4_scan)
                                {
                                    ts = TimeSpan.Parse(shift.shift_ch4);
                                    DateTime dateShift = date.AddHours(ts.Hours).AddMinutes(ts.Minutes);
                                    ts = dateShift.Subtract(timecard.timecard_ch4);

                                    int intLate = (ts.Hours * 60) + ts.Minutes;
                                    if (intLate < 0)
                                        intLate = 0;

                                    timecard.timecard_late_min += intLate;
                                }

                                //-- F add 29/05/2022
                                if (!timecard.timecard_ch3_scan && !timecard.timecard_ch4_scan)
                                {
                                    ts = TimeSpan.Parse(shift.shift_ch3);
                                    DateTime dateLateStart = date.AddHours(ts.Hours).AddMinutes(ts.Minutes);
                                    ts = TimeSpan.Parse(shift.shift_ch4);
                                    DateTime dateLateEnd = date.AddHours(ts.Hours).AddMinutes(ts.Minutes);

                                    ts = dateLateEnd.Subtract(dateLateStart);

                                    timecard.timecard_late_min = (ts.Hours * 60) + ts.Minutes;
                                }
                            }

                            if (blnCH[7])
                            {
                                if (timecard.timecard_ch7 != null && timecard.timecard_ch7_scan)
                                {
                                    ts = TimeSpan.Parse(shift.shift_ch7);
                                    DateTime dateShift = date.AddHours(ts.Hours).AddMinutes(ts.Minutes);
                                    ts = timecard.timecard_ch7.Subtract(dateShift);

                                    int intLate = (ts.Hours * 60) + ts.Minutes;
                                    if (intLate < 0)
                                        intLate = 0;

                                    timecard.timecard_late_min += intLate;
                                }

                                if (timecard.timecard_ch8 != null && timecard.timecard_ch8_scan)
                                {
                                    ts = TimeSpan.Parse(shift.shift_ch8);
                                    DateTime dateShift = date.AddHours(ts.Hours).AddMinutes(ts.Minutes);
                                    ts = dateShift.Subtract(timecard.timecard_ch8);

                                    int intLate = (ts.Hours * 60) + ts.Minutes;
                                    if (intLate < 0)
                                        intLate = 0;

                                    timecard.timecard_late_min += intLate;

                                }
                            }
                            #endregion

                            double douWorkPerday = worker.hrs_perday * 60;

                            //-- Working
                            timecard.timecard_before_min = 0;
                            //-- OT IN
                            if (timecard.timecard_ch1_scan && timecard.timecard_ch2_scan)
                            {
                                ts = timecard.timecard_ch2.Subtract(timecard.timecard_ch1);
                                timecard.timecard_before_min = (ts.Hours * 60) + ts.Minutes;
                                timecard.timecard_before_min_app = timecard.timecard_before_min;
                            }

                            //-- Normal 1
                            if (timecard.timecard_ch3_scan && timecard.timecard_ch4_scan)
                            {
                                ts = timecard.timecard_ch4.Subtract(timecard.timecard_ch3);
                                timecard.timecard_work1_min = (ts.Hours * 60) + ts.Minutes;

                            }
                            else
                            {
                                if (strDaytype.Equals("O") || strDaytype.Equals("H") || strDaytype.Equals("C") || strDaytype.Equals("L"))
                                {

                                }
                                else
                                {
                                    strDaytype = "A";
                                }
                            }

                            //-- Normal 2
                            if (round > 1)
                            {
                                if (timecard.timecard_ch7_scan && timecard.timecard_ch8_scan)
                                {

                                    ts = timecard.timecard_ch8.Subtract(timecard.timecard_ch7);
                                    timecard.timecard_work2_min = (ts.Hours * 60) + ts.Minutes;

                                }
                                else
                                {
                                    if (strDaytype.Equals("O") || strDaytype.Equals("H") || strDaytype.Equals("C") || strDaytype.Equals("L"))
                                    {

                                    }
                                    else
                                    {
                                        strDaytype = "A";
                                    }
                                }
                            }



                            //-- OT OUT
                            if (timecard.timecard_ch9_scan && timecard.timecard_ch10_scan)
                            {
                                ts = timecard.timecard_ch10.Subtract(timecard.timecard_ch9);
                                timecard.timecard_after_min = (ts.Hours * 60) + ts.Minutes;
                                timecard.timecard_after_min_app = timecard.timecard_after_min;
                            }

                            #endregion

                            intCountSuccess++;



                            //-- Break
                            if (shift_break != null)
                            {
                                TimeSpan ts_break;
                                if (blnCH[3] && blnCH[4])
                                {
                                    ts_break = TimeSpan.Parse(shift_break.shiftbreak_from);
                                    DateTime startbreak = date.AddHours(ts_break.Hours).AddMinutes(ts_break.Minutes);

                                    ts_break = TimeSpan.Parse(shift_break.shiftbreak_to);
                                    DateTime endbreak = date.AddHours(ts_break.Hours).AddMinutes(ts_break.Minutes);

                                    if (timecard.timecard_ch3_scan)
                                    {
                                        if (startbreak < timecard.timecard_ch3)
                                            startbreak = timecard.timecard_ch3;
                                    }

                                    if (timecard.timecard_ch4_scan)
                                    {
                                        if (endbreak > timecard.timecard_ch4)
                                            endbreak = timecard.timecard_ch4;
                                    }


                                    ts = endbreak.Subtract(startbreak);
                                    int minbreak = (ts.Hours * 60) + ts.Minutes;

                                    timecard.timecard_work1_min -= minbreak;
                                    //if(timecard.timecard_late_min > 0)
                                    //    timecard.timecard_late_min -= minbreak;
                                }

                            }

                            //-- Summary

                            if (timecard.timecard_work1_min < 0)
                                timecard.timecard_work1_min = 0;

                            if (timecard.timecard_work2_min < 0)
                                timecard.timecard_work2_min = 0;

                            if (timecard.timecard_late_min < 0)
                                timecard.timecard_late_min = 0;


                            timecard.timecard_work1_min_app = timecard.timecard_work1_min + timecard.timecard_work2_min;
                            timecard.timecard_work2_min_app = 0;

                            if (timecard.timecard_work1_min > douWorkPerday)
                                timecard.timecard_work1_min = (int)douWorkPerday;

                            if (timecard.timecard_work1_min_app > douWorkPerday)
                                timecard.timecard_work1_min_app = (int)douWorkPerday;

                            if (intLeaveMin_deduct > douWorkPerday)
                                intLeaveMin_deduct = (int)douWorkPerday;

                            timecard.timecard_leavededuct_min = intLeaveMin_deduct;

                            if ((timecard.timecard_late_min + intLeaveMin) > (int)douWorkPerday)
                                timecard.timecard_late_min = (int)douWorkPerday - intLeaveMin;

                            timecard.timecard_late_min_app = timecard.timecard_late_min;


                            if (strDaytype.Equals("O") || strDaytype.Equals("H") || strDaytype.Equals("C"))
                            {
                                timecard.timecard_late_min = 0;
                                timecard.timecard_late_min_app = 0;
                            }
                            else
                            {
                                //-- Leave delete late
                                timecard.timecard_late_min = timecard.timecard_late_min - intLeaveMin;
                                timecard.timecard_late_min_app = timecard.timecard_late_min_app - intLeaveMin;

                                if (timecard.timecard_late_min < 0) timecard.timecard_late_min = 0;
                                if (timecard.timecard_late_min_app < 0) timecard.timecard_late_min_app = 0;

                                if (timecard.timecard_work1_min_app > 0 && !strDaytype.Equals("L"))
                                {
                                    strDaytype = "N";
                                }

                            }

                            //if (blnLeaveFullday)
                            //{
                            //    strDaytype = "L";
                            //}

                            //-- F add 18/05/2023
                            if (intLeaveMin > 0)
                                strDaytype = "L";


                            if (strDaytype.Equals("A"))
                            {
                                timecard.timecard_late_min = 0;
                                timecard.timecard_late_min_app = 0;
                            }

                            timecard.timecard_daytype = strDaytype;

                            //-- Overtime request
                            //if (req_ot == null)
                            //{
                            //    if (strDaytype.Equals("O") || strDaytype.Equals("H") || strDaytype.Equals("C"))
                            //    {
                            //        timecard.timecard_work1_min_app = 0;
                            //        timecard.timecard_work2_min_app = 0;
                            //    }

                            //    timecard.timecard_before_min_app = 0;
                            //    timecard.timecard_after_min_app = 0;
                            //    timecard.timecard_break_min_app = 0;

                            //}
                            //else
                            //{

                            //    if (strDaytype.Equals("O") || strDaytype.Equals("H") || strDaytype.Equals("C"))
                            //    {

                            //        if (timecard.timecard_work1_min_app > req_ot.timeot_normalmin)
                            //        {
                            //            timecard.timecard_work1_min_app = req_ot.timeot_normalmin;
                            //        }

                            //    }

                            //    //-- Before
                            //    if (timecard.timecard_before_min_app > req_ot.timeot_beforemin)
                            //    {
                            //        timecard.timecard_before_min_app = req_ot.timeot_beforemin;
                            //    }

                            //    //-- After
                            //    if (timecard.timecard_after_min_app > req_ot.timeot_aftermin)
                            //    {
                            //        timecard.timecard_after_min_app = req_ot.timeot_aftermin;
                            //    }

                            //    //-- Break
                            //    if (timecard.timecard_break_min_app > req_ot.timeot_break)
                            //    {
                            //        timecard.timecard_break_min_app = req_ot.timeot_break;
                            //    }

                            //}

                            //--******************
                            //-- Step 6 Record
                            //--******************
                            objTimecard.updateWithCH(timecard);

                            //--******************
                            //-- Step 7 Update time input
                            //--******************
                            List<cls_TRTimeinput> listTimeinput_update = new List<cls_TRTimeinput>();

                            foreach (cls_TRTimeinput model in listTimeinput)
                            {
                                if (model.timeinput_function.Equals("RECORD"))
                                    continue;

                                if (model.timeinput_compare.Equals("Y"))
                                    listTimeinput_update.Add(model);
                            }

                            if (listTimeinput_update.Count > 0)
                            {
                                objTimeinput.update_compare(listTimeinput_update);
                            }

                        }
                        catch (Exception ex)
                        {
                            listError.Add(whose.worker_code + "-" + date.ToString("dd/MM/yyyy") + ",");
                        }

                    }//-- End loop date

                }//-- End loop emp

                strResult = "Success::" + intCountSuccess.ToString();

                if (listError.Count > 0)
                {
                    strResult += "| Error::" + listError.ToString();
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

        public string doSummarizeTime(string com, string taskid)
        {
            string strResult = "";

            cls_ctMTTask objMTTask = new cls_ctMTTask();
            List<cls_MTTask> listMTTask = objMTTask.getDataByFillter(com, taskid, "SUM_TIME", "");
            List<string> listError = new List<string>();

            if (listMTTask.Count > 0)
            {
                cls_MTTask task = listMTTask[0];

                task.task_start = DateTime.Now;

                cls_ctMTTask objTaskDetail = new cls_ctMTTask();
                cls_TRTaskdetail task_detail = objTaskDetail.getTaskDetail(task.task_id.ToString());

                cls_ctMTTask objTaskWhose = new cls_ctMTTask();
                List<cls_TRTaskwhose> listWhose = objTaskWhose.getTaskWhose(task.task_id.ToString());

                DateTime dateFrom = task_detail.taskdetail_fromdate;
                DateTime dateTo = task_detail.taskdetail_todate;

                bool proshift = false;
                if (!task.project_code.ToString().Equals(""))
                {
                    proshift = true;
                }
                //-- get shitf
                cls_ctMTShift objShift = new cls_ctMTShift();
                List<cls_MTShift> listShift = objShift.getDataByFillter(com, "", "",proshift);

                //-- get shitf
                cls_ctTRShiftbreak objBreak = new cls_ctTRShiftbreak();
                List<cls_TRShiftbreak> listBreak = objBreak.getDataByFillter(com, "");

                if (listShift.Count == 0)
                {
                    //-- Not set timecard
                    return "Not Found shift policy";
                }

                //-- Get worker
                cls_ctMTWorker objWorker = new cls_ctMTWorker();
                List<cls_MTWorker> listWorker = objWorker.getDataByCompanyCode(com);


                //-- Controller
                cls_ctTRTimeleave objTimeleave = new cls_ctTRTimeleave();
                cls_ctTRTimeshift objTimeshift = new cls_ctTRTimeshift();
                cls_ctTRTimedaytype objTimedaytype = new cls_ctTRTimedaytype();
                cls_ctTRTimeot objTimeot = new cls_ctTRTimeot();
                cls_ctTRTimeonsite objTimeonsite = new cls_ctTRTimeonsite();

                //cls_ctTRPlanschedule objTimeschedule = new cls_ctTRPlanschedule();

                string[] process = task_detail.taskdetail_process.Split('|');


                bool fillauto = false;
                try
                {
                    if (process[1].Equals("AUTO"))
                        fillauto = true;
                }
                catch { }


                //-- Loop emp
                int intCountSuccess = 0;
                foreach (cls_TRTaskwhose whose in listWhose)
                {
                    cls_ctTRTimeinput objTimeinput = new cls_ctTRTimeinput();

                    //-- Genarate Time card from terminal
                    this.doGenarateTimecard(com, whose.worker_code, dateFrom, dateTo, task.modified_by);
                    //--


                    //-- Get time card
                    cls_ctTRTimecard objTimecard = new cls_ctTRTimecard();
                    

                    //-- Get doc request
                    List<cls_TRTimeshift> listTimeshift = objTimeshift.getDataByFillter(0, 3, com, whose.worker_code, dateFrom, dateTo);
                    List<cls_TRTimedaytype> listTimedaytype = objTimedaytype.getDataByFillter(com, 0, whose.worker_code, dateFrom.ToString("MM/dd/yyyy"), dateTo.ToString("MM/dd/yyyy"), 3);                    
                    List<cls_TRTimeot> listTimeot = objTimeot.getDataByFillter(0, 3, com, whose.worker_code, dateFrom, dateTo);
                    List<cls_TRTimeonsite> listTimeonsite = objTimeonsite.getDataByFillter(com, 0, "", whose.worker_code, dateFrom.ToString("MM/dd/yyyy"), dateTo.ToString("MM/dd/yyyy"), 3);    
                    List<cls_TRTimeleave> listTimeleave = objTimeleave.getDataByFillter(0, 3, com, whose.worker_code, dateFrom.ToString("MM/dd/yyyy"), dateTo.ToString("MM/dd/yyyy"));
                    


                    //-- Flexible time
                    //cls_ctTRShiftflexible objShiftflexible = new cls_ctTRShiftflexible();
                    //List<cls_TRShiftflexible> listShiftflexible = objShiftflexible.getDataByWorker(com, whose.worker_code);
                    //--

                    cls_ctTRHoliday ctTRHoliday = new cls_ctTRHoliday();
                    List<cls_TRHoliday> listHoliday = ctTRHoliday.getDataByWorker(com, whose.worker_code);

                    //-- Get worker detail;
                    cls_MTWorker worker = null;
                    foreach (cls_MTWorker model in listWorker)
                    {
                        if (whose.worker_code.Equals(model.worker_code))
                        {
                            worker = model;
                            break;
                        }
                    }

                    if (worker == null)
                    {
                        //-- Not found worker
                        continue;
                    }

                    //-- Clear status compare time input
                    if (!fillauto)
                        objTimeinput.clear_compare(worker.worker_card, dateFrom, dateTo);

                    

                    //-- Loop date                    
                    for (DateTime date = dateFrom.Date; date.Date <= dateTo.Date; date = date.AddDays(1))
                    {
                        int hrs, min;

                        try
                        {


                            //--******************
                            //-- Step 1 Get timecard
                            //--******************

                            List<cls_TRTimecard> listTimecard = objTimecard.getDataByFillter(com, "", whose.worker_code, date, date);

                            foreach (cls_TRTimecard timecard in listTimecard)
                            {                               

                                if (timecard.timecard_lock)
                                {
                                    continue;
                                }
                                else
                                {
                                    timecard.modified_by = task.modified_by;
                                    objTimecard.clearCH(timecard);
                                }

                                //-- Get daytype OLD
                                string daytype = "N";
                                //string daytype = timecard.timecard_daytype_plan;
                                //foreach (cls_TRHoliday holiday in listHoliday)
                                //{
                                //    if (holiday.holiday_date == date)
                                //    {
                                //        daytype = holiday.holiday_daytype;
                                //        break;
                                //    }
                                //}
                                timecard.timecard_daytype = daytype;


                                #region Request document
                                //-- Get request doc
                                //-- Change shift
                                cls_TRTimeshift req_shift = null;
                                foreach (cls_TRTimeshift model in listTimeshift)
                                {
                                    if (model.timeshift_workdate.Date == date)
                                    {
                                        req_shift = model;
                                        break;
                                    }
                                }

                                //-- Change daytype
                                cls_TRTimedaytype req_daytype = null;
                                foreach (cls_TRTimedaytype model in listTimedaytype)
                                {
                                    if (model.timedaytype_workdate.Date == date)
                                    {
                                        req_daytype = model;
                                        break;
                                    }
                                }

                                //-- Record time
                                cls_TRTimeonsite req_onsite = null;
                                foreach (cls_TRTimeonsite model in listTimeonsite)
                                {
                                    if (model.timeonsite_workdate.Date == date)
                                    {
                                        req_onsite = model;
                                        break;
                                    }
                                }

                                //-- Request overtime
                                cls_TRTimeot req_ot = null;
                                foreach (cls_TRTimeot model in listTimeot)
                                {
                                    if (model.timeot_workdate.Date == date)
                                    {
                                        req_ot = model;
                                        break;
                                    }
                                }

                                //-- Request leave                                 
                                int intLeaveMin = 0;
                                bool blnLeaveFullday = false;


                                int intLeaveMin_deduct = 0;

                                foreach (cls_TRTimeleave model in listTimeleave)
                                {
                                    if (model.timeleave_fromdate.Date == date || model.timeleave_todate.Date == date)
                                    {
                                        if (model.timeleave_type.Equals("F"))
                                        {
                                            intLeaveMin = (int)worker.hrs_perday * 60;
                                            blnLeaveFullday = true;

                                            if (model.timeleave_deduct)
                                                intLeaveMin_deduct = (int)worker.hrs_perday * 60;

                                            break;
                                        }
                                        else
                                        {
                                            intLeaveMin += model.timeleave_min;

                                            if (model.timeleave_deduct)
                                                intLeaveMin_deduct += model.timeleave_min;
                                        }
                                    }
                                    else
                                    {
                                        if (date < model.timeleave_todate.Date && date > model.timeleave_fromdate.Date)
                                        {
                                            if (model.timeleave_type.Equals("F"))
                                            {
                                                intLeaveMin = (int)worker.hrs_perday * 60;
                                                blnLeaveFullday = true;

                                                if (model.timeleave_deduct)
                                                    intLeaveMin_deduct = (int)worker.hrs_perday * 60;

                                                break;
                                            }
                                            else
                                            {
                                                intLeaveMin += model.timeleave_min;

                                                if (model.timeleave_deduct)
                                                    intLeaveMin_deduct += model.timeleave_min;
                                            }
                                        }
                                    }
                                }

                                #endregion

                                //--******************
                                //-- Step 2 Get timeinput       
                                //--******************
                                List<cls_TRTimeinput> listTimeinput = new List<cls_TRTimeinput>();

                                if (!fillauto)
                                {
                                    listTimeinput = objTimeinput.getDataByFillter(com, whose.worker_code, date.AddDays(-1), date.AddDays(1), true, timecard.project_code, timecard.projob_code);
                                }


                                //-- Request onsite
                                if (req_onsite != null)
                                {
                                    if (!req_onsite.timeonsite_in.Equals("00:00"))
                                    {
                                        cls_TRTimeinput time = new cls_TRTimeinput();
                                        time.timeinput_hhmm = req_onsite.timeonsite_in;
                                        time.timeinput_compare = "N";
                                        time.timeinput_function = "RECORD";
                                        time.timeinput_date = date;

                                        listTimeinput.Add(time);
                                    }

                                    if (!req_onsite.timeonsite_out.Equals("00:00"))
                                    {
                                        cls_TRTimeinput time = new cls_TRTimeinput();
                                        time.timeinput_hhmm = req_onsite.timeonsite_out;
                                        time.timeinput_compare = "N";
                                        time.timeinput_function = "RECORD";
                                        time.timeinput_date = date;

                                        listTimeinput.Add(time);
                                    }
                                }

                                //--******************
                                //-- Step 3 get shift policy
                                //--******************
                                List<cls_Timechannel> listTimechannel = new List<cls_Timechannel>();
                                bool[] blnCH = new bool[11];
                                for (int i = 1; i <= 10; i++)
                                {
                                    blnCH[i] = false;
                                }

                                //-- Request change shift
                                if (req_shift != null)
                                {
                                    timecard.shift_code = req_shift.timeshift_new;
                                }

                        FLEXIBLESHIFT:

                                cls_MTShift shift = null;
                                foreach (cls_MTShift mdShift in listShift)
                                {
                                    if (!timecard.project_code.Equals(""))
                                    {
                                        if (mdShift.project && mdShift.shift_code == timecard.shift_code)
                                        {
                                            shift = mdShift;
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        if (mdShift.shift_code == timecard.shift_code)
                                        {
                                            shift = mdShift;
                                            break;
                                        }
                                    }
                                }
                                if (shift == null)
                                {
                                    continue;
                                }
                                else
                                {
                                    #region Time channel
                                    if (!shift.shift_ch1.Equals("00:00"))
                                    {
                                        blnCH[1] = true;

                                        if (fillauto)
                                            this.addTimeManual(ref listTimeinput, date, shift.shift_ch1);
                                    }

                                    if (!shift.shift_ch3.Equals("00:00"))
                                    {
                                        blnCH[3] = true;

                                        if (fillauto)
                                            this.addTimeManual(ref listTimeinput, date, shift.shift_ch3);
                                    }

                                    if (!shift.shift_ch4.Equals("00:00"))
                                    {
                                        blnCH[4] = true;
                                        if (fillauto)
                                            this.addTimeManual(ref listTimeinput, date, shift.shift_ch4);
                                    }

                                    if (!shift.shift_ch9.Equals("00:00"))
                                    {
                                        blnCH[9] = true;
                                        if (fillauto)
                                            this.addTimeManual(ref listTimeinput, date, shift.shift_ch9);
                                    }

                                    if (!shift.shift_ch10.Equals("00:00"))
                                    {
                                        blnCH[10] = true;
                                        if (fillauto)
                                            this.addTimeManual(ref listTimeinput, date, shift.shift_ch10);
                                    }

                                    #endregion
                                }

                                //-- Break
                                cls_TRShiftbreak shift_break = null;
                                foreach (cls_TRShiftbreak mdBreak in listBreak)
                                {
                                    if (mdBreak.shift_code == timecard.shift_code)
                                    {
                                        shift_break = mdBreak;
                                        break;
                                    }
                                }

                                int round = 1;

                                if (listTimeinput.Count == 0)
                                    goto CALCULATE;

                                //-- Step 4 
                                #region Match time

                                if (blnCH[3] && blnCH[7])
                                    round++;

                                TimeSpan ts;

                                //-- *********************** IN *************************

                                //-- OT IN
                                bool ot_in = false;
                                if (blnCH[1])
                                {

                                    ts = TimeSpan.Parse(shift.shift_ch1);
                                    DateTime dateShiftFrom = date.AddHours(ts.Hours).AddMinutes(ts.Minutes);

                                    ts = TimeSpan.Parse(shift.shift_ch3);
                                    DateTime dateShiftTo = date.AddHours(ts.Hours).AddMinutes(ts.Minutes);

                                    if (dateShiftFrom.CompareTo(dateShiftTo) > 0)
                                        dateShiftFrom = dateShiftFrom.AddDays(-1);

                                    cls_Timecompare compare = this.doGetTimeInputIN(ref listTimeinput, dateShiftFrom, dateShiftTo);

                                    if (compare.found)
                                    {
                                        timecard.timecard_ch1 = compare.date;
                                        timecard.timecard_ch2 = dateShiftTo;
                                        timecard.timecard_ch3 = dateShiftTo;

                                        timecard.timecard_ch1_scan = true;
                                        timecard.timecard_ch2_scan = true;
                                        timecard.timecard_ch3_scan = true;

                                        //-- F edit 18/05/2023
                                        //blnCH[3] = false;
                                        ot_in = true;
                                    }
                                }

                                //-- Time normal IN
                                //-- F edit 18/05/2023
                                //if (blnCH[3])
                                if (!ot_in)
                                {
                                    ts = TimeSpan.Parse(shift.shift_ch3);
                                    DateTime dateShift = date.AddHours(ts.Hours).AddMinutes(ts.Minutes);

                                    string[] ch3 = shift.shift_ch3.Split(':');
                                    string[] ch3_from = shift.shift_ch3_from.Split(':');
                                    string[] ch3_to = shift.shift_ch3_to.Split(':');

                                    int min_ch3 = this.doConvertTime2Int(ch3[0]) * 60 + this.doConvertTime2Int(ch3[1]);
                                    int min_ch3_from = this.doConvertTime2Int(ch3_from[0]) * 60 + this.doConvertTime2Int(ch3_from[1]);
                                    int min_ch3_to = this.doConvertTime2Int(ch3_to[0]) * 60 + this.doConvertTime2Int(ch3_to[1]);

                                    min_ch3_from = min_ch3 - min_ch3_from;
                                    hrs = min_ch3_from / 60;
                                    min = min_ch3_from % 60;

                                    DateTime dateStart = dateShift.AddHours(-hrs).AddMinutes(-min);

                                    min_ch3_to = min_ch3_to - min_ch3;
                                    hrs = min_ch3_to / 60;
                                    min = min_ch3_to % 60;

                                    DateTime dateEnd = dateShift.AddHours(hrs).AddMinutes(min);

                                    if (dateEnd.CompareTo(dateStart) < 0)
                                        dateEnd = dateEnd.AddDays(1);

                                    cls_Timecompare compare = this.doGetTimeInputIN(ref listTimeinput, dateStart, dateEnd);

                                    if (compare.found)
                                    {
                                        timecard.timecard_ch3 = compare.date;
                                        timecard.timecard_ch3_scan = true;

                                        //-- F add 22/02/2022
                                        //-- Flexible time
                                        //string shift_new = objShiftflexible.getShiftFlexibel(com, whose.worker_code, compare.date.ToString("HH:mm"), "");
                                        //if (!shift_new.Equals(""))
                                        //{
                                        //    timecard.shift_code = shift_new;
                                        //    goto FLEXIBLESHIFT;
                                        //}
                                        //--


                                    }
                                }

                                //-- *********************** OUT *************************

                                //-- OT OUT
                                if (blnCH[10])
                                {
                                    ts = TimeSpan.Parse(shift.shift_ch9);
                                    DateTime dateShiftFrom = date.AddHours(ts.Hours).AddMinutes(ts.Minutes);

                                    ts = TimeSpan.Parse(shift.shift_ch10);
                                    DateTime dateShiftTo = date.AddHours(ts.Hours).AddMinutes(ts.Minutes);

                                    if (dateShiftTo.CompareTo(dateShiftFrom) < 0)
                                        dateShiftTo = dateShiftTo.AddDays(1);

                                    cls_Timecompare compare = this.doGetTimeInputOUT(ref listTimeinput, dateShiftFrom, dateShiftTo);

                                    if (compare.found)
                                    {
                                        timecard.timecard_ch10 = compare.date;
                                        timecard.timecard_ch9 = dateShiftFrom;
                                        timecard.timecard_ch4 = dateShiftFrom;

                                        timecard.timecard_ch10_scan = true;
                                        timecard.timecard_ch9_scan = true;
                                        timecard.timecard_ch4_scan = true;
                                    }
                                }

                                //-- OT IN
                                if (blnCH[9])
                                {
                                    ts = TimeSpan.Parse(shift.shift_ch9);
                                    DateTime dateShiftFrom = date.AddHours(ts.Hours).AddMinutes(ts.Minutes);

                                    ts = TimeSpan.Parse(shift.shift_ch10);
                                    DateTime dateShiftTo = date.AddHours(ts.Hours).AddMinutes(ts.Minutes);

                                    if (dateShiftTo.CompareTo(dateShiftFrom) < 0)
                                        dateShiftTo = dateShiftTo.AddDays(1);

                                    cls_Timecompare compare = this.doGetTimeInputIN(ref listTimeinput, dateShiftFrom, dateShiftTo);

                                    if (compare.found)
                                    {
                                        timecard.timecard_ch9 = compare.date;
                                        timecard.timecard_ch4 = dateShiftFrom;

                                        timecard.timecard_ch9_scan = true;
                                        timecard.timecard_ch4_scan = true;
                                    }
                                }

                                //-- Normal OUT
                                if (blnCH[4])
                                {

                                    ts = TimeSpan.Parse(shift.shift_ch4);
                                    DateTime dateShift = date.AddHours(ts.Hours).AddMinutes(ts.Minutes);

                                    string[] ch4 = shift.shift_ch4.Split(':');
                                    string[] ch4_from = shift.shift_ch4_from.Split(':');
                                    string[] ch4_to = shift.shift_ch4_to.Split(':');

                                    int min_ch4 = this.doConvertTime2Int(ch4[0]) * 60 + this.doConvertTime2Int(ch4[1]);
                                    int min_ch4_from = this.doConvertTime2Int(ch4_from[0]) * 60 + this.doConvertTime2Int(ch4_from[1]);
                                    int min_ch4_to = this.doConvertTime2Int(ch4_to[0]) * 60 + this.doConvertTime2Int(ch4_to[1]);

                                    min_ch4_from = min_ch4 - min_ch4_from;
                                    hrs = min_ch4_from / 60;
                                    min = min_ch4_from % 60;

                                    DateTime dateStart = dateShift.AddHours(-hrs).AddMinutes(-min);

                                    if (min_ch4_to < min_ch4)
                                    {
                                        min_ch4_to = (24 * 60) - min_ch4 + min_ch4_to;
                                    }
                                    else
                                    {
                                        min_ch4_to = min_ch4_to - min_ch4;
                                    }


                                    hrs = min_ch4_to / 60;
                                    min = min_ch4_to % 60;


                                    DateTime dateEnd = dateShift.AddHours(hrs).AddMinutes(min);

                                    if (dateEnd.CompareTo(dateStart) < 0)
                                        dateEnd = dateEnd.AddDays(1);

                                    cls_Timecompare compare = this.doGetTimeInputOUT(ref listTimeinput, dateStart, dateEnd);

                                    if (compare.found)
                                    {
                                        timecard.timecard_ch4 = compare.date;
                                        timecard.timecard_ch4_scan = true;
                                    }
                                }
                                #endregion

                            CALCULATE:

                                //-- Step 5 
                                #region Calculate time
                                string strDaytype = timecard.timecard_daytype;

                                timecard.timecard_work1_min = 0;
                                timecard.timecard_work2_min = 0;
                                timecard.timecard_work1_min_app = 0;
                                timecard.timecard_work2_min_app = 0;
                                timecard.timecard_before_min = 0;
                                timecard.timecard_before_min_app = 0;
                                timecard.timecard_after_min = 0;
                                timecard.timecard_after_min_app = 0;


                                //-- Request change daytype
                                if (req_daytype != null)
                                {
                                    strDaytype = req_daytype.timedaytype_new;
                                }


                                #region Calculate Late

                                timecard.timecard_late_min = 0;


                                if (blnCH[3])
                                {
                                    if (timecard.timecard_ch3 != null && timecard.timecard_ch3_scan)
                                    {
                                        ts = TimeSpan.Parse(shift.shift_ch3);
                                        DateTime dateShift = date.AddHours(ts.Hours).AddMinutes(ts.Minutes);
                                        ts = timecard.timecard_ch3.Subtract(dateShift);

                                        int intLate = (ts.Hours * 60) + ts.Minutes;
                                        if (intLate < 0)
                                            intLate = 0;

                                        timecard.timecard_late_min = intLate;
                                    }

                                    if (timecard.timecard_ch4 != null && timecard.timecard_ch4_scan)
                                    {
                                        ts = TimeSpan.Parse(shift.shift_ch4);
                                        DateTime dateShift = date.AddHours(ts.Hours).AddMinutes(ts.Minutes);
                                        ts = dateShift.Subtract(timecard.timecard_ch4);

                                        int intLate = (ts.Hours * 60) + ts.Minutes;
                                        if (intLate < 0)
                                            intLate = 0;

                                        timecard.timecard_late_min += intLate;
                                    }

                                    //-- F add 29/05/2022
                                    if (!timecard.timecard_ch3_scan && !timecard.timecard_ch4_scan)
                                    {
                                        ts = TimeSpan.Parse(shift.shift_ch3);
                                        DateTime dateLateStart = date.AddHours(ts.Hours).AddMinutes(ts.Minutes);
                                        ts = TimeSpan.Parse(shift.shift_ch4);
                                        DateTime dateLateEnd = date.AddHours(ts.Hours).AddMinutes(ts.Minutes);

                                        ts = dateLateEnd.Subtract(dateLateStart);

                                        timecard.timecard_late_min = (ts.Hours * 60) + ts.Minutes;
                                    }
                                }

                                if (blnCH[7])
                                {
                                    if (timecard.timecard_ch7 != null && timecard.timecard_ch7_scan)
                                    {
                                        ts = TimeSpan.Parse(shift.shift_ch7);
                                        DateTime dateShift = date.AddHours(ts.Hours).AddMinutes(ts.Minutes);
                                        ts = timecard.timecard_ch7.Subtract(dateShift);

                                        int intLate = (ts.Hours * 60) + ts.Minutes;
                                        if (intLate < 0)
                                            intLate = 0;

                                        timecard.timecard_late_min += intLate;
                                    }

                                    if (timecard.timecard_ch8 != null && timecard.timecard_ch8_scan)
                                    {
                                        ts = TimeSpan.Parse(shift.shift_ch8);
                                        DateTime dateShift = date.AddHours(ts.Hours).AddMinutes(ts.Minutes);
                                        ts = dateShift.Subtract(timecard.timecard_ch8);

                                        int intLate = (ts.Hours * 60) + ts.Minutes;
                                        if (intLate < 0)
                                            intLate = 0;

                                        timecard.timecard_late_min += intLate;

                                    }
                                }
                                #endregion

                                double douWorkPerday = worker.hrs_perday * 60;

                                //-- Working
                                timecard.timecard_before_min = 0;
                                //-- OT IN
                                if (timecard.timecard_ch1_scan && timecard.timecard_ch2_scan)
                                {
                                    ts = timecard.timecard_ch2.Subtract(timecard.timecard_ch1);
                                    timecard.timecard_before_min = (ts.Hours * 60) + ts.Minutes;
                                    timecard.timecard_before_min_app = timecard.timecard_before_min;
                                }

                                //-- Normal 1
                                if (timecard.timecard_ch3_scan && timecard.timecard_ch4_scan)
                                {
                                    ts = timecard.timecard_ch4.Subtract(timecard.timecard_ch3);
                                    timecard.timecard_work1_min = (ts.Hours * 60) + ts.Minutes;

                                }
                                else
                                {
                                    if (strDaytype.Equals("O") || strDaytype.Equals("H") || strDaytype.Equals("C") || strDaytype.Equals("L"))
                                    {

                                    }
                                    else
                                    {
                                        strDaytype = "A";
                                    }
                                }

                                //-- Normal 2
                                if (round > 1)
                                {
                                    if (timecard.timecard_ch7_scan && timecard.timecard_ch8_scan)
                                    {

                                        ts = timecard.timecard_ch8.Subtract(timecard.timecard_ch7);
                                        timecard.timecard_work2_min = (ts.Hours * 60) + ts.Minutes;

                                    }
                                    else
                                    {
                                        if (strDaytype.Equals("O") || strDaytype.Equals("H") || strDaytype.Equals("C") || strDaytype.Equals("L"))
                                        {

                                        }
                                        else
                                        {
                                            strDaytype = "A";
                                        }
                                    }
                                }



                                //-- OT OUT
                                if (timecard.timecard_ch9_scan && timecard.timecard_ch10_scan)
                                {
                                    ts = timecard.timecard_ch10.Subtract(timecard.timecard_ch9);
                                    timecard.timecard_after_min = (ts.Hours * 60) + ts.Minutes;
                                    timecard.timecard_after_min_app = timecard.timecard_after_min;
                                }

                                #endregion

                                intCountSuccess++;



                                //-- Break
                                if (shift_break != null)
                                {
                                    TimeSpan ts_break;
                                    if (blnCH[3] && blnCH[4])
                                    {
                                        ts_break = TimeSpan.Parse(shift_break.shiftbreak_from);
                                        DateTime startbreak = date.AddHours(ts_break.Hours).AddMinutes(ts_break.Minutes);

                                        ts_break = TimeSpan.Parse(shift_break.shiftbreak_to);
                                        DateTime endbreak = date.AddHours(ts_break.Hours).AddMinutes(ts_break.Minutes);

                                        if (timecard.timecard_ch3_scan)
                                        {
                                            if (startbreak < timecard.timecard_ch3)
                                                startbreak = timecard.timecard_ch3;
                                        }

                                        if (timecard.timecard_ch4_scan)
                                        {
                                            if (endbreak > timecard.timecard_ch4)
                                                endbreak = timecard.timecard_ch4;
                                        }


                                        ts = endbreak.Subtract(startbreak);
                                        int minbreak = (ts.Hours * 60) + ts.Minutes;

                                        timecard.timecard_work1_min -= minbreak;
                                        //if(timecard.timecard_late_min > 0)
                                        //    timecard.timecard_late_min -= minbreak;
                                    }

                                }

                                //-- Summary

                                if (timecard.timecard_work1_min < 0)
                                    timecard.timecard_work1_min = 0;

                                if (timecard.timecard_work2_min < 0)
                                    timecard.timecard_work2_min = 0;

                                if (timecard.timecard_late_min < 0)
                                    timecard.timecard_late_min = 0;


                                timecard.timecard_work1_min_app = timecard.timecard_work1_min + timecard.timecard_work2_min;
                                timecard.timecard_work2_min_app = 0;

                                if (timecard.timecard_work1_min > douWorkPerday)
                                    timecard.timecard_work1_min = (int)douWorkPerday;

                                if (timecard.timecard_work1_min_app > douWorkPerday)
                                    timecard.timecard_work1_min_app = (int)douWorkPerday;

                                if (intLeaveMin_deduct > douWorkPerday)
                                    intLeaveMin_deduct = (int)douWorkPerday;

                                timecard.timecard_leavededuct_min = intLeaveMin_deduct;

                                if ((timecard.timecard_late_min + intLeaveMin) > (int)douWorkPerday)
                                    timecard.timecard_late_min = (int)douWorkPerday - intLeaveMin;

                                timecard.timecard_late_min_app = timecard.timecard_late_min;


                                if (strDaytype.Equals("O") || strDaytype.Equals("H") || strDaytype.Equals("C"))
                                {
                                    timecard.timecard_late_min = 0;
                                    timecard.timecard_late_min_app = 0;
                                }
                                else
                                {
                                    //-- Leave delete late
                                    timecard.timecard_late_min = timecard.timecard_late_min - intLeaveMin;
                                    timecard.timecard_late_min_app = timecard.timecard_late_min_app - intLeaveMin;

                                    if (timecard.timecard_late_min < 0) timecard.timecard_late_min = 0;
                                    if (timecard.timecard_late_min_app < 0) timecard.timecard_late_min_app = 0;

                                    if (timecard.timecard_work1_min_app > 0 && !strDaytype.Equals("L"))
                                    {
                                        strDaytype = "N";
                                    }

                                }

                                //if (blnLeaveFullday)
                                //{
                                //    strDaytype = "L";
                                //}

                                //-- F add 18/05/2023
                                if (intLeaveMin > 0)
                                    strDaytype = "L";


                                if (strDaytype.Equals("A"))
                                {
                                    timecard.timecard_late_min = 0;
                                    timecard.timecard_late_min_app = 0;
                                }

                                timecard.timecard_daytype = strDaytype;

                                //-- Overtime request
                                if (req_ot == null)
                                {
                                    if (strDaytype.Equals("O") || strDaytype.Equals("H") || strDaytype.Equals("C"))
                                    {
                                        timecard.timecard_work1_min_app = 0;
                                        timecard.timecard_work2_min_app = 0;
                                    }

                                    timecard.timecard_before_min_app = 0;
                                    timecard.timecard_after_min_app = 0;
                                    timecard.timecard_break_min_app = 0;

                                }
                                else
                                {

                                    if (strDaytype.Equals("O") || strDaytype.Equals("H") || strDaytype.Equals("C"))
                                    {

                                        if (timecard.timecard_work1_min_app > req_ot.timeot_normalmin)
                                        {
                                            timecard.timecard_work1_min_app = req_ot.timeot_normalmin;
                                        }

                                    }

                                    //-- Before
                                    if (timecard.timecard_before_min_app > req_ot.timeot_beforemin)
                                    {
                                        timecard.timecard_before_min_app = req_ot.timeot_beforemin;
                                    }

                                    //-- After
                                    if (timecard.timecard_after_min_app > req_ot.timeot_aftermin)
                                    {
                                        timecard.timecard_after_min_app = req_ot.timeot_aftermin;
                                    }

                                    //-- Break
                                    if (timecard.timecard_break_min_app > req_ot.timeot_breakmin)
                                    {
                                        timecard.timecard_break_min_app = req_ot.timeot_breakmin;
                                    }

                                }

                                //--******************
                                //-- Step 6 Record
                                //--******************
                                objTimecard.updateWithCH(timecard);

                                //--******************
                                //-- Step 7 Update time input
                                //--******************
                                List<cls_TRTimeinput> listTimeinput_update = new List<cls_TRTimeinput>();

                                foreach (cls_TRTimeinput model in listTimeinput)
                                {
                                    if (model.timeinput_function.Equals("RECORD"))
                                        continue;

                                    if (model.timeinput_compare.Equals("Y"))
                                        listTimeinput_update.Add(model);
                                }

                                if (listTimeinput_update.Count > 0)
                                {
                                    objTimeinput.update_compare(listTimeinput_update);
                                }

                            }

                        }
                        catch (Exception ex)
                        {
                            listError.Add(whose.worker_code + "-" + date.ToString("dd/MM/yyyy") + ",");
                        }

                    }//-- End loop date

                }//-- End loop emp

                strResult = "Success::" + intCountSuccess.ToString();

                if (listError.Count > 0)
                {
                    strResult += "| Error::" + listError.ToString();
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


        ///
        public string doSummarizeTimelostwages(string com, string taskid)
        {
            string strResult = "";

            cls_ctMTTask objMTTask = new cls_ctMTTask();
            List<cls_MTTask> listMTTask = objMTTask.getDataByFillter(com, taskid, "SUM_TIME", "");
            List<string> listError = new List<string>();

            if (listMTTask.Count > 0)
            {
                cls_MTTask task = listMTTask[0];

                task.task_start = DateTime.Now;

                cls_ctMTTask objTaskDetail = new cls_ctMTTask();
                cls_TRTaskdetail task_detail = objTaskDetail.getTaskDetail(task.task_id.ToString());

                cls_ctMTTask objTaskWhose = new cls_ctMTTask();
                List<cls_TRTaskwhose> listWhose = objTaskWhose.getTaskWhose(task.task_id.ToString());

                DateTime dateFrom = task_detail.taskdetail_fromdate;
                DateTime dateTo = task_detail.taskdetail_todate;

                bool proshift = false;
                if (!task.project_code.ToString().Equals(""))
                {
                    proshift = true;
                }
                //-- get shitf
                cls_ctMTShift objShift = new cls_ctMTShift();
                List<cls_MTShift> listShift = objShift.getDataByFillter(com, "", "", proshift);

                //-- get shitf
                cls_ctTRShiftbreak objBreak = new cls_ctTRShiftbreak();
                List<cls_TRShiftbreak> listBreak = objBreak.getDataByFillter(com, "");

                if (listShift.Count == 0)
                {
                    //-- Not set timecard
                    return "Not Found shift policy";
                }

                //-- Get worker
                cls_ctMTWorker objWorker = new cls_ctMTWorker();
                List<cls_MTWorker> listWorker = objWorker.getDataByCompanyCode(com);


                //-- Controller
                cls_ctTRTimeleave objTimeleave = new cls_ctTRTimeleave();
                cls_ctTRTimeshift objTimeshift = new cls_ctTRTimeshift();
                cls_ctTRTimedaytype objTimedaytype = new cls_ctTRTimedaytype();
                cls_ctTRTimeot objTimeot = new cls_ctTRTimeot();
                cls_ctTRTimeonsite objTimeonsite = new cls_ctTRTimeonsite();

                //cls_ctTRPlanschedule objTimeschedule = new cls_ctTRPlanschedule();

                string[] process = task_detail.taskdetail_process.Split('|');


                bool fillauto = false;
                try
                {
                    if (process[1].Equals("AUTO"))
                        fillauto = true;
                }
                catch { }


                //-- Loop emp
                int intCountSuccess = 0;
                foreach (cls_TRTaskwhose whose in listWhose)
                {
                    cls_ctTRTimeinput objTimeinput = new cls_ctTRTimeinput();

                    //-- Genarate Time card from terminal
                    this.doGenarateLostwages(com, whose.worker_code, dateFrom, dateTo, task.modified_by);
                    //--


                    //-- Get time card
                    cls_ctTRLostwages objLostwages = new cls_ctTRLostwages();


                    //-- Get doc request
                    List<cls_TRTimeshift> listTimeshift = objTimeshift.getDataByFillter(0, 3, com, whose.worker_code, dateFrom, dateTo);
                    List<cls_TRTimedaytype> listTimedaytype = objTimedaytype.getDataByFillter(com, 0, whose.worker_code, dateFrom.ToString("MM/dd/yyyy"), dateTo.ToString("MM/dd/yyyy"), 3);
                    List<cls_TRTimeot> listTimeot = objTimeot.getDataByFillter(0, 3, com, whose.worker_code, dateFrom, dateTo);
                    List<cls_TRTimeonsite> listTimeonsite = objTimeonsite.getDataByFillter(com, 0, "", whose.worker_code, dateFrom.ToString("MM/dd/yyyy"), dateTo.ToString("MM/dd/yyyy"), 3);
                    List<cls_TRTimeleave> listTimeleave = objTimeleave.getDataByFillter(0, 3, com, whose.worker_code, dateFrom.ToString("MM/dd/yyyy"), dateTo.ToString("MM/dd/yyyy"));



                    //-- Flexible time
                    //cls_ctTRShiftflexible objShiftflexible = new cls_ctTRShiftflexible();
                    //List<cls_TRShiftflexible> listShiftflexible = objShiftflexible.getDataByWorker(com, whose.worker_code);
                    //--

                    cls_ctTRHoliday ctTRHoliday = new cls_ctTRHoliday();
                    List<cls_TRHoliday> listHoliday = ctTRHoliday.getDataByWorker(com, whose.worker_code);

                    //-- Get worker detail;
                    cls_MTWorker worker = null;
                    foreach (cls_MTWorker model in listWorker)
                    {
                        if (whose.worker_code.Equals(model.worker_code))
                        {
                            worker = model;
                            break;
                        }
                    }

                    if (worker == null)
                    {
                        //-- Not found worker
                        continue;
                    }

                    //-- Clear status compare time input
                    if (!fillauto)
                        objTimeinput.clear_compare(worker.worker_card, dateFrom, dateTo);



                    //-- Loop date                    
                    for (DateTime date = dateFrom.Date; date.Date <= dateTo.Date; date = date.AddDays(1))
                    {
                        int hrs, min;

                        try
                        {


                            //--******************
                            //-- Step 1 Get timecard
                            //--******************

                            List<cls_TRLostwages> listLostwages = objLostwages.getDataByFillter(com, "", whose.worker_code, "", date, date); ///กลับมาเช็ค

                            foreach (cls_TRLostwages Lostwages in listLostwages)
                            {

                                if (Lostwages.lostwages_lock)
                                {
                                    continue;
                                }
                                else
                                {
                                    Lostwages.modified_by = task.modified_by;
                                    objLostwages.clearCH(Lostwages);
                                }

                                //-- Get daytype OLD
                                string daytype = "N";

                                Lostwages.lostwages_daytype = daytype;


                                #region Request document
                                //-- Get request doc
                                //-- Change shift
                                cls_TRTimeshift req_shift = null;
                                foreach (cls_TRTimeshift model in listTimeshift)
                                {
                                    if (model.timeshift_workdate.Date == date)
                                    {
                                        req_shift = model;
                                        break;
                                    }
                                }

                                //-- Change daytype
                                cls_TRTimedaytype req_daytype = null;
                                foreach (cls_TRTimedaytype model in listTimedaytype)
                                {
                                    if (model.timedaytype_workdate.Date == date)
                                    {
                                        req_daytype = model;
                                        break;
                                    }
                                }

                                //-- Record time
                                cls_TRTimeonsite req_onsite = null;
                                foreach (cls_TRTimeonsite model in listTimeonsite)
                                {
                                    if (model.timeonsite_workdate.Date == date)
                                    {
                                        req_onsite = model;
                                        break;
                                    }
                                }

                                //-- Request overtime
                                cls_TRTimeot req_ot = null;
                                foreach (cls_TRTimeot model in listTimeot)
                                {
                                    if (model.timeot_workdate.Date == date)
                                    {
                                        req_ot = model;
                                        break;
                                    }
                                }

                                //-- Request leave                                 
                                int intLeaveMin = 0;
                                bool blnLeaveFullday = false;


                                int intLeaveMin_deduct = 0;

                                foreach (cls_TRTimeleave model in listTimeleave)
                                {
                                    if (model.timeleave_fromdate.Date == date || model.timeleave_todate.Date == date)
                                    {
                                        if (model.timeleave_type.Equals("F"))
                                        {
                                            intLeaveMin = (int)worker.hrs_perday * 60;
                                            blnLeaveFullday = true;

                                            if (model.timeleave_deduct)
                                                intLeaveMin_deduct = (int)worker.hrs_perday * 60;

                                            break;
                                        }
                                        else
                                        {
                                            intLeaveMin += model.timeleave_min;

                                            if (model.timeleave_deduct)
                                                intLeaveMin_deduct += model.timeleave_min;
                                        }
                                    }
                                    else
                                    {
                                        if (date < model.timeleave_todate.Date && date > model.timeleave_fromdate.Date)
                                        {
                                            if (model.timeleave_type.Equals("F"))
                                            {
                                                intLeaveMin = (int)worker.hrs_perday * 60;
                                                blnLeaveFullday = true;

                                                if (model.timeleave_deduct)
                                                    intLeaveMin_deduct = (int)worker.hrs_perday * 60;

                                                break;
                                            }
                                            else
                                            {
                                                intLeaveMin += model.timeleave_min;

                                                if (model.timeleave_deduct)
                                                    intLeaveMin_deduct += model.timeleave_min;
                                            }
                                        }
                                    }
                                }

                                #endregion

                                //--******************
                                //-- Step 2 Get timeinput       
                                //--******************
                                List<cls_TRTimeinput> listTimeinput = new List<cls_TRTimeinput>();

                                if (!fillauto)
                                {
                                    listTimeinput = objTimeinput.getDataByFillter(com, whose.worker_code, date.AddDays(-1), date.AddDays(1), true, Lostwages.project_code, Lostwages.projob_code);
                                }


                                //-- Request onsite
                                if (req_onsite != null)
                                {
                                    if (!req_onsite.timeonsite_in.Equals("00:00"))
                                    {
                                        cls_TRTimeinput time = new cls_TRTimeinput();
                                        time.timeinput_hhmm = req_onsite.timeonsite_in;
                                        time.timeinput_compare = "N";
                                        time.timeinput_function = "RECORD";
                                        time.timeinput_date = date;

                                        listTimeinput.Add(time);
                                    }

                                    if (!req_onsite.timeonsite_out.Equals("00:00"))
                                    {
                                        cls_TRTimeinput time = new cls_TRTimeinput();
                                        time.timeinput_hhmm = req_onsite.timeonsite_out;
                                        time.timeinput_compare = "N";
                                        time.timeinput_function = "RECORD";
                                        time.timeinput_date = date;

                                        listTimeinput.Add(time);
                                    }
                                }

                                //--******************
                                //-- Step 3 get shift policy
                                //--******************
                                List<cls_Timechannel> listTimechannel = new List<cls_Timechannel>();
                                bool[] blnCH = new bool[11];
                                for (int i = 1; i <= 10; i++)
                                {
                                    blnCH[i] = false;
                                }

                                //-- Request change shift
                                if (req_shift != null)
                                {
                                    Lostwages.shift_code = req_shift.timeshift_new;
                                }

                            FLEXIBLESHIFT:

                                cls_MTShift shift = null;
                                foreach (cls_MTShift mdShift in listShift)
                                {
                                    if (!Lostwages.project_code.Equals(""))
                                    {
                                        if (mdShift.project && mdShift.shift_code == Lostwages.shift_code)
                                        {
                                            shift = mdShift;
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        if (mdShift.shift_code == Lostwages.shift_code)
                                        {
                                            shift = mdShift;
                                            break;
                                        }
                                    }
                                }
                                if (shift == null)
                                {
                                    continue;
                                }
                                else
                                {
                                    #region Time channel
                                    if (!shift.shift_ch1.Equals("00:00"))
                                    {
                                        blnCH[1] = true;

                                        if (fillauto)
                                            this.addTimeManual(ref listTimeinput, date, shift.shift_ch1);
                                    }

                                    if (!shift.shift_ch3.Equals("00:00"))
                                    {
                                        blnCH[3] = true;

                                        if (fillauto)
                                            this.addTimeManual(ref listTimeinput, date, shift.shift_ch3);
                                    }

                                    if (!shift.shift_ch4.Equals("00:00"))
                                    {
                                        blnCH[4] = true;
                                        if (fillauto)
                                            this.addTimeManual(ref listTimeinput, date, shift.shift_ch4);
                                    }

                                    if (!shift.shift_ch9.Equals("00:00"))
                                    {
                                        blnCH[9] = true;
                                        if (fillauto)
                                            this.addTimeManual(ref listTimeinput, date, shift.shift_ch9);
                                    }

                                    if (!shift.shift_ch10.Equals("00:00"))
                                    {
                                        blnCH[10] = true;
                                        if (fillauto)
                                            this.addTimeManual(ref listTimeinput, date, shift.shift_ch10);
                                    }

                                    #endregion
                                }

                                //-- Break
                                cls_TRShiftbreak shift_break = null;
                                foreach (cls_TRShiftbreak mdBreak in listBreak)
                                {
                                    if (mdBreak.shift_code == Lostwages.shift_code)
                                    {
                                        shift_break = mdBreak;
                                        break;
                                    }
                                }

                                int round = 1;

                                if (listTimeinput.Count == 0)
                                    goto CALCULATE;

                                //-- Step 4 
                                #region Match time

                                if (blnCH[3] && blnCH[7])
                                    round++;

                                TimeSpan ts;

                                //-- *********************** IN *************************

                                //-- OT IN
                                bool ot_in = false;
                                if (blnCH[1])
                                {

                                    ts = TimeSpan.Parse(shift.shift_ch1);
                                    DateTime dateShiftFrom = date.AddHours(ts.Hours).AddMinutes(ts.Minutes);

                                    ts = TimeSpan.Parse(shift.shift_ch3);
                                    DateTime dateShiftTo = date.AddHours(ts.Hours).AddMinutes(ts.Minutes);

                                    if (dateShiftFrom.CompareTo(dateShiftTo) > 0)
                                        dateShiftFrom = dateShiftFrom.AddDays(-1);

                                    cls_Timecompare compare = this.doGetTimeInputIN(ref listTimeinput, dateShiftFrom, dateShiftTo);

                                    if (compare.found)
                                    {
                                        Lostwages.lostwages_ch1 = compare.date;
                                        Lostwages.lostwages_ch2 = dateShiftTo;
                                        Lostwages.lostwages_ch3 = dateShiftTo;

                                        Lostwages.lostwages_ch1_scan = true;
                                        Lostwages.lostwages_ch2_scan = true;
                                        Lostwages.lostwages_ch3_scan = true;

                                        //-- F edit 18/05/2023
                                        //blnCH[3] = false;
                                        ot_in = true;
                                    }
                                }

                                //-- Time normal IN
                                //-- F edit 18/05/2023
                                //if (blnCH[3])
                                if (!ot_in)
                                {
                                    ts = TimeSpan.Parse(shift.shift_ch3);
                                    DateTime dateShift = date.AddHours(ts.Hours).AddMinutes(ts.Minutes);

                                    string[] ch3 = shift.shift_ch3.Split(':');
                                    string[] ch3_from = shift.shift_ch3_from.Split(':');
                                    string[] ch3_to = shift.shift_ch3_to.Split(':');

                                    int min_ch3 = this.doConvertTime2Int(ch3[0]) * 60 + this.doConvertTime2Int(ch3[1]);
                                    int min_ch3_from = this.doConvertTime2Int(ch3_from[0]) * 60 + this.doConvertTime2Int(ch3_from[1]);
                                    int min_ch3_to = this.doConvertTime2Int(ch3_to[0]) * 60 + this.doConvertTime2Int(ch3_to[1]);

                                    min_ch3_from = min_ch3 - min_ch3_from;
                                    hrs = min_ch3_from / 60;
                                    min = min_ch3_from % 60;

                                    DateTime dateStart = dateShift.AddHours(-hrs).AddMinutes(-min);

                                    min_ch3_to = min_ch3_to - min_ch3;
                                    hrs = min_ch3_to / 60;
                                    min = min_ch3_to % 60;

                                    DateTime dateEnd = dateShift.AddHours(hrs).AddMinutes(min);

                                    if (dateEnd.CompareTo(dateStart) < 0)
                                        dateEnd = dateEnd.AddDays(1);

                                    cls_Timecompare compare = this.doGetTimeInputIN(ref listTimeinput, dateStart, dateEnd);

                                    if (compare.found)
                                    {
                                        Lostwages.lostwages_ch3 = compare.date;
                                        Lostwages.lostwages_ch3_scan = true;
 
                                    }
                                }

                                //-- *********************** OUT *************************

                                //-- OT OUT
                                if (blnCH[10])
                                {
                                    ts = TimeSpan.Parse(shift.shift_ch9);
                                    DateTime dateShiftFrom = date.AddHours(ts.Hours).AddMinutes(ts.Minutes);

                                    ts = TimeSpan.Parse(shift.shift_ch10);
                                    DateTime dateShiftTo = date.AddHours(ts.Hours).AddMinutes(ts.Minutes);

                                    if (dateShiftTo.CompareTo(dateShiftFrom) < 0)
                                        dateShiftTo = dateShiftTo.AddDays(1);

                                    cls_Timecompare compare = this.doGetTimeInputOUT(ref listTimeinput, dateShiftFrom, dateShiftTo);

                                    if (compare.found)
                                    {
                                        Lostwages.lostwages_ch10 = compare.date;
                                        Lostwages.lostwages_ch9 = dateShiftFrom;
                                        Lostwages.lostwages_ch4 = dateShiftFrom;

                                        Lostwages.lostwages_ch10_scan = true;
                                        Lostwages.lostwages_ch9_scan = true;
                                        Lostwages.lostwages_ch4_scan = true;
                                    }
                                }

                                //-- OT IN
                                if (blnCH[9])
                                {
                                    ts = TimeSpan.Parse(shift.shift_ch9);
                                    DateTime dateShiftFrom = date.AddHours(ts.Hours).AddMinutes(ts.Minutes);

                                    ts = TimeSpan.Parse(shift.shift_ch10);
                                    DateTime dateShiftTo = date.AddHours(ts.Hours).AddMinutes(ts.Minutes);

                                    if (dateShiftTo.CompareTo(dateShiftFrom) < 0)
                                        dateShiftTo = dateShiftTo.AddDays(1);

                                    cls_Timecompare compare = this.doGetTimeInputIN(ref listTimeinput, dateShiftFrom, dateShiftTo);

                                    if (compare.found)
                                    {
                                        Lostwages.lostwages_ch9 = compare.date;
                                        Lostwages.lostwages_ch4 = dateShiftFrom;

                                        Lostwages.lostwages_ch9_scan = true;
                                        Lostwages.lostwages_ch4_scan = true;
                                    }
                                }

                                //-- Normal OUT
                                if (blnCH[4])
                                {

                                    ts = TimeSpan.Parse(shift.shift_ch4);
                                    DateTime dateShift = date.AddHours(ts.Hours).AddMinutes(ts.Minutes);

                                    string[] ch4 = shift.shift_ch4.Split(':');
                                    string[] ch4_from = shift.shift_ch4_from.Split(':');
                                    string[] ch4_to = shift.shift_ch4_to.Split(':');

                                    int min_ch4 = this.doConvertTime2Int(ch4[0]) * 60 + this.doConvertTime2Int(ch4[1]);
                                    int min_ch4_from = this.doConvertTime2Int(ch4_from[0]) * 60 + this.doConvertTime2Int(ch4_from[1]);
                                    int min_ch4_to = this.doConvertTime2Int(ch4_to[0]) * 60 + this.doConvertTime2Int(ch4_to[1]);

                                    min_ch4_from = min_ch4 - min_ch4_from;
                                    hrs = min_ch4_from / 60;
                                    min = min_ch4_from % 60;

                                    DateTime dateStart = dateShift.AddHours(-hrs).AddMinutes(-min);

                                    if (min_ch4_to < min_ch4)
                                    {
                                        min_ch4_to = (24 * 60) - min_ch4 + min_ch4_to;
                                    }
                                    else
                                    {
                                        min_ch4_to = min_ch4_to - min_ch4;
                                    }


                                    hrs = min_ch4_to / 60;
                                    min = min_ch4_to % 60;


                                    DateTime dateEnd = dateShift.AddHours(hrs).AddMinutes(min);

                                    if (dateEnd.CompareTo(dateStart) < 0)
                                        dateEnd = dateEnd.AddDays(1);

                                    cls_Timecompare compare = this.doGetTimeInputOUT(ref listTimeinput, dateStart, dateEnd);

                                    if (compare.found)
                                    {
                                        Lostwages.lostwages_ch4 = compare.date;
                                        Lostwages.lostwages_ch4_scan = true;
                                    }
                                }
                                #endregion

                            CALCULATE:

                                //-- Step 5 
                                #region Calculate time
                                string strDaytype = Lostwages.lostwages_daytype;

                            Lostwages.lostwages_work1_min = 0;
                            Lostwages.lostwages_work2_min = 0;
                            Lostwages.lostwages_work1_min_app = 0;
                            Lostwages.lostwages_work2_min_app = 0;
                            Lostwages.lostwages_before_min = 0;
                            Lostwages.lostwages_before_min_app = 0;
                            Lostwages.lostwages_after_min = 0;
                            Lostwages.lostwages_after_min_app = 0;


                                //-- Request change daytype
                                if (req_daytype != null)
                                {
                                    strDaytype = req_daytype.timedaytype_new;
                                }


                                #region Calculate Late

                                Lostwages.lostwages_late_min = 0;


                                if (blnCH[3])
                                {
                                    if (Lostwages.lostwages_ch3 != null && Lostwages.lostwages_ch3_scan)
                                    {
                                        ts = TimeSpan.Parse(shift.shift_ch3);
                                        DateTime dateShift = date.AddHours(ts.Hours).AddMinutes(ts.Minutes);
                                        ts = Lostwages.lostwages_ch3.Subtract(dateShift);

                                        int intLate = (ts.Hours * 60) + ts.Minutes;
                                        if (intLate < 0)
                                            intLate = 0;

                                        Lostwages.lostwages_late_min = intLate;
                                    }

                                    if (Lostwages.lostwages_ch4 != null && Lostwages.lostwages_ch4_scan)
                                    {
                                        ts = TimeSpan.Parse(shift.shift_ch4);
                                        DateTime dateShift = date.AddHours(ts.Hours).AddMinutes(ts.Minutes);
                                        ts = dateShift.Subtract(Lostwages.lostwages_ch4);

                                        int intLate = (ts.Hours * 60) + ts.Minutes;
                                        if (intLate < 0)
                                            intLate = 0;

                                        Lostwages.lostwages_late_min += intLate;
                                    }

                                    //-- F add 29/05/2022
                                    if (!Lostwages.lostwages_ch3_scan && !Lostwages.lostwages_ch4_scan)
                                    {
                                        ts = TimeSpan.Parse(shift.shift_ch3);
                                        DateTime dateLateStart = date.AddHours(ts.Hours).AddMinutes(ts.Minutes);
                                        ts = TimeSpan.Parse(shift.shift_ch4);
                                        DateTime dateLateEnd = date.AddHours(ts.Hours).AddMinutes(ts.Minutes);

                                        ts = dateLateEnd.Subtract(dateLateStart);

                                        Lostwages.lostwages_late_min = (ts.Hours * 60) + ts.Minutes;
                                    }
                                }

                                if (blnCH[7])
                                {
                                    if (Lostwages.lostwages_ch7 != null && Lostwages.lostwages_ch7_scan)
                                    {
                                        ts = TimeSpan.Parse(shift.shift_ch7);
                                        DateTime dateShift = date.AddHours(ts.Hours).AddMinutes(ts.Minutes);
                                        ts = Lostwages.lostwages_ch7.Subtract(dateShift);

                                        int intLate = (ts.Hours * 60) + ts.Minutes;
                                        if (intLate < 0)
                                            intLate = 0;

                                        Lostwages.lostwages_late_min += intLate;
                                    }

                                    if (Lostwages.lostwages_ch8 != null && Lostwages.lostwages_ch8_scan)
                                    {
                                        ts = TimeSpan.Parse(shift.shift_ch8);
                                        DateTime dateShift = date.AddHours(ts.Hours).AddMinutes(ts.Minutes);
                                        ts = dateShift.Subtract(Lostwages.lostwages_ch8);

                                        int intLate = (ts.Hours * 60) + ts.Minutes;
                                        if (intLate < 0)
                                            intLate = 0;

                                        Lostwages.lostwages_late_min += intLate;

                                    }
                                }
                                #endregion

                                double douWorkPerday = worker.hrs_perday * 60;

                                //-- Working
                                Lostwages.lostwages_before_min = 0;
                                //-- OT IN
                                if (Lostwages.lostwages_ch1_scan && Lostwages.lostwages_ch2_scan)
                                {
                                    ts = Lostwages.lostwages_ch2.Subtract(Lostwages.lostwages_ch1);
                                    Lostwages.lostwages_before_min = (ts.Hours * 60) + ts.Minutes;
                                    Lostwages.lostwages_before_min_app = Lostwages.lostwages_before_min;
                                }

                                //-- Normal 1
                                if (Lostwages.lostwages_ch3_scan && Lostwages.lostwages_ch4_scan)
                                {
                                    ts = Lostwages.lostwages_ch4.Subtract(Lostwages.lostwages_ch3);
                                    Lostwages.lostwages_work1_min = (ts.Hours * 60) + ts.Minutes;

                                }
                                else
                                {
                                    if (strDaytype.Equals("O") || strDaytype.Equals("H") || strDaytype.Equals("C") || strDaytype.Equals("L"))
                                    {

                                    }
                                    else
                                    {
                                        strDaytype = "A";
                                    }
                                }

                                //-- Normal 2
                                if (round > 1)
                                {
                                    if (Lostwages.lostwages_ch7_scan && Lostwages.lostwages_ch8_scan)
                                    {

                                        ts = Lostwages.lostwages_ch8.Subtract(Lostwages.lostwages_ch7);
                                        Lostwages.lostwages_work2_min = (ts.Hours * 60) + ts.Minutes;

                                    }
                                    else
                                    {
                                        if (strDaytype.Equals("O") || strDaytype.Equals("H") || strDaytype.Equals("C") || strDaytype.Equals("L"))
                                        {

                                        }
                                        else
                                        {
                                            strDaytype = "A";
                                        }
                                    }
                                }



                                //-- OT OUT
                                if (Lostwages.lostwages_ch9_scan && Lostwages.lostwages_ch10_scan)
                                {
                                    ts = Lostwages.lostwages_ch10.Subtract(Lostwages.lostwages_ch9);
                                    Lostwages.lostwages_after_min = (ts.Hours * 60) + ts.Minutes;
                                    Lostwages.lostwages_after_min_app = Lostwages.lostwages_after_min;
                                }

                                #endregion

                                intCountSuccess++;



                                //-- Break
                                if (shift_break != null)
                                {
                                    TimeSpan ts_break;
                                    if (blnCH[3] && blnCH[4])
                                    {
                                        ts_break = TimeSpan.Parse(shift_break.shiftbreak_from);
                                        DateTime startbreak = date.AddHours(ts_break.Hours).AddMinutes(ts_break.Minutes);

                                        ts_break = TimeSpan.Parse(shift_break.shiftbreak_to);
                                        DateTime endbreak = date.AddHours(ts_break.Hours).AddMinutes(ts_break.Minutes);

                                        if (Lostwages.lostwages_ch3_scan)
                                        {
                                            if (startbreak < Lostwages.lostwages_ch3)
                                                startbreak = Lostwages.lostwages_ch3;
                                        }

                                        if (Lostwages.lostwages_ch4_scan)
                                        {
                                            if (endbreak > Lostwages.lostwages_ch4)
                                                endbreak = Lostwages.lostwages_ch4;
                                        }


                                        ts = endbreak.Subtract(startbreak);
                                        int minbreak = (ts.Hours * 60) + ts.Minutes;

                                        Lostwages.lostwages_work1_min -= minbreak;
                                        
                                    }

                                }

                                //-- Summary

                                if (Lostwages.lostwages_work1_min < 0)
                                    Lostwages.lostwages_work1_min = 0;

                                if (Lostwages.lostwages_work2_min < 0)
                                    Lostwages.lostwages_work2_min = 0;

                                if (Lostwages.lostwages_late_min < 0)
                                    Lostwages.lostwages_late_min = 0;


                                Lostwages.lostwages_work1_min_app = Lostwages.lostwages_work1_min + Lostwages.lostwages_work2_min;
                                Lostwages.lostwages_work2_min_app = 0;

                                if (Lostwages.lostwages_work1_min > douWorkPerday)
                                    Lostwages.lostwages_work1_min = (int)douWorkPerday;

                                if (Lostwages.lostwages_work1_min_app > douWorkPerday)
                                    Lostwages.lostwages_work1_min_app = (int)douWorkPerday;

                                if (intLeaveMin_deduct > douWorkPerday)
                                    intLeaveMin_deduct = (int)douWorkPerday;

                                Lostwages.lostwages_leavededuct_min = intLeaveMin_deduct;

                                if ((Lostwages.lostwages_late_min + intLeaveMin) > (int)douWorkPerday)
                                    Lostwages.lostwages_late_min = (int)douWorkPerday - intLeaveMin;

                                Lostwages.lostwages_late_min_app = Lostwages.lostwages_late_min;


                                if (strDaytype.Equals("O") || strDaytype.Equals("H") || strDaytype.Equals("C"))
                                {
                                    Lostwages.lostwages_late_min = 0;
                                    Lostwages.lostwages_late_min_app = 0;
                                }
                                else
                                {
                                    //-- Leave delete late
                                    Lostwages.lostwages_late_min = Lostwages.lostwages_late_min - intLeaveMin;
                                    Lostwages.lostwages_late_min_app = Lostwages.lostwages_late_min_app - intLeaveMin;

                                    if (Lostwages.lostwages_late_min < 0) Lostwages.lostwages_late_min = 0;
                                    if (Lostwages.lostwages_late_min_app < 0) Lostwages.lostwages_late_min_app = 0;

                                    if (Lostwages.lostwages_work1_min_app > 0 && !strDaytype.Equals("L"))
                                    {
                                        strDaytype = "N";
                                    }

                                }

                                

                                //-- F add 18/05/2023
                                if (intLeaveMin > 0)
                                    strDaytype = "L";


                                if (strDaytype.Equals("A"))
                                {
                                    Lostwages.lostwages_late_min = 0;
                                    Lostwages.lostwages_late_min_app = 0;
                                }

                                Lostwages.lostwages_daytype = strDaytype;

                                //-- Overtime request
                                if (req_ot == null)
                                {
                                    if (strDaytype.Equals("O") || strDaytype.Equals("H") || strDaytype.Equals("C"))
                                    {
                                        Lostwages.lostwages_work1_min_app = 0;
                                        Lostwages.lostwages_work2_min_app = 0;
                                    }

                                    Lostwages.lostwages_before_min_app = 0;
                                    Lostwages.lostwages_after_min_app = 0;
                                    Lostwages.lostwages_break_min_app = 0;

                                }
                                else
                                {

                                    if (strDaytype.Equals("O") || strDaytype.Equals("H") || strDaytype.Equals("C"))
                                    {

                                        if (Lostwages.lostwages_work1_min_app > req_ot.timeot_normalmin)
                                        {
                                            Lostwages.lostwages_work1_min_app = req_ot.timeot_normalmin;
                                        }

                                    }

                                    //-- Before
                                    if (Lostwages.lostwages_before_min_app > req_ot.timeot_beforemin)
                                    {
                                        Lostwages.lostwages_before_min_app = req_ot.timeot_beforemin;
                                    }

                                    //-- After
                                    if (Lostwages.lostwages_after_min_app > req_ot.timeot_aftermin)
                                    {
                                        Lostwages.lostwages_after_min_app = req_ot.timeot_aftermin;
                                    }

                                    //-- Break
                                    if (Lostwages.lostwages_break_min_app > req_ot.timeot_breakmin)
                                    {
                                        Lostwages.lostwages_break_min_app = req_ot.timeot_breakmin;
                                    }

                                }

                                //--******************
                                //-- Step 6 Record
                                //--******************
                                objLostwages.updateWithCH(Lostwages);

                                //--******************
                                //-- Step 7 Update time input
                                //--******************
                                List<cls_TRTimeinput> listTimeinput_update = new List<cls_TRTimeinput>();

                                foreach (cls_TRTimeinput model in listTimeinput)
                                {
                                    if (model.timeinput_function.Equals("RECORD"))
                                        continue;

                                    if (model.timeinput_compare.Equals("Y"))
                                        listTimeinput_update.Add(model);
                                }

                                if (listTimeinput_update.Count > 0)
                                {
                                    objTimeinput.update_compare(listTimeinput_update);
                                }

                            }

                        }
                        catch (Exception ex)
                        {
                            listError.Add(whose.worker_code + "-" + date.ToString("dd/MM/yyyy") + ",");
                        }

                    }//-- End loop date

                }//-- End loop emp

                strResult = "Success::" + intCountSuccess.ToString();

                if (listError.Count > 0)
                {
                    strResult += "| Error::" + listError.ToString();
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

        //
        public string doSummarizeTimelostwagesno(string com, string taskid)
        {
            string strResult = "";

            cls_ctMTTask objMTTask = new cls_ctMTTask();
            List<cls_MTTask> listMTTask = objMTTask.getDataByFillter(com, taskid, "SUM_TIME", "");
            List<string> listError = new List<string>();

            if (listMTTask.Count > 0)
            {
                cls_MTTask task = listMTTask[0];

                task.task_start = DateTime.Now;

                cls_ctMTTask objTaskDetail = new cls_ctMTTask();
                cls_TRTaskdetail task_detail = objTaskDetail.getTaskDetail(task.task_id.ToString());

                cls_ctMTTask objTaskWhose = new cls_ctMTTask();
                List<cls_TRTaskwhose> listWhose = objTaskWhose.getTaskWhose(task.task_id.ToString());

                DateTime dateFrom = task_detail.taskdetail_fromdate;
                DateTime dateTo = task_detail.taskdetail_todate;

                bool proshift = false;
                if (!task.project_code.ToString().Equals(""))
                {
                    proshift = true;
                }
                //-- get shitf
                cls_ctMTShift objShift = new cls_ctMTShift();
                List<cls_MTShift> listShift = objShift.getDataByFillter(com, "", "", proshift);

                //-- get shitf
                cls_ctTRShiftbreak objBreak = new cls_ctTRShiftbreak();
                List<cls_TRShiftbreak> listBreak = objBreak.getDataByFillter(com, "");

                if (listShift.Count == 0)
                {
                    //-- Not set timecard
                    return "Not Found shift policy";
                }

                //-- Get worker
                cls_ctMTWorker objWorker = new cls_ctMTWorker();
                List<cls_MTWorker> listWorker = objWorker.getDataByCompanyCode(com);
                 //-- Get worker
                cls_ctTRLostwages objLostwages = new cls_ctTRLostwages();
                List<cls_TRLostwages> listLostwages = objLostwages.getDataByCompanyCode(com);

                
                //-- Controller
                cls_ctTRTimeleave objTimeleave = new cls_ctTRTimeleave();
                cls_ctTRTimeshift objTimeshift = new cls_ctTRTimeshift();
                cls_ctTRTimedaytype objTimedaytype = new cls_ctTRTimedaytype();
                cls_ctTRTimeot objTimeot = new cls_ctTRTimeot();
                cls_ctTRTimeonsite objTimeonsite = new cls_ctTRTimeonsite();

                //cls_ctTRPlanschedule objTimeschedule = new cls_ctTRPlanschedule();

                string[] process = task_detail.taskdetail_process.Split('|');


                bool fillauto = false;
                try
                {
                    if (process[1].Equals("AUTO"))
                        fillauto = true;
                }
                catch { }


                //-- Loop emp
                int intCountSuccess = 0;
                foreach (cls_TRLostwages whose in listLostwages)
                {
                    cls_ctTRTimeinput objTimeinput = new cls_ctTRTimeinput();

                    //-- Genarate Time card from terminal
                    this.doGenarateLostwages2(com, whose.lostwages_cardno, dateFrom, dateTo, task.modified_by);
                    //--


                    //-- Get time card
                    cls_ctTRLostwages objLostwages1 = new cls_ctTRLostwages();
                    cls_TRLostwages objLostwagess1 = new cls_TRLostwages();


                    //-- Get doc request
                    List<cls_TRTimeshift> listTimeshift = objTimeshift.getDataByFillterLostwages(0, 3, com, whose.lostwages_cardno, dateFrom, dateTo);
                    List<cls_TRTimedaytype> listTimedaytype = objTimedaytype.getDataByFillterLostwages(com, 0, whose.lostwages_cardno, dateFrom.ToString("MM/dd/yyyy"), dateTo.ToString("MM/dd/yyyy"), 3);
                    List<cls_TRTimeot> listTimeot = objTimeot.getDataByFillterLostwages(0, 3, com, whose.lostwages_cardno, dateFrom, dateTo);
                    List<cls_TRTimeonsite> listTimeonsite = objTimeonsite.getDataByFillterLostwages(com, 0, "", whose.lostwages_cardno, dateFrom.ToString("MM/dd/yyyy"), dateTo.ToString("MM/dd/yyyy"), 3);
                    List<cls_TRTimeleave> listTimeleave = objTimeleave.getDataByFillterLostwages(0, 3, com, whose.lostwages_cardno, dateFrom.ToString("MM/dd/yyyy"), dateTo.ToString("MM/dd/yyyy"));



                    //-- Flexible time
                    //cls_ctTRShiftflexible objShiftflexible = new cls_ctTRShiftflexible();
                    //List<cls_TRShiftflexible> listShiftflexible = objShiftflexible.getDataByWorker(com, whose.worker_code);
                    //--

                    cls_ctTRHoliday ctTRHoliday = new cls_ctTRHoliday();
                    List<cls_TRHoliday> listHoliday = ctTRHoliday.getDataByWorker(com, whose.lostwages_cardno);

                    //-- Get worker detail;
                    cls_MTWorker worker = null;
                    cls_TRLostwages Lostwages1 = null;

                    foreach (cls_MTWorker model in listWorker)
                    {
                        if (whose.worker_code.Equals(model.worker_code))
                        {
                            worker = model;
                            break;
                        }
                    }

                    if (worker == null)
                    {
                        //cls_TRLostwages Lostwages1 = null;
                        foreach (cls_TRLostwages model in listLostwages)
                        {
                            if (model.lostwages_cardno.Equals(model.lostwages_cardno))
                            {
                                Lostwages1 = model;
                                break;
                            }
                        }
                    }

                    //-- Clear status compare time input
                    if (!fillauto)
                        objTimeinput.clear_compare(Lostwages1.lostwages_cardno, dateFrom, dateTo);



                    //-- Loop date                    
                    for (DateTime date = dateFrom.Date; date.Date <= dateTo.Date; date = date.AddDays(1))
                    {
                        int hrs, min;

                        try
                        {


                            //--******************
                            //-- Step 1 Get timecard
                            //--******************

                            List<cls_TRLostwages> listLostwages1 = objLostwages.getDataByFillter(com, "", "", Lostwages1.lostwages_cardno,   date, date); ///กลับมาเช็ค

                            foreach (cls_TRLostwages Lostwages in listLostwages1)
                            {

                                if (Lostwages.lostwages_lock)
                                {
                                    continue;
                                }
                                else
                                {
                                    Lostwages.modified_by = task.modified_by;
                                    objLostwages.clearCH(Lostwages);
                                }

                                //-- Get daytype OLD
                                string daytype = "N";

                                Lostwages.lostwages_daytype = daytype;


                                #region Request document
                                //-- Get request doc
                                //-- Change shift
                                cls_TRTimeshift req_shift = null;
                                foreach (cls_TRTimeshift model in listTimeshift)
                                {
                                    if (model.timeshift_workdate.Date == date)
                                    {
                                        req_shift = model;
                                        break;
                                    }
                                }

                                //-- Change daytype
                                cls_TRTimedaytype req_daytype = null;
                                foreach (cls_TRTimedaytype model in listTimedaytype)
                                {
                                    if (model.timedaytype_workdate.Date == date)
                                    {
                                        req_daytype = model;
                                        break;
                                    }
                                }

                                //-- Record time
                                cls_TRTimeonsite req_onsite = null;
                                foreach (cls_TRTimeonsite model in listTimeonsite)
                                {
                                    if (model.timeonsite_workdate.Date == date)
                                    {
                                        req_onsite = model;
                                        break;
                                    }
                                }

                                //-- Request overtime
                                cls_TRTimeot req_ot = null;
                                foreach (cls_TRTimeot model in listTimeot)
                                {
                                    if (model.timeot_workdate.Date == date)
                                    {
                                        req_ot = model;
                                        break;
                                    }
                                }

                                //-- Request leave                                 
                                int intLeaveMin = 0;
                                bool blnLeaveFullday = false;


                                int intLeaveMin_deduct = 0;

 

                                foreach (cls_TRTimeleave model in listTimeleave)
                                {
                                    if (model.timeleave_fromdate.Date == date || model.timeleave_todate.Date == date)
                                    {
                                        if (model.timeleave_type.Equals("F"))
                                        {
                                            intLeaveMin = (int)8 * 60;
                                            blnLeaveFullday = true;

                                            if (model.timeleave_deduct)
                                                intLeaveMin_deduct = (int)8 * 60;

                                            break;
                                        }
                                        else
                                        {
                                            intLeaveMin += model.timeleave_min;

                                            if (model.timeleave_deduct)
                                                intLeaveMin_deduct += model.timeleave_min;
                                        }
                                    }
                                    else
                                    {
                                        if (date < model.timeleave_todate.Date && date > model.timeleave_fromdate.Date)
                                        {
                                            if (model.timeleave_type.Equals("F"))
                                            {
                                                intLeaveMin = (int)8 * 60;
                                                blnLeaveFullday = true;

                                                if (model.timeleave_deduct)
                                                    intLeaveMin_deduct = (int)8 * 60;

                                                break;
                                            }
                                            else
                                            {
                                                intLeaveMin += model.timeleave_min;

                                                if (model.timeleave_deduct)
                                                    intLeaveMin_deduct += model.timeleave_min;
                                            }
                                        }
                                    }
                                }

                                #endregion

                                //--******************
                                //-- Step 2 Get timeinput       
                                //--******************
                                List<cls_TRTimeinput> listTimeinput = new List<cls_TRTimeinput>();

                                if (!fillauto)
                                {
                                    listTimeinput = objTimeinput.getDataByFillter(com, Lostwages1.lostwages_cardno, date.AddDays(-1), date.AddDays(1), true, Lostwages.project_code, Lostwages.projob_code);
                                }


                                //-- Request onsite
                                if (req_onsite != null)
                                {
                                    if (!req_onsite.timeonsite_in.Equals("00:00"))
                                    {
                                        cls_TRTimeinput time = new cls_TRTimeinput();
                                        time.timeinput_hhmm = req_onsite.timeonsite_in;
                                        time.timeinput_compare = "N";
                                        time.timeinput_function = "RECORD";
                                        time.timeinput_date = date;

                                        listTimeinput.Add(time);
                                    }

                                    if (!req_onsite.timeonsite_out.Equals("00:00"))
                                    {
                                        cls_TRTimeinput time = new cls_TRTimeinput();
                                        time.timeinput_hhmm = req_onsite.timeonsite_out;
                                        time.timeinput_compare = "N";
                                        time.timeinput_function = "RECORD";
                                        time.timeinput_date = date;

                                        listTimeinput.Add(time);
                                    }
                                }

                                //--******************
                                //-- Step 3 get shift policy
                                //--******************
                                List<cls_Timechannel> listTimechannel = new List<cls_Timechannel>();
                                bool[] blnCH = new bool[11];
                                for (int i = 1; i <= 10; i++)
                                {
                                    blnCH[i] = false;
                                }

                                //-- Request change shift
                                if (req_shift != null)
                                {
                                    Lostwages.shift_code = req_shift.timeshift_new;
                                }

                            FLEXIBLESHIFT:

                                cls_MTShift shift = null;
                                foreach (cls_MTShift mdShift in listShift)
                                {
                                    if (!Lostwages.project_code.Equals(""))
                                    {
                                        if (mdShift.project && mdShift.shift_code == Lostwages.shift_code)
                                        {
                                            shift = mdShift;
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        if (mdShift.shift_code == Lostwages.shift_code)
                                        {
                                            shift = mdShift;
                                            break;
                                        }
                                    }
                                }
                                if (shift == null)
                                {
                                    continue;
                                }
                                else
                                {
                                    #region Time channel
                                    if (!shift.shift_ch1.Equals("00:00"))
                                    {
                                        blnCH[1] = true;

                                        if (fillauto)
                                            this.addTimeManual(ref listTimeinput, date, shift.shift_ch1);
                                    }

                                    if (!shift.shift_ch3.Equals("00:00"))
                                    {
                                        blnCH[3] = true;

                                        if (fillauto)
                                            this.addTimeManual(ref listTimeinput, date, shift.shift_ch3);
                                    }

                                    if (!shift.shift_ch4.Equals("00:00"))
                                    {
                                        blnCH[4] = true;
                                        if (fillauto)
                                            this.addTimeManual(ref listTimeinput, date, shift.shift_ch4);
                                    }

                                    if (!shift.shift_ch9.Equals("00:00"))
                                    {
                                        blnCH[9] = true;
                                        if (fillauto)
                                            this.addTimeManual(ref listTimeinput, date, shift.shift_ch9);
                                    }

                                    if (!shift.shift_ch10.Equals("00:00"))
                                    {
                                        blnCH[10] = true;
                                        if (fillauto)
                                            this.addTimeManual(ref listTimeinput, date, shift.shift_ch10);
                                    }

                                    #endregion
                                }

                                //-- Break
                                cls_TRShiftbreak shift_break = null;
                                foreach (cls_TRShiftbreak mdBreak in listBreak)
                                {
                                    if (mdBreak.shift_code == Lostwages.shift_code)
                                    {
                                        shift_break = mdBreak;
                                        break;
                                    }
                                }

                                int round = 1;

                                if (listTimeinput.Count == 0)
                                    goto CALCULATE;

                                //-- Step 4 
                                #region Match time

                                if (blnCH[3] && blnCH[7])
                                    round++;

                                TimeSpan ts;

                                //-- *********************** IN *************************

                                //-- OT IN
                                bool ot_in = false;
                                if (blnCH[1])
                                {

                                    ts = TimeSpan.Parse(shift.shift_ch1);
                                    DateTime dateShiftFrom = date.AddHours(ts.Hours).AddMinutes(ts.Minutes);

                                    ts = TimeSpan.Parse(shift.shift_ch3);
                                    DateTime dateShiftTo = date.AddHours(ts.Hours).AddMinutes(ts.Minutes);

                                    if (dateShiftFrom.CompareTo(dateShiftTo) > 0)
                                        dateShiftFrom = dateShiftFrom.AddDays(-1);

                                    cls_Timecompare compare = this.doGetTimeInputIN(ref listTimeinput, dateShiftFrom, dateShiftTo);

                                    if (compare.found)
                                    {
                                        Lostwages.lostwages_ch1 = compare.date;
                                        Lostwages.lostwages_ch2 = dateShiftTo;
                                        Lostwages.lostwages_ch3 = dateShiftTo;

                                        Lostwages.lostwages_ch1_scan = true;
                                        Lostwages.lostwages_ch2_scan = true;
                                        Lostwages.lostwages_ch3_scan = true;

                                        //-- F edit 18/05/2023
                                        //blnCH[3] = false;
                                        ot_in = true;
                                    }
                                }

                                //-- Time normal IN
                                //-- F edit 18/05/2023
                                //if (blnCH[3])
                                if (!ot_in)
                                {
                                    ts = TimeSpan.Parse(shift.shift_ch3);
                                    DateTime dateShift = date.AddHours(ts.Hours).AddMinutes(ts.Minutes);

                                    string[] ch3 = shift.shift_ch3.Split(':');
                                    string[] ch3_from = shift.shift_ch3_from.Split(':');
                                    string[] ch3_to = shift.shift_ch3_to.Split(':');

                                    int min_ch3 = this.doConvertTime2Int(ch3[0]) * 60 + this.doConvertTime2Int(ch3[1]);
                                    int min_ch3_from = this.doConvertTime2Int(ch3_from[0]) * 60 + this.doConvertTime2Int(ch3_from[1]);
                                    int min_ch3_to = this.doConvertTime2Int(ch3_to[0]) * 60 + this.doConvertTime2Int(ch3_to[1]);

                                    min_ch3_from = min_ch3 - min_ch3_from;
                                    hrs = min_ch3_from / 60;
                                    min = min_ch3_from % 60;

                                    DateTime dateStart = dateShift.AddHours(-hrs).AddMinutes(-min);

                                    min_ch3_to = min_ch3_to - min_ch3;
                                    hrs = min_ch3_to / 60;
                                    min = min_ch3_to % 60;

                                    DateTime dateEnd = dateShift.AddHours(hrs).AddMinutes(min);

                                    if (dateEnd.CompareTo(dateStart) < 0)
                                        dateEnd = dateEnd.AddDays(1);

                                    cls_Timecompare compare = this.doGetTimeInputIN(ref listTimeinput, dateStart, dateEnd);

                                    if (compare.found)
                                    {
                                        Lostwages.lostwages_ch3 = compare.date;
                                        Lostwages.lostwages_ch3_scan = true;

                                    }
                                }

                                //-- *********************** OUT *************************

                                //-- OT OUT
                                if (blnCH[10])
                                {
                                    ts = TimeSpan.Parse(shift.shift_ch9);
                                    DateTime dateShiftFrom = date.AddHours(ts.Hours).AddMinutes(ts.Minutes);

                                    ts = TimeSpan.Parse(shift.shift_ch10);
                                    DateTime dateShiftTo = date.AddHours(ts.Hours).AddMinutes(ts.Minutes);

                                    if (dateShiftTo.CompareTo(dateShiftFrom) < 0)
                                        dateShiftTo = dateShiftTo.AddDays(1);

                                    cls_Timecompare compare = this.doGetTimeInputOUT(ref listTimeinput, dateShiftFrom, dateShiftTo);

                                    if (compare.found)
                                    {
                                        Lostwages.lostwages_ch10 = compare.date;
                                        Lostwages.lostwages_ch9 = dateShiftFrom;
                                        Lostwages.lostwages_ch4 = dateShiftFrom;

                                        Lostwages.lostwages_ch10_scan = true;
                                        Lostwages.lostwages_ch9_scan = true;
                                        Lostwages.lostwages_ch4_scan = true;
                                    }
                                }

                                //-- OT IN
                                if (blnCH[9])
                                {
                                    ts = TimeSpan.Parse(shift.shift_ch9);
                                    DateTime dateShiftFrom = date.AddHours(ts.Hours).AddMinutes(ts.Minutes);

                                    ts = TimeSpan.Parse(shift.shift_ch10);
                                    DateTime dateShiftTo = date.AddHours(ts.Hours).AddMinutes(ts.Minutes);

                                    if (dateShiftTo.CompareTo(dateShiftFrom) < 0)
                                        dateShiftTo = dateShiftTo.AddDays(1);

                                    cls_Timecompare compare = this.doGetTimeInputIN(ref listTimeinput, dateShiftFrom, dateShiftTo);

                                    if (compare.found)
                                    {
                                        Lostwages.lostwages_ch9 = compare.date;
                                        Lostwages.lostwages_ch4 = dateShiftFrom;

                                        Lostwages.lostwages_ch9_scan = true;
                                        Lostwages.lostwages_ch4_scan = true;
                                    }
                                }

                                //-- Normal OUT
                                if (blnCH[4])
                                {

                                    ts = TimeSpan.Parse(shift.shift_ch4);
                                    DateTime dateShift = date.AddHours(ts.Hours).AddMinutes(ts.Minutes);

                                    string[] ch4 = shift.shift_ch4.Split(':');
                                    string[] ch4_from = shift.shift_ch4_from.Split(':');
                                    string[] ch4_to = shift.shift_ch4_to.Split(':');

                                    int min_ch4 = this.doConvertTime2Int(ch4[0]) * 60 + this.doConvertTime2Int(ch4[1]);
                                    int min_ch4_from = this.doConvertTime2Int(ch4_from[0]) * 60 + this.doConvertTime2Int(ch4_from[1]);
                                    int min_ch4_to = this.doConvertTime2Int(ch4_to[0]) * 60 + this.doConvertTime2Int(ch4_to[1]);

                                    min_ch4_from = min_ch4 - min_ch4_from;
                                    hrs = min_ch4_from / 60;
                                    min = min_ch4_from % 60;

                                    DateTime dateStart = dateShift.AddHours(-hrs).AddMinutes(-min);

                                    if (min_ch4_to < min_ch4)
                                    {
                                        min_ch4_to = (24 * 60) - min_ch4 + min_ch4_to;
                                    }
                                    else
                                    {
                                        min_ch4_to = min_ch4_to - min_ch4;
                                    }


                                    hrs = min_ch4_to / 60;
                                    min = min_ch4_to % 60;


                                    DateTime dateEnd = dateShift.AddHours(hrs).AddMinutes(min);

                                    if (dateEnd.CompareTo(dateStart) < 0)
                                        dateEnd = dateEnd.AddDays(1);

                                    cls_Timecompare compare = this.doGetTimeInputOUT(ref listTimeinput, dateStart, dateEnd);

                                    if (compare.found)
                                    {
                                        Lostwages.lostwages_ch4 = compare.date;
                                        Lostwages.lostwages_ch4_scan = true;
                                    }
                                }
                                #endregion

                            CALCULATE:

                                //-- Step 5 
                                #region Calculate time
                                string strDaytype = Lostwages.lostwages_daytype;

                                Lostwages.lostwages_work1_min = 0;
                                Lostwages.lostwages_work2_min = 0;
                                Lostwages.lostwages_work1_min_app = 0;
                                Lostwages.lostwages_work2_min_app = 0;
                                Lostwages.lostwages_before_min = 0;
                                Lostwages.lostwages_before_min_app = 0;
                                Lostwages.lostwages_after_min = 0;
                                Lostwages.lostwages_after_min_app = 0;


                                //-- Request change daytype
                                if (req_daytype != null)
                                {
                                    strDaytype = req_daytype.timedaytype_new;
                                }


                                #region Calculate Late

                                Lostwages.lostwages_late_min = 0;


                                if (blnCH[3])
                                {
                                    if (Lostwages.lostwages_ch3 != null && Lostwages.lostwages_ch3_scan)
                                    {
                                        ts = TimeSpan.Parse(shift.shift_ch3);
                                        DateTime dateShift = date.AddHours(ts.Hours).AddMinutes(ts.Minutes);
                                        ts = Lostwages.lostwages_ch3.Subtract(dateShift);

                                        int intLate = (ts.Hours * 60) + ts.Minutes;
                                        if (intLate < 0)
                                            intLate = 0;

                                        Lostwages.lostwages_late_min = intLate;
                                    }

                                    if (Lostwages.lostwages_ch4 != null && Lostwages.lostwages_ch4_scan)
                                    {
                                        ts = TimeSpan.Parse(shift.shift_ch4);
                                        DateTime dateShift = date.AddHours(ts.Hours).AddMinutes(ts.Minutes);
                                        ts = dateShift.Subtract(Lostwages.lostwages_ch4);

                                        int intLate = (ts.Hours * 60) + ts.Minutes;
                                        if (intLate < 0)
                                            intLate = 0;

                                        Lostwages.lostwages_late_min += intLate;
                                    }

                                    //-- F add 29/05/2022
                                    if (!Lostwages.lostwages_ch3_scan && !Lostwages.lostwages_ch4_scan)
                                    {
                                        ts = TimeSpan.Parse(shift.shift_ch3);
                                        DateTime dateLateStart = date.AddHours(ts.Hours).AddMinutes(ts.Minutes);
                                        ts = TimeSpan.Parse(shift.shift_ch4);
                                        DateTime dateLateEnd = date.AddHours(ts.Hours).AddMinutes(ts.Minutes);

                                        ts = dateLateEnd.Subtract(dateLateStart);

                                        Lostwages.lostwages_late_min = (ts.Hours * 60) + ts.Minutes;
                                    }
                                }

                                if (blnCH[7])
                                {
                                    if (Lostwages.lostwages_ch7 != null && Lostwages.lostwages_ch7_scan)
                                    {
                                        ts = TimeSpan.Parse(shift.shift_ch7);
                                        DateTime dateShift = date.AddHours(ts.Hours).AddMinutes(ts.Minutes);
                                        ts = Lostwages.lostwages_ch7.Subtract(dateShift);

                                        int intLate = (ts.Hours * 60) + ts.Minutes;
                                        if (intLate < 0)
                                            intLate = 0;

                                        Lostwages.lostwages_late_min += intLate;
                                    }

                                    if (Lostwages.lostwages_ch8 != null && Lostwages.lostwages_ch8_scan)
                                    {
                                        ts = TimeSpan.Parse(shift.shift_ch8);
                                        DateTime dateShift = date.AddHours(ts.Hours).AddMinutes(ts.Minutes);
                                        ts = dateShift.Subtract(Lostwages.lostwages_ch8);

                                        int intLate = (ts.Hours * 60) + ts.Minutes;
                                        if (intLate < 0)
                                            intLate = 0;

                                        Lostwages.lostwages_late_min += intLate;

                                    }
                                }
                                #endregion

                                double douWorkPerday = 8 * 60;

                                //-- Working
                                Lostwages.lostwages_before_min = 0;
                                //-- OT IN
                                if (Lostwages.lostwages_ch1_scan && Lostwages.lostwages_ch2_scan)
                                {
                                    ts = Lostwages.lostwages_ch2.Subtract(Lostwages.lostwages_ch1);
                                    Lostwages.lostwages_before_min = (ts.Hours * 60) + ts.Minutes;
                                    Lostwages.lostwages_before_min_app = Lostwages.lostwages_before_min;
                                }

                                //-- Normal 1
                                if (Lostwages.lostwages_ch3_scan && Lostwages.lostwages_ch4_scan)
                                {
                                    ts = Lostwages.lostwages_ch4.Subtract(Lostwages.lostwages_ch3);
                                    Lostwages.lostwages_work1_min = (ts.Hours * 60) + ts.Minutes;

                                }
                                else
                                {
                                    if (strDaytype.Equals("O") || strDaytype.Equals("H") || strDaytype.Equals("C") || strDaytype.Equals("L"))
                                    {

                                    }
                                    else
                                    {
                                        strDaytype = "A";
                                    }
                                }

                                //-- Normal 2
                                if (round > 1)
                                {
                                    if (Lostwages.lostwages_ch7_scan && Lostwages.lostwages_ch8_scan)
                                    {

                                        ts = Lostwages.lostwages_ch8.Subtract(Lostwages.lostwages_ch7);
                                        Lostwages.lostwages_work2_min = (ts.Hours * 60) + ts.Minutes;

                                    }
                                    else
                                    {
                                        if (strDaytype.Equals("O") || strDaytype.Equals("H") || strDaytype.Equals("C") || strDaytype.Equals("L"))
                                        {

                                        }
                                        else
                                        {
                                            strDaytype = "A";
                                        }
                                    }
                                }



                                //-- OT OUT
                                if (Lostwages.lostwages_ch9_scan && Lostwages.lostwages_ch10_scan)
                                {
                                    ts = Lostwages.lostwages_ch10.Subtract(Lostwages.lostwages_ch9);
                                    Lostwages.lostwages_after_min = (ts.Hours * 60) + ts.Minutes;
                                    Lostwages.lostwages_after_min_app = Lostwages.lostwages_after_min;
                                }

                                #endregion

                                intCountSuccess++;



                                //-- Break
                                if (shift_break != null)
                                {
                                    TimeSpan ts_break;
                                    if (blnCH[3] && blnCH[4])
                                    {
                                        ts_break = TimeSpan.Parse(shift_break.shiftbreak_from);
                                        DateTime startbreak = date.AddHours(ts_break.Hours).AddMinutes(ts_break.Minutes);

                                        ts_break = TimeSpan.Parse(shift_break.shiftbreak_to);
                                        DateTime endbreak = date.AddHours(ts_break.Hours).AddMinutes(ts_break.Minutes);

                                        if (Lostwages.lostwages_ch3_scan)
                                        {
                                            if (startbreak < Lostwages.lostwages_ch3)
                                                startbreak = Lostwages.lostwages_ch3;
                                        }

                                        if (Lostwages.lostwages_ch4_scan)
                                        {
                                            if (endbreak > Lostwages.lostwages_ch4)
                                                endbreak = Lostwages.lostwages_ch4;
                                        }


                                        ts = endbreak.Subtract(startbreak);
                                        int minbreak = (ts.Hours * 60) + ts.Minutes;

                                        Lostwages.lostwages_work1_min -= minbreak;

                                    }

                                }

                                //-- Summary

                                if (Lostwages.lostwages_work1_min < 0)
                                    Lostwages.lostwages_work1_min = 0;

                                if (Lostwages.lostwages_work2_min < 0)
                                    Lostwages.lostwages_work2_min = 0;

                                if (Lostwages.lostwages_late_min < 0)
                                    Lostwages.lostwages_late_min = 0;


                                Lostwages.lostwages_work1_min_app = Lostwages.lostwages_work1_min + Lostwages.lostwages_work2_min;
                                Lostwages.lostwages_work2_min_app = 0;

                                if (Lostwages.lostwages_work1_min > douWorkPerday)
                                    Lostwages.lostwages_work1_min = (int)douWorkPerday;

                                if (Lostwages.lostwages_work1_min_app > douWorkPerday)
                                    Lostwages.lostwages_work1_min_app = (int)douWorkPerday;

                                if (intLeaveMin_deduct > douWorkPerday)
                                    intLeaveMin_deduct = (int)douWorkPerday;

                                Lostwages.lostwages_leavededuct_min = intLeaveMin_deduct;

                                if ((Lostwages.lostwages_late_min + intLeaveMin) > (int)douWorkPerday)
                                    Lostwages.lostwages_late_min = (int)douWorkPerday - intLeaveMin;

                                Lostwages.lostwages_late_min_app = Lostwages.lostwages_late_min;


                                if (strDaytype.Equals("O") || strDaytype.Equals("H") || strDaytype.Equals("C"))
                                {
                                    Lostwages.lostwages_late_min = 0;
                                    Lostwages.lostwages_late_min_app = 0;
                                }
                                else
                                {
                                    //-- Leave delete late
                                    Lostwages.lostwages_late_min = Lostwages.lostwages_late_min - intLeaveMin;
                                    Lostwages.lostwages_late_min_app = Lostwages.lostwages_late_min_app - intLeaveMin;

                                    if (Lostwages.lostwages_late_min < 0) Lostwages.lostwages_late_min = 0;
                                    if (Lostwages.lostwages_late_min_app < 0) Lostwages.lostwages_late_min_app = 0;

                                    if (Lostwages.lostwages_work1_min_app > 0 && !strDaytype.Equals("L"))
                                    {
                                        strDaytype = "N";
                                    }

                                }



                                //-- F add 18/05/2023
                                if (intLeaveMin > 0)
                                    strDaytype = "L";


                                if (strDaytype.Equals("A"))
                                {
                                    Lostwages.lostwages_late_min = 0;
                                    Lostwages.lostwages_late_min_app = 0;
                                }

                                Lostwages.lostwages_daytype = strDaytype;

                                //-- Overtime request
                                if (req_ot == null)
                                {
                                    if (strDaytype.Equals("O") || strDaytype.Equals("H") || strDaytype.Equals("C"))
                                    {
                                        Lostwages.lostwages_work1_min_app = 0;
                                        Lostwages.lostwages_work2_min_app = 0;
                                    }

                                    Lostwages.lostwages_before_min_app = 0;
                                    Lostwages.lostwages_after_min_app = 0;
                                    Lostwages.lostwages_break_min_app = 0;

                                }
                                else
                                {

                                    if (strDaytype.Equals("O") || strDaytype.Equals("H") || strDaytype.Equals("C"))
                                    {

                                        if (Lostwages.lostwages_work1_min_app > req_ot.timeot_normalmin)
                                        {
                                            Lostwages.lostwages_work1_min_app = req_ot.timeot_normalmin;
                                        }

                                    }

                                    //-- Before
                                    if (Lostwages.lostwages_before_min_app > req_ot.timeot_beforemin)
                                    {
                                        Lostwages.lostwages_before_min_app = req_ot.timeot_beforemin;
                                    }

                                    //-- After
                                    if (Lostwages.lostwages_after_min_app > req_ot.timeot_aftermin)
                                    {
                                        Lostwages.lostwages_after_min_app = req_ot.timeot_aftermin;
                                    }

                                    //-- Break
                                    if (Lostwages.lostwages_break_min_app > req_ot.timeot_breakmin)
                                    {
                                        Lostwages.lostwages_break_min_app = req_ot.timeot_breakmin;
                                    }

                                }

                                //--******************
                                //-- Step 6 Record
                                //--******************
                                objLostwages.updateWithCH(Lostwages);

                                //--******************
                                //-- Step 7 Update time input
                                //--******************
                                List<cls_TRTimeinput> listTimeinput_update = new List<cls_TRTimeinput>();

                                foreach (cls_TRTimeinput model in listTimeinput)
                                {
                                    if (model.timeinput_function.Equals("RECORD"))
                                        continue;

                                    if (model.timeinput_compare.Equals("Y"))
                                        listTimeinput_update.Add(model);
                                }

                                if (listTimeinput_update.Count > 0)
                                {
                                    objTimeinput.update_compare(listTimeinput_update);
                                }

                            }

                        }
                        catch (Exception ex)
                        {
                            listError.Add(Lostwages1.lostwages_cardno + "-" + date.ToString("dd/MM/yyyy") + ",");
                        }

                    }//-- End loop date

                }//-- End loop emp

                strResult = "Success::" + intCountSuccess.ToString();

                if (listError.Count > 0)
                {
                    strResult += "| Error::" + listError.ToString();
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

        //
        public string doImportTime(string com, string taskid)
        {
            string strResult = "";

            bool blnResult = false;

            cls_ctMTTask objMTTask = new cls_ctMTTask();
            List<cls_MTTask> listMTTask = objMTTask.getDataByFillter(com, taskid, "IMP_TIME", "");
            List<string> listError = new List<string>();

            if (listMTTask.Count > 0)
            {
                cls_MTTask task = listMTTask[0];

                task.task_start = DateTime.Now;

                cls_ctMTTask objTaskDetail = new cls_ctMTTask();
                cls_TRTaskdetail task_detail = objTaskDetail.getTaskDetail(task.task_id.ToString());

                DateTime dateFrom = task_detail.taskdetail_fromdate;
                DateTime dateTo = task_detail.taskdetail_todate;

                string fileName = task_detail.taskdetail_process;

                string filePath = Path.Combine(ClassLibrary_BPC.Config.PathFileImport + "\\Att\\Import", fileName);

                try
                {
                    //-- Step 1 Get format 
                    cls_ctMTTimeimpformat objMTTimeimpformat = new cls_ctMTTimeimpformat();
                    List<cls_MTTimeimpformat> listMTTimeimpformat = objMTTimeimpformat.getDataByFillter(com);
                    cls_MTTimeimpformat md_format = new cls_MTTimeimpformat();

                    if (listMTTimeimpformat.Count == 0)
                    {
                        return "Not config format import";
                    }
                    else
                    {
                        md_format = listMTTimeimpformat[0];
                    }


                    cls_ctTRTimeinput objTimeinput = new cls_ctTRTimeinput();
                    List<cls_TRTimeinput> listTimeinput = new List<cls_TRTimeinput>();


                    using (StreamReader file = new StreamReader(filePath))
                    {
                        int counter = 0;
                        string ln;

                        while ((ln = file.ReadLine()) != null)
                        {
                            Console.WriteLine(ln);
                            counter++;

                            string temp = ln;

                            string card = temp.Substring(md_format.card_start, md_format.card_lenght);
                            string date = temp.Substring(md_format.date_start, md_format.date_lenght);
                            string hours = temp.Substring(md_format.hours_start, md_format.hours_lenght);
                            string minute = temp.Substring(md_format.minute_start, md_format.minute_lenght);
                            string function = temp.Substring(md_format.function_start, md_format.function_lenght);
                            string machine = temp.Substring(md_format.machine_start, md_format.machine_lenght);

                            DateTime dt = DateTime.ParseExact(date, md_format.date_format, CultureInfo.InvariantCulture);

                            cls_TRTimeinput md_time = new cls_TRTimeinput();
                            md_time.timeinput_card = card;
                            md_time.timeinput_date = dt;
                            md_time.timeinput_hhmm = hours.PadLeft(2, '0') + ":" + minute.PadLeft(2, '0');
                            md_time.timeinput_function = function;
                            md_time.timeinput_terminal = machine;
                            md_time.timeinput_compare = "N";

                            listTimeinput.Add(md_time);

                        }

                        file.Close();

                    }

                    if (listTimeinput.Count > 0)
                    {
                        cls_ctTRTimeinput ct_time = new cls_ctTRTimeinput();
                        blnResult = ct_time.insert(listTimeinput);

                        if (blnResult)
                            strResult = "Success::" + listTimeinput.Count.ToString();
                    }

                }
                catch (Exception ex)
                {
                    strResult = ex.ToString();
                }

                task.task_end = DateTime.Now;
                task.task_status = "F";
                task.task_note = strResult;
                objMTTask.updateStatus(task);
            }
            else
            {
                strResult = "Task not found::" + taskid;
            }

            return strResult;
        }
        public string doSetEmpleaveacc(string year, string com, string emp, string modified_by)
        {
            string strResult = "";

            bool blnResult = false;

            try
            {
                //-- Step 1 Get policy leave (emp)
                cls_ctTREmppolatt ct_polemp = new cls_ctTREmppolatt();
                List<cls_TREmppolatt> list_polemp = ct_polemp.getDataByFillter(com, emp, "LV");

                if (list_polemp.Count > 0)
                {
                    cls_TREmppolatt polemp = list_polemp[0];

                    //-- Step 2 Get policy leave

                    cls_ctMTLeave ct_leave = new cls_ctMTLeave();
                    List<cls_MTLeave> list_leave = ct_leave.getDataByFillter(com, "", "");

                    cls_ctTRPlanleave ct_planleave = new cls_ctTRPlanleave();
                    List<cls_TRPlanleave> list_planleave = ct_planleave.getDataByFillter(com, polemp.emppolatt_policy_code);

                    if (list_planleave.Count > 0)
                    {
                        List<cls_TREmpleaveacc> list_leaveacc = new List<cls_TREmpleaveacc>();

                        foreach (cls_TRPlanleave planleave in list_planleave)
                        {
                            cls_MTLeave polleave = null;

                            foreach (cls_MTLeave leave in list_leave)
                            {
                                if (planleave.leave_code.Equals(leave.leave_code))
                                {
                                    polleave = leave;
                                    break;
                                }
                            }

                            if (polleave != null)
                            {
                                cls_TREmpleaveacc leaveacc = new cls_TREmpleaveacc();
                                leaveacc.company_code = com;
                                leaveacc.worker_code = emp;
                                leaveacc.year_code = year;
                                leaveacc.leave_code = polleave.leave_code;
                                leaveacc.empleaveacc_annual = polleave.leave_day_peryear;
                                leaveacc.empleaveacc_bf = 0;
                                leaveacc.empleaveacc_used = 0;
                                leaveacc.empleaveacc_remain = leaveacc.empleaveacc_annual;
                                leaveacc.modified_by = modified_by;

                                list_leaveacc.Add(leaveacc);

                            }

                        }


                        //-- Step 3 Record
                        if (list_leaveacc.Count > 0)
                        {
                            cls_ctTREmpleaveacc ct_empleaveacc = new cls_ctTREmpleaveacc();
                            blnResult = ct_empleaveacc.insert(com, emp, year, list_leaveacc);
                        }

                    }

                }
                                
            }
            catch (Exception ex)
            {
                strResult = "ProcessTime(doSetEmpleaveacc)::" + ex.ToString();
            }
            

            return strResult;
        }
        public bool doCalleaveacc(string year, string com, string emp, string modified_by)
        {
            string strResult = "";

            bool blnResult = false;

            try
            {
                //-- Step 1 Get plan year
                cls_ctMTYear ct_year = new cls_ctMTYear();
                List<cls_MTYear> list_year = ct_year.getDataByFillter(com, "LEAVE", "", year);

                if (list_year.Count == 0)
                    return false;

                cls_MTYear md_year = list_year[0];

                //-- Step 2 Get leave acc
                cls_ctTREmpleaveacc ct_empleaveacc = new cls_ctTREmpleaveacc();
                List<cls_TREmpleaveacc> list_leaveacc = ct_empleaveacc.getDataByFillter(com, emp, year);

                List<cls_TREmpleaveacc> list_leaveacc_new = new List<cls_TREmpleaveacc>();

                foreach (cls_TREmpleaveacc model in list_leaveacc)
                {
                    model.empleaveacc_used = 0;
                    model.empleaveacc_remain = model.empleaveacc_annual;

                    list_leaveacc_new.Add(model);
                }

                //-- Step 3 get leave request
                cls_ctTRTimeleave objTRTimeleave = new cls_ctTRTimeleave();
                List<cls_TRTimeleave> listTRTimeleave = objTRTimeleave.getDataByFillteracc("", "4", com, emp, Convert.ToDateTime(md_year.year_fromdate), Convert.ToDateTime(md_year.year_todate));

                foreach (cls_TRTimeleave leave_used in listTRTimeleave)
                {

                    foreach (cls_TREmpleaveacc leave_acc in list_leaveacc_new)
                    {

                        if (leave_used.leave_code.Equals(leave_acc.leave_code))
                        {
                            if (leave_used.timeleave_type.Equals("F"))
                                leave_acc.empleaveacc_used += leave_used.timeleave_actualday;
                            else
                                leave_acc.empleaveacc_used += (leave_used.timeleave_min / 480.0);
                        }

                    }

                }

                List<cls_TREmpleaveacc> list_leaveacc_record = new List<cls_TREmpleaveacc>();

                foreach (cls_TREmpleaveacc leave_acc in list_leaveacc_new)
                {
                    leave_acc.empleaveacc_remain = leave_acc.empleaveacc_annual - leave_acc.empleaveacc_used;

                    if (leave_acc.empleaveacc_remain < 0)
                        leave_acc.empleaveacc_remain = 0;

                    list_leaveacc_record.Add(leave_acc);

                }

                ct_empleaveacc.insert(com, emp, year, list_leaveacc_record);



            }
            catch (Exception ex)
            {
                strResult = "ProcessTime(doCalleaveacc)::" + ex.ToString();
            }


            return blnResult;
        }

        public string doCalculateTime(string com, string taskid)
        {
            string strResult = "";

            cls_ctConnection obj_conn = new cls_ctConnection();

            try
            {

                System.Text.StringBuilder obj_str = new System.Text.StringBuilder();

                obj_conn.doConnect();


                obj_str.Append(" EXEC [dbo].[ATT_PRO_CALTIME] '" + com + "', '" + taskid + "' ");

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


        private void doGenarateTimecard(string com, string emp, DateTime dateFrom, DateTime dateTo, string modified_by)
        {
            try
            {
                cls_ctTRTimeinput objTimeinput = new cls_ctTRTimeinput();
                cls_ctTRTimecard objTimecard = new cls_ctTRTimecard();

                //-- Loop date                    
                for (DateTime date = dateFrom.Date; date.Date <= dateTo.Date; date = date.AddDays(1))
                {

                    //-- Step 1 Get Time input
                    List<cls_TRTimeinput> listTimeinput = objTimeinput.getDataDistinctProject(com, emp, date, date);

                    if (listTimeinput.Count > 0)
                    {
                        //-- delete non project
                        objTimecard.delete_nonproject(com, emp, date);

                        foreach (cls_TRTimeinput timeinput in listTimeinput)
                        {
                            //-- Step 2 Genarate Time card
                            cls_TRTimecard timecard = new cls_TRTimecard();
                            timecard.company_code = com;
                            timecard.worker_code = emp;
                            timecard.project_code = timeinput.project_code;
                            timecard.projob_code = timeinput.projob_code;
                            timecard.timecard_workdate = Convert.ToDateTime(timeinput.timeinput_date);
                            timecard.timecard_daytype = "N";
                            timecard.shift_code = timeinput.shift_code;
                            timecard.timecard_color = "0";
                            timecard.modified_by = modified_by;
                            bool blnTimecard = objTimecard.insert(timecard);

                        }
                    }

                }

            }
            catch (Exception ex)
            {

            }


        }


        private void doGenarateLostwages(string com, string emp, DateTime dateFrom, DateTime dateTo, string modified_by)
        {
            try
            {
                cls_ctTRTimeinput objTimeinput = new cls_ctTRTimeinput();
                cls_ctTRLostwages objLostwages = new cls_ctTRLostwages();

                //-- Loop date                    
                for (DateTime date = dateFrom.Date; date.Date <= dateTo.Date; date = date.AddDays(1))
                {

                    //-- Step 1 Get Time input
                    List<cls_TRTimeinput> listTimeinput = objTimeinput.getDataDistinctProject(com, emp, date, date);

                    if (listTimeinput.Count > 0)
                    {
                        //-- delete non project
                        objLostwages.delete_nonproject(com, emp, date);

                        foreach (cls_TRTimeinput timeinput in listTimeinput)
                        {
                            //-- Step 2 Genarate Time card
                            cls_TRLostwages lostwages = new cls_TRLostwages();
                            lostwages.company_code = com;
                            lostwages.worker_code = emp;
                            lostwages.project_code = timeinput.project_code;
                            lostwages.projob_code = timeinput.projob_code;
                            lostwages.lostwages_workdate = Convert.ToDateTime(timeinput.timeinput_date);
                            lostwages.lostwages_daytype = "N";
                            lostwages.shift_code = timeinput.shift_code;
                            lostwages.lostwages_color = "0";
                            lostwages.modified_by = modified_by;
                            bool blnLostwages = objLostwages.insert(lostwages);

                        }
                    }

                }

            }
            catch (Exception ex)
            {

            }


        }
        //
        private void doGenarateLostwages2(string com, string cardno, DateTime dateFrom, DateTime dateTo, string modified_by)
        {
            try
            {
                cls_ctTRTimeinput objTimeinput = new cls_ctTRTimeinput();
                cls_ctTRLostwages objLostwages = new cls_ctTRLostwages();

                //-- Loop date                    
                for (DateTime date = dateFrom.Date; date.Date <= dateTo.Date; date = date.AddDays(1))
                {

                    //-- Step 1 Get Time input
                    List<cls_TRTimeinput> listTimeinput = objTimeinput.getDataDistinctProject2(com, cardno, date, date);

                    if (listTimeinput.Count > 0)
                    {
                        //-- delete non project
                        objLostwages.delete_nonproject(com, cardno, date);

                        foreach (cls_TRTimeinput timeinput in listTimeinput)
                        {
                            //-- Step 2 Genarate Time card
                            cls_TRLostwages lostwages = new cls_TRLostwages();
                            lostwages.company_code = com;
                            lostwages.lostwages_cardno = cardno;

                            lostwages.project_code = timeinput.project_code;
                            lostwages.projob_code = timeinput.projob_code;
                            lostwages.lostwages_workdate = Convert.ToDateTime(timeinput.timeinput_date);
                            lostwages.lostwages_daytype = "N";
                            lostwages.shift_code = timeinput.shift_code;
                            lostwages.lostwages_color = "0";
                            lostwages.modified_by = modified_by;
                            bool blnLostwages = objLostwages.insert(lostwages);

                        }
                    }

                }

            }
            catch (Exception ex)
            {

            }


        }
        //
    }



    public class cls_Timecompare
    {
        public cls_Timecompare()
        {
            this.date = DateTime.Now;
            this.found = false;
        }
        public bool found { get; set; }
        public DateTime date { get; set; }

    }

    public class cls_Timechannel
    {
        public cls_Timechannel() { }

        public string ch { get; set; }
        public string ch_time { get; set; }
        public int ch_from_min { get; set; }
        public int ch_to_min { get; set; }

    }
}
