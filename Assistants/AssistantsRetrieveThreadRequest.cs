using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAI.Net.Assistants
{
    public class AssistantsRetrieveThreadRequest: OpenAIAssistantsHttp
    {
        public async Task<AssistantsThreadCreateResponse> SendMsg(string threadId)
        {
            return await GetdMsg<AssistantsThreadCreateResponse>($"https://api.openai.com/v1/threads{threadId}");
        }
    }
}
