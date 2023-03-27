using System;

namespace ClassLibrary_BPC.hrfocus.model
{
    public class cls_TRTimeleave
    {
        public cls_TRTimeleave() { }

        public string company_code { get; set; }
        public string worker_code { get; set; }


        public int timeleave_id { get; set; }
        public string timeleave_doc { get; set; }

        public DateTime timeleave_fromdate { get; set; }
        public DateTime timeleave_todate { get; set; }

        public string timeleave_type { get; set; }
        public int timeleave_min { get; set; }

        public int timeleave_actualday { get; set; }
        public bool timeleave_incholiday { get; set; }
        public bool timeleave_deduct { get; set; }

        public string timeleave_note { get; set; }
        public string leave_code { get; set; }
        public string reason_code { get; set; }
        public int status { get; set; }

        public string created_by { get; set; }
        public DateTime created_date { get; set; }
        public string modified_by { get; set; }
        public DateTime modified_date { get; set; }

        public bool flag { get; set; }

        //-- Show only
        public string worker_detail_th { get; set; }
        public string leave_detail_th { get; set; }
        public string worker_detail_en { get; set; }
        public string leave_detail_en { get; set; }
    }
}
