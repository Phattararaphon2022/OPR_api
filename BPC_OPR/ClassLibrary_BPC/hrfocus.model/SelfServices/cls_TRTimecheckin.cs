using System;
using System.Collections.Generic;
namespace ClassLibrary_BPC.hrfocus.model
{
    public class cls_TRTimecheckin
    {
        public cls_TRTimecheckin() { }
        public string company_code { get; set; }
        public int timecheckin_id { get; set; }
        public string timecheckin_doc { get; set; }
        public DateTime timecheckin_workdate { get; set; }
        public string timecheckin_time { get; set; }
        public string timecheckin_type { get; set; }
        public double timecheckin_lat { get; set; }
        public double timecheckin_long { get; set; }
        public string timecheckin_note { get; set; }
        public string location_code { get; set; }
        public string worker_code { get; set; }
        public string created_by { get; set; }
        public DateTime created_date { get; set; }
        public string modified_by { get; set; }
        public DateTime modified_date { get; set; }
        public bool flag { get; set; }
        public int status { get; set; }

        public string worker_detail_th { get; set; }
        public string worker_detail_en { get; set; }
        public string location_name_th { get; set; }
        public string location_name_en { get; set; }
        public string status_job { get; set; }
        public List<cls_MTReqdocument> reqdoc_data { get; set; }
    }
}
