using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace BPC_OPR
{
    public class DataModuleAttendance
    {
    }
    [DataContract]
    public class InputMTYear
    {

        [DataMember]
        public string device_name { get; set; }
        [DataMember]
        public string ip { get; set; }
        [DataMember]
        public string username { get; set; }
        [DataMember]
        public string company_code { get; set; }
        [DataMember]
        public string year_id { get; set; }
        [DataMember]
        public string year_code { get; set; }
        [DataMember]
        public string year_name_th { get; set; }
        [DataMember]
        public string year_name_en { get; set; }
        [DataMember]
        public string year_fromdate { get; set; }
        [DataMember]
        public string year_todate { get; set; }
        [DataMember]
        public string year_group { get; set; }
        [DataMember]
        public string modified_by { get; set; }
        [DataMember]
        public DateTime modified_date { get; set; }
        [DataMember]
        public bool flag { get; set; }
    }

}