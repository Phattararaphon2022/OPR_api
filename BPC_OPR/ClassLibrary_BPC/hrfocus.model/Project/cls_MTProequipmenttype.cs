using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary_BPC.hrfocus.model.Project
{
   public class cls_MTProequipmenttype
     {
       public cls_MTProequipmenttype() { }
        public int proequipmenttype_id { get; set; }
        public string proequipmenttype_code { get; set; }
        public string proequipmenttype_name_th { get; set; }
        public string proequipmenttype_name_en { get; set; }       
        public string modified_by { get; set; }
        public DateTime modified_date { get; set; }   
    }
}
