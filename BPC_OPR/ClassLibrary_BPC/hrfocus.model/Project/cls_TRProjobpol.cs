using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary_BPC.hrfocus.model
{
    public class cls_TRProjobpol
    {
        public cls_TRProjobpol() { }
        public int projobpol_id { get; set; }        

        public string projobpol_type { get; set; }

        public string projobpol_timepol { get; set; }
        public string projobpol_slip { get; set; }
        public string projobpol_uniform { get; set; }

        public string project_code { get; set; }
        public string projobmain_code { get; set; }

        public string modified_by { get; set; }
        public DateTime modified_date { get; set; }   

    }
}
