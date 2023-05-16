using System;

namespace ClassLibrary_BPC.hrfocus.model
{
   public class cls_MTReqdoc
    {
       public cls_MTReqdoc() { }
        public string company_code { get; set; }
        public string worker_code { get; set; }
        public int reqdoc_id { get; set; }
        public string reqdoc_doc { get; set; }
        public DateTime reqdoc_date { get; set; }
        public string reqdoc_note { get; set; }
        public int status { get; set; }
        public string created_by { get; set; }
        public DateTime created_date { get; set; }
        public string modified_by { get; set; }
        public DateTime modified_date { get; set; }
        public bool flag { get; set; }
    }
}
