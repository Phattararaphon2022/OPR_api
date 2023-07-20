using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary_BPC.hrfocus.model.Payroll
{
    public class cls_MTItem
    {
        public cls_MTItem() { }

        public string company_code { get; set; }

        public int item_id { get; set; }
        public string item_code { get; set; }
        public string item_name_th { get; set; }
        public string item_name_en { get; set; }

        public string item_type { get; set; }
        public string item_regular { get; set; }
        public string item_caltax { get; set; }
        public string item_calpf { get; set; }
        public string item_calot { get; set; }
        public string item_allowance { get; set; }
        public string item_calsso { get; set; }
        public string item_contax { get; set; }
        public string item_section { get; set; }

        public double item_rate { get; set; }
        public string item_account { get; set; }

        public string created_by { get; set; }
        public DateTime created_date { get; set; }


        public string modified_by { get; set; }
        public DateTime modified_date { get; set; }

        public bool flag { get; set; }

    }
}
