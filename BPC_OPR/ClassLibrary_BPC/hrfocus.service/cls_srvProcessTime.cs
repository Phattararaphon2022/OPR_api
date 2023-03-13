using ClassLibrary_BPC.hrfocus.controller;
using ClassLibrary_BPC.hrfocus.model;
using System;
using System.Collections.Generic;
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

                //-- get shitf
                cls_ctMTShift objShift = new cls_ctMTShift();
                List<cls_MTShift> listShift = objShift.getDataByFillter(com, "", "");

                //-- get shitf break
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
                            ////-- Get request doc
                            ////-- Change shift
                            //cls_TRTimeshift req_shift = null;
                            //foreach (cls_TRTimeshift model in listTimeshift)
                            //{
                            //    if (model.timeshift_workdate.Date == date)
                            //    {
                            //        req_shift = model;
                            //        break;
                            //    }
                            //}

                            ////-- Change daytype
                            //cls_TRTimedaytype req_daytype = null;
                            //foreach (cls_TRTimedaytype model in listTimedaytype)
                            //{
                            //    if (model.timedaytype_workdate.Date == date)
                            //    {
                            //        req_daytype = model;
                            //        break;
                            //    }
                            //}

                            ////-- Record time
                            //cls_TRTimeonsite req_onsite = null;
                            //foreach (cls_TRTimeonsite model in listTimeonsite)
                            //{
                            //    if (model.timeonsite_workdate.Date == date)
                            //    {
                            //        req_onsite = model;
                            //        break;
                            //    }
                            //}

                            ////-- Request overtime
                            //cls_TRTimeot req_ot = null;
                            //foreach (cls_TRTimeot model in listTimeot)
                            //{
                            //    if (model.timeot_workdate.Date == date)
                            //    {
                            //        req_ot = model;
                            //        break;
                            //    }
                            //}

                            ////-- Request leave                                 
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

                            if (listTimeinput.Count == 0)
                                goto CALCULATE;

                            //-- Step 4 
                            #region Match time
                            int round = 1;
                            if (blnCH[3] && blnCH[7])
                                round++;

                            TimeSpan ts;

                            //-- *********************** IN *************************

                            //-- OT IN
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

                                    blnCH[3] = false;
                                }
                            }

                            //-- Time normal IN
                            if (blnCH[3])
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
                                    if (timecard.timecard_late_min > 0)
                                        timecard.timecard_late_min -= minbreak;
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

                            if (blnLeaveFullday)
                            {
                                strDaytype = "L";
                            }

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
                            strResult += "| Error::" + ex.ToString();
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
