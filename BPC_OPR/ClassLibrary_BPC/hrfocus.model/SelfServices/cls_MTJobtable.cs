using System;
namespace ClassLibrary_BPC.hrfocus.model
{
    public class cls_MTJobtable
    {
        public cls_MTJobtable() { }
        public string company_code { get; set; }
        public int jobtable_id { get; set; }
        public string job_id { get; set; }
        public string job_type { get; set; }
        public string status_job { get; set; }
        public int job_nextstep { get; set; }
        public DateTime? job_date { get; set; }
        public DateTime? job_finishdate { get; set; }
        public string workflow_code { get; set; }
        public string created_by { get; set; }
        public DateTime created_date { get; set; }
    }
}
