using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductListWebApp.Utils
{
    public class SerializationHelper : ISerializationHelper
    {
        public List<Dictionary<string, string>> Deserialize(string DatatoDeserialize)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            return JsonConvert.DeserializeObject<List<Dictionary<String, String>>>(DatatoDeserialize);
        }
    }
}
