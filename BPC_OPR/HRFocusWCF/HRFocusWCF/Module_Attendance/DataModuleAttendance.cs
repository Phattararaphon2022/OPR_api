using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using ClassLibrary_BPC.hrfocus.model;
namespace BPC_OPR
{
    public class DataModuleAttendance
    {
    }

    #region InputMTPeriod
    //[DataContract]
    //public class InputMTPeriod
    //{
    //    [DataMember]
    //    public string device_name { get; set; }
    //    [DataMember]
    //    public string ip { get; set; }
    //    [DataMember]
    //    public string username { get; set; }
    //    [DataMember]
    //    public string company_code { get; set; }
    //    [DataMember]
    //    public string period_id { get; set; }
    //    [DataMember]
    //    public string period_type { get; set; }
    //    [DataMember]
    //    public string emptype_code { get; set; }
    //    [DataMember]
    //    public string year_code { get; set; }
    //    [DataMember]
    //    public string period_no { get; set; }
    //    [DataMember]
    //    public string period_name_th { get; set; }
    //    [DataMember]
    //    public string period_name_en { get; set; }
    //    [DataMember]
    //    public string period_from { get; set; }
    //    [DataMember]
    //    public string period_to { get; set; }
    //    [DataMember]
    //    public string period_payment { get; set; }
    //    [DataMember]
    //    public bool period_dayonperiod { get; set; }
    //    [DataMember]
    //    public string modified_by { get; set; }
    //    [DataMember]
    //    public DateTime modified_date { get; set; }
    //    [DataMember]
    //    public bool flag { get; set; }

    //}
    #endregion

    #region InputMTPlanholiday
    [DataContract]
    public class InputMTPlanholiday
    {
        [DataMember]
        public string device_name { get; set; }
        [DataMember]
        public string ip { get; set; }
        [DataMember]
        public string username { get; set; }
        [DataMember]
        public string company_code { get; set; }
        [DataMember]
        public string planholiday_id { get; set; }
        [DataMember]
        public string planholiday_code { get; set; }
        [DataMember]
        public string planholiday_name_th { get; set; }
        [DataMember]
        public string planholiday_name_en { get; set; }
        [DataMember]
        public string year_code { get; set; }
        [DataMember]
        public List<cls_TRHoliday> holiday_list { get; set; }
        [DataMember]
        public string modified_by { get; set; }
        [DataMember]
        public DateTime modified_date { get; set; }
        [DataMember]
        public bool flag { get; set; }

    }
#endregion

    #region InputMTShift
    [DataContract]
    public class InputMTShift
    {
        [DataMember]
        public string device_name { get; set; }
        [DataMember]
        public string ip { get; set; }
        [DataMember]
        public string username { get; set; }
        [DataMember]
        public string company_code { get; set; }

        [DataMember]
        public string shift_id { get; set; }
        [DataMember]
        public string shift_code { get; set; }
        [DataMember]
        public string shift_name_th { get; set; }
        [DataMember]
        public string shift_name_en { get; set; }

        [DataMember]
        public string shift_ch1 { get; set; }
        [DataMember]
        public string shift_ch2 { get; set; }
        [DataMember]
        public string shift_ch3 { get; set; }
        [DataMember]
        public string shift_ch4 { get; set; }
        [DataMember]
        public string shift_ch5 { get; set; }
        [DataMember]
        public string shift_ch6 { get; set; }
        [DataMember]
        public string shift_ch7 { get; set; }
        [DataMember]
        public string shift_ch8 { get; set; }
        [DataMember]
        public string shift_ch9 { get; set; }
        [DataMember]
        public string shift_ch10 { get; set; }

        [DataMember]
        public string shift_ch3_from { get; set; }
        [DataMember]
        public string shift_ch3_to { get; set; }
        [DataMember]
        public string shift_ch4_from { get; set; }
        [DataMember]
        public string shift_ch4_to { get; set; }

