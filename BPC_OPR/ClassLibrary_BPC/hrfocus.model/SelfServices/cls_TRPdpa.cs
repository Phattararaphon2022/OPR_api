using System;
namespace ClassLibrary_BPC.hrfocus.model
{
   public class cls_TRPdpa
    {
        public cls_TRPdpa() { }
        public string company_code { get; set; }
        public string worker_code { get; set; }
        public string worker_detail_th { get; set; }
        public string worker_detail_en { get; set; }
        public bool status { get; set; }
        public string created_by { get; set; }
        public DateTime created_date { get; set; }
    }
}
