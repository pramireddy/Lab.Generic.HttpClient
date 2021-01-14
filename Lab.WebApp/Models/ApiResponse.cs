using System.Collections.Generic;

namespace Lab.WebApp.Models
{
    public class ApiResponse<T>
    {
        public ApiResponse() => ErrorMessages = new List<ErrorMessage>();

        public bool WasSuccessful { get; set; }
        public string ResponseCode { get; set; }
        public string ResponseReason { get; set; }
        public List<ErrorMessage> ErrorMessages { get; set; }
        public T Data { get; set; }
    }
}