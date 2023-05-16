using System;

namespace ClassLibrary_BPC.hrfocus.model
{
    public class cls_TRReqempinfo
    {
        public cls_TRReqempinfo() { }
        public int reqdoc_id { get; set; }
        public int reqdocempinfo_no { get; set; }
        public string topic_code { get; set; }
        public string reqempinfo_detail { get; set; }
    }
}
