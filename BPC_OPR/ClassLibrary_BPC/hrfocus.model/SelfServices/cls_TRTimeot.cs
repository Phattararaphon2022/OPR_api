using System;
using System.Collections.Generic;
namespace ClassLibrary_BPC.hrfocus.model
{
    public class cls_TRTimeot
    {
        public cls_TRTimeot() { }

        public string company_code { get; set; }
        public string worker_code { get; set; }

        public int timeot_id { get; set; }
        public string timeot_doc { get; set; }

        public DateTime timeot_workdate { get; set; }

        public int timeot_beforemin { get; set; }
        public int timeot_normalmin { get; set; }
        public int timeot_breakmin { get; set; }
        public int timeot_aftermin { get; set; }

        public string timeot_note { get; set; }
        public string location_code { get; set; }
        public string location_name_th { get; set; }
        public string location_name_en { get; set; }
        public string reason_code { get; set; }
        public string reason_name_th { get; set; }
        public string reason_name_en { get; set; }
        public int status { get; set; }
        public string status_job { get; set; }

        public string created_by { get; set; }
        public DateTime created_date { get; set; }
        public string modified_by { get; set; }
        public DateTime modified_date { get; set; }

        public bool flag { get; set; }

        //-- Show only
        public string worker_detail_th { get; set; }
        public string worker_detail_en { get; set; }

        public List<cls_MTReqdocument> reqdoc_data { get; set; }

    }
}
