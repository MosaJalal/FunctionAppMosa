using FunctionAppMosa.Data;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureServices((hostContext, services) =>
    {
        string sqlConnectionString = Environment.GetEnvironmentVariable("SqlConnectionStringmosafunctionapp");

        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlServer(sqlConnectionString);
        });

        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();
    })
    .Build();

await host.RunAsync();