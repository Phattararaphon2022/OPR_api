using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary_BPC.hrfocus.model.Project
{
   public class cls_TRProwithdraw
     {
       public cls_TRProwithdraw() { }
       public int prowithdraw_id { get; set; }
       public DateTime prowithdraw_workdate { get; set; }
       public string company_code { get; set; }
       public string worker_code { get; set; }
       public string project_code { get; set; }
       public string projob_type { get; set; }
       public string projob_code { get; set; }

       public string created_by { get; set; }
       public DateTime created_date { get; set; }
       public string modified_by { get; set; }
       public DateTime modified_date { get; set; }
       public bool flag { get; set; }
    }
}
