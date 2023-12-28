using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAI.Net.Assistants
{
    public class AssistantsRoleCreateResponse : AssistantsResponse
    {
        /// <summary>
        /// 
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string _object{ get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int created_at { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string description { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string model { get; set; }
        /// <summary>
        /// 怎么在C# 中使用 openai api curl
        /// </summary>
        public string instructions { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<ToolsItem> tools { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<string> file_ids { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Metadata metadata { get; set; }
    }
    public class ToolsItem
    {
        /// <summary>
        /// 
        /// </summary>
        public string type { get; set; }
    }

    public class Metadata
    {
    }
}
