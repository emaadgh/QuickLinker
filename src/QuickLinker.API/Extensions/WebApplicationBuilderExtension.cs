using Microsoft.EntityFrameworkCore;
using OpenTelemetry.Metrics;
using QuickLinker.API.DbContexts;
using QuickLinker.API.Observability;
using QuickLinker.API.Services;
using System.Reflection;

namespace QuickLinker.API.Extensions;

public static class WebApplicationBuilderExtension
{
    public static void AddApplicationServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddControllers(options =>
        {
            options.ReturnHttpNotAcceptable = true;
        }).AddNewtonsoftJson();

        builder.Services.Configure<AppSettings>(builder.Configuration);
        var settings = builder.Configuration.Get<AppSettings>();
        ArgumentNullException.ThrowIfNull(settings, nameof(settings));

        builder.Services.AddTransient<IQuickLinkerRepository, QuickLinkerRepository>();
        builder.Services.AddScoped<IShortLinkService, ShortLinkService>();
        builder.Services.AddSingleton<IQuickLinkerDiagnostic, QuickLinkerDiagnostic>();

        builder.Services.AddDbContext<QuickLinkerDbContext>(options =>
            options.UseSqlServer(settings.ConnectionStrings.QuickLinkerDbContextConnection));

        builder.Services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = settings.ConnectionStrings.QuickLinkerRedisConnection;
            options.InstanceName = "QuickLinker";
        });

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(setupAction =>
        {
            var xmlCommentsFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlCommentsFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentsFile);

            setupAction.IncludeXmlComments(xmlCommentsFullPath);
        });
    }

    public static void AddOpenTelemetry(this WebApplicationBuilder builder)
    {
        builder.Services.AddOpenTelemetry()
                    .WithMetrics(builder =>
                    {
                        builder.AddPrometheusExporter();
                        builder.AddAspNetCoreInstrumentation();
                        builder.AddRuntimeInstrumentation();
                        var meter = new[]
                        {
                            "QuickLinkerDiagnostic"
                        };

                        builder.AddMeter(meter);
                    });
    }
}
