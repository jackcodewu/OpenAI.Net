using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAI.Net.Assistants
{
    public class AssistantsRoleCreateRequest: OpenAIAssistantsHttp
    {

        public string instructions { get; set; } = "You are a personal math tutor. Write and run code to answer math questions. ";
        public string name { get; set; } = "Math Tutor"; 
        public Tool[] tools { get; set; }
        public string model { get; set; } = "gpt-4-1106-preview";

        public AssistantsRoleCreateRequest()
        {
            tools = new Tool[] { new Tool() { type = "code_interpreter" } };
        }

        public async Task<AssistantsRoleCreateResponse> SendMsg()
        {

            AssistantsRoleCreateRequest assistantsRoleCreateRequest = new AssistantsRoleCreateRequest();

           return await  SendMsg<AssistantsRoleCreateRequest, AssistantsRoleCreateResponse>(assistantsRoleCreateRequest, "https://api.openai.com/v1/assistants");
        }

    }

    public class Tool
    {
        public string type { get; set; }
    }
}
