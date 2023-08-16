using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary_BPC.hrfocus.model
{
    public class cls_MTProcost
    {
        public cls_MTProcost() { }
        public int procost_id { get; set; }
        public string procost_code { get; set; }
        public string procost_name_th { get; set; }
        public string procost_name_en { get; set; }

        public string procost_type { get; set; }
        public bool procost_auto { get; set; }
        public string procost_itemcode { get; set; }
        public string company_code { get; set; }   

        public string modified_by { get; set; }
        public DateTime modified_date { get; set; }   

        //-- Readonly
        public double procost_amount { get; set; }
    }
}
