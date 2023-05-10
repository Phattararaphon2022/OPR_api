using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary_BPC.hrfocus.model
{
    public class cls_TREmplocation
    {
        public cls_TREmplocation() { }
        public string company_code { get; set; }
        public string worker_code { get; set; }
        public string location_code { get; set; }
        public DateTime emplocation_startdate { get; set; }
        public DateTime emplocation_enddate { get; set; }
        public string emplocation_note { get; set; }
        public string modified_by { get; set; }
        public DateTime modified_date { get; set; }
        public string created_by { get; set; }
        public DateTime created_date { get; set; }
    }
}
