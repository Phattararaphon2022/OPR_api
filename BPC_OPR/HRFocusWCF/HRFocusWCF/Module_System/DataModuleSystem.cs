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
    public class InputMTEmpId
    {
        [DataMember]
        public string empid_id { get; set; }
        [DataMember]
        public string empid_code { get; set; }
        [DataMember]
        public string empid_name_th { get; set; }
        [DataMember]
        public string empid_name_en { get; set; }
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
    [DataContract]
    public class InputMTFamily
    {
        [DataMember]
        public string family_id { get; set; }
        [DataMember]
        public string family_code { get; set; }
        [DataMember]
        public string family_name_th { get; set; }
        [DataMember]
        public string family_name_en { get; set; }
        [DataMember]
        public string created_by { get; set; }
        [DataMember]
        public string modified_by { get; set; }

    }

    [DataContract]
    public class InputMTLevel
    {
        [DataMember]
        public string level_id { get; set; }
        [DataMember]
        public string level_code { get; set; }
        [DataMember]
        public string level_name_th { get; set; }
        [DataMember]
        public string level_name_en { get; set; }
        [DataMember]
        public string created_by { get; set; }
        [DataMember]
        public string company_code { get; set; }
        [DataMember]
        public string modified_by { get; set; }



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
        public string created_by { get; set; }
        [DataMember]
        public string location_detail { get; set; }
        [DataMember]
        public string modified_by { get; set; }



    }
    [DataContract]
    public class InputMTReduce
    {
        [DataMember]
        public int reduce_id { get; set; }
        [DataMember]
        public string reduce_code { get; set; }
        [DataMember]
        public string reduce_name_th { get; set; }
        [DataMember]
        public string reduce_name_en { get; set; }

        [DataMember]
        public double reduce_amount { get; set; }
        [DataMember]
        public double reduce_percent { get; set; }
        [DataMember]
        public double reduce_percent_max { get; set; }

        [DataMember]
        public string modified_by { get; set; }
        [DataMember]
        public DateTime modified_date { get; set; }

        [DataMember]
        public int index { get; set; }
    }


    [DataContract]
    public class InputMTReligion
    {
        [DataMember]
        public string religion_id { get; set; }
        [DataMember]
        public string religion_code { get; set; }
        [DataMember]
        public string religion_name_th { get; set; }
        [DataMember]
        public string religion_name_en { get; set; }
        [DataMember]
        public string modified_by { get; set; }



    }

    [DataContract]
    public class InputMTProvince
    {
        [DataMember]
        public string province_id { get; set; }
        [DataMember]
        public string province_code { get; set; }
        [DataMember]
        public string province_name_th { get; set; }
        [DataMember]
        public string province_name_en { get; set; }
        [DataMember]
        public string modified_by { get; set; }



    }
    [DataContract]
    public class InputMTAddresstype
    {
        [DataMember]
        public string addresstype_id { get; set; }
        [DataMember]
        public string addresstype_code { get; set; }
        [DataMember]
        public string addresstype_name_th { get; set; }
        [DataMember]
        public string addresstype_name_en { get; set; }
        [DataMember]
        public string modified_by { get; set; }



    }
    [DataContract]
    public class InputMTEthnicity
    {
        [DataMember]
        public string ethnicity_id { get; set; }
        [DataMember]
        public string ethnicity_code { get; set; }
        [DataMember]
        public string ethnicity_name_th { get; set; }
        [DataMember]
        public string ethnicity_name_en { get; set; }
        [DataMember]
        public string modified_by { get; set; }



    }

    [DataContract]
    public class InputMTBloodtype
    {
        [DataMember]
        public string bloodtype_id { get; set; }
        [DataMember]
        public string bloodtype_code { get; set; }
        [DataMember]
        public string bloodtype_name_th { get; set; }
        [DataMember]
        public string bloodtype_name_en { get; set; }
        [DataMember]
        public string modified_by { get; set; }




    }
    [DataContract]
    public class InputMTHospital
    {
        [DataMember]
        public string hospital_id { get; set; }
        [DataMember]
        public string hospital_code { get; set; }
        [DataMember]
        public string hospital_name_th { get; set; }
        [DataMember]
        public string hospital_name_en { get; set; }
        [DataMember]
        public string modified_by { get; set; }

    }
}

 
