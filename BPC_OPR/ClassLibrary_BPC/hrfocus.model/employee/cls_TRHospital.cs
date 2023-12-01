﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary_BPC.hrfocus.model
{
    public class cls_TRHospital
    {
        public cls_TRHospital() { }

        public string company_code { get; set; }
        public string worker_code { get; set; }

        public int emphospital_id { get; set; }
        public string emphospital_code { get; set; }

        public DateTime emphospital_date { get; set; }
        
        public string created_by { get; set; }
        public DateTime created_date { get; set; }


        public string modified_by { get; set; }
        public DateTime modified_date { get; set; }

        public bool flag { get; set; }

        public string emphospital_order { get; set; }
        public bool emphospital_activate { get; set; }
    }
}
