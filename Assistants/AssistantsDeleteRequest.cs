using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAI.Net.Assistants
{
    public class AssistantsDeleteRequest: OpenAIAssistantsHttp
    {
        public AssistantsDeleteRequest()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }

        public async Task<AssistantsDeleteResponse> SendMsg(string assid)
        {
            return await Delete<AssistantsDeleteResponse>($"https://api.openai.com/v1/assistants/{assid}");
        }
    }
}
