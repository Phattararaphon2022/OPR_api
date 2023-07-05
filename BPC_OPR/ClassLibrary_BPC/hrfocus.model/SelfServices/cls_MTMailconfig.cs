using System;

namespace ClassLibrary_BPC.hrfocus.model
{
    public class cls_MTMailconfig
    {
        public cls_MTMailconfig() { }
        public string company_code { get; set; }
        public int mail_id { get; set; }
        public string mail_server { get; set; }
        public string mail_serverport { get; set; }
        public string mail_login { get; set; }
        public string mail_password { get; set; }
        public string mail_fromname { get; set; }
        public string modified_by { get; set; }
        public DateTime modified_date { get; set; }
    }
}
