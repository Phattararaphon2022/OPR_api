using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using ClassLibrary_BPC.hrfocus.model;

namespace BPC_OPR
{
    public class DataModuleEmployee
    {
    }

    [DataContract]
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
        [DataMember]
        public string worker_tel { get; set; }
        [DataMember]
        public string worker_email { get; set; }
        [DataMember]
        public string worker_line { get; set; }
        [DataMember]
        public string worker_facebook { get; set; }
        [DataMember]
        public string worker_military { get; set; }


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


    //Fillter All
    [DataContract]
    public class FillterWorker
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
        public string worker_code { get; set; }
        [DataMember]
        public DateTime paydate { get; set; }
        [DataMember]
        public string level_code { get; set; }
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
        public string parent_level { get; set; }
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
        public int position_level { get; set; }
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

    [DataContract]
    public class InputTREmpLocation
    {
        [DataMember]
        public string company_code { get; set; }
        [DataMember]
        public string worker_code { get; set; }
        [DataMember]
        public string location_code { get; set; }
        [DataMember]
        public string emplocation_startdate { get; set; }
        [DataMember]
        public string emplocation_enddate { get; set; }
        [DataMember]
        public string emplocation_note { get; set; }
        [DataMember]
        public string modified_by { get; set; }
        [DataMember]
        public string modified_date { get; set; }
    }

    [DataContract]
    public class InputTREmpBranch
    {
        [DataMember]
        public string company_code { get; set; }
        [DataMember]
        public string worker_code { get; set; }
        [DataMember]
        public string branch_code { get; set; }
        [DataMember]
        public string empbranch_startdate { get; set; }
        [DataMember]
        public string empbranch_enddate { get; set; }
        [DataMember]
        public string empbranch_note { get; set; }
        [DataMember]
        public string modified_by { get; set; }
        [DataMember]
        public string modified_date { get; set; }
    }

    [DataContract]
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

    [DataContract]
    public class InputTRCard
    {
        [DataMember]
        public string company_code { get; set; }
        [DataMember]
        public string worker_code { get; set; }

        [DataMember]
        public int card_id { get; set; }
        [DataMember]
        public string card_code { get; set; }
        [DataMember]
        public string card_type { get; set; }
        [DataMember]
        public string card_issue { get; set; }
        [DataMember]
        public string card_expire { get; set; }
        [DataMember]
        public string modified_by { get; set; }
        [DataMember]
        public DateTime modified_date { get; set; }
        [DataMember]
        public int index { get; set; }
    }

    [DataContract]
    public class InputTRBank
    {
        [DataMember]
        public string company_code { get; set; }
        [DataMember]
        public string worker_code { get; set; }
        [DataMember]
        public int bank_id { get; set; }
        [DataMember]
        public string bank_code { get; set; }
        [DataMember]
        public string bank_account { get; set; }
        [DataMember]
        public double bank_percent { get; set; }
        [DataMember]
        public double bank_cashpercent { get; set; }
        [DataMember]
        public string modified_by { get; set; }
        [DataMember]
        public DateTime modified_date { get; set; }
        [DataMember]
        public double bank_bankname { get; set; }
        [DataMember]
        public int index { get; set; }


    }

    [DataContract]
    public class InputTREmpfamily
    {
        [DataMember]
        public int family_id { get; set; }
        [DataMember]
        public string family_code { get; set; }
        [DataMember]
        public string family_type { get; set; }
        [DataMember]
        public string family_fname_th { get; set; }
        [DataMember]
        public string family_lname_th { get; set; }
        [DataMember]
        public string family_fname_en { get; set; }
        [DataMember]
        public string family_lname_en { get; set; }
        [DataMember]
        public string family_birthdate { get; set; }

        [DataMember]
        public string company_code { get; set; }
        [DataMember]
        public string worker_code { get; set; }

        [DataMember]
        public string modified_by { get; set; }
        [DataMember]
        public DateTime modified_date { get; set; }

        [DataMember]
        public int index { get; set; }
    }

    [DataContract]
    public class InputTREmpHospital
    {
        [DataMember]
        public int emphospital_id { get; set; }
        [DataMember]
        public string emphospital_code { get; set; }
        [DataMember]
        public string emphospital_date { get; set; }

        [DataMember]
        public string company_code { get; set; }
        [DataMember]
        public string worker_code { get; set; }

        [DataMember]
        public string modified_by { get; set; }
        [DataMember]
        public DateTime modified_date { get; set; }

        [DataMember]
        public int index { get; set; }
    }
    [DataContract]

