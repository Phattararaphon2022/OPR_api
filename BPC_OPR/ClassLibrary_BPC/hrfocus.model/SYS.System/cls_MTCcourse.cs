using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary_BPC.hrfocus.model.SYS.System
{
    public class cls_MTCcourse
     {
        public cls_MTCcourse() { }
        public int course_id { get; set; }
        public string course_code { get; set; }
        public string course_name_th { get; set; }
        public string course_name_en { get; set; }
        public string created_by { get; set; }
        public DateTime created_date { get; set; }
        public string modified_by { get; set; }
        public DateTime modified_date { get; set; }
        public bool flag { get; set; }

    }
}
