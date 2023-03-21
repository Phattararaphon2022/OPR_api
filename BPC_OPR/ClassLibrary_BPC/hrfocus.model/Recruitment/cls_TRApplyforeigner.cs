using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary_BPC.hrfocus.model.Recruitment
{
    public class cls_TRApplyforeigner
   {
        public cls_TRApplyforeigner() { }

        public string company_code { get; set; }
        public string applywork_code { get; set; }

        public int foreigner_id { get; set; }
        public string passport_no { get; set; }
        public DateTime passport_issue { get; set; }
        public DateTime passport_expire { get; set; }
        public string visa_no { get; set; }
        public DateTime visa_issue { get; set; }
        public DateTime visa_expire { get; set; }
        public string workpermit_no { get; set; }
        public string workpermit_by { get; set; }
        public DateTime workpermit_issue { get; set; }
        public DateTime workpermit_expire { get; set; }
        public DateTime entry_date { get; set; }
        public string certificate_no { get; set; }
        public DateTime certificate_expire { get; set; }
        public string otherdoc_no { get; set; }
        public DateTime otherdoc_expire { get; set; }

        public string created_by { get; set; }
        public DateTime created_date { get; set; }


        public string modified_by { get; set; }
        public DateTime modified_date { get; set; }

        public bool flag { get; set; }
    }
}
