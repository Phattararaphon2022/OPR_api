using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary_BPC.hrfocus.model
{
    public class cls_MTComcard
   {
        public cls_MTComcard()
        {

            this.comcard_code = "";
            this.card_type = "";
        }

        public int comcard_id { get; set; }
        public string comcard_code { get; set; }
       

        
        public string combank_bankaccount { get; set; }
        public string card_type { get; set; }
        
        //
        public string item_code { get; set; }
        public double payitem_amount { get; set; }
        //

        public DateTime comcard_issue { get; set; }
        public DateTime comcard_expire { get; set; }

        public string company_code { get; set; }
        public string combranch_code { get; set; }

	    public string created_by { get;set; }
        public DateTime created_date { get; set; }
	    

	    public string modified_by { get;set; }
        public DateTime modified_date { get; set; }

        public bool flag { get; set; }

    }
}
