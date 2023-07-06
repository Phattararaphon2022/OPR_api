using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary_BPC.hrfocus.model
{
  public  class cls_TRComimagesMaps
 
   {
      public cls_TRComimagesMaps() { }

        public string company_code { get; set; }
 
        public int comimagesmaps_id { get; set; }
        public byte[] comimagesmaps_imagesmaps { get; set; }

        public string created_by { get;set; }
        public DateTime created_date { get; set; }  
	    public string modified_by { get;set; }
        public DateTime modified_date { get; set; }

        public bool flag { get; set; }

    
    }
}

