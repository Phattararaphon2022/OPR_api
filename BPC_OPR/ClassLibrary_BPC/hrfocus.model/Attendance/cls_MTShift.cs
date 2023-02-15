using System;
using System.Collections.Generic;

namespace ClassLibrary_BPC.hrfocus.model
{
    public class cls_MTShift
    {
        public cls_MTShift() { }

        public string company_code { get; set;} 
        public int shift_id { get; set; }
        public string shift_code { get; set; }
        public string shift_name_th { get; set; }
        public string shift_name_en { get; set; }

        public string shift_ch1 { get; set; }
        public string shift_ch2 { get; set; }

        public string shift_ch3 { get; set; }
        public string shift_ch4 { get; set; }

        public string shift_ch5 { get; set; }
        public string shift_ch6 { get; set; }

        public string shift_ch7 { get; set; }
        public string shift_ch8 { get; set; }

        public string shift_ch9 { get; set; }
        public string shift_ch10 { get; set; }

        public string shift_ch3_from { get; set; }
        public string shift_ch3_to { get; set; }

        public string shift_ch4_from { get; set; }
        public string shift_ch4_to { get; set; }

        public string shift_ch7_from { get; set; }
        public string shift_ch7_to { get; set; }

        public string shift_ch8_from { get; set; }
        public string shift_ch8_to { get; set; }

        public int shift_otin_min { get; set; }
        public int shift_otin_max { get; set; }

        public int shift_otout_min { get; set; }
        public int shift_otout_max { get; set; }

        public bool shift_flexiblebreak { get; set; }

        public string created_by { get; set; }
        public DateTime created_date { get; set; }

        public string modified_by { get; set; }
        public DateTime modified_date { get; set; }

        public bool flag { get; set; }

    }
}
