using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary_BPC.hrfocus.model
{
    public class cls_TRAssessment
    {
        public cls_TRAssessment() { }

        public string company_code { get; set; }
        public string worker_code { get; set; }

        public int empassessment_id { get; set;}
        public string empassessment_location { get; set;}
        public string empassessment_topic { get;set; }
        public DateTime empassessment_fromdate { get;set; }
        public DateTime empassessment_todate { get; set; }
        public double empassessment_count { get; set; }
        public string empassessment_result { get; set; }

        public string created_by { get; set; }
        public DateTime created_date { get; set; }


        public string modified_by { get; set; }
        public DateTime modified_date { get; set; }

        public bool flag { get; set; }

        //-- Show only
        
        public string worker_detail_th { get; set; }
        public string worker_detail_en { get; set; }
    }
}
