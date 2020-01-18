using Newtonsoft.Json;
using ProductWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductListWebApp.Utils
{
    public class SerializationHelper : ISerializationHelper
    {
        public List<ProductItemViewModel> Deserialize(string DatatoDeserialize)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            return JsonConvert.DeserializeObject<List<ProductItemViewModel>>(DatatoDeserialize);
        }
        public string ConvertObjectToJSON(object item)
        {
            return JsonConvert.SerializeObject(new
            {
                Title = item
            });
        }

    }
}
