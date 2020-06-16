using System.Linq;
using System.Threading.Tasks;
using DatingApp.API.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DatingApp.API.Filters
{
    public class ValidationFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.ModelState.IsValid)
            {
                var errors = context.ModelState
                .Where(x=>x.Value.Errors.Count > 0)
                .ToDictionary(n=>n.Key, v=>v.Value.Errors.Select(msg => msg.ErrorMessage)).ToArray();

                var errorResponse = new ErrorResponse();

                foreach (var error in errors)
                {
                    foreach (var errorMsg in error.Value)
                    {
                        var errorModel = new ErrorModel
                        {
                            FieldName = error.Key,
                            Message = errorMsg
                        };
                        errorResponse.Errors.Add(errorModel);
                    }
                }
                context.Result = new BadRequestObjectResult(errorResponse);
                return;
            }

            await next();
        }
    }
}