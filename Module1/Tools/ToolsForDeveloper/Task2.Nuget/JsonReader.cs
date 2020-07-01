using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace Tools
{
    public class jsonReader
    {
        public object ReadJson(List<string> Json)
        {
            return JsonConvert.DeserializeObject(Json.First());
        }
    }
}
