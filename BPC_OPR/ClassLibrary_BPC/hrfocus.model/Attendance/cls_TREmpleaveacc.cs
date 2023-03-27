using System;
namespace ClassLibrary_BPC.hrfocus.model
{
    public class cls_TREmpleaveacc
    {
        public cls_TREmpleaveacc() { }

        public string company_code { get; set; }
        public string worker_code { get; set; }

        public string leave_code { get; set; }
        public string year_code { get; set; }

        public double empleaveacc_bf { get; set; }
        public double empleaveacc_annual { get; set; }
        public double empleaveacc_used { get; set; }
        public double empleaveacc_remain { get; set; }

        public string created_by { get; set; }
        public DateTime created_date { get; set; }

        public string modified_by { get; set; }
        public DateTime modified_date { get; set; }

        public bool flag { get; set; }

        public string worker_detail_th { get; set; }
        public string leave_detail_th { get; set; }
        public string worker_detail_en { get; set; }
        public string leave_detail_en { get; set; }

    }
}
