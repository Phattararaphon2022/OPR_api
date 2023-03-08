using System;
namespace ClassLibrary_BPC.hrfocus.model
{
    public class cls_MTPlanleave
    {
        public cls_MTPlanleave() { }

        public int planleave_id { get; set; }
        public string planleave_code { get; set; }
        public string planleave_name_th { get; set; }
        public string planleave_name_en { get; set; }

        public string company_code { get; set; }

        public string created_by { get; set; }
        public DateTime created_date { get; set; }


        public string modified_by { get; set; }
        public DateTime modified_date { get; set; }

        public bool flag { get; set; }

    }
}
