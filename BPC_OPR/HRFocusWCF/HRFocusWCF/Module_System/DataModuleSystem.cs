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
     [DataContract]
    public class InputSYSReason
    {
        [DataMember]
        public string reason_id { get; set; }
        [DataMember]
        public string reason_code { get; set; }
        [DataMember]
        public string reason_name_th { get; set; }
        [DataMember]
        public string reason_name_en { get; set; }
        [DataMember]
        public string reason_group { get; set; }
        [DataMember]
        public string created_by { get; set; }
        [DataMember]
        public string modified_by { get; set; }

    }
    [DataContract]
     public class InputMTCardtype
     {
         [DataMember]
         public string cardtype_id { get; set; }
         [DataMember]
         public string cardtype_code { get; set; }
         [DataMember]
         public string cardtype_name_th { get; set; }
         [DataMember]
         public string cardtype_name_en { get; set; }
         [DataMember]
         public string created_by { get; set; }
         [DataMember]
         public string modified_by { get; set; }

     }
}