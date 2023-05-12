using System;
namespace ClassLibrary_BPC.hrfocus.model
{
    public class cls_TRTimedaytype
    {
        public cls_TRTimedaytype() { }

        public string company_code { get; set; }
        public string worker_code { get; set; }
        public int timedaytype_id { get; set; }
        public string timedaytype_doc { get; set; }
        public DateTime timedaytype_workdate { get; set; }
        public string timedaytype_old { get; set; }
        public string timedaytype_new { get; set; }
        public string timedaytype_note { get; set; }
        public string reason_code { get; set; }
        public int status { get; set; }     
        public string created_by { get; set; }
        public DateTime created_date { get; set; }
        public string modified_by { get; set; }
        public DateTime modified_date { get; set; }
        public bool flag { get; set; }
    }
}
