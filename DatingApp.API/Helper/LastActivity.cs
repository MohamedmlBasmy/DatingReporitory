using DatingApp.API.Models;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;
using Microsoft.Extensions.DependencyInjection;
using System;
using DatingApp.API.Data;
using System.Threading.Tasks;

namespace DatingApp.API.Helper
{
    public class LastActive : IAsyncActionFilter
    {
        
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var contextResult =  next();
            
            var userId = int.Parse(context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var repo =  context.HttpContext.RequestServices.GetService<IDatingRepository>();

            User user = await repo.GetUser(userId);
            user.LastActive = DateTime.Now;

            await repo.SaveAll();
        }
    }
}