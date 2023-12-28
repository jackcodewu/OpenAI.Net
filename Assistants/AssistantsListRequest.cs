using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAI.Net.Assistants
{
    public class AssistantsListRequest: OpenAIAssistantsHttp
    {
        public async Task<AssistantsListResponse> SendMsg(int limit=100)
        {
            return await GetdMsg<AssistantsListResponse>($"https://api.openai.com/v1/assistants?order=desc&limit={limit}");
        }
    }
}
