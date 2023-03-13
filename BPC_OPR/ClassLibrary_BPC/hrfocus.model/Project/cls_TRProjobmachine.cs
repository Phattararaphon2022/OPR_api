using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary_BPC.hrfocus.model
{
    public class cls_TRProjobmachine
    {
        public cls_TRProjobmachine() { }

        public int projobmachine_id { get; set; }
        public string projobmachine_ip { get; set; }
        public string projobmachine_port { get; set; }
        public bool projobmachine_enable { get; set; }
        
        public string projob_code { get; set; }    
        public string project_code { get; set; }
                   
	    public string created_by { get;set; }
        public DateTime created_date { get; set; }
	    public string modified_by { get;set; }
        public DateTime modified_date { get; set; }

        public bool flag { get; set; }
    }
}