        [DataMember]
        public string shift_ch7_from { get; set; }
        [DataMember]
        public string shift_ch7_to { get; set; }
        [DataMember]
        public string shift_ch8_from { get; set; }
        [DataMember]
        public string shift_ch8_to { get; set; }

        [DataMember]
        public int shift_otin_min { get; set; }
        [DataMember]
        public int shift_otin_max { get; set; }

        [DataMember]
        public int shift_otout_min { get; set; }
        [DataMember]
        public int shift_otout_max { get; set; }

        [DataMember]
        public bool shift_flexiblebreak { get; set; }

        [DataMember]
        public string shiftallowance_data { get; set; }

        [DataMember]
        public string break_data { get; set; }

        [DataMember]
        public string modified_by { get; set; }
        [DataMember]
        public DateTime modified_date { get; set; }
        [DataMember]
        public bool flag { get; set; }
        [DataMember]
        public List<cls_TRShiftbreak> shift_break { get; set; }
        [DataMember]
        public List<cls_TRShiftallowance> shift_allowance { get; set; }

        [DataMember]
        public bool project { get; set; }


    }
    #endregion

    #region InputMTPlanshift
    [DataContract]
    public class InputMTPlanshift
    {
        [DataMember]
        public string device_name { get; set; }
        [DataMember]
        public string ip { get; set; }
        [DataMember]
        public string username { get; set; }
        [DataMember]
        public string company_code { get; set; }

        [DataMember]
        public string planshift_id { get; set; }
        [DataMember]
        public string planshift_code { get; set; }
        [DataMember]
        public string planshift_name_th { get; set; }
        [DataMember]
        public string planshift_name_en { get; set; }

        //-- Transaction
        [DataMember]
        public List<cls_TRPlanschedule> planschedule { get; set; }


        [DataMember]
        public string modified_by { get; set; }
        [DataMember]
        public DateTime modified_date { get; set; }
        [DataMember]
        public bool flag { get; set; }
    }
    #endregion

    #region InputMTLeave
    [DataContract]
    public class InputMTLeave
    {
        [DataMember]
        public string device_name { get; set; }
        [DataMember]
        public string ip { get; set; }
        [DataMember]
        public string username { get; set; }
        [DataMember]
        public string company_code { get; set; }
        [DataMember]
        public string leave_id { get; set; }
        [DataMember]
        public string leave_code { get; set; }
        [DataMember]
        public string leave_name_th { get; set; }
        [DataMember]
        public string leave_name_en { get; set; }
        [DataMember]
        public double leave_day_peryear { get; set; }
        [DataMember]
        public double leave_day_acc { get; set; }
        [DataMember]
        public string leave_day_accexpire { get; set; }
        [DataMember]
        public string leave_incholiday { get; set; }
        [DataMember]
        public string leave_passpro { get; set; }
        [DataMember]
        public string leave_deduct { get; set; }
        [DataMember]
        public string leave_caldiligence { get; set; }
        [DataMember]
        public string leave_agework { get; set; }
        [DataMember]
        public int leave_ahead { get; set; }
        [DataMember]
        public string leave_min_hrs { get; set; }
        [DataMember]
        public int leave_max_day { get; set; }

        [DataMember]
        public List<cls_TRLeaveWorkage> leave_workage { get; set; }

        [DataMember]
        public string modified_by { get; set; }
        [DataMember]
        public DateTime modified_date { get; set; }
        [DataMember]
        public bool flag { get; set; }

    }
#endregion

