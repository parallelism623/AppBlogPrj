using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AppBlog.Api.Extensions
{
    public class ModelValidExtensions : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            if (!context.ModelState.IsValid) 
            {
                context.Result = new BadRequestObjectResult("Modelstate is not valid");
                return;
            }
        }
    }
}
