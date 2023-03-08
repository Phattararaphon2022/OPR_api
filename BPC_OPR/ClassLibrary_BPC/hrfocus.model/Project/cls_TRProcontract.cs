using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary_BPC.hrfocus.model
{
    public class cls_TRProcontract
    {
        public cls_TRProcontract() { }

        public int procontract_id { get; set; }        
        public string procontract_ref { get; set; }
        public DateTime procontract_date { get; set; }
        public decimal procontract_amount { get; set; }
        public DateTime procontract_fromdate { get; set; }
        public DateTime procontract_todate { get; set; }
        public string procontract_customer { get; set; }
        public string procontract_bidder { get; set; }
       
        public string project_code { get; set; }
                       
	    public string created_by { get;set; }
        public DateTime created_date { get; set; }
	    public string modified_by { get;set; }
        public DateTime modified_date { get; set; }

        public bool flag { get; set; }
    }
}
