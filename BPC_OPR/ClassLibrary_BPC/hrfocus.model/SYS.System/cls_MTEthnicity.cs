using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary_BPC.hrfocus.model
{
    public class cls_MTEthnicity
    {

        public cls_MTEthnicity() { }
 
        public int ethnicity_id { get; set; }
        public string ethnicity_code { get; set; }
        public string ethnicity_name_th { get; set; }
        public string ethnicity_name_en { get; set; }       
        public string modified_by { get; set; }
        public DateTime modified_date { get; set; }   
    }
}
