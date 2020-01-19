#region Namespaces
using ProductWebApp.Models;
using System.Collections.Generic;

#endregion
namespace ProductListWebApp.Utils
{
    /// <summary>Serialization Helper</summary>
    public interface ISerializationHelper
    {
        /// <summary>Deserializes the specified datato deserialize.</summary>
        /// <param name="DatatoDeserialize">The datato deserialize.</param>
        /// <returns></returns>
        List<ProductItemViewModel> Deserialize(string DatatoDeserialize);


        /// <summary>Converts the object to json.</summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        string ConvertObjectToJSON(object item);
    }
}
