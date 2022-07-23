using System;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using ClassLibrary;

namespace Alfetta159.Functions
{
    public class TimerTrigger1
    {
        private readonly ILogger _logger;
        private readonly Class1 One;
        private readonly Class2 Two;
        private readonly Class3 Three;

        public TimerTrigger1(Class1 class1, Class2 class2, Class3 class3, ILoggerFactory loggerFactory)
        {
            One = class1;
            Two = class2;
            Three = class3;

            _logger = loggerFactory.CreateLogger<TimerTrigger1>();
        }

        [Function("TimerTrigger1")]
        public void Run([TimerTrigger("*/10 * * * * *", RunOnStartup = true)] MyInfo myTimer)
        {
            _logger.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
            // _logger.LogInformation($"Next timer schedule at: {myTimer.ScheduleStatus.Next}");

            One.Run();
            Two.Run();
            Three.Run();
        }
    }

    public class MyInfo
    {
        public MyScheduleStatus ScheduleStatus { get; set; }

        public bool IsPastDue { get; set; }
    }

    public class MyScheduleStatus
    {
        public DateTime Last { get; set; }

        public DateTime Next { get; set; }

        public DateTime LastUpdated { get; set; }
    }
}