    public class InputTREmpForeigner
    {
        [DataMember]
        public int foreigner_id { get; set; }
        [DataMember]
        public string passport_no { get; set; }
        [DataMember]
        public string passport_issue { get; set; }
        [DataMember]
        public string passport_expire { get; set; }
        [DataMember]
        public string visa_no { get; set; }
        [DataMember]
        public string visa_issue { get; set; }
        [DataMember]
        public string visa_expire { get; set; }
        [DataMember]
        public string workpermit_no { get; set; }
        [DataMember]
        public string workpermit_by { get; set; }
        [DataMember]
        public string workpermit_issue { get; set; }
        [DataMember]
        public string workpermit_expire { get; set; }
        [DataMember]
        public string entry_date { get; set; }
        [DataMember]
        public string certificate_no { get; set; }
        [DataMember]
        public string certificate_expire { get; set; }
        [DataMember]
        public string otherdoc_no { get; set; }
        [DataMember]
        public string otherdoc_expire { get; set; }
        [DataMember]
        public string company_code { get; set; }
        [DataMember]
        public string worker_code { get; set; }

        [DataMember]
        public string modified_by { get; set; }
        [DataMember]
        public DateTime modified_date { get; set; }
        [DataMember]
        public int index { get; set; }
    }

    [DataContract]
    public class InputTREmpDep
    {
        [DataMember]
        public int empdep_id { get; set; }
        [DataMember]
        public string empdep_date { get; set; }
        [DataMember]
        public string empdep_level01 { get; set; }
        [DataMember]
        public string empdep_level02 { get; set; }
        [DataMember]
        public string empdep_level03 { get; set; }
        [DataMember]
        public string empdep_level04 { get; set; }
        [DataMember]
        public string empdep_level05 { get; set; }
        [DataMember]
        public string empdep_level06 { get; set; }
        [DataMember]
        public string empdep_level07 { get; set; }
        [DataMember]
        public string empdep_level08 { get; set; }
        [DataMember]
        public string empdep_level09 { get; set; }
        [DataMember]
        public string empdep_level10 { get; set; }
        [DataMember]
        public string empdep_reason { get; set; }
        [DataMember]
        public string company_code { get; set; }
        [DataMember]
        public string worker_code { get; set; }

        [DataMember]
        public string modified_by { get; set; }
        [DataMember]
        public DateTime modified_date { get; set; }
        [DataMember]
        public int index { get; set; }
    }

    [DataContract]
    public class InputTREmpPosition
    {
        [DataMember]
        public int empposition_id { get; set; }
        [DataMember]
        public string empposition_date { get; set; }
        [DataMember]
        public string empposition_position { get; set; }
        [DataMember]
        public string empposition_reason { get; set; }
        [DataMember]
        public string company_code { get; set; }
        [DataMember]
        public string worker_code { get; set; }

        [DataMember]
        public string modified_by { get; set; }
        [DataMember]
        public DateTime modified_date { get; set; }
        [DataMember]
        public int index { get; set; }
    }

    [DataContract]
    public class InputTREmpGroup
    {
        [DataMember]
        public int empgroup_id { get; set; }
        [DataMember]
        public string empgroup_code { get; set; }
        [DataMember]
        public string empgroup_date { get; set; }
        [DataMember]
        public string company_code { get; set; }
        [DataMember]
        public string worker_code { get; set; }

        [DataMember]
        public string modified_by { get; set; }
        [DataMember]
        public DateTime modified_date { get; set; }
        [DataMember]
        public int index { get; set; }
    }

    [DataContract]
    public class InputTREmpEducation
    {
        [DataMember]
        public string company_code { get; set; }
        [DataMember]
        public string worker_code { get; set; }
        [DataMember]
        public int empeducation_no { get; set; }
        [DataMember]
        public string empeducation_gpa { get; set; }
        [DataMember]
        public string empeducation_start { get; set; }
        [DataMember]
        public string empeducation_finish { get; set; }
        [DataMember]
        public string institute_code { get; set; }
        [DataMember]
        public string faculty_code { get; set; }
        [DataMember]
        public string major_code { get; set; }
        [DataMember]
        public string qualification_code { get; set; }
        [DataMember]
        public string modified_by { get; set; }
        [DataMember]
        public string modified_date { get; set; }
    }

