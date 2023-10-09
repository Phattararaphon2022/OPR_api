using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace BPC_OPR
{
      
    [DataContract]
    public class APIHRJob
    {
        [DataMember]
        public string JobNo { get; set; }
        [DataMember]
        public string DocumentDate { get; set; }
        [DataMember]
        public string Currency { get; set; }
        [DataMember]
        public string RefAPPCostID { get; set; }
        [DataMember]
        public string CustNo { get; set; }
        [DataMember]
        public string RefSO { get; set; }
        [DataMember]
        public string Version { get; set; }
        [DataMember]
        public string StartDate { get; set; }
        [DataMember]
        public string EndDate { get; set; }
        [DataMember]
        public List<JobPlaningLines> JobPlaningLines { get; set; }
        [DataMember]
        public decimal TotalCost { get; set; }
        [DataMember]
        public string PostBy { get; set; }        
    }

    public class JobPlaningLines
    {
        public string JabTaskNo { get; set; }
        public string JabTaskTH { get; set; }
        public string JabTaskEN { get; set; }
        public List<JobTaskShift> JobTaskShift { get; set; }
        public List<JobTaskCost> JobTaskCost { get; set; }
        public List<JabTaskItem> JabTaskItem { get; set; }    
    }

    public class JobTaskShift
    {
        public string ShiftCode { get; set; }
        public string TimeIN { get; set; }
        public string TimeOUT { get; set; }
        public string WorkingDay { get; set; }
        public decimal QtyDay { get; set; }
        public decimal QtyHour { get; set; }
        public decimal QtyOt { get; set; }
        public decimal QtyPerson { get; set; }
    }

    public class JobTaskCost
    {
        public string CostCode { get; set; }
        public string CostType { get; set; }
        public decimal CostAmount { get; set; }
        public string CostAuto { get; set; }
        
    }

    public class JabTaskItem
    {
        public string ItemNo { get; set; }
        public int ItemQty { get; set; }      

    }

   

}