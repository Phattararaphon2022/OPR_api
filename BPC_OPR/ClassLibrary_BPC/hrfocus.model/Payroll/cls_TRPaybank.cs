using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary_BPC.hrfocus.model.Payroll
{
    public  class cls_TRPaybank
    {
        public cls_TRPaybank() { }

    
        public string paybank_code { get; set; }

        public string worker_code { get; set; }

        public string paybank_bankcode { get; set; }

        public string paybank_bankaccount { get; set; }

        public string paybank_bankamount { get; set; }

        public string modified_by { get; set; }

        public DateTime modified_date { get; set; }


    }
}

