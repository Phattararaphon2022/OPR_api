using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary_BPC.hrfocus.model
{
    public class cls_MTProjobsub
    {
        public cls_MTProjobsub() { }
        public int projobsub_id { get; set; }
        public string projobsub_code { get; set; }
        public string projobsub_name_th { get; set; }
        public string projobsub_name_en { get; set; }

        public string project_code { get; set; }
      
        public string modified_by { get; set; }
        public DateTime modified_date { get; set; }   


        //-- For display
        public double allow1 { get; set; }
        public double allow2 { get; set; }
        public double allow3 { get; set; }
        public double allow4 { get; set; }
        public double allow5 { get; set; }
        public double allow6 { get; set; }
        public double allow7 { get; set; }
        public double allow8 { get; set; }
        public double allow9 { get; set; }
        public double allow10 { get; set; }

        public double emp_total { get; set; }
        public double allow_emp { get; set; }
        public double allow_total { get; set; }

    }
}
