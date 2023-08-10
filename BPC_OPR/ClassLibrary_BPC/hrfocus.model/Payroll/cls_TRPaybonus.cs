using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary_BPC.hrfocus.model.Payroll
{
  public class cls_TRPaybonus
  {
      public cls_TRPaybonus() { }

      public string company_code { get; set; }
      public string worker_code { get; set; }
      public DateTime paybonus_date { get; set; }
      public double paybonus_amount { get; set; }
      public double paybonus_quantity { get; set; }
      public double paybonus_rate { get; set; }
      public double paybonus_tax { get; set; }
      public double paybonus_netpay { get; set; }


      public string paybonus_note { get; set; }
      public string created_by { get; set; }
      public DateTime created_date { get; set; }
      public string modified_by { get; set; }
      public DateTime modified_date { get; set; }
      public bool flag { get; set; }

      //-- Show       
      public string worker_detail { get; set; }

  }
}
