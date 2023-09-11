using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary_BPC.hrfocus.model
{
    public class cls_TRPolcode
    {
        public cls_TRPolcode() { }
        public string company_code { get; set; }

        public int polcode_id { get; set; }
        public string codestructure_code { get; set; }
        public int polcode_lenght { get; set; }
        public string polcode_text { get; set; }
        public int polcode_order { get; set; }
        public string modified_by { get; set; }
        public DateTime modified_date { get; set; }


    }
}
