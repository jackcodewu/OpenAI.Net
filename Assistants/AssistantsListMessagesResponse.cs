using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAI.Net.Assistants
{
    public class AssistantsListMessagesResponse : AssistantsResponse
    {
        public string _object{ get; set; }
        public AssistantsAddMsgResponse[] data { get; set; }
        public string first_id { get; set; }
        public string last_id { get; set; }
        public bool has_more { get; set; }
    }

    public class Datum
    {
        public string id { get; set; }
        public string _object{ get; set; }
        public int created_at { get; set; }
        public string thread_id { get; set; }
        public string role { get; set; }
        public Content[] content { get; set; }
        public string[] file_ids { get; set; }
        public object assistant_id { get; set; }
        public object run_id { get; set; }
        public Metadata metadata { get; set; }
    }


}
