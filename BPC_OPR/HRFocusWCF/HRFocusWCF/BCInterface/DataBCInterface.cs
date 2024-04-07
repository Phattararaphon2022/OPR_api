﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace BPC_OPR
{

    public class ApiResponse<T>
    {
        public bool success { get; set; }
        public string message { get; set; }
        public List<T> data { get; set; }
    }
      
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

    public class APIHRProject
    {
        public int ProjectId { get; set; }
        public string ProjectCode { get; set; }
        public string ProjectNameTh { get; set; }
        public string ProjectNameEn { get; set; }
        public string ProjectNameSub { get; set; }
        public string ProjectCodeCentral { get; set; }
        public string ProjectProType { get; set; }
        public string ProjectProArea { get; set; }
        public string ProjectProGroup { get; set; }
        public string ProjectProBusiness { get; set; }
        public string ProjectRoundTime { get; set; }
        public string ProjectRoundMoney { get; set; }
        public string ProjectProHoliday { get; set; }
        public char? ProjectStatus { get; set; }
        public string CompanyCode { get; set; }
        public string ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public bool Flag { get; set; }
        public string ProGroupCode { get; set; }
        // ProAddress
        public int ProAddressId { get; set; }
        public char ProAddressType { get; set; }
        public string ProAddressNo { get; set; }
        public string ProAddressMoo { get; set; }
        public string ProAddressSoi { get; set; }
        public string ProAddressRoad { get; set; }
        public string ProAddressTambon { get; set; }
        public string ProAddressAmphur { get; set; }
        public string ProvinceCode { get; set; }
        public string ProAddressZipCode { get; set; }
        public string ProAddressTel { get; set; }
        public string ProAddressEmail { get; set; }
        public string ProAddressLine { get; set; }
        public string ProAddressFacebook { get; set; }
        // ProContact
        public List<ProContact> Contact { get; set; }
    }
    public class ProAddress
    {
        public int ProAddressId { get; set; }
        public char ProAddressType { get; set; }
        public string ProAddressNo { get; set; }
        public string ProAddressMoo { get; set; }
        public string ProAddressSoi { get; set; }
        public string ProAddressRoad { get; set; }
        public string ProAddressTambon { get; set; }
        public string ProAddressAmphur { get; set; }
        public string ProvinceCode { get; set; }
        public string ProAddressZipCode { get; set; }
        public string ProAddressTel { get; set; }
        public string ProAddressEmail { get; set; }
        public string ProAddressLine { get; set; }
        public string ProAddressFacebook { get; set; }
        public string ProjectCode { get; set; }
        public string ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public bool Flag { get; set; }
    }

    public class ProContact
    {
        public int ProContactId { get; set; }
        public string ProContactRef { get; set; }
        public string ProContactFirstNameTh { get; set; }
        public string ProContactLastNameTh { get; set; }
        public string ProContactFirstNameEn { get; set; }
        public string ProContactLastNameEn { get; set; }
        public string ProContactTel { get; set; }
        public string ProContactEmail { get; set; }
        public string PositionCode { get; set; }
        public string InitialCode { get; set; }
        public string ProjectCode { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedDate { get; set; }
        public bool Flag { get; set; }
    }

    public class ProContract
    {
        public int ProContractId { get; set; }
        public string ProContractRef { get; set; }
        public string ProContractDate { get; set; }
        public decimal ProContractAmount { get; set; }
        public string ProContractFromDate { get; set; }
        public string ProContractToDate { get; set; }
        public string ProContractCustomer { get; set; }
        public string ProContractBidder { get; set; }
        public string ProjectCode { get; set; }
        public string ProContractType { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedDate { get; set; }
    }

    public class APIHRJobmain
    {
        public string ProjectCode { get; set; }
        public string Version { get; set; }
        public string StartDate { get; set; }
        public string FromDate { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedDate { get; set; }
        public List<ProJobMain> JobPlaningLines { get; set; }
    }
    public class ProJobMain
    {
        public int ProJobMainId { get; set; }
        public string ProJobMainCode { get; set; }
        public string ProJobMainNameTh { get; set; }
        public string ProJobMainNameEn { get; set; }
        public char ProJobMainJobType { get; set; }
        public string ProJobMainFromDate { get; set; }
        public string ProJobMainToDate { get; set; }
        public char ProJobMainType { get; set; }
        public string ProJobMainTimePol { get; set; }
        public string ProJobMainSlip { get; set; }
        public string ProJobMainUniform { get; set; }
        public string ProjectCode { get; set; }
        public string Version { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedDate { get; set; }
        public List<ProJobShift> JobTaskShift { get; set; }
        public List<ProJobCost> JobTaskCost { get; set; }
        public List<ProJobMachine> JobTaskMachine { get; set; }
    }
    public class ProJobShift
    {
        public int ProJobShiftId { get; set; }
        public string ShiftCode { get; set; }
        public bool ProJobShiftSun { get; set; }
        public bool ProJobShiftMon { get; set; }
        public bool ProJobShiftTue { get; set; }
        public bool ProJobShiftWed { get; set; }
        public bool ProJobShiftThu { get; set; }
        public bool ProJobShiftFri { get; set; }
        public bool ProJobShiftSat { get; set; }
        public int ProJobShiftEmp { get; set; }
        public bool ProJobShiftPh { get; set; }
        public int ProJobShiftWorking { get; set; }
        public double ProJobShiftHrsPerDay { get; set; }
        public double ProJobShiftHrsOT { get; set; }
        public string ProJobCode { get; set; }
        public string ProjectCode { get; set; }
        public string Version { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedDate { get; set; }
    }

    public class ProJobCost
    {
        public int ProJobCostId { get; set; }
        public string ProJobCostCode { get; set; }
        public double ProJobCostAmount { get; set; }
        public bool ProJobCostAuto { get; set; }
        public char ProJobCostStatus { get; set; }
        public string ProJobCode { get; set; }
        public string ProjectCode { get; set; }
        public string Version { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedDate { get; set; }
    }

    public class ProJobMachine
    {
        public int ProJobMachineId { get; set; }
        public string ProJobMachineIp { get; set; }
        public string ProJobMachinePort { get; set; }
        public bool ProJobMachineEnable { get; set; }
        public string ProJobCode { get; set; }
        public string ProjectCode { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedDate { get; set; }
    }

    public class ProUniform
    {
        public string CompanyCode { get; set; }
        public int ProUniformId { get; set; }
        public string ProUniformCode { get; set; }
        public string ProUniformNameTh { get; set; }
        public string ProUniformNameEn { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedDate { get; set; }
    }

    public class ProEquipmentReq
    {
        public int ProEquipmentReqId { get; set; }
        public string ProUniformCode { get; set; }
        public string ProEquipmentReqDate { get; set; }
        public int ProEquipmentReqQty { get; set; }
        public string ProEquipmentReqNote { get; set; }
        public string ProEquipmentReqBy { get; set; }
        public string ProEquipmentTypeCode { get; set; }
        public string ProJobCode { get; set; }
        public string ProjectCode { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedDate { get; set; }
    }

    public class EmpTrUniform
    {
        public string CompanyCode { get; set; }
        public int EmpUniformId { get; set; }
        public string EmpUniformCode { get; set; }
        public string ProjectCode { get; set; }
        public string ProjobCode { get; set; }
        public string ProequipmenttypeCode { get; set; }
        public string EmpUniformSize { get; set; }
        public string ItemCode { get; set; }
        public double EmpUniformQuantity { get; set; }
        public double EmpUniformAmount { get; set; }
        public double EmpUniformTotal { get; set; }
        public string EmpUniformIssueDate { get; set; }
        public string EmpUniformNote { get; set; }
        public string EmpUniformBy { get; set; }
        public double EmpUniformPayPeriod { get; set; }
        public double EmpUniformPayAmount { get; set; }
        public string EmpUniformPeriod { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedDate { get; set; }
    }

}