    #region InputTRLeave
    [DataContract]
    public class InputTRLeave
    {
        [DataMember]
        public string device_name { get; set; }
        [DataMember]
        public string ip { get; set; }
        [DataMember]
        public string username { get; set; }
        [DataMember]
        public string company_code { get; set; }
        [DataMember]
        public string worker_code { get; set; }
        [DataMember]
        public string year_code { get; set; }
        [DataMember]
        public string leave_code { get; set; }
        [DataMember]
        public double empleaveacc_bf { get; set; }
        [DataMember]
        public double empleaveacc_annual { get; set; }
        [DataMember]
        public double empleaveacc_used { get; set; }
        [DataMember]
        public double empleaveacc_remain { get; set; }
        [DataMember]
        public string modified_by { get; set; }
        [DataMember]
        public DateTime modified_date { get; set; }
        [DataMember]
        public bool flag { get; set; }

    }
    #endregion

    #region InputMTPlanleave
    [DataContract]
    public class InputMTPlanleave
    {
        [DataMember]
        public string device_name { get; set; }
        [DataMember]
        public string ip { get; set; }
        [DataMember]
        public string username { get; set; }
        [DataMember]
        public string company_code { get; set; }
        [DataMember]
        public string planleave_id { get; set; }
        [DataMember]
        public string planleave_code { get; set; }
        [DataMember]
        public string planleave_name_th { get; set; }
        [DataMember]
        public string planleave_name_en { get; set; }
        [DataMember]
        public List<cls_TRPlanleave> leavelists { get; set; }

        [DataMember]
        public string modified_by { get; set; }
        [DataMember]
        public DateTime modified_date { get; set; }
        [DataMember]
        public bool flag { get; set; }
    }
#endregion

    #region InputMTRateot

    [DataContract]
    public class InputMTRateot
    {
        [DataMember]
        public string device_name { get; set; }
        [DataMember]
        public string ip { get; set; }
        [DataMember]
        public string username { get; set; }
        [DataMember]
        public string company_code { get; set; }
        [DataMember]
        public string rateot_id { get; set; }
        [DataMember]
        public string rateot_code { get; set; }
        [DataMember]
        public string rateot_name_th { get; set; }
        [DataMember]
        public string rateot_name_en { get; set; }
        [DataMember]
        public List<cls_TRRateot> rateot_data { get; set; }

        [DataMember]
        public string modified_by { get; set; }
        [DataMember]
        public DateTime modified_date { get; set; }
        [DataMember]
        public bool flag { get; set; }
    }
     #endregion

    #region InputMTDiligence

    [DataContract]
    public class InputMTDiligence
    {
        [DataMember]
        public string device_name { get; set; }
        [DataMember]
        public string ip { get; set; }
        [DataMember]
        public string username { get; set; }
        [DataMember]
        public string company_code { get; set; }
        [DataMember]
        public string diligence_id { get; set; }
        [DataMember]
        public string diligence_code { get; set; }
        [DataMember]
        public string diligence_name_th { get; set; }
        [DataMember]
        public string diligence_name_en { get; set; }
        [DataMember]
        public string diligence_punchcard { get; set; }
        [DataMember]
        public int diligence_punchcard_times { get; set; }
        [DataMember]
        public int diligence_punchcard_timespermonth { get; set; }
        [DataMember]
        public string diligence_late { get; set; }
        [DataMember]
        public int diligence_late_times { get; set; }
        [DataMember]
        public int diligence_late_timespermonth { get; set; }
        [DataMember]
        public int diligence_late_acc { get; set; }

        [DataMember]
        public string diligence_ba { get; set; }
        [DataMember]
        public int diligence_before_min { get; set; }
        [DataMember]
        public int diligence_after_min { get; set; }

        [DataMember]
        public string diligence_passpro { get; set; }
        [DataMember]
        public string diligence_wrongcondition { get; set; }
        [DataMember]
        public string diligence_someperiod { get; set; }
        [DataMember]
        public string diligence_someperiod_first { get; set; }

        [DataMember]
        public List<cls_TRDiligenceSteppay> steppay_data { get; set; }

        [DataMember]
        public string modified_by { get; set; }
        [DataMember]
        public DateTime modified_date { get; set; }
        [DataMember]
        public bool flag { get; set; }
    }
    #endregion

    #region InputMTLate

