using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary_BPC.hrfocus.model
{
    public class cls_TRFamily
    {
        public cls_TRFamily() { }

        public string company_code { get; set; }
        public string worker_code { get; set; }

        public int family_id { get; set; }
        public string family_code { get; set; }

        public string family_fname_th { get; set; }
        public string family_lname_th { get; set; }
        public string family_fname_en { get; set; }
        public string family_lname_en { get; set; }

        public DateTime family_birthdate { get; set; }

        public string family_type { get; set; }

        public string created_by { get; set; }
        public DateTime created_date { get; set; }


        public string modified_by { get; set; }
        public DateTime modified_date { get; set; }

        public bool flag { get; set; }

        public string family_occupation { get; set; }
        public string family_tel { get; set; }

    }
}

