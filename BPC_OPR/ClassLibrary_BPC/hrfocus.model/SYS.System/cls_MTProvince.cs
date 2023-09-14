using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary_BPC.hrfocus.model
{
    public class cls_MTProvince
    
    {
        public cls_MTProvince() {
            this.province_code = "";
            this.province_name_th = "";
            this.province_name_en = "";
        }
 
        public int province_id { get; set; }
        public string province_code { get; set; }
        public string province_name_th { get; set; }
        public string province_name_en { get; set; }

	    public string created_by { get;set; }
        public DateTime created_date { get; set; }
	    
	    public string modified_by { get;set; }
        public DateTime modified_date { get; set; }

        public bool flag { get; set; }

    }
}
