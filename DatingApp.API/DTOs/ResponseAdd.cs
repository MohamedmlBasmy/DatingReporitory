using Microsoft.AspNetCore.Http;

namespace DatingApp.API.DTOs
{
    public class ResponseAdd
    {
        public object ResponseBody { get; set; }
        public string Exception { get; set; }
    }
}