    [DataContract]
    public class InputTREmpTraining
    {
        [DataMember]
        public string company_code { get; set; }
        [DataMember]
        public string worker_code { get; set; }
        [DataMember]
        public int emptraining_no { get; set; }
        [DataMember]
        public DateTime emptraining_start { get; set; }
        [DataMember]
        public DateTime emptraining_finish { get; set; }
        [DataMember]
        public string emptraining_status { get; set; }
        [DataMember]
        public double emptraining_hours { get; set; }
        [DataMember]
        public double emptraining_cost { get; set; }
        [DataMember]
        public string emptraining_note { get; set; }
        [DataMember]
        public string institute_code { get; set; }
        [DataMember]
        public string institute_other { get; set; }
        [DataMember]
        public string course_code { get; set; }
        [DataMember]
        public string course_other { get; set; }
        [DataMember]
        public string modified_by { get; set; }
        [DataMember]
        public string modified_date { get; set; }
    }

    [DataContract]
    public class InputTREmpAssessment
    {
        [DataMember]
        public string company_code { get; set; }
        [DataMember]
        public string worker_code { get; set; }
        [DataMember]
        public int empassessment_id { get; set; }
        [DataMember]
        public string empassessment_location { get; set; }
        [DataMember]
        public string empassessment_topic { get; set; }
        [DataMember]
        public DateTime empassessment_fromdate { get; set; }
        [DataMember]
        public DateTime empassessment_todate { get; set; }
        [DataMember]
        public double empassessment_count { get; set; }
        [DataMember]
        public string empassessment_result { get; set; }
        [DataMember]
        public string modified_by { get; set; }
        [DataMember]
        public string modified_date { get; set; }
    }

    [DataContract]
    public class InputTREmpCriminal
    {
        [DataMember]
        public string company_code { get; set; }
        [DataMember]
        public string worker_code { get; set; }
        [DataMember]
        public int empcriminal_id { get; set; }
        [DataMember]
        public string empcriminal_location { get; set; }
        [DataMember]
        public DateTime empcriminal_fromdate { get; set; }
        [DataMember]
        public DateTime empcriminal_todate { get; set; }
        [DataMember]
        public double empcriminal_count { get; set; }
        [DataMember]
        public string empcriminal_result { get; set; }
        [DataMember]
        public string modified_by { get; set; }
        [DataMember]
        public string modified_date { get; set; }
    }

    [DataContract]
    public class InputTREmpSalary
    {
        [DataMember]
        public int empsalary_id { get; set; }
        [DataMember]
        public string company_code { get; set; }
        [DataMember]
        public string worker_code { get; set; }
        [DataMember]
        public double empsalary_amount { get; set; }
        [DataMember]
        public string empsalary_date { get; set; }
        [DataMember]
        public string empsalary_reason { get; set; }
        [DataMember]
        public double empsalary_incamount { get; set; }
        [DataMember]
        public double empsalary_incpercent { get; set; }
        [DataMember]
        public string modified_by { get; set; }
        [DataMember]
        public DateTime modified_date { get; set; }
        [DataMember]
        public int index { get; set; }

        [DataMember]
        public string emp_data { get; set; }
    }

    [DataContract]
    public class InputTREmpProvident
    {
        [DataMember]
        public string company_code { get; set; }
        [DataMember]
        public string worker_code { get; set; }
        [DataMember]
        public string provident_code { get; set; }
        [DataMember]
        public string empprovident_card { get; set; }
        [DataMember]
        public string empprovident_entry { get; set; }
        [DataMember]
        public string empprovident_start { get; set; }
        [DataMember]
        public string empprovident_end { get; set; }
        [DataMember]
        public string modified_by { get; set; }
        [DataMember]
        public string modified_date { get; set; }
    }

    [DataContract]
    public class InputTREmpBenefit
    {
        [DataMember]
        public int empbenefit_id { get; set; }
        [DataMember]
        public string company_code { get; set; }
        [DataMember]
        public string worker_code { get; set; }
        [DataMember]
        public string item_code { get; set; }
        [DataMember]
        public double empbenefit_amount { get; set; }
        [DataMember]
        public string empbenefit_startdate { get; set; }
        [DataMember]
        public string empbenefit_enddate { get; set; }
        [DataMember]
        public string empbenefit_reason { get; set; }
        [DataMember]
        public string empbenefit_note { get; set; }
        [DataMember]
        public string empbenefit_paytype { get; set; }
        [DataMember]
        public bool empbenefit_break { get; set; }
        [DataMember]
        public string empbenefit_breakreason { get; set; }

        [DataMember]
        public string empbenefit_conditionpay { get; set; }

        [DataMember]
        public string empbenefit_payfirst { get; set; }

        [DataMember]
        public string modified_by { get; set; }
        [DataMember]
        public DateTime modified_date { get; set; }
        [DataMember]
        public int index { get; set; }

        [DataMember]
        public string emp_data { get; set; }
    }

