using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary_BPC.hrfocus.model.Payroll
{
    public class cls_TRPayreduce
    {
        public cls_TRPayreduce() { }

        public string company_code { get; set; }
        public string worker_code { get; set; }
        public DateTime payreduce_paydate { get; set; }
        public string reduce_code { get; set; }
        public double payreduce_amount { get; set; }

        //-- Display only
        public string reduce_name_th { get; set; }
        public string reduce_name_en { get; set; }
    }
}
