using Microsoft.EntityFrameworkCore;
using NeerCore.Api;
using NeerCore.Api.Extensions;
using NeerCore.Api.Extensions.Swagger;
using NeerCore.Data.EntityFramework;
using NeerCore.Mapping.Extensions;
using NLog;
using TraineeTemplate.Api;
using TraineeTemplate.Api.Data;

var logger = LoggerInstaller.InitDefault();

try
{
    var builder = WebApplication.CreateBuilder(args);
    ConfigureBuilder(builder);

    var app = builder.Build();
    ConfigureWebApp(app);

    app.Run();
}
catch (Exception e)
{
    logger.Fatal(e);
}
finally
{
    logger.Info("Application is now stopping");
    LogManager.Shutdown();
}

// ==========================================

static void ConfigureBuilder(WebApplicationBuilder builder)
{
    builder.Services.AddDatabase<SqliteDbContext>(db =>
        db.UseSqlite(builder.Configuration.GetConnectionString("Sqlite")));

    builder.AddNeerApi();
    builder.Services.RegisterMapper<MapperRegister>();
}

static void ConfigureWebApp(WebApplication app)
{
    if (app.Configuration.GetSwaggerSettings().Enabled)
        app.UseCustomSwagger();

    app.UseCors(CorsPolicies.AcceptAll);
    app.UseHttpsRedirection();

    app.UseCustomExceptionHandler();

    app.MapControllers();
}