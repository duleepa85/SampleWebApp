using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using Newtonsoft.Json;
using Sample.Auth;
using Serilog.Exceptions.Core;
using Serilog.Exceptions;
using Serilog.Exceptions.SqlServer.Destructurers;
using Newtonsoft.Json.Serialization;
using Sample.EmployeeService.EmployeeFunctions;
using Microsoft.ApplicationInsights.Extensibility;
using Serilog.Filters;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using Serilog.Core;
using Serilog.Events;
using Sample.Logging;
using System.Reflection.PortableExecutable;
using Sample.EmployeeService.EmployeeManager;
using Sample.EmployeeService.Data;

[assembly: FunctionsStartup(typeof(FunctionAppStartup))]
namespace Sample.EmployeeService.EmployeeFunctions
{
    public class FunctionAppStartup : FunctionsStartup
    {
        private IConfiguration _configuration { get; set; }
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var configBuilder = new ConfigurationBuilder().AddEnvironmentVariables();
            _configuration = configBuilder.Build();
            ConfigureServices(builder.Services).BuildServiceProvider(true);
            builder.AddAccessTokenBinding();
        }
        private IServiceCollection ConfigureServices(IServiceCollection services)
        {
            var logger = new LoggerConfiguration()
            .Filter.ByExcluding(Matching.FromSource("Microsoft"))
                 .Filter.ByExcluding(Matching.FromSource("Host"))
                 .Enrich.FromLogContext()
                 .Destructure.ByTransforming<AuthorizedRequest>(r => new { TenantId = r.AuthorizedUser.TenantId, AzureUserId = r.AuthorizedUser.AzureUserId })
                 .WriteTo.Console()
                 .Enrich.WithExceptionDetails(new DestructuringOptionsBuilder().WithDefaultDestructurers().WithDestructurers(new[] { new SqlExceptionDestructurer() }))
                 .CreateLogger();

            services.AddLogging(lb => lb.AddSerilog(logger));
            services.AddScoped<IEmployeeService, EmployeeManager.EmployeeService>();
            services.AddSingleton<IEmployeeDataManager, EmployeeDataManager>();
            return services;
        }
    }
}
