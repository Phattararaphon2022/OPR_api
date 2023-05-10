using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary_BPC.hrfocus.model
{
    public class cls_TRSupply
    {
        public cls_TRSupply() { }

        public string company_code { get; set; }
        public string worker_code { get; set; }

        public int empsupply_id { get; set; }
        public string empsupply_code { get; set; }
        public double empsupply_qauntity { get; set; }
        public DateTime empsupply_issuedate { get; set; }
        public string empsupply_note { get; set; }
        public DateTime empsupply_returndate { get; set; }
        public bool empsupply_returnstatus { get; set; }

        public string created_by { get; set; }
        public DateTime created_date { get; set; }
        public string modified_by { get; set; }
        public DateTime modified_date { get; set; }
        public bool flag { get; set; }

    }
}
