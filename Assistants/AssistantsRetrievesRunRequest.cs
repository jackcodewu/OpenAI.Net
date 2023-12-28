using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAI.Net.Assistants
{
    public class AssistantsRetrievesRunRequest: OpenAIAssistantsHttp
    {
        public async Task<AssistantsRetrievesRunResponse> SendMsg(string threadId, string runId)
        {

            return await GetdMsg<AssistantsRetrievesRunResponse>($"https://api.openai.com/v1/threads/{threadId}/runs/{runId}");
        }
    }
}
