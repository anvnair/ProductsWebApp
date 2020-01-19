#region Namespaces
using Newtonsoft.Json;
using ProductWebApp.Models;
using System.Collections.Generic;

#endregion
/// <summary>
/// ProductListWebApp Utils
/// </summary>
namespace ProductListWebApp.Utils
{
    public class SerializationHelper : ISerializationHelper
    {

        /// <summary>Deserializes the specified datato deserialize.</summary>
        /// <param name="DatatoDeserialize">The datato deserialize.</param>
        /// <returns></returns>
        public List<ProductItemViewModel> Deserialize(string DatatoDeserialize)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            return JsonConvert.DeserializeObject<List<ProductItemViewModel>>(DatatoDeserialize);
        }


        /// <summary>Converts the object to json.</summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        public string ConvertObjectToJSON(object item)
        {
            return JsonConvert.SerializeObject(new
            {
                Title = item
            });
        }

    }
}
