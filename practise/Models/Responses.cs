using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace practise.Models
{
    public class Responses<T>
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }

        public T Data { get;set; }
    }
}
