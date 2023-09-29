using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary_BPC.hrfocus.model
{
    public class cls_MTProject
    {
        public cls_MTProject() { }
        public int project_id { get; set; }
        public string project_code { get; set; }
        public string project_name_th { get; set; }
        public string project_name_en { get; set; }

        public string project_name_sub { get; set; }
        public string project_codecentral { get; set; }

        public string project_protype { get; set; }

        public string project_proarea { get; set; }
        public string project_progroup { get; set; }
        public string fromdate { get; set; }


        public string project_probusiness { get; set; }
        public string project_roundtime { get; set; }
        public string project_roundmoney { get; set; }

        public string project_proholiday { get; set; }


        public string project_status { get; set; }
        public string company_code { get; set; }  

        public string modified_by { get; set; }
        public DateTime modified_date { get; set; }   
    }
}
