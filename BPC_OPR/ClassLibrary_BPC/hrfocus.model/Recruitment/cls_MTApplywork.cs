using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary_BPC.hrfocus.model.Recruitment
{
    public class cls_MTApplywork
     {
        public cls_MTApplywork() { }
            

        public string company_code { get; set; }
        public int applywork_id { get; set; }
        public string applywork_code { get; set; }
        public string applywork_initial { get; set; }
        public string applywork_fname_th { get; set; }
        public string applywork_lname_th { get; set; }
        public string applywork_fname_en { get; set; }
        public string applywork_lname_en { get; set; }
        public DateTime applywork_birthdate { get; set; }
        public DateTime applywork_startdate { get; set; }
        public string province_code { get; set; }
        public string bloodtype_code { get; set; }
        public double applywork_height { get; set; }
        public double applywork_weight { get; set; }
        public string created_by { get; set; }
        public DateTime created_date { get; set; }
        public string modified_by { get; set; }
        public DateTime modified_date { get; set; }
        public bool flag { get; set; }


        //show only
        //public string initial_name_th { get; set; }
        //public string initial_name_en { get; set; }


    }
}
