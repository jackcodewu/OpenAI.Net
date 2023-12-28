using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace OpenAI.Net.Assistants
{
    public class OpenAIAssistantsHttp:OpenAIHttp
    {
        public OpenAIAssistantsHttp()
        {
            OpenAIRequestHeaders.SetAssistantsHeaders(httpClient);
        }
    }
}
