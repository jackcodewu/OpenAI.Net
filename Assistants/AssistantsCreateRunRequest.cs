using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAI.Net.Assistants
{
    public class AssistantsCreateRunRequest: OpenAIAssistantsHttp
    {
        public string assistant_id { get; set; }

        public async Task<AssistantsCreateRunResponse> SendMsg(string threadId, string ssistantId)
        {

            AssistantsCreateRunRequest assistantsCreateRunRequest = new AssistantsCreateRunRequest { assistant_id= ssistantId };

            return await SendMsg<AssistantsCreateRunRequest, AssistantsCreateRunResponse>(assistantsCreateRunRequest, $"https://api.openai.com/v1/threads/{threadId}/runs");
        }
    }

}
