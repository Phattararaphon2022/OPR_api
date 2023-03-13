using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary_BPC.hrfocus.model
{
    public class cls_TRProjobemp
    {
        public cls_TRProjobemp() { }

        public int projobemp_id { get; set; }
        public string projobemp_emp { get; set; }     
        public DateTime projobemp_fromdate { get; set; }
        public DateTime projobemp_todate { get; set; }     
        public string projobemp_status { get; set; }

        public string projob_code { get; set; }    
        public string project_code { get; set; }
                   
	    public string created_by { get;set; }
        public DateTime created_date { get; set; }
	    public string modified_by { get;set; }
        public DateTime modified_date { get; set; }

        public bool flag { get; set; }
    }
}
