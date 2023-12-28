using OpenAI.Net.api.VisionResponse;
using OpenAI.Net.Assistants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAI.Net.api
{
    public class Gpt4VisionRequest : OpenAIHttp
    {
        public string model { get; set; } = "gpt-4-vision-preview";
        public Message[] messages { get; set; }
        public int max_tokens { get; set; }

        public Gpt4VisionRequest()
        {
            OpenAIRequestHeaders.SetAssistantsHeaders(httpClient);
        }

        public async Task<Gpt4VisionResponse> SendMsg(string image_url)
        {

            Gpt4VisionRequest gpt4VisionRequest = new Gpt4VisionRequest
            {
                max_tokens = 3000,
                messages = new Message[]
                {
                    new Message
                    {
                        role="user",
                        content=new Content[]
                        {
                            new ContentText(),
                            new ContentImage
                            {
                                image_url=new Image_Url(image_url),
                            }
                        }
                    }
                }
            };

            return await SendMsg<Gpt4VisionRequest, Gpt4VisionResponse>(gpt4VisionRequest, $"https://api.openai.com/v1/chat/completions");
        }
    }

    public class Message
    {
        public string role { get; set; }
        public Content[] content { get; set; }
    }

    public class Content
    {
    }

    public class ContentText:Content
    {

        public string type { get; set; } = "text";
        public string text { get; set; } = @"You are a professional Vue.js developer
You take a screenshot of a reference web page from the user and then build a single page application
Using Element UI, Vue and TS.
You may also receive a screenshot (second image) of the web page you have built and be asked
Update it to look more like the reference image (first image).

- Make sure the app looks exactly like the screenshot.
- Pay close attention to background color, text color, font size, font family,
Padding, margins, borders, etc. Color and size should match exactly.
- Use the exact text from the screenshot.
- Do not add comments in the code such as ""<!-- Add additional navigation links as needed -->"" and ""<!-- ...Other news items... -->"" instead of writing the full content code. Write complete code.
- Repeat elements as needed to match the screenshot. For example, if there are 15 projects, the code should have 15 projects. Don't leave comments like ""<!--Repeat for every news item-->"" or something bad will happen.
- For images, use the placeholder image from https://placehold.co and include a detailed description of the image in the alt text so that the image generation AI can generate the image later.

In terms of libraries,

Your use includes but is not limited to: vue router, vuex, vite, element-ui, axios, etc.

Please generate and return relevant Vue code text fragments based on the image.";
    }

    public class ContentImage:Content
    {

        public string type { get; set; } = "image_url";

        public Image_Url image_url { get; set; }
    }


    public class Image_Url
    {
        public string url { get; set; }

        public Image_Url(string _url)
        {
            url = _url;
        }
    }

}
