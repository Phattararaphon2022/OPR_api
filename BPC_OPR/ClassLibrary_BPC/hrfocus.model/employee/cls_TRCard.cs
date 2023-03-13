using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary_BPC.hrfocus.model
{
    public class cls_TRCard
    {
        public cls_TRCard() { }

        public string company_code { get; set; }
        public string worker_code { get; set; }

        public int card_id { get; set; }
        public string card_code { get; set; }

        public string card_type { get; set; }

        public DateTime card_issue { get; set; }
        public DateTime card_expire { get; set; }


        public string created_by { get; set; }
        public DateTime created_date { get; set; }


        public string modified_by { get; set; }
        public DateTime modified_date { get; set; }

        public bool flag { get; set; }
    }
}
