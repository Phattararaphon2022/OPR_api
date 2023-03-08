using System;

namespace ClassLibrary_BPC.hrfocus.model
{
    public class cls_MTPlanshift
    {
        public cls_MTPlanshift() { }

        public int planshift_id { get; set; }
        public string planshift_code { get; set; }
        public string planshift_name_th { get; set; }
        public string planshift_name_en { get; set; }

        public string company_code { get; set; }

        public string created_by { get; set; }
        public DateTime created_date { get; set; }

        public string modified_by { get; set; }
        public DateTime modified_date { get; set; }

        public bool flag { get; set; }

    }
}
