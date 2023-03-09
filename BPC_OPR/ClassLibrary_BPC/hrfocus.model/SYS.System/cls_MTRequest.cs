using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary_BPC.hrfocus.model
{
   public class cls_MTRequest
    {
       public cls_MTRequest() { }

       public int request_id { get; set; }

       public string request_code { get; set; }

       public DateTime request_date { get; set; }
       public string request_agency { get; set; }
       public string request_work { get; set; }
       public string request_job_type { get; set; }
       public string request_employee_type { get; set; }
       public double request_quantity { get; set; }
       public string request_urgency { get; set; }
       public double request_wage_rate { get; set; }
       public double request_overtime { get; set; }
       public string request_another { get; set; }

        //public string created_by { get; set; }
        //public DateTime created_date { get; set; }
        public string modified_by { get; set; }
        public DateTime modified_date { get; set; }
        public bool flag { get; set; }

    }
}
