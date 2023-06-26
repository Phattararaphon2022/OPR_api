using System;
namespace ClassLibrary_BPC.hrfocus.model
{
    public class cls_MTReqdocument
    {
        public cls_MTReqdocument() { }
        public string company_code { get; set; }
        public int document_id { get; set; }
        public string job_id { get; set; }
        public string job_type { get; set; }
        public string document_name { get; set; }
        public string document_type { get; set; }
        public string document_path { get; set; }
        public string created_by { get; set; }
        public DateTime created_date { get; set; }
    }
}
