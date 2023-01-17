using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace BPC_OPR
{
    public class DataModuleEmployee
    {
    }

    [DataContract]
    public class InputMTLocation
    {
        [DataMember]
        public string location_id { get; set; }
        [DataMember]
        public string location_code { get; set; }
        [DataMember]
        public string location_name_th { get; set; }
        [DataMember]
        public string location_name_en { get; set; }
        [DataMember]
        public string location_detail { get; set; }
        [DataMember]
        public string modified_by { get; set; }

    }

    [DataContract]
    public class InputMTDep
    {
        [DataMember]
        public string dep_id { get; set; }
        [DataMember]
        public string dep_code { get; set; }
        [DataMember]
        public string dep_name_th { get; set; }
        [DataMember]
        public string dep_name_en { get; set; }
        [DataMember]
        public string dep_parent { get; set; }
        [DataMember]
        public string dep_level { get; set; }
        [DataMember]
        public string company_code { get; set; }
        [DataMember]
        public string modified_by { get; set; }

    }

    [DataContract]
    public class InputMTPosition
    {
        [DataMember]
        public string position_id { get; set; }
        [DataMember]
        public string position_code { get; set; }
        [DataMember]
        public string position_name_th { get; set; }
        [DataMember]
        public string position_name_en { get; set; }
        [DataMember]
        public string company_code { get; set; }
        [DataMember]
        public string modified_by { get; set; }

    }

    [DataContract]
    public class InputMTInitial
    {
        [DataMember]
        public string initial_id { get; set; }
        [DataMember]
        public string initial_code { get; set; }
        [DataMember]
        public string initial_name_th { get; set; }
        [DataMember]
        public string initial_name_en { get; set; }
        [DataMember]
        public string modified_by { get; set; }
    }

    [DataContract]
    public class InputMTGroup
    {
        [DataMember]
        public string group_id { get; set; }
        [DataMember]
        public string group_code { get; set; }
        [DataMember]
        public string group_name_th { get; set; }
        [DataMember]
        public string group_name_en { get; set; }
        [DataMember]
        public string company_code { get; set; }
        [DataMember]
        public string modified_by { get; set; }
    }

    [DataContract]
    public class InputMTType
    {
        [DataMember]
        public string type_id { get; set; }
        [DataMember]
        public string type_code { get; set; }
        [DataMember]
        public string type_name_th { get; set; }
        [DataMember]
        public string type_name_en { get; set; }
        [DataMember]
        public string modified_by { get; set; }
    }

    [DataContract]
    public class InputMTStatus
    {
        [DataMember]
        public string status_id { get; set; }
        [DataMember]
        public string status_code { get; set; }
        [DataMember]
        public string status_name_th { get; set; }
        [DataMember]
        public string status_name_en { get; set; }
        [DataMember]
        public string modified_by { get; set; }
    }
}