    [DataContract]
    public class InputMTLate
    {
        [DataMember]
        public string device_name { get; set; }
        [DataMember]
        public string ip { get; set; }
        [DataMember]
        public string username { get; set; }
        [DataMember]
        public string company_code { get; set; }
        [DataMember]
        public string late_id { get; set; }
        [DataMember]
        public string late_code { get; set; }
        [DataMember]
        public string late_name_th { get; set; }
        [DataMember]
        public string late_name_en { get; set; }
        [DataMember]
        public List<cls_TRLate> late_data { get; set; }

        [DataMember]
        public string modified_by { get; set; }
        [DataMember]
        public DateTime modified_date { get; set; }
        [DataMember]
        public bool flag { get; set; }
    }
    #endregion

    #region InputMTPlantimeallw
    [DataContract]
    public class InputMTPlantimeallw
    {
        [DataMember]
        public string device_name { get; set; }
        [DataMember]
        public string ip { get; set; }
        [DataMember]
        public string username { get; set; }
        [DataMember]
        public string company_code { get; set; }
        [DataMember]
        public string plantimeallw_id { get; set; }
        [DataMember]
        public string plantimeallw_code { get; set; }
        [DataMember]
        public string plantimeallw_name_th { get; set; }
        [DataMember]
        public string plantimeallw_name_en { get; set; }

        [DataMember]
        public string plantimeallw_passpro { get; set; }
        [DataMember]
        public string plantimeallw_lastperiod { get; set; }

        [DataMember]
        public List<cls_TRTimeallw> timeallw_data { get; set; }

        [DataMember]
        public string modified_by { get; set; }
        [DataMember]
        public DateTime modified_date { get; set; }
        [DataMember]
        public bool flag { get; set; }

    }
    #endregion

    #region InputSetPolicyAtt

    [DataContract]
    public class InputSetPolicyAtt
    {
        [DataMember]
        public string device_name { get; set; }
        [DataMember]
        public string ip { get; set; }
        [DataMember]
        public string username { get; set; }
        [DataMember]
        public string company_code { get; set; }
        [DataMember]
        public string pol_code { get; set; }
        [DataMember]
        public string pol_type { get; set; }
        [DataMember]
        public string pol_note { get; set; }

        [DataMember]
        public string year_code { get; set; }
        [DataMember]
        public List<cls_MTWorker> emp_data { get; set; }
        [DataMember]
        public string modified_by { get; set; }
        [DataMember]
        public bool flag { get; set; }

    }
    #endregion

    #region InputBatchPlanshift
    [DataContract]
    public class InputBatchPlanshift
    {
        [DataMember]
        public string device_name { get; set; }
        [DataMember]
        public string ip { get; set; }
        [DataMember]
        public string username { get; set; }
        [DataMember]
        public string company_code { get; set; }
        [DataMember]
        public string planshift_code { get; set; }
        [DataMember]
        public string year_code { get; set; }
        [DataMember]
        public List<cls_MTWorker> transaction_data { get; set; }

    }
    #endregion

    #region FillterAttendance
    [DataContract]
    public class FillterAttendance
    {
        [DataMember]
        public string device_name { get; set; }
        [DataMember]
        public string ip { get; set; }
        [DataMember]
        public string username { get; set; }
        [DataMember]
        public string company { get; set; }
        [DataMember]
        public string language { get; set; }
        [DataMember]
        public string project_code { get; set; }
        [DataMember]
        public string worker_code { get; set; }
        [DataMember]
        public string fromdate { get; set; }
        [DataMember]
        public string todate { get; set; }
         [DataMember]
        public string lostwages_cardno { get; set; }
        
    }
    #endregion
    #region FillterALostwagesApprove
    [DataContract]
    public class FillterLostwagesApprove
    {
        [DataMember]
        public string device_name { get; set; }
        [DataMember]
        public string ip { get; set; }
        [DataMember]
        public string username { get; set; }
        [DataMember]
        public string company { get; set; }
        [DataMember]
        public string language { get; set; }
        [DataMember]
        public string project_code { get; set; }
        [DataMember]
        public string worker_code { get; set; }
        [DataMember]
        public string fromdate { get; set; }
        [DataMember]
        public string todate { get; set; }
        [DataMember]
        public string lostwages_cardno { get; set; }

