using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAI.Net.Assistants
{
    public class AssistantsListMessagesRequest: OpenAIAssistantsHttp
    {
        public AssistantsListMessagesRequest()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }

        public async Task<AssistantsListMessagesResponse> SendMsg(string threadId)
        {
            return await GetdMsg<AssistantsListMessagesResponse>($"https://api.openai.com/v1/threads/{threadId}/messages");
        }//https://api.openai.com/v1/threads/thread_abc123/messages

    }
}
