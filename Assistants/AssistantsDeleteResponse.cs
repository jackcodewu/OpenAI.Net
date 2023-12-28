using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAI.Net.Assistants
{
    public class AssistantsDeleteResponse: AssistantsResponse
    {
        public string id { get; set; }
        public string _object { get; set; }
        public bool deleted { get; set; }
    }

}
