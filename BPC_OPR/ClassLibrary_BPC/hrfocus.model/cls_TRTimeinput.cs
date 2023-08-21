using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary_BPC.hrfocus.model
{
    public class cls_TRTimeinput
    {
        public cls_TRTimeinput() { }

        public string timeinput_card { get; set; }
        public DateTime timeinput_date { get; set; }
        public string timeinput_hhmm { get; set; }
        public string timeinput_terminal { get; set; }
        public string timeinput_function { get; set; }
        public string timeinput_compare { get; set; }

        //-- show only
        public string projob_code { get; set; }
        public string project_code { get; set; }
        public string shift_code { get; set; }
    }
}
