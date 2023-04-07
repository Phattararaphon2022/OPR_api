using System;
namespace ClassLibrary_BPC.hrfocus.model
{
    public class cls_MTAccount
    {
        public cls_MTAccount() { }
        public string company_code { get; set; }
        public string account_user { get; set; }
        public string account_pwd { get; set; }
        public string account_type { get; set; }
        public int account_level { get; set; }
        public string account_emp { get; set; }
        public string account_email { get; set; }
        public bool account_email_alert { get; set; }
        public string account_line { get; set; }
        public bool account_line_alert { get; set; }
       
        public string created_by { get; set; }
        public DateTime created_date { get; set; }
        public string modified_by { get; set; }
        public DateTime modified_date { get; set; }
        public bool flag { get; set; }
    }
}
