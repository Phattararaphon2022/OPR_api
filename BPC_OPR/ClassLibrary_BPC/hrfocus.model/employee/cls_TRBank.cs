using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary_BPC.hrfocus.model
{
    public class cls_TRBank
    {
        public cls_TRBank() { }
        public string company_code { get; set; }
        public string worker_code { get; set; }

        public int bank_id { get; set; }
        public string bank_code { get; set; }
        public string bank_account { get; set; }
        public double bank_percent { get; set; }
        public double bank_cashpercent { get; set; }

        public string bank_bankname { get; set; }

        public string created_by { get; set; }
        public DateTime created_date { get; set; }
        public string modified_by { get; set; }
        public DateTime modified_date { get; set; }
    }
}
