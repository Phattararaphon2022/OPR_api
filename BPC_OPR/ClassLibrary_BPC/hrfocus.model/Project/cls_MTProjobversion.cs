using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary_BPC.hrfocus.model
{
    public class cls_MTProjobversion
    {
        public cls_MTProjobversion() {
            this.transaction_data = "";
            this.transaction_old = "";
            this.refso = "";
            this.custno = "";
            this.refappcostid = "";
            this.currency = "THB";
        
        }
        public int projobversion_id { get; set; }
        public string transaction_id { get; set; }
        public string version { get; set; }

        public DateTime fromdate { get; set; }
        public DateTime todate { get; set; }

        public string transaction_data { get; set; }
        public string transaction_old { get; set; }
        public string refso { get; set; }
        public string custno { get; set; }
        public string refappcostid { get; set; }
        public string currency { get; set; }

        public string project_code { get; set; }
 
        public string modified_by { get; set; }
        public DateTime modified_date { get; set; }   
    }
}
