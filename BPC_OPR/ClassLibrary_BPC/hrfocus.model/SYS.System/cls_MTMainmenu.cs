using System;
using System.Collections.Generic;
namespace ClassLibrary_BPC.hrfocus.model
{
    public class cls_MTMainmenu
    {
        public cls_MTMainmenu() { }
        public string mainmenu_code { get; set; }
        public string mainmenu_detail_th { get; set; }
        public string mainmenu_detail_en { get; set; }
        public int mainmenu_order { get; set; }
        public List<cls_MTSubmenu> submenu { get; set; }
    }
}