        [DataMember]
        public string lostwages_status { get; set; }
    }
    #endregion

    #region InputTRTimecard
    [DataContract]
    public class InputTRTimecard
    {
        [DataMember]
        public string device_name { get; set; }
        [DataMember]
        public string ip { get; set; }
        [DataMember]
        public string username { get; set; }
        [DataMember]
        public string company_code { get; set; }        
        [DataMember]
        public string worker_code { get; set; }
        [DataMember]
        public string project_code { get; set; }
        [DataMember]
        public string projob_code { get; set; }
        [DataMember]
        public string projobsub_code { get; set; }
        
        [DataMember]
        public string shift_code { get; set; }
        [DataMember]
        public string timecard_workdate { get; set; }
        [DataMember]
        public string timecard_workdate_to { get; set; }
        [DataMember]
        public string timecard_daytype { get; set; }
        [DataMember]
        public string timecard_color { get; set; }

        [DataMember]
        public string timecard_ch1 { get; set; }
        [DataMember]
        public string timecard_ch2 { get; set; }
        [DataMember]
        public string timecard_ch3 { get; set; }
        [DataMember]
        public string timecard_ch4 { get; set; }
        [DataMember]
        public string timecard_ch5 { get; set; }
        [DataMember]
        public string timecard_ch6 { get; set; }
        [DataMember]
        public string timecard_ch7 { get; set; }
        [DataMember]
        public string timecard_ch8 { get; set; }
        [DataMember]
        public string timecard_ch9 { get; set; }
        [DataMember]
        public string timecard_ch10 { get; set; }

        [DataMember]
        public string timecard_in { get; set; }
        [DataMember]
        public string timecard_out { get; set; }

        [DataMember]
        public int timecard_before_min { get; set; }
        [DataMember]
        public int timecard_work1_min { get; set; }
        [DataMember]
        public int timecard_work2_min { get; set; }
        [DataMember]
        public int timecard_break_min { get; set; }
        [DataMember]
        public int timecard_after_min { get; set; }
        [DataMember]
        public int timecard_late_min { get; set; }

        [DataMember]
        public int timecard_before_min_app { get; set; }
        [DataMember]
        public int timecard_work1_min_app { get; set; }
        [DataMember]
        public int timecard_work2_min_app { get; set; }
        [DataMember]
        public int timecard_break_min_app { get; set; }
        [DataMember]
        public int timecard_after_min_app { get; set; }
        [DataMember]
        public int timecard_late_min_app { get; set; }

        [DataMember]
        public bool timecard_lock { get; set; }

        [DataMember]
        public string modified_by { get; set; }
        [DataMember]
        public DateTime modified_date { get; set; }

        [DataMember]
        public int index { get; set; }
        [DataMember]
        public List<cls_MTWorker> emp_data { get; set; }

    }
    #endregion

    #region InputMTTimeimpformat
    [DataContract]
    public class InputMTTimeimpformat
    {
        [DataMember]
        public string company_code { get; set; }
        [DataMember]
        public string date_format { get; set; }
        [DataMember]
        public int card_start { get; set; }
        [DataMember]
        public int card_lenght { get; set; }
        [DataMember]
        public int date_start { get; set; }
        [DataMember]
        public int date_lenght { get; set; }
        [DataMember]
        public int hours_start { get; set; }
        [DataMember]
        public int hours_lenght { get; set; }
        [DataMember]
        public int minute_start { get; set; }
        [DataMember]
        public int minute_lenght { get; set; }
        [DataMember]
        public int function_start { get; set; }
        [DataMember]
        public int function_lenght { get; set; }
        [DataMember]
        public int machine_start { get; set; }
        [DataMember]
        public int machine_lenght { get; set; }
        [DataMember]
        public string modified_by { get; set; }
        [DataMember]
        public DateTime modified_date { get; set; }
        [DataMember]
        public int index { get; set; }

    }
    #endregion