    [DataContract]
    public class InputTREmpReduce
    {
        [DataMember]
        public int empreduce_id { get; set; }
        [DataMember]
        public string company_code { get; set; }
        [DataMember]
        public string worker_code { get; set; }
        [DataMember]
        public string reduce_type { get; set; }
        [DataMember]
        public double empreduce_amount { get; set; }
        [DataMember]
        public string modified_by { get; set; }
        [DataMember]
        public DateTime modified_date { get; set; }
        [DataMember]
        public int index { get; set; }
    }

    [DataContract]
    public class InputTREmpSupply
    {
        [DataMember]
        public int empsupply_id { get; set; }
        [DataMember]
        public string empsupply_code { get; set; }
        [DataMember]
        public string empsupply_qauntity { get; set; }
        [DataMember]
        public string empsupply_issuedate { get; set; }
        [DataMember]
        public string empsupply_note { get; set; }
        [DataMember]
        public string empsupply_returndate { get; set; }
        [DataMember]
        public string empsupply_returnstatus { get; set; }
        [DataMember]
        public string company_code { get; set; }
        [DataMember]
        public string worker_code { get; set; }

        [DataMember]
        public string modified_by { get; set; }
        [DataMember]
        public DateTime modified_date { get; set; }
        [DataMember]
        public int index { get; set; }
    }

    [DataContract]
    public class InputTREmpUniform
    {
        [DataMember]
        public int empuniform_id { get; set; }
        [DataMember]
        public string empuniform_code { get; set; }
        [DataMember]
        public string empuniform_qauntity { get; set; }
        [DataMember]
        public string empuniform_amount { get; set; }
        [DataMember]
        public string empuniform_issuedate { get; set; }
        [DataMember]
        public string empuniform_note { get; set; }
        [DataMember]
        public string company_code { get; set; }
        [DataMember]
        public string worker_code { get; set; }

        [DataMember]
        public string modified_by { get; set; }
        [DataMember]
        public DateTime modified_date { get; set; }
        [DataMember]
        public int index { get; set; }
    }

    [DataContract]
    public class InputWorkerTransaction
    {
        [DataMember]
        public string worker_code { get; set; }
        [DataMember]
        public string company_code { get; set; }
        [DataMember]
        public string transaction_data { get; set; }
        [DataMember]
        public string modified_by { get; set; }
    }

