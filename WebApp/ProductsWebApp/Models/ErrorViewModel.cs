#region Namespaces
using System;
#endregion

/// <summary>
/// Namespace for erro
/// </summary>
namespace ProductApp.Models
{
    public class ErrorViewModel
    {
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}