using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary_BPC.hrfocus.model
{
   public class cls_MTEmpID
    {
       public cls_MTEmpID() { }
       public int empid_id { get; set; }
       public string empid_code { get; set; }
       public string empid_name_th { get; set; }
       public string empid_name_en { get; set; }       
        public string modified_by { get; set; }
        public DateTime modified_date { get; set; }   
    }
}
