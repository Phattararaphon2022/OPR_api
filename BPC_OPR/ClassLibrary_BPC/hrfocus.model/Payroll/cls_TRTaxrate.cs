using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary_BPC.hrfocus.model.Payroll
{
    public class cls_TRTaxrate
    {
        public cls_TRTaxrate() { }

        public string company_code { get; set; }
        public int taxrate_id { get; set; }
        public double taxrate_from { get; set; }
        public double taxrate_to { get; set; }
        public double taxrate_tax { get; set; }
        public string created_by { get; set; }
        public DateTime created_date { get; set; }
        public string modified_by { get; set; }
        public DateTime modified_date { get; set; }

        public bool flag { get; set; }

    }
}
