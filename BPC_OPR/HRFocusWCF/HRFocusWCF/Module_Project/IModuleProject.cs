using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;

namespace BPC_OPR
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IModuleProject" in both code and config file together.
    [ServiceContract]
    public interface IModuleProject
    {
        [OperationContract(Name = "probusiness_list")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string getMTProbusinessList(BasicRequest req);

        [OperationContract(Name = "probusiness")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doManageMTProbusiness(InputMTProbusiness input);

        [OperationContract(Name = "probusiness_del")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doDeleteMTProbusiness(InputMTProbusiness input);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/doUploadMTProbusiness?fileName={fileName}&token={token}&by={by}&com={com}", ResponseFormat = WebMessageFormat.Json)]
        Task<string> doUploadMTProbusiness(string token, string by, string fileName, Stream stream, string com);

        //--

        [OperationContract(Name = "protype_list")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string getMTProtypeList(BasicRequest req);

        [OperationContract(Name = "protype")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doManageMTProtype(InputMTProtype input);

        [OperationContract(Name = "protype_del")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doDeleteMTProtype(InputMTProtype input);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/doUploadMTProtype?fileName={fileName}&token={token}&by={by}&com={com}", ResponseFormat = WebMessageFormat.Json)]
        Task<string> doUploadMTProtype(string token, string by, string fileName, Stream stream, string com);

        //--

        [OperationContract(Name = "prouniform_list")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string getMTProuniformList(BasicRequest req);

        [OperationContract(Name = "prouniform")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doManageMTProuniform(InputMTProuniform input);

        [OperationContract(Name = "prouniform_del")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doDeleteMTProuniform(InputMTProuniform input);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/doUploadMTProuniform?fileName={fileName}&token={token}&by={by}&com={com}", ResponseFormat = WebMessageFormat.Json)]
        Task<string> doUploadMTProuniform(string token, string by, string fileName, Stream stream, string com);

        //--

        [OperationContract(Name = "proslip_list")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string getMTProslipList(BasicRequest req);

        [OperationContract(Name = "proslip")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doManageMTProslip(InputMTProslip input);

        [OperationContract(Name = "proslip_del")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doDeleteMTProslip(InputMTProslip input);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/doUploadMTProslip?fileName={fileName}&token={token}&by={by}&com={com}", ResponseFormat = WebMessageFormat.Json)]
        Task<string> doUploadMTProslip(string token, string by, string fileName, Stream stream, string com);

        //--

        [OperationContract(Name = "procost_list")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string getMTProcostList(BasicRequest req);

        [OperationContract(Name = "procost")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doManageMTProcost(InputMTProcost input);

        [OperationContract(Name = "procost_del")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doDeleteMTProcost(InputMTProcost input);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/doUploadMTProcost?fileName={fileName}&token={token}&by={by}&com={com}", ResponseFormat = WebMessageFormat.Json)]
        Task<string> doUploadMTProcost(string token, string by, string fileName, Stream stream, string com);

        //--

        [OperationContract(Name = "project_list")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string getMTProjectList(FillterProject req);

        [OperationContract(Name = "project")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doManageMTProject(InputMTProject input);

        [OperationContract(Name = "project_del")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doDeleteMTProject(InputMTProject input);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/doUploadMTProject?fileName={fileName}&token={token}&by={by}&com={com}", ResponseFormat = WebMessageFormat.Json)]
        Task<string> doUploadMTProject(string token, string by, string fileName, Stream stream, string com);

        //--

        [OperationContract(Name = "proaddress_list")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string getTRProaddressList(FillterProject req);

        [OperationContract(Name = "proaddress")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doManageTRProaddress(InputTRProaddress input);

        [OperationContract(Name = "proaddress_del")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doDeleteTRProaddress(InputTRProaddress input);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/doUploadTRProaddress?fileName={fileName}&token={token}&by={by}&com={com}", ResponseFormat = WebMessageFormat.Json)]
        Task<string> doUploadTRProaddress(string token, string by, string fileName, Stream stream, string com);

        //--

        [OperationContract(Name = "procontact_list")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string getTRProcontactList(FillterProject req);

        [OperationContract(Name = "procontact")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doManageTRProcontact(InputTRProcontact input);
        [OperationContract(Name = "procontacts")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doManageTRProcontactList(InputProjectTransaction input);

        [OperationContract(Name = "procontact_del")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doDeleteTRProcontact(InputTRProcontact input);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/doUploadTRProcontact?fileName={fileName}&token={token}&by={by}&com={com}", ResponseFormat = WebMessageFormat.Json)]
        Task<string> doUploadTRProcontact(string token, string by, string fileName, Stream stream, string com);

        //--
        [OperationContract(Name = "procontract_list")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string getTRProcontractList(FillterProject req);

        [OperationContract(Name = "procontracts")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doManageTRProcontractList(InputProjectTransaction input);

        [OperationContract(Name = "procontract_del")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doDeleteTRProcontract(InputTRProcontract input);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/doUploadTRProcontract?fileName={fileName}&token={token}&by={by}&com={com}", ResponseFormat = WebMessageFormat.Json)]
        Task<string> doUploadTRProcontract(string token, string by, string fileName, Stream stream, string com);

        //--
        [OperationContract(Name = "proresponsible_list")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string getTRProresponsibleList(FillterProject req);

        [OperationContract(Name = "proresponsibles")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doManageTRProresponsibleList(InputProjectTransaction input);

        [OperationContract(Name = "proresponsible_del")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doDeleteTRProresponsible(InputTRProresponsible input);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/doUploadTRProresponsible?fileName={fileName}&token={token}&by={by}&com={com}", ResponseFormat = WebMessageFormat.Json)]
        Task<string> doUploadTRProresponsible(string token, string by, string fileName, Stream stream, string com);

        //--
        [OperationContract(Name = "protimepol_list")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string getMTProtimepolList(FillterProject req);

        [OperationContract(Name = "protimepols")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doManageMTProtimepolList(InputProjectTransaction input);

        [OperationContract(Name = "protimepol_del")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doDeleteMTProtimepol(InputMTProtimepol input);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/doUploadMTProtimepol?fileName={fileName}&token={token}&by={by}&com={com}", ResponseFormat = WebMessageFormat.Json)]
        Task<string> doUploadMTProtimepol(string token, string by, string fileName, Stream stream, string com);

        //--
        [OperationContract(Name = "projobmain_list")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string getMTProjobmainList(FillterProject req);

        [OperationContract(Name = "projobmains")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doManageMTProjobmainList(InputProjectTransaction input);

        [OperationContract(Name = "projobmain_del")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doDeleteMTProjobmain(InputMTProjobmain input);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/doUploadMTProjobmain?fileName={fileName}&token={token}&by={by}&com={com}", ResponseFormat = WebMessageFormat.Json)]
        Task<string> doUploadMTProjobmain(string token, string by, string fileName, Stream stream, string com);


        //--

        [OperationContract(Name = "projobcontract_list")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string getTRProjobcontractList(FillterProject req);

        [OperationContract(Name = "projobcontract")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doManageTRProjobcontract(InputTRProjobcontract input);
        [OperationContract(Name = "projobcontracts")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doManageTRProjobcontractList(InputProjectTransaction input);

        [OperationContract(Name = "projobcontract_del")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doDeleteTRProjobcontract(InputTRProjobcontract input);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/doUploadTRProjobcontract?fileName={fileName}&token={token}&by={by}&com={com}", ResponseFormat = WebMessageFormat.Json)]
        Task<string> doUploadTRProjobcontract(string token, string by, string fileName, Stream stream, string com);

        //--

        [OperationContract(Name = "projobcost_list")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string getTRProjobcostList(FillterProject req);

        [OperationContract(Name = "projobcost")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doManageTRProjobcost(InputTRProjobcost input);
        [OperationContract(Name = "projobcosts")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doManageTRProjobcostList(InputProjectTransaction input);

        [OperationContract(Name = "projobcost_del")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doDeleteTRProjobcost(InputTRProjobcost input);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/doUploadTRProjobcost?fileName={fileName}&token={token}&by={by}&com={com}", ResponseFormat = WebMessageFormat.Json)]
        Task<string> doUploadTRProjobcost(string token, string by, string fileName, Stream stream, string com);

        //--

        [OperationContract(Name = "projobmachine_list")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string getTRProjobmachineList(FillterProject req);

        [OperationContract(Name = "projobmachine")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doManageTRProjobmachine(InputTRProjobmachine input);
        [OperationContract(Name = "projobmachines")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doManageTRProjobmachineList(InputProjectTransaction input);

        [OperationContract(Name = "projobmachine_del")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doDeleteTRProjobmachine(InputTRProjobmachine input);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/doUploadTRProjobmachine?fileName={fileName}&token={token}&by={by}&com={com}", ResponseFormat = WebMessageFormat.Json)]
        Task<string> doUploadTRProjobmachine(string token, string by, string fileName, Stream stream, string com);

        //--

        [OperationContract(Name = "projobshift_list")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string getTRProjobshiftList(FillterProject req);

        [OperationContract(Name = "projobshifts")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doManageTRProjobshiftList(InputProjectTransaction input);
       
        [OperationContract(Name = "projobshift_del")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doDeleteTRProjobshift(InputTRProjobshift input);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/doUploadTRProjobshift?fileName={fileName}&token={token}&by={by}&com={com}", ResponseFormat = WebMessageFormat.Json)]
        Task<string> doUploadTRProjobshift(string token, string by, string fileName, Stream stream, string com);



        //--
        [OperationContract(Name = "projobsub_list")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string getMTProjobsubList(FillterProject req);

        [OperationContract(Name = "projobsubs")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doManageMTProjobsubList(InputProjectTransaction input);

        [OperationContract(Name = "projobsub_del")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doDeleteMTProjobsub(InputMTProjobsub input);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/doUploadMTProjobsub?fileName={fileName}&token={token}&by={by}&com={com}", ResponseFormat = WebMessageFormat.Json)]
        Task<string> doUploadMTProjobsub(string token, string by, string fileName, Stream stream, string com);

        //--

        [OperationContract(Name = "projobemp2_list")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string getTRProjobempList2(FillterProject req);


        [OperationContract(Name = "projobemp_list")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string getTRProjobempList(FillterProject req);

        [OperationContract(Name = "projobemp")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doManageTRProjobemp(InputTRProjobemp input);

        [OperationContract(Name = "projobemps")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doManageTRProjobempList(InputProjectTransaction input);

        [OperationContract(Name = "projobemp_del")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doDeleteTRProjobemp(InputTRProjobemp input);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/doUploadMTProjobemp?fileName={fileName}&token={token}&by={by}&com={com}", ResponseFormat = WebMessageFormat.Json)]
        Task<string> doUploadTRProjobemp(string token, string by, string fileName, Stream stream, string com);


        //
       


        [OperationContract(Name = "projobempfillter_list")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string getProjobempFillterList(FillterProject req);

        [OperationContract(Name = "MTProjectFillter_list")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string getMTProjectFillterList(FillterProject req);

        [OperationContract(Name = "MTProjectFillter2_list")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string getMTProjectFillterList2(FillterProject req);

        //[OperationContract(Name = "TRProjobempfillterList")]
        //[WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        //string getTRProjobempfillterList(FillterProject req);

        
        //
        //--
        [OperationContract(Name = "projobworking_list")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string getTRProjobworkingList(FillterProject req);

        [OperationContract(Name = "projobworkings")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doManageTRProjobworkingList(InputProjectTransaction input);

        [OperationContract(Name = "projobworking_del")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doDeleteTRProjobworking(InputTRProjobworking input);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/doUploadMTProjobworking?fileName={fileName}&token={token}&by={by}&com={com}", ResponseFormat = WebMessageFormat.Json)]
        Task<string> doUploadTRProjobworking(string token, string by, string fileName, Stream stream, string com);

        //Task
        [OperationContract(Name = "task_list")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string getMTTaskList(FillterTask req);

        [OperationContract(Name = "task_detail")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string getTRTaskdetail(InputMTTask input);

        [OperationContract(Name = "task_whose")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string getTRTaskwhose(InputMTTask input);

        [OperationContract(Name = "task")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doManageTask(InputMTTask input);

        [OperationContract(Name = "task_del")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doDeleteMTTask(InputMTTask input);


        //-- Project monitor
        [OperationContract(Name = "project_monitor")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string getProjectMonitor(FillterProject req);
       
        [OperationContract(Name = "job_monitor")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string getJobMonitor(FillterProject req);

        //-- Job version
        [OperationContract(Name = "projobversion_list")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string getMTProjobversionList(FillterProject req);

        [OperationContract(Name = "projobversion")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doManageMTProjobversion(InputMTProjobversion input);

        [OperationContract(Name = "projobversion_del")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doDeleteMTProjobversion(InputMTProjobversion input);

        //-- Job policy
        [OperationContract(Name = "projobpol_list")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string getTRProjobpolList(FillterProject req);

        [OperationContract(Name = "projobpol")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doManageTRProjobpol(InputTRProjobpol input);

        [OperationContract(Name = "projobpol_del")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doDeleteTRProjobpol(InputTRProjobpol input);
    
        //-- MTProarea
        [OperationContract(Name = "proarea_list")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string getMTProareaList(BasicRequest req);

        [OperationContract(Name = "proarea")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doManageMTProarea(InputMTProarea input);

        [OperationContract(Name = "proarea_del")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doDeleteMTProarea(InputMTProarea input);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/doUploadMTProarea?fileName={fileName}&token={token}&by={by}&com={com}", ResponseFormat = WebMessageFormat.Json)]
        Task<string> doUploadMTProarea(string token, string by, string fileName, Stream stream, string com);

        //-- MTProgroup
        [OperationContract(Name = "progroup_list")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string getMTProgroup(BasicRequest req);

        [OperationContract(Name = "progroup")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doManageMTProgroup(InputMTProgroup input);

        [OperationContract(Name = "progroup_del")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doDeleteMTProgroup(InputMTProgroup input);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/doUploadMTProgroup?fileName={fileName}&token={token}&by={by}&com={com}", ResponseFormat = WebMessageFormat.Json)]
        Task<string> doUploadMTProgroup(string token, string by, string fileName, Stream stream, string com);
        
        
        //

        //-- MTProequipmenttype
        [OperationContract(Name = "proequipmenttype_list")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string getMTProequipmenttype(BasicRequest req);

        [OperationContract(Name = "proequipmenttype")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doManageMTProequipmenttype(InputMTProequipmenttype input);

        [OperationContract(Name = "proequipmenttype_del")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doDeleteMTProequipmenttype(InputMTProequipmenttype input);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/doUploadMTProequipmenttype?fileName={fileName}&token={token}&by={by}&com={com}", ResponseFormat = WebMessageFormat.Json)]
        Task<string> doUploadMTProequipmenttype(string token, string by, string fileName, Stream stream, string com);

        //


        //--TRProequipmentreq
        [OperationContract(Name = "TRProequipmentreq_list")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string getTRProequipmenttype(FillterProject req);

        [OperationContract(Name = "TRProequipmentreq")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doManageTRProequipmentreqList(InputProjectTransaction input);

        [OperationContract(Name = "TRProequipmentreqdel")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string doDeleteTRProequipmenttype(InputTRProequipmentreq input);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/doUploadTRProequipmentreq?fileName={fileName}&token={token}&by={by}&com={com}", ResponseFormat = WebMessageFormat.Json)]
        Task<string> doUploadTRProequipmenttype(string token, string by, string fileName, Stream stream, string com);

        //--

        //-- Cost compare
        [OperationContract(Name = "cost_compare")]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string getCostCompare(FillterProject req);

    }
}