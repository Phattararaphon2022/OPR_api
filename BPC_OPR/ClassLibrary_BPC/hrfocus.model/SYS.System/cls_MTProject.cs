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
        public string project_name_short { get; set; }
        public string project_type { get; set; }
        public string modified_by { get; set; }
        public DateTime modified_date { get; set; }   
    }
}
