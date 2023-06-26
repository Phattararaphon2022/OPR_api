using System;
namespace ClassLibrary_BPC.hrfocus.model
{
    public class cls_MTArea
    {
        public cls_MTArea() { }
        public string company_code { get; set; }
        public int area_id { get; set; }
        public double area_lat { get; set; }
        public double area_long { get; set; }
        public double area_distance { get; set; }
        public string location_code { get; set; }
        public string project_code { get; set; }

        public string created_by { get; set; }
        public DateTime created_date { get; set; }
        public string modified_by { get; set; }
        public DateTime modified_date { get; set; }
        public bool flag { get; set; }
    }
}