    //Set batch
    [DataContract]
    public class InputSetPosition
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
        public string empposition_position { get; set; }
        [DataMember]
        public string empposition_reason { get; set; }
        [DataMember]
        public string empposition_date { get; set; }
        [DataMember]
        public List<cls_MTWorker> emp_data { get; set; }
        [DataMember]
        public string modified_by { get; set; }
        [DataMember]
        public bool flag { get; set; }
    }

    [DataContract]
    public class InputSetDep
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
        public string empdep_date { get; set; }
        [DataMember]
        public string empdep_level01 { get; set; }
        [DataMember]
        public string empdep_level02 { get; set; }
        [DataMember]
        public string empdep_level03 { get; set; }
        [DataMember]
        public string empdep_level04 { get; set; }
        [DataMember]
        public string empdep_level05 { get; set; }
        [DataMember]
        public string empdep_level06 { get; set; }
        [DataMember]
        public string empdep_level07 { get; set; }
        [DataMember]
        public string empdep_level08 { get; set; }
        [DataMember]
        public string empdep_level09 { get; set; }
        [DataMember]
        public string empdep_level10 { get; set; }
        [DataMember]
        public string empdep_reason { get; set; }
        [DataMember]
        public List<cls_MTWorker> emp_data { get; set; }
        [DataMember]
        public string modified_by { get; set; }
        [DataMember]
        public bool flag { get; set; }
    }

    [DataContract]
    public class InputSetGroup
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
        public string empgroup_code { get; set; }
        [DataMember]
        public string empgroup_date { get; set; }
        [DataMember]
        public List<cls_MTWorker> emp_data { get; set; }
        [DataMember]
        public string modified_by { get; set; }
        [DataMember]
        public bool flag { get; set; }
    }

    [DataContract]
    public class InputSetLocation
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
        public string location_code { get; set; }
        [DataMember]
        public string emplocation_startdate { get; set; }
        [DataMember]
        public string emplocation_enddate { get; set; }
        [DataMember]
        public string emplocation_note { get; set; }
        [DataMember]
        public List<cls_MTWorker> emp_data { get; set; }
        [DataMember]
        public string modified_by { get; set; }
        [DataMember]
        public bool flag { get; set; }
    }

    [DataContract]
    public class InputSetSalary
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
        public double empsalary_amount { get; set; }
        [DataMember]
        public double empsalary_incamount { get; set; }
        [DataMember]
        public double empsalary_incpercent { get; set; }
        [DataMember]
        public string empsalary_reason { get; set; }
        [DataMember]
        public string empsalary_date { get; set; }
        [DataMember]
        public List<cls_MTWorker> emp_data { get; set; }
        [DataMember]
        public string modified_by { get; set; }
        [DataMember]
        public bool flag { get; set; }
    }

    [DataContract]
    public class InputSetProvident
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
        public string provident_code { get; set; }
        [DataMember]
        public string empprovident_card { get; set; }
        [DataMember]
        public string empprovident_entry { get; set; }
        [DataMember]
        public string empprovident_start { get; set; }
        [DataMember]
        public string empprovident_end { get; set; }
        [DataMember]
        public List<cls_MTWorker> emp_data { get; set; }
        [DataMember]
        public string modified_by { get; set; }
        [DataMember]
        public bool flag { get; set; }
    }

    [DataContract]
    public class InputSetBenefits
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
        public string item_code { get; set; }
        [DataMember]
        public double empbenefit_amount { get; set; }
        [DataMember]
        public string empbenefit_startdate { get; set; }
        [DataMember]
        public string empbenefit_enddate { get; set; }
        [DataMember]
        public string empbenefit_reason { get; set; }
        [DataMember]
        public string empbenefit_note { get; set; }
        [DataMember]
        public string empbenefit_paytype { get; set; }
        [DataMember]
        public bool empbenefit_break { get; set; }
        [DataMember]
        public string empbenefit_breakreason { get; set; }

        [DataMember]
        public string empbenefit_conditionpay { get; set; }

        [DataMember]
        public string empbenefit_payfirst { get; set; }
        [DataMember]
        public List<cls_MTWorker> emp_data { get; set; }
        [DataMember]
        public string modified_by { get; set; }
        [DataMember]
        public bool flag { get; set; }
    }

    [DataContract]
    public class InputSetTraining
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
        public int emptraining_no { get; set; }
        [DataMember]
        public string emptraining_start { get; set; }
        [DataMember]
        public string emptraining_finish { get; set; }
        [DataMember]
        public string emptraining_status { get; set; }
        [DataMember]
        public double emptraining_hours { get; set; }
        [DataMember]
        public double emptraining_cost { get; set; }
        [DataMember]
        public string emptraining_note { get; set; }
        [DataMember]
        public string institute_code { get; set; }
        [DataMember]
        public string institute_other { get; set; }
        [DataMember]
        public string course_code { get; set; }
        [DataMember]
        public string course_other { get; set; }
        [DataMember]
        public List<cls_MTWorker> emp_data { get; set; }
        [DataMember]
        public string modified_by { get; set; }
        [DataMember]
        public bool flag { get; set; }
    }

    [DataContract]
    public class InputSetAssessment
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
        public int empassessment_id { get; set; }
        [DataMember]
        public string empassessment_location { get; set; }
        [DataMember]
        public string empassessment_topic { get; set; }
        [DataMember]
        public string empassessment_fromdate { get; set; }
        [DataMember]
        public string empassessment_todate { get; set; }
        [DataMember]
        public double empassessment_count { get; set; }
        [DataMember]
        public string empassessment_result { get; set; }
        [DataMember]
        public List<cls_MTWorker> emp_data { get; set; }
        [DataMember]
        public string modified_by { get; set; }
        [DataMember]
        public bool flag { get; set; }
    }

    [DataContract]
    public class InputTREmpSuggest
    {
        [DataMember]
        public string company_code { get; set; }
        [DataMember]
        public string worker_code { get; set; }
        [DataMember]
        public int empsuggest_id { get; set; }
        [DataMember]
        public string empsuggest_code { get; set; }
        [DataMember]
        public string empsuggest_date { get; set; }
        [DataMember]
        public string empsuggest_note { get; set; }
        [DataMember]
        public string modified_by { get; set; }
        [DataMember]
        public DateTime modified_date { get; set; }
        [DataMember]
        public int index { get; set; }
    }

    public class FillterSearch
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
        public string worker_code { get; set; }
        [DataMember]
        public string level_code { get; set; }
        [DataMember]
        public string dep_code { get; set; }
        [DataMember]
        public string position_code { get; set; }
        [DataMember]
        public string location_code { get; set; }
        [DataMember]
        public DateTime date_fill { get; set; }
        [DataMember]
        public bool worker_resignstatus { get; set; }
        [DataMember]
        public string searchemp { get; set; }
        [DataMember]
        public string worker_emptype { get; set; }
        [DataMember]
        public string worker_id { get; set; }
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
        public string worker_gender { get; set; }
        [DataMember]
        public string group_code { get; set; }

    }
}