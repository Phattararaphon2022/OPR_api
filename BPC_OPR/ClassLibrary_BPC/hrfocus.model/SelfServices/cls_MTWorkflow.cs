using System;
namespace ClassLibrary_BPC.hrfocus.model
{
    public class cls_MTWorkflow
    {
        public cls_MTWorkflow() { }
        public string company_code { get; set; }
        public int workflow_id { get; set; }
        public string workflow_code { get; set; }
        public string workflow_name_th { get; set; }
        public string workflow_name_en { get; set; }
        public string workflow_type { get; set; }
        public int step1 { get; set; }
        public int step2 { get; set; }
        public int step3 { get; set; }
        public int step4 { get; set; }
        public int step5 { get; set; }

        public int totalapprove { get; set; }

        public string created_by { get; set; }
        public DateTime created_date { get; set; }
        public string modified_by { get; set; }
        public DateTime modified_date { get; set; }
        public bool flag { get; set; }
    }
}