    #region InputSetPolicyAttItem
    [DataContract]
    public class InputSetPolicyAttItem
    {
        [DataMember]
        public string company_code { get; set; }
        [DataMember]
        public string worker_code { get; set; }
        [DataMember]
        public string item_sa { get; set; }
        [DataMember]
        public string item_ot { get; set; }
        [DataMember]
        public string item_aw { get; set; }
        [DataMember]
        public string item_dg { get; set; }
        [DataMember]
        public string item_lv { get; set; }
        [DataMember]
        public string item_ab { get; set; }
        [DataMember]
        public string item_lt { get; set; }
        [DataMember]
        public List<cls_MTWorker> emp_data { get; set; }
        [DataMember]
        public string modified_by { get; set; }

    }

    #endregion

 
    #region InputTRLostwages
    [DataContract]
    public class InputTRLostwages
    {
        [DataMember]
        public string device_name { get; set; }
        [DataMember]
        public string ip { get; set; }
        [DataMember]
        public string username { get; set; }
        [DataMember]
        public string company_code { get; set; }
        [DataMember]
        public string worker_code { get; set; }
        [DataMember]
        public string project_code { get; set; }
        [DataMember]
        public string projob_code { get; set; }
        //
        [DataMember]
        public string lostwages_status { get; set; }
          [DataMember]
        public string lostwages_type { get; set; }
        
        [DataMember]
        public string lostwages_salary { get; set; }
        [DataMember]
        public string lostwages_diligence { get; set; }
        [DataMember]
        public string lostwages_travelexpenses { get; set; }
        [DataMember]
        public string lostwages_other { get; set; }
        //
        //
        [DataMember]
        public string lostwages_initial { get; set; }
        [DataMember]
        public string lostwages_cardno { get; set; }
        [DataMember]
        public string lostwages_gender { get; set; }
        [DataMember]
        public string lostwages_fname_th { get; set; }
        [DataMember]
        public string lostwages_laname_th { get; set; }
        [DataMember]
        public string worker_cardno { get; set; }
        
        //

        [DataMember]
        public string projobsub_code { get; set; }

        [DataMember]
        public string shift_code { get; set; }
        [DataMember]
        public string lostwages_workdate { get; set; }
        [DataMember]
        public string lostwages_workdate_to { get; set; }
        [DataMember]
        public string lostwages_daytype { get; set; }
        [DataMember]
        public string lostwages_color { get; set; }

        [DataMember]
        public string lostwages_ch1 { get; set; }
        [DataMember]
        public string lostwages_ch2 { get; set; }
        [DataMember]
        public string lostwages_ch3 { get; set; }
        [DataMember]
        public string lostwages_ch4 { get; set; }
        [DataMember]
        public string lostwages_ch5 { get; set; }
        [DataMember]
        public string lostwages_ch6 { get; set; }
        [DataMember]
        public string lostwages_ch7 { get; set; }
        [DataMember]
        public string lostwages_ch8 { get; set; }
        [DataMember]
        public string lostwages_ch9 { get; set; }
        [DataMember]
        public string lostwages_ch10 { get; set; }

        [DataMember]
        public string lostwages_in { get; set; }
        [DataMember]
        public string lostwages_out { get; set; }

        [DataMember]
        public int lostwages_before_min { get; set; }
        [DataMember]
        public int lostwages_work1_min { get; set; }
        [DataMember]
        public int lostwages_work2_min { get; set; }
        [DataMember]
        public int lostwages_break_min { get; set; }
        [DataMember]
        public int lostwages_after_min { get; set; }
        [DataMember]
        public int lostwages_late_min { get; set; }

