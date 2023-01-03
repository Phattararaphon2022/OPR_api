using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace BPC_OPR
{
    public class DataModuleSystem
    {
    }

    
    [DataContract]
    public class InputMTBank
    {
        [DataMember]
        public string bank_id { get; set; }
        [DataMember]
        public string bank_code { get; set; }
        [DataMember]
        public string bank_name_th { get; set; }
        [DataMember]
        public string bank_name_en { get; set; }   
        [DataMember]
        public string modified_by { get; set; }

    }
}