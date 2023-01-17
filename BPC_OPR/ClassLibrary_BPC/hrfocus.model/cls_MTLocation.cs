using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary_BPC.hrfocus.model
{
    public class cls_MTLocation
    {
        public cls_MTLocation() { }
        public int location_id { get; set; }
        public string location_code { get; set; }
        public string location_name_th { get; set; }
        public string location_name_en { get; set; }
        public string location_detail { get; set; }
        public string modified_by { get; set; }
        public DateTime modified_date { get; set; }
    }
}