        [DataMember]
        public int lostwages_before_min_app { get; set; }
        [DataMember]
        public int lostwages_work1_min_app { get; set; }
        [DataMember]
        public int lostwages_work2_min_app { get; set; }
        [DataMember]
        public int lostwages_break_min_app { get; set; }
        [DataMember]
        public int lostwages_after_min_app { get; set; }
        [DataMember]
        public int lostwages_late_min_app { get; set; }

        [DataMember]
        public bool lostwages_lock { get; set; }

        [DataMember]
        public string modified_by { get; set; }
        [DataMember]
        public DateTime modified_date { get; set; }

        [DataMember]
        public int index { get; set; }
        [DataMember]
        public List<cls_MTWorker> emp_data { get; set; }
        public string fromdate { get; set; }
        [DataMember]
        public string todate { get; set; }
    }
    #endregion


    #region InputTRATTTimeleave
    [DataContract]
    public class InputTRATTTimeot
    {
        [DataMember]
        public string company_code { get; set; }
        [DataMember]
        public string worker_code { get; set; }
        [DataMember]
        public string worker_name { get; set; }
        [DataMember]
        public int timeot_id { get; set; }
        [DataMember]
        public string timeot_doc { get; set; }
        [DataMember]
        public string timeot_workdate { get; set; }

        [DataMember]
        public string timeot_worktodate { get; set; }
        [DataMember]
        public int timeot_beforemin { get; set; }
        [DataMember]
        public int timeot_normalmin { get; set; }
        [DataMember]
        public int timeot_aftermin { get; set; }
        [DataMember]
        public int timeot_break { get; set; }
        [DataMember]
        public string timeot_note { get; set; }
        [DataMember]
        public string location_code { get; set; }
        [DataMember]
        public string reason_code { get; set; }
        [DataMember]
        public string modified_by { get; set; }
        [DataMember]
        public DateTime modified_date { get; set; }
        [DataMember]
        public int index { get; set; }
        [DataMember]
        public string username { get; set; }
        [DataMember]
        public string language { get; set; }
        [DataMember]

        public string ot_data { get; set; }

    }
    #endregion


    #region InputTRATTTimeleave
    [DataContract]
    public class InputTRATTTimeleave
    {
        [DataMember]
        public string company_code { get; set; }
        [DataMember]
        public string worker_code { get; set; }
        [DataMember]
        public string worker_name { get; set; }
        [DataMember]
        public int timeleave_id { get; set; }
        [DataMember]
        public string timeleave_doc { get; set; }
        [DataMember]
        public string timeleave_fromdate { get; set; }
        [DataMember]
        public string timeleave_todate { get; set; }
        [DataMember]
        public string timeleave_type { get; set; }
        [DataMember]
        public int timeleave_min { get; set; }
        [DataMember]
        public int timeleave_actualday { get; set; }
        [DataMember]
        public bool timeleave_incholiday { get; set; }
        [DataMember]
        public bool timeleave_deduct { get; set; }
        [DataMember]
        public string timeleave_note { get; set; }
        [DataMember]
        public string leave_code { get; set; }
        [DataMember]
        public string reason_code { get; set; }
        [DataMember]
        public string modified_by { get; set; }
        [DataMember]
        public DateTime modified_date { get; set; }
        [DataMember]
        public int index { get; set; }
        [DataMember]
        public string language { get; set; }
        [DataMember]
        public string username { get; set; }
        [DataMember]
        public string leave_data { get; set; }
    }
    #endregion


