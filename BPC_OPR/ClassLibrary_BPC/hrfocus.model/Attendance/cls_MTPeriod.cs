using System;

namespace ClassLibrary_BPC.hrfocus.model
{
    public class cls_MTPeriod
    {
        public cls_MTPeriod() { }
        public string company_code { get; set; }
        public int period_id { get; set; }
        public string period_type { get; set; }
        public string emptype_code { get; set; }
        public string year_code { get; set; }
        public string period_no { get; set; }
        public string period_name_th { get; set; }
        public string period_name_en { get; set; }
        public DateTime period_from { get; set; }
        public DateTime period_to { get; set; }
        public DateTime period_payment { get; set; }
        public bool period_dayonperiod { get; set; }
        public string created_by { get; set; }
        public DateTime created_date { get; set; }
        public string modified_by { get; set; }
        public DateTime modified_date { get; set; }
        public bool flag { get; set; }

    }
}
