﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary_BPC.hrfocus.model
{
    public class cls_TRPaysuspend
    {
        public cls_TRPaysuspend() { }

        public string company_code { get; set; }
        public string worker_code { get; set; }

        public int paysuspend_id { get; set; }
        public DateTime payitem_date { get; set; }
        public string paysuspend_note { get; set; }
        public string reason_code { get; set; }
        public string paysuspend_type { get; set; }
        public string paysuspend_payment { get; set; }

        public string created_by { get; set; }
        public DateTime created_date { get; set; }
        public string modified_by { get; set; }
        public DateTime modified_date { get; set; }
        public bool flag { get; set; }

    }
}
