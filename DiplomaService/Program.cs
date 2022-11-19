using DiplomaService;
using NLog;
using NLog.Extensions.Logging;
using NLog.Web;

var config = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .Build();

LogManager.Configuration = new NLogLoggingConfiguration(config.GetSection("NLog"));

var logger = NLog.Web.NLogBuilder.ConfigureNLog(LogManager.Configuration).GetCurrentClassLogger();
try
{
    logger.Debug("Инициализация программы");
    CreateHostBuilder(args).Build().Run();
}
catch (Exception e)
{
    logger.Error(e, "Инициализация не удалась");
    Console.WriteLine(e);
    throw;
}
finally {
    LogManager.Shutdown();
}

static IHostBuilder CreateHostBuilder(string[] args) =>
    Host.CreateDefaultBuilder(args)
        .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); }).UseNLog();
