using System;
namespace ClassLibrary_BPC.hrfocus.model
{
    public class cls_TRApprove
    {
        public cls_TRApprove() { }
        
        public string company_code { get; set; }
        public string approve_code { get; set; }
        public string workflow_type { get; set; }
        public string approve_by { get; set; }
        public DateTime approve_date { get; set; }

        public string approve_status { get; set; }

        public string approve_note { get; set; }
        
    }
}
