using System;

namespace ClassLibrary_BPC.hrfocus.model
{
    public class cls_TRRateot
    {
        public cls_TRRateot() { }

        public string company_code { get; set; }
        public string rateot_code { get; set; }
        public string rateot_daytype { get; set; }
        public double rateot_before { get; set; }
        public double rateot_normal { get; set; }
        public double rateot_break { get; set; }
        public double rateot_after { get; set; }
    }
}
