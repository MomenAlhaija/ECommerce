using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Ecommerce.API.Models
{
    public class ApiError
    {
        public ApiError(string message, Error error)
        {
            this.Message = message;
            this.Error = error;

        }

        public string Message { get; set; }
        public Error Error { get; set; }
    }
}
