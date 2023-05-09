using System;
namespace ClassLibrary_BPC.hrfocus.model
{
   public class cls_TRTimeonsite
    {
       public cls_TRTimeonsite() { }
        public string company_code { get; set; }
        public int timeonsite_id { get; set; }
        public string timeonsite_doc { get; set; }
        public DateTime timeonsite_workdate { get; set; }
        public string timeonsite_in { get; set; }
        public string timeonsite_out { get; set; }
        public string timeonsite_note { get; set; }
        public string reason_code { get; set; }
        public string location_code { get; set; }
        public string worker_code { get; set; }
        public int status { get; set; }
        public string created_by { get; set; }
        public DateTime created_date { get; set; }
        public string modified_by { get; set; }
        public DateTime modified_date { get; set; }
        public bool flag { get; set; }
    }
}
