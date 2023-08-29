using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary_BPC.hrfocus.model
{
    public class cls_MTBlacklist
    {
        public cls_MTBlacklist() { }

        public string company_code { get; set; }
        public int blacklist_id { get; set; }
        public string card_no { get; set; }
        public string worker_code { get; set; }
        public string blacklist_fname_th { get; set; }
        public string blacklist_lname_th { get; set; }
        public string blacklist_fname_en { get; set; }
        public string blacklist_lname_en { get; set; }
        public string reason_code { get; set; }
        public string blacklist_note { get; set; }
        public string created_by { get; set; }
        public DateTime created_date { get; set; }
        public string modified_by { get; set; }
        public DateTime modified_date { get; set; }
        public bool flag { get; set; }

        //show only
        public string worker_fname_th { get; set; }
        public string worker_lname_th { get; set; }
        public string worker_fname_en { get; set; }
        public string worker_lname_en { get; set; }
        public string worker_detail_th { get; set; }

        public string worker_detail_en { get; set; }

    }
}
