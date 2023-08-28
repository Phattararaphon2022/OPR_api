using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary_BPC.hrfocus.model
{
    public class cls_MTWorker
    {
        public cls_MTWorker()
        {
            //this.worker_code = "";
            //this.worker_initial = "";
            //this.worker_fname_th = "";
            //this.worker_lname_th = "";
            //this.worker_fname_en = "";
            //this.worker_lname_en = "";
            //this.initial_name_th = "";
            this.worker_pwd = "";
        }

        public string company_code { get; set; }
        public int worker_id { get; set; }
        public string worker_code { get; set; }
        public string worker_card { get; set; }
        public string worker_initial { get; set; }
        public string worker_fname_th { get; set; }
        public string worker_lname_th { get; set; }
        public string worker_fname_en { get; set; }
        public string worker_lname_en { get; set; }
        public string worker_type { get; set; }
        public string worker_gender { get; set; }
        public DateTime worker_birthdate { get; set; }
        public DateTime worker_hiredate { get; set; }
        public string worker_status { get; set; }
        public string religion_code { get; set; }
        public string blood_code { get; set; }
        public double worker_height { get; set; }
        public double worker_weight { get; set; }
        public DateTime worker_resigndate { get; set; }
        public bool worker_resignstatus { get; set; }
        public string worker_resignreason { get; set; }

        public bool worker_blackliststatus { get; set; }
        public string worker_blacklistreason { get; set; }
        public string worker_blacklistnote{ get; set; }

        public DateTime worker_probationdate { get; set; }
        public DateTime worker_probationenddate { get; set; }
        public double worker_probationday { get; set; }
        public double hrs_perday { get; set; }
        public string worker_taxmethod { get; set; }
        public string worker_pwd { get; set; }
        public bool self_admin { get; set; }
        public string worker_tel { get; set; }
        public string worker_email { get; set; }
        public string worker_line { get; set; }
        public string worker_facebook { get; set; }
        public string worker_military { get; set; }
        public string created_by { get; set; }
        public DateTime created_date { get; set; }
        public string modified_by { get; set; }
        public DateTime modified_date { get; set; }
        public bool flag { get; set; }


        //show only
        public string initial_name_th { get; set; }
        public string initial_name_en { get; set; }

        public string position_name_th { get; set; }
        public string position_name_en { get;set; }

        public double worker_age { get; set; }

        public List<cls_TRDocatt> reqdocatt_data { get; set; }

        public bool checkblacklist { get; set; }
        public bool checkhistory { get; set; }
        public int counthistory { get; set; }
        public bool checkcertificate { get; set; }
        public string nationality_code { get; set; }
        public string status { get; set; }

        public string worker_cardno { get; set; }
        public DateTime worker_cardnoissuedate { get; set; }
        public DateTime worker_cardnoexpiredate { get; set; }
        public string worker_socialno { get; set; }
        public DateTime worker_socialnoissuedate { get; set; }
        public DateTime worker_socialnoexpiredate { get; set; }
        public DateTime worker_socialsentdate { get; set; }
        public bool worker_socialnotsent { get; set; }


    }
}
