using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Servicios_Logisticos.Domain.Exceptions;

namespace Servicios_Logisticos.Presentation.Common;

public class ApiExceptionFilterAttribute : ExceptionFilterAttribute
{
    private readonly ILogger<ApiExceptionFilterAttribute> _logger;

    public ApiExceptionFilterAttribute(ILogger<ApiExceptionFilterAttribute> logger)
    {
        _logger = logger;
    }

    public override void OnException(ExceptionContext context)
    {
        _logger.LogError(context.Exception, "Error al procesar solicitud: {Message}", context.Exception.Message);

        switch (context.Exception)
        {
            case EntityNotFoundException notFound:
                context.Result = new NotFoundObjectResult(new
                {
                    Status = StatusCodes.Status404NotFound,
                    Title = "Recurso no encontrado",
                    Detail = notFound.Message
                });
                context.ExceptionHandled = true;
                break;

            case BusinessRuleException ruleEx:
                context.Result = new BadRequestObjectResult(new
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Regla de negocio no cumplida",
                    Detail = ruleEx.Message
                });
                context.ExceptionHandled = true;
                break;

            case DomainException domainEx:
                context.Result = new BadRequestObjectResult(new
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Error de dominio",
                    Detail = domainEx.Message
                });
                context.ExceptionHandled = true;
                break;

            default:
                context.Result = new ObjectResult(new
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Title = "Error interno del servidor",
                    Detail = context.Exception.Message
                })
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
                context.ExceptionHandled = true;
                break;
        }

        base.OnException(context);
    }
}
