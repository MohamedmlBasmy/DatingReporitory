using DatingApp.API.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace DatingApp.API.Extentions
{
    public static class ResponseExtensions
    {
        public static void AddApplicationError(this HttpResponse response, string message)
        {
            response.Headers.Add("Application-Error", message);
            response.Headers.Add("Access-Control-Expose-Headers", "Application-Error");
            response.Headers.Add("Access-Control-Allow-Origin", "*");
        }
        public static void AddPaginationHeader(this HttpResponse httpResponse, int pageNumber, int pageSize, int totalItems, int totalPages)
        {
            var obj = new {pageNumber, pageSize, totalItems, totalPages};
            httpResponse.Headers.Add("Pagination", JsonConvert.SerializeObject(obj));
            httpResponse.Headers.Add("Access-Control-Expose-Headers", "Pagination");
        }
            // "Access-Control-Expose-Headers"
    }
}