using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace HRFocusWCF
{
    public class cls_DCProject
    {
    }

    [DataContract]
    public class FillterProject
    {
        [DataMember]
        public string project_type { get; set; }
        [DataMember]
        public string username { get; set; }
    }


    [DataContract]
    public class InputMTProject
    {
        [DataMember]
        public string project_id { get; set; }
        [DataMember]
        public string project_code { get; set; }
        [DataMember]
        public string project_name_th { get; set; }
        [DataMember]
        public string project_name_en { get; set; }
        [DataMember]
        public string project_name_short { get; set; }
        [DataMember]
        public string project_type { get; set; }
        [DataMember]
        public string modified_by { get; set; }
        
    }

    [DataContract]
    public class InputTRPCost
    {
        [DataMember]
        public string pcost_id { get; set; }
        [DataMember]
        public string pcost_code { get; set; }
        [DataMember]
        public string pcost_amount { get; set; }
        [DataMember]
        public string pcost_type { get; set; }
        [DataMember]
        public string pcost_start { get; set; }
        [DataMember]
        public string pcost_end { get; set; }
        [DataMember]
        public string pcost_allwcode { get; set; }
        [DataMember]
        public string project_code { get; set; }
        [DataMember]
        public string modified_by { get; set; }        
    }

    

}