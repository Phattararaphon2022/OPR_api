using System;
namespace ClassLibrary_BPC.hrfocus.model
{
   public class cls_TRTimeallw
    {
        public cls_TRTimeallw() { }

        public string company_code { get; set; }
        public string plantimeallw_code { get; set; }
        public int timeallw_no { get; set; }
        public int timeallw_time { get; set; }
        public string timeallw_type { get; set; }
        public double timeallw_normalday { get; set; }
        public double timeallw_offday { get; set; }
        public double timeallw_companyday { get; set; }
        public double timeallw_holiday { get; set; }
        public double timeallw_leaveday { get; set; }

        public string timeallw_method { get; set; }

        public string timeallw_timein { get; set; }
        public string timeallw_timeout { get; set; }
    }
}
