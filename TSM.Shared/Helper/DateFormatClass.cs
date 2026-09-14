

namespace TMS.Shared.Helper
{
    public static class DateFormatClass
    {

        public static DateTime CurrentDate()
        {
            return DateTime.Now;
        }
        public static DateTime CurrentUtcDate()
        {
            return DateTime.UtcNow;
        }
        public static DateTime CurrentDay()
        {
            return DateTime.Now.Date;
        }

        public static DateTime CurrentUtcDay()
        {
            return DateTime.UtcNow.Date;
        }
        public static DateTime GetUtcDate(DateTime date)
        {
            return date.ToUniversalTime();
        }
        public static DateTime GetLocalDate(DateTime date)
        {
            return date.ToLocalTime();
        }
    }
}
