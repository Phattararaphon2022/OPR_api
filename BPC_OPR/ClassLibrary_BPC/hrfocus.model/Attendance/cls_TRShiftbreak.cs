using System;
namespace ClassLibrary_BPC.hrfocus.model
{
    public class cls_TRShiftbreak
    {
        public cls_TRShiftbreak() { }

        public string company_code { get; set; }
        public string shift_code { get; set; }
        public int shiftbreak_no { get; set; }
        public string shiftbreak_from { get; set; }
        public string shiftbreak_to { get; set; }
        public int shiftbreak_break { get; set; }

    }
}
