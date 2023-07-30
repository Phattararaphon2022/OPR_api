using System;
using System.Collections.Generic;
namespace ClassLibrary_BPC.hrfocus.model
{
    public class cls_MTPolmenu
    {
        public cls_MTPolmenu() { }
        public string company_code { get; set; }
        public int polmenu_id { get; set; }
        public string polmenu_code { get; set; }
        public string polmenu_name_th { get; set; }
        public string polmenu_name_en { get; set; }
        public string created_by { get; set; }
        public string created_date { get; set; }
        public string modified_by { get; set; }
        public string modified_date { get; set; }
        public bool flag { get; set; }
        public List<cls_MTAccessdata> accessdata_data { get; set; }
    }
}
