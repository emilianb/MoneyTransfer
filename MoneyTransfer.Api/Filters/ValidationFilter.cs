using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;

using MoneyTransfer.Api.Attributes;

namespace MoneyTransfer.Api.Filters
{
    public class ValidationFilter
        : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            foreach (var parameter in context.ActionDescriptor.Parameters)
            {
                var descriptor = parameter as ControllerParameterDescriptor;

                if (descriptor != null)
                {
                    var nullNotAllowed = descriptor.ParameterInfo
                        .CustomAttributes
                        .Any(a => a.AttributeType == typeof(NotNullAttribute));

                    if (nullNotAllowed && !context.ActionArguments.Any(a => a.Key == parameter.Name))
                    {
                        context.HttpContext
                            .Response
                            .Headers
                            .Add("MoneyTransfer-Trace-Id", context.HttpContext.TraceIdentifier);

                        context.Result = new BadRequestResult();

                        return;
                    }
                }
            }

            if (!context.ModelState.IsValid)
            {
                context.Result = new UnprocessableEntityObjectResult(context.ModelState);
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        { /* Nothing to do here. */ }
    }
}
