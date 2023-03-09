using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary_BPC.hrfocus.model
{
    public class cls_MTCompany
    {
        public cls_MTCompany() { }

        public int company_id { get; set; }
        
        public string company_code { get; set; }

        public string company_initials { get; set; }
        public string company_name_th { get; set; }
        public string company_name_en { get; set; }

        public double hrs_perday { get; set; }
        public double sso_com_rate { get; set; }
        public double sso_emp_rate { get; set; }
        public double sso_min_wage { get; set; }
        public double sso_max_wage { get; set; }
        public int sso_min_age { get; set; }
        public int sso_max_age { get; set; }

        public string created_by { get; set; }
        public DateTime created_date { get; set; }

        public string modified_by { get; set; }
        public DateTime modified_date { get; set; }

        public bool flag { get; set; }

    }
}
