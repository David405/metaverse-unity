using System.Collections.Generic;
using Newtonsoft.Json;

namespace Assets.Scripts
{
    public class ResourceData
    {
        public string type;
        public byte[] data;
    }

    public class ResourceResponse
    {
        public int status;
        public Dictionary<string, string> headers;
        public string data;

        public ResourceData resourceData
        {
            get
            {
                return JsonConvert.DeserializeObject<ResourceData>(data);
            }
        }
    }
    public class ResourceRequestResult
    {
        public ResourceResponse result;
    }
}
