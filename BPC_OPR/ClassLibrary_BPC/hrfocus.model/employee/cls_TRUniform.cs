using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary_BPC.hrfocus.model
{
    public class cls_TRUniform
    {
        public cls_TRUniform() { }

        public string company_code { get; set; }
        public string worker_code { get; set; }

        public int empuniform_id { get; set; }
        public string empuniform_code { get; set; }
        public double empuniform_qauntity { get; set; }
        public double empuniform_amount { get; set; }
        public double empuniform_total { get; set; }

        public DateTime empuniform_issuedate { get; set; }
        public string empuniform_note { get; set; }

        public string project_code { get; set; }
        public string projob_code { get; set; }
        public string proequipmenttype_code { get; set; }
        public string empuniform_size { get; set; }

        public string item_code { get; set; }

        
        public string empuniform_by { get; set; }
        public double empuniform_payperiod { get; set; }
        public double empuniform_payamount { get; set; }
        public DateTime empuniform_period { get; set; }

        public string created_by { get; set; }
        public DateTime created_date { get; set; }
        public string modified_by { get; set; }
        public DateTime modified_date { get; set; }
        public bool flag { get; set; }
    }
}
