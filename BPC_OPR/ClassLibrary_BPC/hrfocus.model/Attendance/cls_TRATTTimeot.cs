using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary_BPC.hrfocus.model.Attendance
{
    public class cls_TRATTTimeot
    {
        public cls_TRATTTimeot() { }

        public string company_code { get; set; }
        public string worker_code { get; set; }

        public int timeot_id { get; set; }
        public string timeot_doc { get; set; }

        public DateTime timeot_workdate { get; set; }
        public DateTime timeot_worktodate { get; set; }

       
        public int timeot_beforemin { get; set; }
        public int timeot_normalmin { get; set; }
        public int timeot_aftermin { get; set; }
        public int timeot_break { get; set; }

        public string timeot_note { get; set; }
        public string location_code { get; set; }
        public string reason_code { get; set; }
      
	    public string created_by { get;set; }
        public DateTime created_date { get; set; }
	    public string modified_by { get;set; }
        public DateTime modified_date { get; set; }

        public bool flag { get; set; }

        //-- Show only
        public string worker_detail { get; set; }
        public List<cls_MTATTReqdocument> reqdoc_data { get; set; }

    }
}