    #region InputTRATTTimeleave
    [DataContract]
    public class InputTRATTTimeonsite
    {
        [DataMember]
        public string company_code { get; set; }
        [DataMember]
        public string worker_code { get; set; }
        [DataMember]
        public string worker_name { get; set; }
        [DataMember]
        public int timeonsite_id { get; set; }
        [DataMember]
        public string timeonsite_doc { get; set; }
        [DataMember]
        public string timeonsite_workdate { get; set; }
        [DataMember]
        public string timeonsite_in { get; set; }
        [DataMember]
        public string timeonsite_out { get; set; }
        [DataMember]
        public string timeonsite_note { get; set; }
        [DataMember]
        public string location_code { get; set; }
        [DataMember]
        public string reason_code { get; set; }
        [DataMember]
        public string modified_by { get; set; }
        [DataMember]
        public DateTime modified_date { get; set; }
        [DataMember]
        public int index { get; set; }
  
        public string username { get; set; }
        [DataMember]
        public string language { get; set; }
        [DataMember]

        public string ot_data { get; set; }
        [DataMember]
        public string todate { get; set; }
        [DataMember]
        public string timeonsite_data { get; set; }
    }
    #endregion

    #region InputMTReqdoc
    [DataContract]
    public class InputMTATTReqdoc
    {
        [DataMember]
        public string device_name { get; set; }
        [DataMember]
        public string ip { get; set; }
        [DataMember]
        public string username { get; set; }

        [DataMember]
        public string company_code { get; set; }
        [DataMember]
        public string worker_code { get; set; }
        [DataMember]
        public int reqdoc_id { get; set; }
        [DataMember]
        public string reqdoc_doc { get; set; }
        [DataMember]
        public string reqdoc_date { get; set; }
        [DataMember]
        public string reqdoc_date_to { get; set; }
        [DataMember]
        public string reqdoc_note { get; set; }
        [DataMember]
        public int status { get; set; }
        [DataMember]
        public List<cls_TRReqempinfo> reqempinfo_data { get; set; }
        [DataMember]
        public List<cls_TRReqdocatt> reqdocatt_data { get; set; }


        [DataMember]
        public string modified_by { get; set; }
        [DataMember]
        public DateTime modified_date { get; set; }
        [DataMember]
        public bool flag { get; set; }

    }
    #endregion


    #region InputMTATTReqdocument
    [DataContract]
    public class InputMTATTReqdocument
    {
        [DataMember]
        public string device_name { get; set; }
        [DataMember]
        public string ip { get; set; }
        [DataMember]
        public string username { get; set; }
        [DataMember]
        public string company_code { get; set; }
        [DataMember]
        public int document_id { get; set; }
        [DataMember]
        public string job_id { get; set; }
        [DataMember]
        public string job_type { get; set; }
        [DataMember]
        public string document_name { get; set; }
        [DataMember]
        public string document_type { get; set; }
        [DataMember]
        public string document_path { get; set; }

        [DataMember]
        public string create_by { get; set; }
        [DataMember]
        public DateTime create_date { get; set; }

    }
    #endregion

    #region InputTRTimecheckin
    [DataContract]
    public class InputTRATTTimecheckin
    {
        [DataMember]
        public string device_name { get; set; }
        [DataMember]
        public string ip { get; set; }
        [DataMember]
        public string username { get; set; }
        [DataMember]
        public string company_code { get; set; }
        [DataMember]
        public int timecheckin_id { get; set; }
        [DataMember]
        public string timecheckin_workdate { get; set; }
        [DataMember]
        public string timecheckin_todate { get; set; }
        [DataMember]
        public string timecheckin_time { get; set; }
        [DataMember]
        public string timecheckin_type { get; set; }
        [DataMember]
        public double timecheckin_lat { get; set; }
        [DataMember]
        public double timecheckin_long { get; set; }
        [DataMember]
        public string timecheckin_note { get; set; }
        [DataMember]
        public string location_code { get; set; }
        [DataMember]
        public string worker_code { get; set; }
        [DataMember]
        public string modified_by { get; set; }
        [DataMember]
        public DateTime modified_date { get; set; }
        [DataMember]
        public bool flag { get; set; }
        [DataMember]
        public string timecheckin_data { get; set; }
 
    }
    #endregion
}