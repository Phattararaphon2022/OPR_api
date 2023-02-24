using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary_BPC.hrfocus.model
{
    public class cls_TRAccumalate
    {
        public cls_TRAccumalate() { }

        public string company_code { get; set; }
        public string worker_code { get; set; }
        public string periodyear { get; set; }
        public string periodid { get; set; }
        public DateTime paydate { get; set; }
        public double incomefix { get; set; }
        public double incomevar { get; set; }
        public double taxfix { get; set; }
        public double taxvar { get; set; }
        public double incomefortyone { get; set; }
        public double incomefortyoneperthree { get; set; }
        public double incomefortyonetwo { get; set; }
        public double incomefortytwoin { get; set; }
        public double incomefortytwoout { get; set; }
        public double taxfortyone { get; set; }
        public double taxfortyoneperthree { get; set; }
        public double taxfortyonetwo { get; set; }
        public double taxfortytwoin { get; set; }
        public double taxfortytwoout { get; set; }
        public double ssoacc_worker { get; set; }
        public double ssoacc_company { get; set; }
        public double pfacc_worker { get; set; }
        public double pfacc_company { get; set; }
        public double allwanceinctax { get; set; }
        public double allwancenotinctax { get; set; }
        public double deductinctax { get; set; }
        public double deductnotinctax { get; set; }



        public string created_by { get; set; }
        public DateTime created_date { get; set; }
        public string modified_by { get; set; }
        public DateTime modified_date { get; set; }

        public bool flag { get; set; }
    }
}
