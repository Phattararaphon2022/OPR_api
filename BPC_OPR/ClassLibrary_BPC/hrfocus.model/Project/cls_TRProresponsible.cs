using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary_BPC.hrfocus.model
{
    public class cls_TRProresponsible
    {
        public cls_TRProresponsible() { }

        public int proresponsible_id { get; set; }        
        public string proresponsible_ref { get; set; }
        public string proresponsible_emp { get; set; }
        public string proresponsible_position { get; set; }
        public string proresponsible_area { get; set; }
        public DateTime proresponsible_fromdate { get; set; }
        public DateTime proresponsible_todate { get; set; }
              
        public string project_code { get; set; }
                       
	    public string created_by { get;set; }
        public DateTime created_date { get; set; }
	    public string modified_by { get;set; }
        public DateTime modified_date { get; set; }

        public bool flag { get; set; }
    }
}
