using System;
namespace ClassLibrary_BPC.hrfocus.model
{
    public class cls_MTPdpafile
    {
        public cls_MTPdpafile() { }
        public string company_code { get; set; }
        public int document_id { get; set; }
        public string document_name { get; set; }
        public string document_type { get; set; }
        public string document_path { get; set; }
        public string created_by { get; set; }
        public DateTime created_date { get; set; }
    }
}
