using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary_BPC.hrfocus.model
{
    public class cls_MTProuniform
    {
        public cls_MTProuniform() { }
        public string company_code { get; set; }

        public int prouniform_id { get; set; }
        public string prouniform_code { get; set; }
        public string prouniform_name_th { get; set; }
        public string prouniform_name_en { get; set; }       
        public string modified_by { get; set; }
        public DateTime modified_date { get; set; }   
    }
}
