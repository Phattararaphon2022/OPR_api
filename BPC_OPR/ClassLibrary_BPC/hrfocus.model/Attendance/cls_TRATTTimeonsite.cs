using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary_BPC.hrfocus.model.Attendance
{
   public class cls_TRATTTimeonsite
   {
       public cls_TRATTTimeonsite() { }

       public string company_code { get; set; }
       public string worker_code { get; set; }

       public int timeonsite_id { get; set; }
       public string timeonsite_doc { get; set; }

       public DateTime timeonsite_workdate { get; set; }

       public string timeonsite_in { get; set; }
       public string timeonsite_out { get; set; }

       public string timeonsite_note { get; set; }
       public string location_code { get; set; }
       public string reason_code { get; set; }

       public string created_by { get; set; }
       public DateTime created_date { get; set; }
       public string modified_by { get; set; }
       public DateTime modified_date { get; set; }

       public bool flag { get; set; }

       //-- Show only
       public string worker_detail { get; set; }
       public string location_detail { get; set; }
       public string reason_detail { get; set; }
       public List<cls_MTATTReqdocument> reqdoc_data { get; set; }

   }
}
