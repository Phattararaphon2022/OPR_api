using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary_BPC.hrfocus.model.Project
{
   public class cls_TRProimages
     {
       public cls_TRProimages() { }

        public string company_code { get; set; }
        public string project_code { get; set; }

        public int proimages_no { get; set; }
        public byte[] proimages_images { get; set; }        
        public string created_by { get;set; }
        public DateTime created_date { get; set; }  
	    public string modified_by { get;set; }
        public DateTime modified_date { get; set; }

        public bool flag { get; set; }

    
    }
}
