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
    #region InputMTYear
    [DataContract]
    public class InputMTYear
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
        public string year_id { get; set; }
        [DataMember]
        public string year_code { get; set; }
        [DataMember]
        public string year_name_th { get; set; }
        [DataMember]
        public string year_name_en { get; set; }
        [DataMember]
        public string year_fromdate { get; set; }
        [DataMember]
        public string year_todate { get; set; }
        [DataMember]
        public string year_group { get; set; }
        [DataMember]
        public string modified_by { get; set; }
        [DataMember]
        public DateTime modified_date { get; set; }
        [DataMember]
        public bool flag { get; set; }
    }
    #endregion

    #region InputMTPeriod
    [DataContract]
    public class InputMTPeriod
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
        public string period_id { get; set; }
        [DataMember]
        public string period_type { get; set; }
        [DataMember]
        public string emptype_code { get; set; }
        [DataMember]
        public string year_code { get; set; }
        [DataMember]
        public string period_no { get; set; }
        [DataMember]
        public string period_name_th { get; set; }
        [DataMember]
        public string period_name_en { get; set; }
        [DataMember]
        public string period_from { get; set; }
        [DataMember]
        public string period_to { get; set; }
        [DataMember]
        public string period_payment { get; set; }
        [DataMember]
        public bool period_dayonperiod { get; set; }
        [DataMember]
        public string modified_by { get; set; }
        [DataMember]
        public DateTime modified_date { get; set; }
        [DataMember]
        public bool flag { get; set; }

    }
    #endregion

    #region InputMTReason
    [DataContract]
    public class InputMTReason
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
        public string reason_id { get; set; }
        [DataMember]
        public string reason_code { get; set; }
        [DataMember]
        public string reason_name_th { get; set; }
        [DataMember]
        public string reason_name_en { get; set; }
        [DataMember]
        public string reason_group { get; set; }

        [DataMember]
        public string modified_by { get; set; }
        [DataMember]
        public DateTime modified_date { get; set; }

        [DataMember]
        public bool flag { get; set; }
    }
#endregion

    #region InputMTLocation
    [DataContract]
    public class InputMTLocation
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
        public string location_id { get; set; }
        [DataMember]
        public string location_code { get; set; }
        [DataMember]
        public string location_name_th { get; set; }
        [DataMember]
        public string location_name_en { get; set; }
        [DataMember]
        public string location_detail { get; set; }
        [DataMember]
        public string location_lat { get; set; }
        [DataMember]
        public string location_long { get; set; }
        [DataMember]
        public string modified_by { get; set; }
        [DataMember]
        public DateTime modified_date { get; set; }
        [DataMember]
        public bool flag { get; set; }
    }
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

    }
    [DataContract]
    public class InputTRTimecard
    {
        [DataMember]
        public string company_code { get; set; }
        [DataMember]
        public string project_code { get; set; }
        [DataMember]
        public string worker_code { get; set; }
        [DataMember]
        public string shift_code { get; set; }
        [DataMember]
        public string timecard_workdate { get; set; }
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


    }
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
}