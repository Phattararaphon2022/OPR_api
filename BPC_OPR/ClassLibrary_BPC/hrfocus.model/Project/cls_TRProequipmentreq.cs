using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary_BPC.hrfocus.model.Project
{
   public class cls_TRProequipmentreq
    {
       public cls_TRProequipmentreq() { }


       public int proequipmentreq_id { get; set; }
       public string prouniform_code { get; set; }
       public DateTime proequipmentreq_date { get; set; }
       public int proequipmentreq_qty { get; set; }

       public string proequipmentreq_note { get; set; }
       public string proequipmentreq_by { get; set; }

       public string proequipmenttype_code { get; set; }

       public string projob_code { get; set; }
       public string project_code { get; set; }

       public string created_by { get; set; }
       public DateTime created_date { get; set; }
       public string modified_by { get; set; }
       public DateTime modified_date { get; set; }
       public bool flag { get; set; }

    }
}
