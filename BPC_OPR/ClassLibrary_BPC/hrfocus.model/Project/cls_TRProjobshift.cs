using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary_BPC.hrfocus.model
{
    public class cls_TRProjobshift
    {
        public cls_TRProjobshift() { }
        public int projobshift_id { get; set; }
        public string shift_code { get; set; }      
        public bool projobshift_sun { get; set; }
        public bool projobshift_mon { get; set; }
        public bool projobshift_tue { get; set; }
        public bool projobshift_wed { get; set; }
        public bool projobshift_thu { get; set; }
        public bool projobshift_fri { get; set; }
        public bool projobshift_sat { get; set; }
        public int projobshift_ph { get; set; }


        public int projobshift_emp { get; set; }
        public int projobshift_working { get; set; }
        public double projobshift_hrsperday { get; set; }
        public double projobshift_hrsot { get; set; }

        public string projob_code { get; set; }
        public string project_code { get; set; }

        public string version { get; set; }    
      
        public string modified_by { get; set; }
        public DateTime modified_date { get; set; }   
        

    }
}
