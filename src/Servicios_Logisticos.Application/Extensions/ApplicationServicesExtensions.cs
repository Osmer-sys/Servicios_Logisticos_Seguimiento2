using Microsoft.Extensions.DependencyInjection;
using Servicios_Logisticos.Application.Commands.AccessLogs;
using Servicios_Logisticos.Application.Commands.AccessPoints;
using Servicios_Logisticos.Application.Commands.Credentials;
using Servicios_Logisticos.Application.Commands.Employees;
using Servicios_Logisticos.Application.DTOs;
using Servicios_Logisticos.Application.Queries.AccessLogs;
using Servicios_Logisticos.Application.Queries.AccessPoints;
using Servicios_Logisticos.Application.Queries.Credentials;
using Servicios_Logisticos.Application.Queries.Employees;
using Servicios_Logisticos.Application.Utilities.Mediator;

namespace Servicios_Logisticos.Application.Extensions;

public static class ApplicationServicesExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Mediador personalizado
        services.AddScoped<IMediator, SimpleMediator>();

        // Command Handlers - Employees
        services.AddScoped<IRequestHandler<CreateEmployeeCommand, EmployeeDto>, CreateEmployeeCommandHandler>();
        services.AddScoped<IRequestHandler<UpdateEmployeeCommand, EmployeeDto>, UpdateEmployeeCommandHandler>();
        services.AddScoped<IRequestHandler<DeactivateEmployeeCommand, EmployeeDto>, DeactivateEmployeeCommandHandler>();

        // Command Handlers - Credentials
        services.AddScoped<IRequestHandler<AssignCredentialCommand, CredentialDto>, AssignCredentialCommandHandler>();
        services.AddScoped<IRequestHandler<BlockCredentialCommand, CredentialDto>, BlockCredentialCommandHandler>();
        services.AddScoped<IRequestHandler<RevokeCredentialCommand, CredentialDto>, RevokeCredentialCommandHandler>();

        // Command Handlers - AccessPoints
        services.AddScoped<IRequestHandler<CreateAccessPointCommand, AccessPointDto>, CreateAccessPointCommandHandler>();
        services.AddScoped<IRequestHandler<UpdateAccessPointCommand, AccessPointDto>, UpdateAccessPointCommandHandler>();

        // Command Handlers - AccessLogs (RF-03)
        services.AddScoped<IRequestHandler<RegisterAccessMovementCommand, AccessValidationResultDto>, RegisterAccessMovementCommandHandler>();

        // Query Handlers - Employees
        services.AddScoped<IRequestHandler<GetAllEmployeesQuery, IReadOnlyList<EmployeeDto>>, GetAllEmployeesQueryHandler>();
        services.AddScoped<IRequestHandler<GetEmployeeByIdQuery, EmployeeDto?>, GetEmployeeByIdQueryHandler>();
        services.AddScoped<IRequestHandler<GetEmployeeByDocumentQuery, EmployeeDto?>, GetEmployeeByDocumentQueryHandler>();

        // Query Handlers - Credentials
        services.AddScoped<IRequestHandler<GetAllCredentialsQuery, IReadOnlyList<CredentialDto>>, GetAllCredentialsQueryHandler>();
        services.AddScoped<IRequestHandler<GetCredentialByCodeQuery, CredentialDto?>, GetCredentialByCodeQueryHandler>();
        services.AddScoped<IRequestHandler<GetCredentialsByEmployeeIdQuery, IReadOnlyList<CredentialDto>>, GetCredentialsByEmployeeIdQueryHandler>();

        // Query Handlers - AccessPoints
        services.AddScoped<IRequestHandler<GetAllAccessPointsQuery, IReadOnlyList<AccessPointDto>>, GetAllAccessPointsQueryHandler>();
        services.AddScoped<IRequestHandler<GetAccessPointByIdQuery, AccessPointDto?>, GetAccessPointByIdQueryHandler>();

        // Query Handlers - AccessLogs (RF-04)
        services.AddScoped<IRequestHandler<GetAccessLogsPagedQuery, PagedResult<AccessLogDto>>, GetAccessLogsPagedQueryHandler>();

        return services;
    }
}
