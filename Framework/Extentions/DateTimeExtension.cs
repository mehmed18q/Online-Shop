using System.Globalization;

namespace Framework
{
    public static class DateTimeExtension
    {
        public static bool IsValidDate(this DateTime? value) => value.HasValue && value > DateTime.MinValue;

        public static bool IsValidDate(this DateTime value) => value > DateTime.MinValue;

        public static string? ToStringDateTime(this DateTime? value) => value.IsValidDate() ? (value ?? DateTime.Now).ToString("yyyy-MM-dd HH:mm:ss") : null;

        public static string? ToStringDateTime(this DateTime value) => value.IsValidDate() ? value.ToString("yyyy-MM-dd HH:mm:ss") : null;

        public static string? ToStringDate(this DateTime? value) => value.IsValidDate() ? (value ?? DateTime.Now).ToString("yyyy-MM-dd") : null;

        public static string? ToStringDate(this DateTime value) => value.IsValidDate() ? value.ToString("yyyy-MM-dd") : null;

        public static string? ToShamsiDateTime(this DateTime? value)
        {
            if (!value.IsValidDate()) return null;

            var persianCalendar = new PersianCalendar();
            var date = $"{persianCalendar.GetYear(value ?? DateTime.Now)}-{persianCalendar.GetMonth(value ?? DateTime.Now):D2}-{persianCalendar.GetDayOfMonth(value ?? DateTime.Now):D2}";
            var time = (value ?? DateTime.Now).ToString("HH:mm");

            return $"{date} {time}";
        }

        public static string? ToShamsiDate(this DateTime? value)
        {
            var dateTime = value.ToShamsiDateTime();
            if (dateTime.IsNotEmpty()) return dateTime?[..10] ?? string.Empty;

            return null;
        }

        public static string ToDateAndTime(this DateTime? date)
        {
            if (date == null)
                return string.Empty;

            return Convert.ToDateTime(date).ToDateAndTime();
        }

        public static string ToDateAndTime(this DateTime date)
        {
            return date.ToString("yyyy/MM/dd HH:mm");
        }

        public static (int? year, int? month, int? day) ToAgeString(this DateTime dob)
        {
            DateTime today = DateTime.Today;

            int months = today.Month - dob.Month;
            int years = today.Year - dob.Year;

            if (today.Day < dob.Day)
                months--;

            if (months < 0)
            {
                years--;
                months += 12;
            }

            int days = (today - dob.AddMonths((years * 12) + months)).Days;

            return (years, months, days);
        }

        public static string ToDate(this DateTime? date)
        {
            if (date == null)
                return string.Empty;

            return ((DateTime)date).ToDate();
        }

        public static string ToDate(this DateTime date)
        {
            return date.ToString("yyyy/MM/dd");
        }

        public static string? ToPersian(this DateTime? value)
        {
            if (!value.HasValue) return null;
            PersianCalendar pc = new();
            return pc.GetYear(value.Value) + "/" + pc.GetMonth(value.Value).ToString("00") + "/" + pc.GetDayOfMonth(value.Value).ToString("00");
        }
    }
}