using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary_BPC.hrfocus.model
{
    public class cls_MTTimeimpformat
    {
        public cls_MTTimeimpformat() { }

        public string company_code { get; set; }

        public string date_format { get; set; }

        public int card_start { get; set; }
        public int card_lenght { get; set; }

        public int date_start{ get; set; }
        public int date_lenght { get; set; }

        public int hours_start { get; set; }
        public int hours_lenght { get; set; }

        public int minute_start { get; set; }
        public int minute_lenght { get; set; }

        public int function_start { get; set; }
        public int function_lenght { get; set; }

        public int machine_start { get; set; }
        public int machine_lenght { get; set; }
        
	    public string created_by { get;set; }
        public DateTime created_date { get; set; }
	    
	    public string modified_by { get;set; }
        public DateTime modified_date { get; set; }

        public bool flag { get; set; }

    }
}
