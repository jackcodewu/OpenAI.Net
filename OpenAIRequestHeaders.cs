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
    public class OpenAIRequestHeaders
    {
        private static AuthenticationHeaderValue AuthenticationHeaderValue { get; set; }
        private static Dictionary<string, string> Headers = new Dictionary<string, string>();
        private static readonly string apiKey = "your openai api key";

        static OpenAIRequestHeaders()
        {
            string json = File.ReadAllText("assistants.json");
            var assistantsSetting = JsonConvert.DeserializeObject<AssistantsSetting>(json);
            if (assistantsSetting != null)
            {
                apiKey = assistantsSetting.Apikey;
            }
            AuthenticationHeaderValue = new AuthenticationHeaderValue("Bearer", apiKey);

            //Headers.Add("Content-Type", "application/json");
        }

        public static void SetAssistantsHeaders(HttpClient httpClient)
        {
            httpClient.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue;

            if(!httpClient.DefaultRequestHeaders.Contains("OpenAI-Beta"))
                httpClient.DefaultRequestHeaders.Add("OpenAI-Beta", "assistants=v1");
        }

        public static void SetVisinHeaders(HttpClient httpClient)
        {
            httpClient.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue;
            if (httpClient.DefaultRequestHeaders.Contains("OpenAI-Beta"))
                httpClient.DefaultRequestHeaders.Remove("OpenAI-Beta");
        }

    }
}
