using System;

namespace ClassLibrary_BPC.hrfocus.model
{
    public class cls_MTTopic
    {
        public cls_MTTopic() { }
        public string company_code { get; set; }
        public int topic_id { get; set; }
        public string topic_code { get; set; }
        public string topic_name_th { get; set; }
        public string topic_name_en { get; set; }
        public string topic_type { get; set; }
        public string created_by { get; set; }
        public DateTime created_date { get; set; }
        public string modified_by { get; set; }
        public DateTime modified_date { get; set; }
        public bool flag { get; set; }
    }
}
