using Microsoft.EntityFrameworkCore;
using QuickLinker.API;
using QuickLinker.API.DbContexts;
using QuickLinker.API.Services;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options =>
{
    options.ReturnHttpNotAcceptable = true;
}).AddNewtonsoftJson();

builder.Services.Configure<AppSettings>(builder.Configuration);
var settings = builder.Configuration.Get<AppSettings>();
ArgumentNullException.ThrowIfNull(settings, nameof(settings));

builder.Services.AddDbContext<QuickLinkerDbContext>(options =>
    options.UseSqlServer(settings.ConnectionStrings.QuickLinkerDbContextConnection));

builder.Services.AddTransient<IQuickLinkerRepository, QuickLinkerRepository>();
builder.Services.AddScoped<IShortLinkService, ShortLinkService>();

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = settings.ConnectionStrings.QuickLinkerRedisConnection;
    options.InstanceName = "QuickLinker";
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(setupAction =>
{
    var xmlCommentsFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlCommentsFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentsFile);

    setupAction.IncludeXmlComments(xmlCommentsFullPath);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler(appBuilder =>
    {
        appBuilder.Run(async context =>
        {
            context.Response.StatusCode = 500;
            await context.Response.WriteAsync("An unexpected fault happened. Try again later.", cancellationToken: context.RequestAborted);
        });
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
