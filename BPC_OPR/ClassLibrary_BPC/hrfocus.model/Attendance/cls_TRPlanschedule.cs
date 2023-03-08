using System;

namespace ClassLibrary_BPC.hrfocus.model
{
    public class cls_TRPlanschedule
    {
        public cls_TRPlanschedule() { }

        public string company_code { get; set; }
        public string planshift_code { get; set; }

        public string planschedule_fromdate { get; set; }
        public string planschedule_todate { get; set; }

        public string shift_code { get; set; }

        public string planschedule_sun_off { get; set; }
        public string planschedule_mon_off { get; set; }
        public string planschedule_tue_off { get; set; }
        public string planschedule_wed_off { get; set; }
        public string planschedule_thu_off { get; set; }
        public string planschedule_fri_off { get; set; }
        public string planschedule_sat_off { get; set; }

        public string created_by { get; set; }
        public DateTime created_date { get; set; }

        public string modified_by { get; set; }
        public DateTime modified_date { get; set; }

        public bool flag { get; set; }
    }
}
