using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary_BPC.hrfocus.model
{
    public class cls_TREmpresign
    {
        public cls_TREmpresign() { }

        public string company_code { get; set; }
        public string worker_code { get; set; }
        public string card_no { get; set; }
        public int empresign_id { get; set; }
        public DateTime empresign_date { get; set; }
        public string reason_code { get; set; }

        public string created_by { get; set; }
        public DateTime created_date { get; set; }
        public string modified_by { get; set; }
        public DateTime modified_date { get; set; }
        public bool flag { get; set; }

    }
}
