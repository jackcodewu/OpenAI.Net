using OpenAI.Net.Assistants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAI.Net
{
    public class AssistantsSetting
    {
        public string Apikey { get; set; }

        public List<AssistantsRoleCreateRequest> Assistants { get; set; }
    }
}
