using System;

namespace ClassLibrary_BPC.hrfocus.model
{
    public class cls_MTRateot
    {
        public cls_MTRateot() { }

        public string company_code { get; set; }
        public int rateot_id { get; set; }
        public string rateot_code { get; set; }
        public string rateot_name_th { get; set; }
        public string rateot_name_en { get; set; }
        public string created_by { get; set; }
        public DateTime created_date { get; set; }

        public string modified_by { get; set; }
        public DateTime modified_date { get; set; }

        public bool flag { get; set; }

    }
}
