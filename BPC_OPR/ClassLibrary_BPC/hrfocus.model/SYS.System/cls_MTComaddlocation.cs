using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary_BPC.hrfocus.model.SYS.System
{
   public class cls_MTComaddlocation
     {
       public cls_MTComaddlocation() { }

        public string company_code { get; set; }
        public string comlocation_code { get; set; }

        public string comaddress_type { get; set; }

        public string comaddlocationth_no { get; set; }
        public string comaddlocationth_moo { get; set; }
        public string comaddlocationth_soi { get; set; }
        public string comaddlocationth_road { get; set; }
        public string comaddlocationth_tambon { get; set; }
        public string comaddlocationth_amphur { get; set; }
        public string provinceth_code { get; set; }

        public string comaddlocationen_no { get; set; }
        public string comaddlocationen_moo { get; set; }
        public string comaddlocationen_soi { get; set; }
        public string comaddlocationen_road { get; set; }
        public string comaddlocationen_tambon { get; set; }
        public string comaddlocationen_amphur { get; set; }
        public string comaddlocation_zipcode { get; set; }
        public string provinceen_code { get; set; }


        public string comaddlocation_tel { get; set; }
        public string comaddlocation_email { get; set; }
        public string comaddlocation_line { get; set; }
        public string comaddlocation_facebook { get; set; }

        public string created_by { get; set; }
        public DateTime created_date { get; set; }
        public string modified_by { get; set; }
        public DateTime modified_date { get; set; }

        public bool flag { get; set; }

    }
}