using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace OpenAI.Net
{
    public class AssistantsRequestHeaders
    {
        private static AuthenticationHeaderValue AuthenticationHeaderValue { get; set; }
        private static Dictionary<string, string> Headers = new Dictionary<string, string>();
        private static readonly string apiKey = "your openai api key";

        static AssistantsRequestHeaders()
        {
            string json = File.ReadAllText("assistants.json");
            var assistantsSetting= JsonConvert.DeserializeObject<AssistantsSetting>(json);
            if(assistantsSetting != null )
            {
                apiKey = assistantsSetting.Apikey;
            }

            AuthenticationHeaderValue = new AuthenticationHeaderValue("Bearer", apiKey);
            Headers.Add("OpenAI-Beta", "assistants=v1");
            //Headers.Add("Content-Type", "application/json");
        }

        public static void SetHeaders(HttpClient httpClient)
        {

            httpClient.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue;
            foreach (var header in Headers)
            {
                httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
            }

            // httpClient.DefaultRequestHeaders.Add("OpenAI-Beta", "assistants=v1");
        }

    }
}
