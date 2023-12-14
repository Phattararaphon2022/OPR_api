using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary_BPC.hrfocus.model.employee
{
   public class cls_MTDashboards
   {
        public cls_MTDashboards() { }
        public string company_code { get; set; }

        public string worker_code { get; set; }
        public int location_code { get; set; }
       
        public string position_name  { get; set; }
        public string position_name_en { get; set; }
        public string worker_gender_en { get; set; }
        public string worker_gender_th { get; set; }
        public string worker_gender { get; set; }

        public string dep_name_th { get; set; }
        public string dep_name_en { get; set; }
        public string age_code { get; set; }
       public string work_age { get; set; }
       public string empposition_position { get; set; }

       
         public string location_name_th { get; set; }
        public string location_name_en { get; set; }
        public string type_name_th { get; set; }
       public string type_name_en { get; set; }
       public string worker_type { get; set; }
       public string worker_resignstatus { get; set; }

       
       

        
    }
}
