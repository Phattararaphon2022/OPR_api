using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary_BPC.hrfocus.model
     {
    public class cls_MTComaddress
    {
        public cls_MTComaddress() { }

        public string company_code { get; set; }
        public string combranch_code { get; set; }

        public string comaddress_type { get; set; }
        public string comaddress_no { get; set; }

        public string comaddress_moo { get; set; }
        public string comaddress_soi { get; set; }
        public string comaddress_road { get; set; }
        public string comaddress_tambon { get; set; }
        public string comaddress_amphur { get; set; }
        public string comaddress_zipcode { get; set; }
        public string comaddress_tel { get; set; }
        public string comaddress_email { get; set; }
        public string comaddress_line { get; set; }
        public string comaddress_facebook { get; set; }
        public string province_code { get; set; }
                
	    public string created_by { get;set; }
        public DateTime created_date { get; set; }
	    public string modified_by { get;set; }
        public DateTime modified_date { get; set; }

        public bool flag { get; set; }

    }
}
//{
//    public class cls_MTComaddress
//    {
//        public cls_MTComaddress()
//        {
//            this.comaddress_code = "";
//            this.comaddress_no = "";
//            this.comaddress_moo = "";
//            this.comaddress_soi = "";
//            this.comaddress_road = "";
//            this.comaddress_tambon = "";
//            this.comaddress_amphur = "";
//            this.comaddress_zipcode = "";
//            this.comaddress_tel = "";
//            this.comaddress_email = "";
//            this.comaddress_line = "";
//            this.comaddress_facebook = "";
//            this.comaddress_province = "";
//        }

//        public int comaddress_id { get; set; }
//        public string comaddress_code { get; set; }
//        public string comaddress_no { get; set; }
//        public string comaddress_moo { get; set; }
//        public string comaddress_soi { get; set; }
//        public string comaddress_road { get; set; }
//        public string comaddress_tambon { get; set; }
//        public string comaddress_amphur { get; set; }
//        public string comaddress_zipcode { get; set; }
//        public string comaddress_tel { get; set; }
//        public string comaddress_email { get; set; }
//        public string comaddress_line { get; set; }
//        public string comaddress_facebook { get; set; }
//        public string comaddress_province { get; set; }

//        public string created_by { get;set; }
//        public DateTime created_date { get; set; }
	    
//        public string modified_by { get;set; }
//        public DateTime modified_date { get; set; }

//        public bool flag { get; set; }

//    }
//}
