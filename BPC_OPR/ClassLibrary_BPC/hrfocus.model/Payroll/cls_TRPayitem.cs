using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary_BPC.hrfocus.model.Payroll
{
   public class cls_TRPayitem
    {
       public cls_TRPayitem() { }

        public string company_code { get; set; }
        public string worker_code { get; set; }
        public string item_code { get; set; }
        public DateTime payitem_date { get; set; }
        public double payitem_amount { get; set; }
        public double payitem_quantity { get; set; }
        public string payitem_paytype { get; set; }
        public string payitem_note { get; set; }
        public string created_by { get; set; }
        public DateTime created_date { get; set; }
        public string modified_by { get; set; }
        public DateTime modified_date { get; set; }
        public bool flag { get; set; }


        //-- Show
        public string item_detail { get; set; }
        public string item_type { get; set; }
        public string worker_detail { get; set; }

    }
}

//        public string company_code { get; set; }
//        public string worker_code { get; set; }
//        public string item_code { get; set; }
//        public DateTime payitem_date { get; set; }
//        public double payitem_amount { get; set; }
//        public double payitem_quantity { get; set; }
//        public string payitem_paytype { get; set; }
//        public string payitem_note { get; set; }
//        public string created_by { get; set; }
//        public DateTime created_date { get; set; }
//        public string modified_by { get; set; }
//        public DateTime modified_date { get; set; }
//        public bool flag { get; set; }


//        //-- Show
//        public string item_detail { get; set; }
//        public string item_type { get; set; }
//        public string worker_detail { get; set; }

//    }
//}
