using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary_BPC.hrfocus.model
{
    public class cls_TRBenefit
    {
        public cls_TRBenefit() { }

        public string company_code { get; set; }
        public string worker_code { get; set; }

        public string item_code { get; set; }

        public int empbenefit_id { get; set; }
        public double empbenefit_amount { get; set; }
        public DateTime empbenefit_startdate { get; set; }
        public DateTime empbenefit_enddate { get; set; }
        public string empbenefit_reason { get; set; }
        public string empbenefit_note { get; set; }

        public string empbenefit_paytype { get; set; }
        public bool empbenefit_break { get; set; }
        public string empbenefit_breakreason { get; set; }

        public string empbenefit_conditionpay { get; set; }
        public string empbenefit_payfirst { get; set; }

        public string created_by { get; set; }
        public DateTime created_date { get; set; }
        public string modified_by { get; set; }
        public DateTime modified_date { get; set; }

        public bool flag { get; set; }


        //-- Show only
        public string worker_initial { get; set; }
        public string worker_fname_th { get; set; }
        public string worker_lname_th { get; set; }
        public string worker_fname_en { get; set; }
        public string worker_lname_en { get; set; }
        public string item_name_th { get; set; }
        public string item_name_en { get; set; }
    }
}
