using System.Collections.Generic;

namespace ProductWebApp.Models
{
    /// <summary>Class to hold product details</summary>
    public class ProductsAppViewModel
    {
        public List<ProductItemViewModel> Products { get; set; }
        public string HttpResponseStatus { get; set; }
    }
}
