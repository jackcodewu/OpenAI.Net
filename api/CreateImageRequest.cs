using OpenAI.Net.api.VisionResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAI.Net.api
{
    public class CreateImageRequest : OpenAIHttp
    {
        public string model { get; set; } = "dall-e-3";
        public string prompt { get; set; }
        public int n { get; set; } = 1;
        public string size { get; set; } = "1024x1024";


        public async Task<CreateImageResponse> SendMsg(string prompt)
        {

            CreateImageRequest request = new CreateImageRequest
            {
                prompt= prompt,
            };

            return await SendMsg<CreateImageRequest, CreateImageResponse>(request, $"https://api.openai.com/v1/images/generations");
        }
    }
}

