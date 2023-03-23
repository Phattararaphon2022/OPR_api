using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary_BPC.hrfocus.model.Recruitment
{
    public class cls_TRReqAssessment
    {
        public cls_TRReqAssessment() { }

        public string company_code { get; set; }
        public string applywork_code { get; set; }

        public int reqassessment_id { get; set; }
        public string reqassessment_location { get; set; }
        public string reqassessment_topic { get; set; }
        public DateTime reqassessment_fromdate { get; set; }
        public DateTime reqassessment_todate { get; set; }
        public double reqassessment_count { get; set; }
        public string reqassessment_result { get; set; }

        public string created_by { get; set; }
        public DateTime created_date { get; set; }


        public string modified_by { get; set; }
        public DateTime modified_date { get; set; }

        public bool flag { get; set; }
    }
}
