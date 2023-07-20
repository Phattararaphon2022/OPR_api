using System;
using System.Collections.Generic;
namespace ClassLibrary_BPC.hrfocus.model
{
    public class cls_MTSubmenu
    {
        public cls_MTSubmenu() { }
        public string mainmenu_code { get; set; }
        public string submenu_code { get; set; }
        public string submenu_detail_th { get; set; }
        public string submenu_detail_en { get; set; }
        public int submenu_order { get; set; }
        public List<cls_MTItemmenu> itemmenu { get; set; }

    }
}
