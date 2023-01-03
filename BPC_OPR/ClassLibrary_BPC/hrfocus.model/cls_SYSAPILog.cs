using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary_BPC.hrfocus.model
{
    public class cls_SYSApilog
    {
        public cls_SYSApilog() { }
        public double apilog_id { get; set; }
        public string apilog_code { get; set; }
        public string apilog_data { get; set; }
        public string apilog_status { get; set; }
        public string apilog_message { get; set; }     
        public string apilog_by { get; set; }
        public DateTime apilog_date { get; set; }   
    }
}
