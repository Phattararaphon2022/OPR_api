using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary_BPC.hrfocus.model
{
    public class cls_MTReqRequest
    {
        public cls_MTReqRequest() { }
        public string company_code { get; set; }
        public int request_id { get; set; }
        public string request_code { get; set; }
        public DateTime request_date { get; set; }
        public DateTime request_startdate { get; set; }
        public DateTime request_enddate { get; set; }
        public string request_position { get; set; }
        public string request_project { get; set; }
        public string request_employee_type { get; set; }
        public double request_quantity { get; set; }
        public string request_urgency { get; set; }
        public string request_note { get; set; }
        public double request_accepted { get; set; }
        public int request_status { get; set; }

        public string created_by { get; set; }
        public DateTime created_date { get; set; }
        public string modified_by { get; set; }
        public DateTime modified_date { get; set; }
        public bool flag { get; set; }

        //show only
        public string position_name_th { get; set; }
        public string position_name_en { get; set; }
        public string project_name_th { get; set; }
        public string project_name_en { get; set; }
    }
}
