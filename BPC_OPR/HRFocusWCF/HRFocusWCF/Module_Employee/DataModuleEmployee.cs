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

    public class InputMTWorker
    {
        [DataMember]
        public string company_code { get; set; }
        [DataMember]
        public int worker_id { get; set; }
        [DataMember]
        public string worker_code { get; set; }
        [DataMember]
        public string worker_card { get; set; }
        [DataMember]
        public string worker_initial { get; set; }
        [DataMember]
        public string worker_fname_th { get; set; }
        [DataMember]
        public string worker_lname_th { get; set; }
        [DataMember]
        public string worker_fname_en { get; set; }
        [DataMember]
        public string worker_lname_en { get; set; }

        [DataMember]
        public string worker_type { get; set; }
        [DataMember]
        public string worker_gender { get; set; }
        [DataMember]
        public string worker_birthdate { get; set; }
        [DataMember]
        public string worker_hiredate { get; set; }
        [DataMember]
        public string worker_status { get; set; }
        [DataMember]
        public string religion_code { get; set; }
        [DataMember]
        public string blood_code { get; set; }
        [DataMember]
        public double worker_height { get; set; }
        [DataMember]
        public double worker_weight { get; set; }
        [DataMember]
        public string worker_resigndate { get; set; }
        [DataMember]
        public bool worker_resignstatus { get; set; }
        [DataMember]
        public string worker_resignreason { get; set; }

        [DataMember]
        public string worker_probationdate { get; set; }
        [DataMember]
        public string worker_probationenddate { get; set; }
        [DataMember]
        public double worker_probationday { get; set; }
        [DataMember]
        public double hrs_perday { get; set; }

        [DataMember]
        public string worker_taxmethod { get; set; }

        //-- Transaction
        [DataMember]
        public string card_data { get; set; }
        [DataMember]
        public string reduce_data { get; set; }
        [DataMember]
        public string salary_data { get; set; }
        [DataMember]
        public string family_data { get; set; }
        [DataMember]
        public string dep_data { get; set; }
        //--

        [DataMember]
        public bool self_admin { get; set; }

        [DataMember]
        public string modified_by { get; set; }
        [DataMember]
        public DateTime modified_date { get; set; }
        [DataMember]
        public int index { get; set; }

        [DataMember]
        public string worker_pwd { get; set; }
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

    public class InputTRAddress
    {
        [DataMember]
        public string company_code { get; set; }
        [DataMember]
        public string worker_code { get; set; }
        [DataMember]
        public string address_id { get; set; }
        [DataMember]
        public string address_type { get; set; }
        [DataMember]
        public string address_no { get; set; }
        [DataMember]
        public string address_moo { get; set; }
        [DataMember]
        public string address_soi { get; set; }
        [DataMember]
        public string address_road { get; set; }
        [DataMember]
        public string address_tambon { get; set; }
        [DataMember]
        public string address_amphur { get; set; }
        [DataMember]
        public string province_code { get; set; }
        [DataMember]
        public string address_zipcode { get; set; }
        [DataMember]
        public string address_tel { get; set; }
        [DataMember]
        public string address_email { get; set; }
        [DataMember]
        public string address_line { get; set; }
        [DataMember]
        public string address_facebook { get; set; }
        [DataMember]
        public string modified_by { get; set; }
        [DataMember]
        public string modified_date { get; set; }
    }
}