using System;

namespace DownloadService
{
    public class Clock
    {
        public static string GetTime()
        {
            return string.Format("{0:MM/dd/yy H:mm:ss}", TimeZoneInfo
                .ConvertTime(DateTime.Now, TimeZoneInfo.CreateCustomTimeZone("Minsk time", new TimeSpan(3, 0, 0), "Time", "Time")));
        }
    }
}
