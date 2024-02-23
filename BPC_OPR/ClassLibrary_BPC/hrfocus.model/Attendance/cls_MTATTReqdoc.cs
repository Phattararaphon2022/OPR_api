using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary_BPC.hrfocus.model.Attendance
{
   public class cls_MTATTReqdoc
    {
       public cls_MTATTReqdoc() { }
        public string company_code { get; set; }
        public string worker_code { get; set; }
        public int reqdoc_id { get; set; }
        public string reqdoc_doc { get; set; }
        public DateTime reqdoc_date { get; set; }
        public string reqdoc_note { get; set; }
         public string created_by { get; set; }
        public DateTime created_date { get; set; }
        public string modified_by { get; set; }
        public DateTime modified_date { get; set; }
        public bool flag { get; set; }

        public string worker_detail_th { get; set; }
        public string worker_detail_en { get; set; }
        public string status_job { get; set; }
    }
}
