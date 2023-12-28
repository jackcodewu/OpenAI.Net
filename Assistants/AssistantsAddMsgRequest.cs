using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAI.Net.Assistants
{
    public class AssistantsAddMsgRequest: OpenAIAssistantsHttp
    {
        public string role { get; set; }
        public string content { get; set; }

        public async Task<AssistantsAddMsgResponse> SendMsg(string threadId,string msg)
        {

            AssistantsAddMsgRequest assistantsAddMsgRequest = new AssistantsAddMsgRequest {role="user",content=msg };

            return await SendMsg<AssistantsAddMsgRequest, AssistantsAddMsgResponse>(assistantsAddMsgRequest, $"https://api.openai.com/v1/threads/{threadId}/messages");
        }
    }

}
