using System;

namespace ClassLibrary_BPC.hrfocus.model
{
   public class cls_TRReqdocatt
    {
       public cls_TRReqdocatt() { }
        public int reqdoc_id { get; set; }
        public int reqdoc_att_no { get; set; }
        public string reqdoc_att_file_name { get; set; }
        public string reqdoc_att_file_type{ get; set; }
        public string reqdoc_att_path { get; set; }
        public string created_by { get; set; }
        public string created_date { get; set; }
    }
}
