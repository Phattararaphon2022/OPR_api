using System;


namespace ClassLibrary_BPC.hrfocus.model
{
    public class cls_MTLocation
    {
        public cls_MTLocation() { }
        public string company_code { get; set; }
        public int location_id { get; set; }
        public string location_code { get; set; }
        public string location_name_th { get; set; }
        public string location_name_en { get; set; }

        public string location_detail { get; set; }
        public string location_lat { get; set; }
        public string location_long { get; set; }
        public string created_by { get; set; }
        public DateTime created_date { get; set; }


        public string modified_by { get; set; }
        public DateTime modified_date { get; set; }

        public bool flag { get; set; }

    }
}
