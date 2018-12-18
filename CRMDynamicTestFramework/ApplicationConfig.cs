using System;
using System.Configuration;

namespace CRMDynamicTestFramework
{
    public class ApplicationConfig
    {
        /// <summary>The settings collection.</summary>
        static ApplicationConfig()
        {
            TimeSpan timeSpan;
            SleepIntervalWaitTime = TimeSpan.TryParse(ConfigurationManager.AppSettings["SleepIntervalWaitTime"], out timeSpan) ? timeSpan : TimeSpan.FromMilliseconds(200);
        }

        /// <summary>
        /// Gets a System.TimeSpan value indicating how often to check for the condition to be true.
        /// </summary>
        public static TimeSpan SleepIntervalWaitTime { get; }

    }
}
