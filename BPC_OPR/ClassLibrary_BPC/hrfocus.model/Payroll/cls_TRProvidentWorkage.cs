using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary_BPC.hrfocus.model.Payroll
{
    public class cls_TRProvidentWorkage
    {
        public cls_TRProvidentWorkage() { }

        public string company_code { get; set; }
        public string provident_code { get; set; }
        public double workage_from { get; set; }
        public double workage_to { get; set; }
        public double rate_emp { get; set; }
        public double rate_com { get; set; }

    }
}
