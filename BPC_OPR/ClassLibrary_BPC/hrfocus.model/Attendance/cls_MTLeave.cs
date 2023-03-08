using System;

namespace ClassLibrary_BPC.hrfocus.model
{
    public class cls_MTLeave
    {
        public cls_MTLeave() { }

        public string company_code { get; set; }

        public int leave_id { get; set; }
        public string leave_code { get; set; }
        public string leave_name_th { get; set; }
        public string leave_name_en { get; set; }

        public double leave_day_peryear { get; set; }
        public double leave_day_acc { get; set; }
        public DateTime leave_day_accexpire { get; set; }

        public string leave_incholiday { get; set; }
        public string leave_passpro { get; set; }
        public string leave_deduct { get; set; }
        public string leave_caldiligence { get; set; }
        public string leave_agework { get; set; }
        public int leave_ahead { get; set; }
        public string leave_min_hrs { get; set; }
        public double leave_max_day { get; set; }

        public string created_by { get; set; }
        public DateTime created_date { get; set; }


        public string modified_by { get; set; }
        public DateTime modified_date { get; set; }

        public bool flag { get; set; }

    }
}
