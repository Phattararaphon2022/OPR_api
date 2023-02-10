using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary_BPC.hrfocus.model
{
    public class cls_TRProcontact
    {
        public cls_TRProcontact() { }

        public int procontact_id { get; set; }        
        public string procontact_ref { get; set; }
        public string procontact_firstname_th { get; set; }
        public string procontact_lastname_th { get; set; }
        public string procontact_firstname_en { get; set; }
        public string procontact_lastname_en { get; set; }
        public string procontact_tel { get; set; }
        public string procontact_email { get; set; }
        public string position_code { get; set; }
        public string initial_code { get; set; }
        public string project_code { get; set; }
                       
	    public string created_by { get;set; }
        public DateTime created_date { get; set; }
	    public string modified_by { get;set; }
        public DateTime modified_date { get; set; }

        public bool flag { get; set; }
    }
}
