using System;
namespace ClassLibrary_BPC.hrfocus.model
{
    public class cls_TRLineapprove
    {
        public cls_TRLineapprove() { }

        public string company_code { get; set; }
        public string workflow_type { get; set; }
        public string workflow_code { get; set; }
        public string position_level { get; set; }     
        public string created_by { get; set; }
        public DateTime created_date { get; set; }
        public string modified_by { get; set; }
        public DateTime modified_date { get; set; }
        public bool flag { get; set; }
    }
}
