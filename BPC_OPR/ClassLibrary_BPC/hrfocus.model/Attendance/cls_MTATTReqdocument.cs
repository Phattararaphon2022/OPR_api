using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary_BPC.hrfocus.model.Attendance
{
    public class cls_MTATTReqdocument
    {
        public cls_MTATTReqdocument() { }
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
