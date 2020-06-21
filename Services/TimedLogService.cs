using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SCMR_Api.Controllers;
using Microsoft.Extensions.DependencyInjection;

namespace SCMR_Api
{
    public class TimedLogService : IHostedService
    {

        private readonly ILogger _logger;
        private Timer _timer;

        public IServiceProvider Services { get; }

        private List<ILogSystem> Logs;

        public TimedLogService(ILogger<TimedLogService> logger, IServiceProvider services)
        {
            Logs = new List<ILogSystem>();
            _logger = logger;
            Services = services;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Timed Background Service is starting.");

            _timer = new Timer(DoWork, null, TimeSpan.Zero,
                TimeSpan.FromMinutes(15));

            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            _logger.LogInformation("Timed Background Service is working.");

            using (var scope = Services.CreateScope())
            {
                var scopedProcessingService =
                    scope.ServiceProvider
                        .GetRequiredService<IScopedLogProcessingService>();

                scopedProcessingService.DoWork();
            }
        }

        public void AddLog(ILogSystem log)
        {
            _logger.LogInformation("Log Added AA at " + DateTime.Now.ToPersianDateWithTime());

            Logs.Add(log);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Timed Background Service is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }


    public class ScopedLogProcessingService : IScopedLogProcessingService
    {
        private readonly ILogger _logger;
        private IHostingEnvironment hostingEnvironment;
        private IConfiguration _config;

        private List<ILogSystem> Logs;

        public ScopedLogProcessingService(ILogger<ScopedLogProcessingService> logger, IHostingEnvironment _hostingEnvironment, IConfiguration config)
        {
            Logs = new List<ILogSystem>();

            _logger = logger;
            hostingEnvironment = _hostingEnvironment;
            _config = config;
        }

        public void DoWork()
        {
            _logger.LogInformation("Log Count = " + Logs.Count);
        }

        public void AddLog(ILogSystem log)
        {
            _logger.LogInformation("Log Added at " + DateTime.Now.ToPersianDateWithTime());

            Logs.Add(log);
        }



        private FileStream GetLogFileStream(string path)
        {
            return System.IO.File.Open(path, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
        }

        private string _getLogPath(DateTime date)
        {
            PersianCalendar pc = new PersianCalendar();

            var persianDateString = pc.GetYear(date).ToString() + pc.GetMonth(date).ToString("0#") + pc.GetDayOfMonth(date).ToString("0#");

            var logPath = Path.Combine(hostingEnvironment.ContentRootPath, _config["Paths:Logs"], "log_" + persianDateString + ".json");

            return logPath;
        }

        private async Task<List<ILogSystem>> getLogsOnDate(DateTime date)
        {
            var path = _getLogPath(date);

            var logs = await _ReadLogAsync(path);

            return logs;
        }

        private async Task<List<ILogSystem>> _ReadLogAsync(string path)
        {
            return JsonConvert.DeserializeObject<List<ILogSystem>>(await System.IO.File.ReadAllTextAsync(path));
        }
    }

    public interface IScopedLogProcessingService
    {
        void DoWork();


        void AddLog(ILogSystem log);
    }
}