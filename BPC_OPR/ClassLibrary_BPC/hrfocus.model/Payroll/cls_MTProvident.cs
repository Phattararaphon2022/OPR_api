using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary_BPC.hrfocus.model.Payroll
{
   public class cls_MTProvident
    {
        public cls_MTProvident() { }

        public string company_code { get; set; }
        public int provident_id { get; set; }
        public string provident_code { get; set; }
        public string provident_name_th { get; set; }
        public string provident_name_en { get; set; }
        public string created_by { get; set; }
        public DateTime created_date { get; set; }
        public string modified_by { get; set; }
        public DateTime modified_date { get; set; }
        public bool flag { get; set; }

    }
}
