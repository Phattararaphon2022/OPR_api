using System;
namespace ClassLibrary_BPC.hrfocus.model
{
    public class cls_TRShiftallowance
    {
        public cls_TRShiftallowance() { }

        public string company_code { get; set; }
        public string shift_code { get; set; }
        public int shiftallowance_no { get; set; }
        public string shiftallowance_name_th { get; set; }
        public string shiftallowance_name_en { get; set; }
        public string shiftallowance_hhmm { get; set; }
        public double shiftallowance_amount { get; set; }

    }
}
