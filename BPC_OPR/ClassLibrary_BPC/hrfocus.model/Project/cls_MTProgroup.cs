using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary_BPC.hrfocus.model.Project
{
   public class cls_MTProgroup
    {
       public cls_MTProgroup() { }

       public int progroup_id { get; set; }
       public string progroup_code { get; set; }
       public string progroup_name_th { get; set; }
       public string progroup_name_en { get; set; }       
        public string modified_by { get; set; }
        public DateTime modified_date { get; set; }   
    }
}
