using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary_BPC.hrfocus.model
{
    public class cls_TRPosition
    {
        public cls_TRPosition() { }

        public string company_code { get; set; }
        public string worker_code { get; set; }
        public int empposition_id { get; set; }
        public DateTime empposition_date { get; set; }
        public string empposition_position { get; set; }
        public string empposition_reason { get; set; }

        public string created_by { get; set; }
        public DateTime created_date { get; set; }


        public string modified_by { get; set; }
        public DateTime modified_date { get; set; }

        public bool flag { get; set; }
    }
}
