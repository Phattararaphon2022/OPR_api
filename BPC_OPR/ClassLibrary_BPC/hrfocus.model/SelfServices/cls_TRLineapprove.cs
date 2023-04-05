using System;
namespace ClassLibrary_BPC.hrfocus.model
{
    public class cls_TRLineapprove
    {
        public cls_TRLineapprove() { }

        public string company_code { get; set; }
        public string worker_code { get; set; }
        public int lineapprove_id { get; set; }
        public string lineapprove_leave { get; set; }
        public string lineapprove_ot { get; set; }
        public string lineapprove_shift { get; set; }
        public string lineapprove_punchcard { get; set; }
        public string lineapprove_checkin { get; set; }
        public string created_by { get; set; }
        public DateTime created_date { get; set; }
        public string modified_by { get; set; }
        public DateTime modified_date { get; set; }
        public bool flag { get; set; }
    }
}
