using System;

namespace ClassLibrary_BPC.hrfocus.model
{
    public class cls_TRLate
    {
        public cls_TRLate() { }
        
        public string company_code { get; set; }
        public string late_code { get; set; }
        public int late_from { get; set; }
        public int late_to { get; set; }
        public string late_deduct_type { get; set; }
        public double late_deduct_amount { get; set; }

    }
}
