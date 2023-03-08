using System;

namespace ClassLibrary_BPC.hrfocus.model
{
    public class cls_TRLeaveWorkage
    {
        public cls_TRLeaveWorkage() { }

        public string company_code { get; set; }
        public string leave_code { get; set; }
        public double workage_from { get; set; }
        public double workage_to { get; set; }
        public double workage_leaveday { get; set; }

    }
}
