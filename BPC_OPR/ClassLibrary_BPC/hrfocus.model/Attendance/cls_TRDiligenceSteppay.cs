using System;

namespace ClassLibrary_BPC.hrfocus.model
{
    public class cls_TRDiligenceSteppay
    {
        public cls_TRDiligenceSteppay() { }

        public string company_code { get; set; }
        public string diligence_code { get; set; }
        public int steppay_step { get; set; }
        public string steppay_type { get; set; }
        public double steppay_amount { get; set; }
    }
}
