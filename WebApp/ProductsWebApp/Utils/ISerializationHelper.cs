using Newtonsoft.Json;
using ProductWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductListWebApp.Utils
{
    public interface ISerializationHelper
    {
        List<ProductItem> Deserialize(string DatatoDeserialize);
        string ConvertObjectToJSON(object item);
    }
}
