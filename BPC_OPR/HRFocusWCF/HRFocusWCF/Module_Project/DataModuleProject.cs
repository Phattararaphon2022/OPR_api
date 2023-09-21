using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace BPC_OPR
{
    public class DataModuleProject
    {
    }

    
    [DataContract]
    public class InputMTProbusiness
    {
        [DataMember]
        public string company_code { get; set; }
        [DataMember]
        public string probusiness_id { get; set; }
        [DataMember]
        public string probusiness_code { get; set; }
        [DataMember]
        public string probusiness_name_th { get; set; }
        [DataMember]
        public string probusiness_name_en { get; set; }   
        [DataMember]
        public string modified_by { get; set; }
    }

    [DataContract]
    public class InputMTProtype
    {
        [DataMember]
        public string protype_id { get; set; }
        [DataMember]
        public string company_code { get; set; }
        [DataMember]
        public string protype_code { get; set; }
        [DataMember]
        public string protype_name_th { get; set; }
        [DataMember]
        public string protype_name_en { get; set; }
        [DataMember]
        public string modified_by { get; set; }
    }

    [DataContract]
    public class InputMTProuniform
    {
        [DataMember]
        public string company_code { get; set; }
        [DataMember]
        public string prouniform_id { get; set; }
        [DataMember]
        public string prouniform_code { get; set; }
        [DataMember]
        public string prouniform_name_th { get; set; }
        [DataMember]
        public string prouniform_name_en { get; set; }
        [DataMember]
        public string modified_by { get; set; }
    }

    [DataContract]
    public class InputMTProslip
    {
        [DataMember]
        public string company_code { get; set; }
        [DataMember]
        public string proslip_id { get; set; }
        [DataMember]
        public string proslip_code { get; set; }
        [DataMember]
        public string proslip_name_th { get; set; }
        [DataMember]
        public string proslip_name_en { get; set; }
        [DataMember]
        public string modified_by { get; set; }
    }

    [DataContract]
    public class InputMTProcost
    {
        [DataMember]
        public string procost_id { get; set; }
        [DataMember]
        public string procost_code { get; set; }
        [DataMember]
        public string procost_name_th { get; set; }
        [DataMember]
        public string procost_name_en { get; set; }

        [DataMember]
        public string procost_type { get; set; }
        [DataMember]
        public bool procost_auto { get; set; }
        [DataMember]
        public string procost_itemcode { get; set; }
        [DataMember]
        public string company_code { get; set; }

        [DataMember]
        public string modified_by { get; set; }
    }

    [DataContract]
    public class InputMTProject
    {
        [DataMember]
        public int project_id { get; set; }
        [DataMember]
        public string comapny_code { get; set; }
        [DataMember]
        public string project_code { get; set; }
        [DataMember]
        public string project_name_th { get; set; }
        [DataMember]
        public string project_name_en { get; set; }
        [DataMember]
        public string project_name_sub { get; set; }
        [DataMember]
        public string project_codecentral { get; set; }
        [DataMember]
        public string project_protype { get; set; }

        [DataMember]
        public string project_proarea { get; set; }
        [DataMember]
        public string project_progroup { get; set; }

        [DataMember]
        public string project_probusiness { get; set; }
        [DataMember]
        public string project_roundtime { get; set; }
        [DataMember]
        public string project_roundmoney { get; set; }
        [DataMember]
        public string project_proholiday { get; set; }
        [DataMember]
        public string project_status { get; set; }
        [DataMember]
        public string company_code { get; set; }
        [DataMember]
        public string modified_by { get; set; }
        
    }

    [DataContract]
    public class FillterProject
    {
        [DataMember]
        public string device_name { get; set; }
        [DataMember]
        public string ip { get; set; }
        [DataMember]
        public string username { get; set; }
        [DataMember]
        public string company { get; set; }
        [DataMember]
        public string company_code { get; set; }
        [DataMember]
        public string language { get; set; }
        [DataMember]
        public string project_code { get; set; }
        [DataMember]
        public string procontract_type { get; set; }
        [DataMember]
        public string project_name_th { get; set; }
        [DataMember]
        public string project_name_en { get; set; }
        [DataMember]
        public string project_name_sub { get; set; }
        [DataMember]
        public string project_codecentral { get; set; }
        [DataMember]
        public string project_protype { get; set; }
        [DataMember]
        public string project_proarea { get; set; }
        [DataMember]
        public string project_progroup { get; set; }
        [DataMember]
        public string project_probusiness { get; set; }

        [DataMember]
        public string job_code { get; set; }

        [DataMember]
        public string fromdate { get; set; }

        [DataMember]
        public string todate { get; set; }

        [DataMember]
        public string version { get; set; }

        [DataMember]
        public string status { get; set; }
        [DataMember]
        public string projob_code { get; set; }
        [DataMember]
        public string projobemp_emp { get; set; }
         [DataMember]
        public string projobemp_type { get; set; }
         [DataMember]
         public string searchemp { get; set; }
        
    }

    [DataContract]
    public class InputProjectTransaction
    {
        
        [DataMember]
        public string project_code { get; set; }
        [DataMember]
        public string job_code { get; set; }
         [DataMember]
        public string procontract_type { get; set; }
        [DataMember]
        public string company_code { get; set; }
        [DataMember]
        public string transaction_data { get; set; }

        [DataMember]
        public string version { get; set; }

        [DataMember]
        public string modified_by { get; set; }
    }

    [DataContract]
    public class InputTRProaddress
    {
        [DataMember]
        public string proaddress_id { get; set; }
        [DataMember]
        public string proaddress_type { get; set; }
        [DataMember]
        public string proaddress_no { get; set; }
        [DataMember]
        public string proaddress_moo { get; set; }
        [DataMember]
        public string proaddress_soi { get; set; }
        [DataMember]
        public string proaddress_road { get; set; }
        [DataMember]
        public string proaddress_tambon { get; set; }
        [DataMember]
        public string proaddress_amphur { get; set; }
        [DataMember]
        public string proaddress_zipcode { get; set; }
        [DataMember]
        public string proaddress_tel { get; set; }
        [DataMember]
        public string proaddress_email { get; set; }
        [DataMember]
        public string proaddress_line { get; set; }
        [DataMember]
        public string proaddress_facebook { get; set; }
        [DataMember]
        public string province_code { get; set; }
        [DataMember]
        public string project_code { get; set; }
        [DataMember]
        public string modified_by { get; set; }
    }

    [DataContract]
    public class InputTRProcontact
    {
        [DataMember]
        public string procontact_id { get; set; }
        [DataMember]
        public string procontact_ref { get; set; }
        [DataMember]
        public string procontact_firstname_th { get; set; }
        [DataMember]
        public string procontact_lastname_th { get; set; }
        [DataMember]
        public string procontact_firstname_en { get; set; }
        [DataMember]
        public string procontact_lastname_en { get; set; }
        [DataMember]
        public string procontact_tel { get; set; }
        [DataMember]
        public string procontact_email { get; set; }
        [DataMember]
        public string position_code { get; set; }
        [DataMember]
        public string initial_code { get; set; }
        [DataMember]
        public string project_code { get; set; }
        [DataMember]
        public string modified_by { get; set; }
    }

    [DataContract]
    public class InputTRProcontract
    {
        [DataMember]
        public int procontract_id { get; set; }
        [DataMember]
        public string procontract_ref { get; set; }       
        [DataMember]
        public string project_code { get; set; }
        [DataMember]
        public string modified_by { get; set; }        
    }

    [DataContract]
    public class InputTRProresponsible
    {

        [DataMember]
        public int proresponsible_id { get; set; }
        [DataMember]
        public string proresponsible_ref { get; set; }
       [DataMember]
        public string project_code { get; set; }
        [DataMember]
        public string modified_by { get; set; }
        
    }

    [DataContract]
    public class InputMTProtimepol
    {

        [DataMember]
        public int protimepol_id { get; set; }
        [DataMember]
        public string protimepol_code { get; set; }
        [DataMember]
        public string project_code { get; set; }
        [DataMember]
        public string modified_by { get; set; }

    }

    [DataContract]
    public class InputMTProjobmain
    {

        [DataMember]
        public int projobmain_id { get; set; }
        [DataMember]
        public string projobmain_code { get; set; }
        [DataMember]
        public string project_code { get; set; }
        [DataMember]
        public string version { get; set; }
        [DataMember]
        public string procontract_type { get; set; }
        [DataMember]
        public string modified_by { get; set; }

    }

    [DataContract]
    public class InputTRProjobcontract
    {
        [DataMember]
        public int projobcontract_id { get; set; }
        [DataMember]
        public string projobcontract_ref { get; set; }
        [DataMember]
        public int projobcontract_working { get; set; }
        [DataMember]
        public double projobcontract_hrsperday { get; set; }
        [DataMember]
        public double projobcontract_hrsot { get; set; }        
        [DataMember]
        public string projob_code { get; set; }
        [DataMember]
        public string project_code { get; set; }
        [DataMember]
        public string version { get; set; }
        [DataMember]
        public string modified_by { get; set; }
    }

    [DataContract]
    public class InputTRProjobcost
    {
        [DataMember]
        public int projobcost_id { get; set; }
        [DataMember]
        public string projobcost_code { get; set; }
        [DataMember]
        public double projobcost_amount { get; set; }
        [DataMember]
        public string projobcost_fromdate { get; set; }
        [DataMember]
        public string projobcost_todate { get; set; }
        [DataMember]
        public string version { get; set; }
        [DataMember]
        public string projobcost_status { get; set; }
        [DataMember]
        public bool projobcost_auto { get; set; }
        [DataMember]
        public string projob_code { get; set; }
        [DataMember]
        public string project_code { get; set; }
        [DataMember]
        public string modified_by { get; set; }
    }

    [DataContract]
    public class InputTRProjobmachine
    {
        [DataMember]
        public int projobmachine_id { get; set; }
        [DataMember]
        public string projobmachine_ip { get; set; }
        [DataMember]
        public string projobmachine_port { get; set; }
        [DataMember]
        public bool projobmachine_enable { get; set; }
        [DataMember]
        public string projob_code { get; set; }
        [DataMember]
        public string project_code { get; set; }
        [DataMember]
        public string modified_by { get; set; }
    }

    [DataContract]
    public class InputTRProjobshift
    {

        [DataMember]
        public int projobshift_id { get; set; }
        [DataMember]
        public string projob_code { get; set; }
        [DataMember]
        public string project_code { get; set; }

        [DataMember]
        public string version { get; set; }

        [DataMember]
        public string modified_by { get; set; }

    }

    [DataContract]
    public class InputMTProjobsub
    {

        [DataMember]
        public int projobsub_id { get; set; }
        [DataMember]
        public string projobsub_code { get; set; }
        [DataMember]
        public string project_code { get; set; }

        [DataMember]
        public string version { get; set; }
        [DataMember]
        public string modified_by { get; set; }

    }

    public class InputTRProjobemp
    {
        [DataMember]
        public int projobemp_id { get; set; }
        [DataMember]
        public string projobemp_emp { get; set; }
        [DataMember]
        public string projobemp_fromdate { get; set; }
        [DataMember]
        public string projobemp_todate { get; set; }
        [DataMember]
        public string projobemp_type { get; set; }
        [DataMember]
        public string projobemp_status { get; set; }
        [DataMember]
        public string projob_code { get; set; }
        [DataMember]
        public string project_code { get; set; }
        
        [DataMember]
        public string modified_by { get; set; }
       
    }
    [DataContract]
    public class InputTRProjobworking
    {
        [DataMember]
        public int projobworking_id { get; set; }
        [DataMember]
        public string projobworking_emp { get; set; }
        [DataMember]
        public string projobworking_workdate { get; set; }
        [DataMember]
        public string projobworking_in { get; set; }
        [DataMember]
        public string projobworking_out { get; set; }
        [DataMember]
        public string projobworking_status { get; set; }
        [DataMember]
        public string projob_code { get; set; }
        [DataMember]
        public string project_code { get; set; }

        [DataMember]
        public string modified_by { get; set; }

    }
    
    [DataContract]
    public class FillterTask
    {
        [DataMember]
        public string device_name { get; set; }
        [DataMember]
        public string ip { get; set; }
        [DataMember]
        public string username { get; set; }
        [DataMember]
        public string company { get; set; }
        [DataMember]
        public string project_code { get; set; }

        [DataMember]
        public string type { get; set; }

        [DataMember]
        public string status { get; set; }

        [DataMember]
        public string fromdate { get; set; }
        [DataMember]
        public string todate { get; set; }

    }
    [DataContract]
    public class InputMTTask
    {
        [DataMember]
        public string company_code { get; set; }
        [DataMember]
        public string project_code { get; set; }
        [DataMember]
        public int task_id { get; set; }
        [DataMember]
        public string task_type { get; set; }
        [DataMember]
        public string task_status { get; set; }
        [DataMember]
        public string task_start { get; set; }
        [DataMember]
        public string task_end { get; set; }
        [DataMember]
        public string task_note { get; set; }

        [DataMember]
        public string detail_data { get; set; }

        [DataMember]
        public string whose_data { get; set; }

        [DataMember]
        public string modified_by { get; set; }
        [DataMember]
        public DateTime modified_date { get; set; }
        [DataMember]
        public int index { get; set; }
    }

     [DataContract]
    public class InputMTProjobversion
    {
        [DataMember]
        public int projobversion_id { get; set; }
        [DataMember]
        public string transaction_id { get; set; }
        [DataMember]
        public string version { get; set; }
        [DataMember]
        public string fromdate { get; set; }
        [DataMember]
        public string todate { get; set; }
        [DataMember]
        public string transaction_data { get; set; }
        [DataMember]
        public string transaction_old { get; set; }
        [DataMember]
        public string refso { get; set; }
        [DataMember]
        public string custno { get; set; }
        [DataMember]
        public string refappcostid { get; set; }
        [DataMember]
        public string currency { get; set; }
        [DataMember]
        public string project_code { get; set; }
        [DataMember]
        public string modified_by { get; set; }   
    }

    [DataContract]
     public class InputTRProjobpol
     {
        [DataMember]
        public int projobpol_id { get; set; }
        [DataMember]
        public string projobpol_type { get; set; }
        [DataMember]
        public string projobpol_timepol { get; set; }
        [DataMember]
        public string projobpol_slip { get; set; }
        [DataMember]
        public string projobpol_uniform { get; set; }
        [DataMember]
        public string project_code { get; set; }
        [DataMember]
        public string projobmain_code { get; set; }
        [DataMember]
        public string modified_by { get; set; }
         

     }

    [DataContract] 
    public class InputMTProarea
    {
        [DataMember]
        public string company_code { get; set; }
        [DataMember]
        public string proarea_id { get; set; }
        [DataMember]
        public string proarea_code { get; set; }
        [DataMember]
        public string proarea_name_th { get; set; }
        [DataMember]
        public string proarea_name_en { get; set; }
        [DataMember]
        public string modified_by { get; set; }
    }

     [DataContract] 
    public class InputMTProgroup
    {
         [DataMember]
         public string company_code { get; set; }
        [DataMember]
        public string progroup_id { get; set; }
        [DataMember]
        public string progroup_code { get; set; }
        [DataMember]
        public string progroup_name_th { get; set; }
        [DataMember]
        public string progroup_name_en { get; set; }
        [DataMember]
        public string modified_by { get; set; }
    

}

}