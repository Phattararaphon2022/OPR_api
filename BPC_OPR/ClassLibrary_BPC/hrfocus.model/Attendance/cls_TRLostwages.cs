using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary_BPC.hrfocus.model.Attendance
{
   public class cls_TRLostwages
    {
       public cls_TRLostwages()
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

       //
        public string lostwages_status { get; set; }
       //
        public string lostwages_initial { get; set; }
        public string lostwages_cardno { get; set; }
        public string lostwages_gender { get; set; }
        public string lostwages_fname_th { get; set; }
        public string lostwages_laname_th { get; set; }

       //
        public string lostwages_salary { get; set; }
        public string lostwages_diligence { get; set; }
        public string lostwages_travelexpenses { get; set; }
        public string lostwages_other { get; set; }
       //

    
        public string worker_code { get; set; }
        public string shift_code { get; set; }
        public DateTime lostwages_workdate { get; set; }
        public string lostwages_daytype { get; set; }

        public string lostwages_color { get; set; }
        public DateTime lostwages_ch1 { get; set; }
        public DateTime lostwages_ch2 { get; set; }
        public DateTime lostwages_ch3 { get; set; }
        public DateTime lostwages_ch4 { get; set; }
        public DateTime lostwages_ch5 { get; set; }
        public DateTime lostwages_ch6 { get; set; }
        public DateTime lostwages_ch7 { get; set; }
        public DateTime lostwages_ch8 { get; set; }
        public DateTime lostwages_ch9 { get; set; }
        public DateTime lostwages_ch10 { get; set; }

        public bool before_scan { get; set; }
        public bool work1_scan { get; set; }
        public bool work2_scan { get; set; }
        public bool break_scan { get; set; }
        public bool after_scan { get; set; }

        public int lostwages_before_min { get; set; }
        public int lostwages_work1_min { get; set; }
        public int lostwages_work2_min { get; set; }
        public int lostwages_break_min { get; set; }
        public int lostwages_after_min { get; set; }
        public int lostwages_late_min { get; set; }

        public int lostwages_before_min_app { get; set; }
        public int lostwages_work1_min_app { get; set; }
        public int lostwages_work2_min_app { get; set; }
        public int lostwages_break_min_app { get; set; }
        public int lostwages_after_min_app { get; set; }
        public int lostwages_late_min_app { get; set; }

        public string created_by { get; set; }
        public DateTime created_date { get; set; }

        public string modified_by { get; set; }
        public DateTime modified_date { get; set; }

        public bool flag { get; set; }


        public bool lostwages_ch1_scan { get; set; }
        public bool lostwages_ch2_scan { get; set; }
        public bool lostwages_ch3_scan { get; set; }
        public bool lostwages_ch4_scan { get; set; }
        public bool lostwages_ch5_scan { get; set; }
        public bool lostwages_ch6_scan { get; set; }
        public bool lostwages_ch7_scan { get; set; }
        public bool lostwages_ch8_scan { get; set; }
        public bool lostwages_ch9_scan { get; set; }
        public bool lostwages_ch10_scan { get; set; }

        public int lostwages_leavepay_min { get; set; }
        public int lostwages_leavededuct_min { get; set; }
        public bool lostwages_before_dg { get; set; }
        public bool lostwages_after_dg { get; set; }
        public bool lostwages_lock { get; set; }

        //-- Show only
        public string worker_name_th { get; set; }
        public string worker_name_en { get; set; }
        public string worker_cardno { get; set; }

        public string emp_data { get; set; }

    }
}