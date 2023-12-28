using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace OpenAI.Net
{
    interface IOpenAIHttp
    {
        Task<ResponseData> SendMsg<RequestData, ResponseData>(RequestData data, string url) where RequestData : class, new() where ResponseData : class, new();

        Task<ResponseData> SendMsg<ResponseData>(string url, string data) where ResponseData : class, new();
    }
}
