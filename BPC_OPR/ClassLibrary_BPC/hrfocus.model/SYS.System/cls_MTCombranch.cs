using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary_BPC.hrfocus.model.System

   {
    public class cls_MTCombranch
    {
        public cls_MTCombranch() { }

        public string company_code { get; set; }
        public int combranch_id { get; set; }
        public string combranch_code { get; set; }
        public string combranch_name_th { get; set; }
        public string combranch_name_en { get; set; }        	    

	    public string created_by { get;set; }
        public DateTime created_date { get; set; }
	    
	    public string modified_by { get;set; }
        public DateTime modified_date { get; set; }

        public bool flag { get; set; }

    }
}
