using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary_BPC.hrfocus.model
{
    public class cls_TRDep
    {
        public cls_TRDep() { }

        public string company_code { get; set; }
        public string worker_code { get; set; }
        public int empdep_id { get; set; }
        public DateTime empdep_date { get; set; }
        public string empdep_level01 { get; set; }
        public string empdep_level02 { get; set; }
        public string empdep_level03 { get; set; }
        public string empdep_level04 { get; set; }
        public string empdep_level05 { get; set; }
        public string empdep_level06 { get; set; }
        public string empdep_level07 { get; set; }
        public string empdep_level08 { get; set; }
        public string empdep_level09 { get; set; }
        public string empdep_level10 { get; set; }
        public string empdep_reason { get; set; }

        public string created_by { get; set; }
        public DateTime created_date { get; set; }


        public string modified_by { get; set; }
        public DateTime modified_date { get; set; }

        public bool flag { get; set; }

        //-- Show only
        public string worker_detail_th { get; set; }
        public string worker_detail_en { get; set; }
    }
}
