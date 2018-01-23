using Microsoft.AspNetCore.Mvc.Controllers;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Cool.Nomalization
{
    internal class RemovePreFixOperationFilter : IOperationFilter
    {
        public void Apply(Operation operation, OperationFilterContext context)
        {
            var prefix = "ApiServicesApp";
            var appServiceName = (context.ApiDescription.ActionDescriptor as ControllerActionDescriptor)?.ControllerName ?? string.Empty;
            prefix += appServiceName;
            if (operation.OperationId.StartsWith( prefix ))
            {
                operation.OperationId = operation.OperationId.Remove( 0, prefix.Length );
            }
            if (operation.OperationId.EndsWith( "Post" ))
            {
                operation.OperationId = operation.OperationId.Remove( operation.OperationId.LastIndexOf( "Post" ) );
            }
            if (operation.OperationId.EndsWith( "Get" ))
            {
                operation.OperationId = operation.OperationId.Remove( operation.OperationId.LastIndexOf( "Get" ) );
            }
        }
    }
}