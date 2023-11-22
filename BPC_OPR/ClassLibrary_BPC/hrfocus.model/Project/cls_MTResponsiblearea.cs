using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary_BPC.hrfocus.model.Project
{
   public class cls_MTResponsiblearea
    {
       public cls_MTResponsiblearea() { }
       public string company_code { get; set; }
       public int responsiblearea_id { get; set; }
       public string responsiblearea_code { get; set; }
       public string responsiblearea_name_th { get; set; }
       public string responsiblearea_name_en { get; set; }       
        public string modified_by { get; set; }
        public DateTime modified_date { get; set; }   
    }
}
