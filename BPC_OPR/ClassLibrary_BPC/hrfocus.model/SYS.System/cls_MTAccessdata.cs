using System;
using System.Collections.Generic;
namespace ClassLibrary_BPC.hrfocus.model
{
    public class cls_MTAccessdata
    {
        public cls_MTAccessdata() { }
        public string company_code { get; set; }
        public string polmenu_code { get; set; }
        public string accessdata_module { get; set; }
        public bool accessdata_new { get; set; }
        public bool accessdata_edit { get; set; }
        public bool accessdata_delete { get; set; }
        public bool accessdata_salary { get; set; }
        public List<cls_MTAccessmenu> accessmenu_data { get; set; }
    }
}
