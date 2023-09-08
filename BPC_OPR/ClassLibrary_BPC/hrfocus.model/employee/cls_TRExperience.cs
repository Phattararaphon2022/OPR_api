using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary_BPC.hrfocus.model
{
    public class cls_TRExperience
    {
        public cls_TRExperience() { }

        public string company_code { get; set; }
        public string worker_code { get; set; }
        public int experience_id { get; set; }
        public string company_name { get; set; }
        public string position { get; set; }
        public double salary { get; set; }
        public DateTime startdate { get; set; }
        public DateTime enddate { get; set; }
        public string description { get; set; }
        public string created_by { get; set; }
        public DateTime created_date { get; set; }


        public string modified_by { get; set; }
        public DateTime modified_date { get; set; }

        public bool flag { get; set; }
    }
}
