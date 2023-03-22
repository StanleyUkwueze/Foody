using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foody.DTOs
{
    public class Response<T>
    {
        public Response( int statusCode, string message, T data = default)
        {
            StatusCode = statusCode;
            Message = message;
            Data = data;
        }
        public Response()
        {

        }

        public int StatusCode { get; set; }
        public string Message { get; set; }
        public bool IsSuccessful { get; set; }
        public string[] Errors { get; set; }
        public T Data { get; set; }
    }
}
