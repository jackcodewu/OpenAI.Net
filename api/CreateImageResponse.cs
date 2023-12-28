using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAI.Net.api
{
    public class CreateImageResponse
    {
        public int created { get; set; }
        public Datum[] data { get; set; }
    }

    public class Datum
    {
        public string url { get; set; }
    }
}


