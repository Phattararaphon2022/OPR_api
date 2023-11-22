using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary_BPC.hrfocus.model.Project
{
   public class cls_MTResponsiblepos
   {
       public cls_MTResponsiblepos() { }
       public string company_code { get; set; }
       public int responsiblepos_id { get; set; }
       public string responsiblepos_code { get; set; }
       public string responsiblepos_name_th { get; set; }
       public string responsiblepos_name_en { get; set; }       
        public string modified_by { get; set; }
        public DateTime modified_date { get; set; }   
    }
}
