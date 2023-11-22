using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary_BPC.hrfocus.model.Project
{
  public class cls_TRProDocatt
    {
      public cls_TRProDocatt() { }
        public string company_code { get; set; }
        public string project_code { get; set; }
        public int document_id { get; set; }
        public string job_type { get; set; }
        public string document_name { get; set; }
        public string document_type { get; set; }
        public string document_path { get; set; }
        public string created_by { get; set; }
        public DateTime created_date { get; set; }
        public string modified_by { get; set; }
        public DateTime modified_date { get; set; }
    }
}
