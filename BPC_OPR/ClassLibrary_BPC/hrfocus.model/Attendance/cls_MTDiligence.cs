using System;

namespace ClassLibrary_BPC.hrfocus.model
{
    public class cls_MTDiligence
    {
        public cls_MTDiligence() { }

        public string company_code { get; set; }

        public int diligence_id { get; set; }
        public string diligence_code { get; set; }
        public string diligence_name_th { get; set; }
        public string diligence_name_en { get; set; }

        public string diligence_punchcard { get; set; }
        public int diligence_punchcard_times { get; set; }
        public int diligence_punchcard_timespermonth { get; set; }

        public string diligence_late { get; set; }
        public int diligence_late_times { get; set; }
        public int diligence_late_timespermonth { get; set; }
        public int diligence_late_acc { get; set; }

        public string diligence_ba { get; set; }
        public int diligence_before_min { get; set; }

        public int diligence_after_min { get; set; }



        public string diligence_passpro { get; set; }

        public string diligence_wrongcondition { get; set; }

        public string diligence_someperiod { get; set; }
        public string diligence_someperiod_first { get; set; }

        public string created_by { get; set; }
        public DateTime created_date { get; set; }

        public string modified_by { get; set; }
        public DateTime modified_date { get; set; }

        public bool flag { get; set; }
    }
}
