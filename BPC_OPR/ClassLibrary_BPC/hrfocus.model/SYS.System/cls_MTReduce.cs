using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary_BPC.hrfocus.model
{
    public class cls_MTReduce
    {
        public cls_MTReduce() { }

        public int reduce_id { get; set; }
        public string reduce_code { get; set; }
        public string reduce_name_th { get; set; }
        public string reduce_name_en { get; set; }

        public double reduce_amount { get; set; }
        public double reduce_amount_max { get; set; }

        public double reduce_percent { get; set; }
        public double reduce_percent_max { get; set; }

        public string created_by { get; set; }
        public DateTime created_date { get; set; }


        public string modified_by { get; set; }
        public DateTime modified_date { get; set; }

        public bool flag { get; set; }

    }
}
