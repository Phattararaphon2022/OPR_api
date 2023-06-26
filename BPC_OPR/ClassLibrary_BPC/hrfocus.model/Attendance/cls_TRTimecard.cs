using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary_BPC.hrfocus.model
{
    public class cls_TRTimecard
    {
        public cls_TRTimecard()
        {
            this.before_scan = false;
            this.work1_scan = false;
            this.work2_scan = false;
            this.break_scan = false;
            this.after_scan = false;
        }

        public string company_code { get; set; }

        public string project_code { get; set; }
        public string projob_code { get; set; }

        public string worker_code { get; set; }
        public string shift_code { get; set; }
        public DateTime timecard_workdate { get; set; }
        public string timecard_daytype { get; set; }

        public string timecard_color { get; set; }
        public DateTime timecard_ch1 { get; set; }
        public DateTime timecard_ch2 { get; set; }
        public DateTime timecard_ch3 { get; set; }
        public DateTime timecard_ch4 { get; set; }
        public DateTime timecard_ch5 { get; set; }
        public DateTime timecard_ch6 { get; set; }
        public DateTime timecard_ch7 { get; set; }
        public DateTime timecard_ch8 { get; set; }
        public DateTime timecard_ch9 { get; set; }
        public DateTime timecard_ch10 { get; set; }

        public bool before_scan { get; set; }
        public bool work1_scan { get; set; }
        public bool work2_scan { get; set; }
        public bool break_scan { get; set; }
        public bool after_scan { get; set; }

        public int timecard_before_min { get; set; }
        public int timecard_work1_min { get; set; }
        public int timecard_work2_min { get; set; }
        public int timecard_break_min { get; set; }
        public int timecard_after_min { get; set; }
        public int timecard_late_min { get; set; }

        public int timecard_before_min_app { get; set; }
        public int timecard_work1_min_app { get; set; }
        public int timecard_work2_min_app { get; set; }
        public int timecard_break_min_app { get; set; }
        public int timecard_after_min_app { get; set; }
        public int timecard_late_min_app { get; set; }

        public string created_by { get; set; }
        public DateTime created_date { get; set; }

        public string modified_by { get; set; }
        public DateTime modified_date { get; set; }

        public bool flag { get; set; }


        public bool timecard_ch1_scan { get; set; }
        public bool timecard_ch2_scan { get; set; }
        public bool timecard_ch3_scan { get; set; }
        public bool timecard_ch4_scan { get; set; }
        public bool timecard_ch5_scan { get; set; }
        public bool timecard_ch6_scan { get; set; }
        public bool timecard_ch7_scan { get; set; }
        public bool timecard_ch8_scan { get; set; }
        public bool timecard_ch9_scan { get; set; }
        public bool timecard_ch10_scan { get; set; }

        public int timecard_leavepay_min{ get; set; }
        public int timecard_leavededuct_min { get; set; }
        public bool timecard_before_dg { get; set; }
        public bool timecard_after_dg { get; set; }
        public bool timecard_lock { get; set; }

        //-- Show only
        public string worker_name_th { get; set; }
        public string worker_name_en { get; set; }
        

    }
}