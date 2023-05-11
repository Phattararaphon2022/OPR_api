using ClassLibrary_BPC.hrfocus.model;
using ClassLibrary_BPC.hrfocus.model.Payroll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace BPC_OPR
{
    public class DataModulePayroll
    {
    }


    #region TRTaxrate
    [DataContract]
    public class InputTRTaxrate
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
        public int taxrate_id { get; set; }
        [DataMember]
        public double taxrate_from { get; set; }
        [DataMember]
        public double taxrate_to { get; set; }
        [DataMember]
        public double taxrate_tax { get; set; }
        [DataMember]
        public string modified_by { get; set; }
        [DataMember]
        public DateTime modified_date { get; set; }
        [DataMember]
        public int index { get; set; }
    }
    #endregion

    #region MTItem
    [DataContract]
    public class InputMTItem
    {
        [DataMember]
        public string company_code { get; set; }
        [DataMember]
        public int item_id { get; set; }
        [DataMember]
        public string item_code { get; set; }
        [DataMember]
        public string item_name_th { get; set; }
        [DataMember]
        public string item_name_en { get; set; }
        [DataMember]
        public string item_type { get; set; }
        [DataMember]
        public string item_regular { get; set; }
        [DataMember]
        public string item_caltax { get; set; }
        [DataMember]
        public string item_calpf { get; set; }
        [DataMember]
        public string item_calsso { get; set; }
        [DataMember]
        public string item_calot { get; set; }
        [DataMember]
        public string item_contax { get; set; }
        [DataMember]
        public string item_section { get; set; }
        [DataMember]
        public double item_rate { get; set; }
        [DataMember]
        public string item_account { get; set; }
        [DataMember]
        public string modified_by { get; set; }
        [DataMember]
        public DateTime modified_date { get; set; }

        [DataMember]
        public int index { get; set; }
    }

    #endregion


    #region MTProvident
    [DataContract]
    public class InputMTProvident
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
        public int provident_id { get; set; }
        [DataMember]
        public string provident_code { get; set; }
        [DataMember]
        public string provident_name_th { get; set; }
        [DataMember]
        public string provident_name_en { get; set; }

        [DataMember]
        public List<cls_TRProvidentWorkage> providentWorkage_data { get; set; }

        [DataMember]
        public string workage_data { get; set; }

        [DataMember]
        public string modified_by { get; set; }
        [DataMember]
        public DateTime modified_date { get; set; }

        [DataMember]
        public int index { get; set; }
        [DataMember]
        public bool flag { get; set; }
    }

     #endregion

    #region bonus
    [DataContract]
    public class InputMTBonus
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
        public int bonus_id { get; set; }
        [DataMember]
        public string bonus_code { get; set; }
        [DataMember]
        public string bonus_name_th { get; set; }
        [DataMember]
        public string bonus_name_en { get; set; }

        [DataMember]
        public List<cls_TRBonusrate> bonus_data { get; set; }
        [DataMember]
        public string item_code { get; set; }
        [DataMember]
        public string modified_by { get; set; }
        [DataMember]
        public DateTime modified_date { get; set; }

        [DataMember]
        public int index { get; set; }
        [DataMember]
        public bool flag { get; set; }
    }

    #endregion

    #region Bonusrate
    [DataContract]
    public class InputTRBonusrate
    {
        [DataMember]
        public string company_code { get; set; }
        [DataMember]
        public string bonus_code { get; set; }
        [DataMember]
        public double bonusrate_from { get; set; }
        [DataMember]
        public double bonusrate_to { get; set; }
        [DataMember]
        public double bonusrate_rate { get; set; }

        [DataMember]
        public int index { get; set; }
    }

     #endregion

    #region set Batch
    [DataContract]
    public class InputTRList
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
        public string worker_detail { get; set; }

        [DataMember]
        public List<cls_MTWorker> emp_data { get; set; }


        //set bonus
        [DataMember]
        public string bonus_code { get; set; }
        
        [DataMember]
        public string paypolbonus_code { get; set; }

        //set provident
        [DataMember]
        public string provident_code { get; set; }
        [DataMember]
        public string paypolprovident_code { get; set; }

        //set item
        [DataMember]
        public string item_code { get; set; }
        [DataMember]
        public string paypolitem_code { get; set; }



        [DataMember]
        public string modified_by { get; set; }
        [DataMember]
        public DateTime modified_date { get; set; }
        [DataMember]
        public int index { get; set; }
        [DataMember]
        public bool flag { get; set; }
        
        [DataMember]
        public string transaction_data { get; set; }
        
    }

    #endregion

    

    #region InputMTPeriod
    [DataContract]
    public class InputMTPeriod
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
        public string period_id { get; set; }
        [DataMember]
        public string period_type { get; set; }
        [DataMember]
        public string emptype_code { get; set; }
        [DataMember]
        public string year_code { get; set; }
        [DataMember]
        public string period_no { get; set; }
        [DataMember]
        public string period_name_th { get; set; }
        [DataMember]
        public string period_name_en { get; set; }
        [DataMember]
        public string period_from { get; set; }
        [DataMember]
        public string period_to { get; set; }
        [DataMember]
        public string period_payment { get; set; }
        [DataMember]
        public bool period_dayonperiod { get; set; }
        [DataMember]
        public string modified_by { get; set; }
        [DataMember]
        public DateTime modified_date { get; set; }
        [DataMember]
        public bool flag { get; set; }

    }
    #endregion

}

 
