using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductListWebApp.Utils
{
    public interface ISerializationHelper
    {
        List<Dictionary<string, string>> Deserialize(string DatatoDeserialize);
    }
}
