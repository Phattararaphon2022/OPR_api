using System;

namespace ClassLibrary_BPC.hrfocus.model
{
    public class cls_TRAccount
    {
        public cls_TRAccount() { }
        public string company_code { get; set; }
        public string account_user { get; set; }
        public string account_type { get; set; }
        public string worker_code { get; set; }
        public string empposition_position { get; set; }
        public int position_level { get; set; }
        public string workflow_code { get; set; }
        public string workflow_type { get; set; }
    }
}
