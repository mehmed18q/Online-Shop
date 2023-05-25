namespace Framework
{
    public static class NumericExtention
    {
        public static bool IsZero(this int value)
        {
            return value == 0;
        }
        public static bool IsNotZero(this int value) => !IsZero(value);
        public static bool IsZeroOrNull(this int? value)
        {
            return (value == null || value == 0);
        }
        public static bool IsNotZeroOrNull(this int? value) => !IsZeroOrNull(value);

        public static bool IsZero(this double value)
        {
            return value == 0;
        }
        public static bool IsNotZero(this double value) => !IsZero(value);
        public static bool IsZeroOrNull(this double? value)
        {
            return (value == null || value == 0);
        }
        public static bool IsNotZeroOrNull(this double? value) => !IsZeroOrNull(value);

        public static bool IsZero(this long value)
        {
            return value == 0;
        }
        public static bool IsNotZero(this long value) => !value.IsZero();
        public static bool IsZeroOrNull(this long? value)
        {
            return (value == 0 || value == null);
        }
        public static bool IsNotZeroOrNull(this long? num) => !num.IsZeroOrNull();
        
        public static string ToPeridString(this int? period)
        {
            if (period == null)
                return string.Empty;

            var h = period / 60;
            return $" {h} h ";
        }

        public static string To3Digit(this decimal data)
        {
            var result = string.Format("{0:0,0.##############}", data);

            if (result[..1] == "0")
                result = result[1..];

            return result;
        }

        public static string To3Digit(this decimal? data)
        {
            if (data == null)
                return string.Empty;

            return ((decimal)data).To3Digit();
        }

        public static decimal ToDecimal(this decimal? data)
        {
            return data ?? 0;
        }

        public static string ToTryInt(this int? data)
        {
            if (data == null)
                return string.Empty;

            return ((decimal)data).To3Digit();
        }

        public static int ToTryInt(this Double? data)
        {
            if (data == null)
                return 0;

            return (int)data;
        }

        public static string To3Digit(this Double? data)
        {
            if (data == null)
                return string.Empty;

            return ((decimal)data).To3Digit();
        }

        public static string To3Digit(this int? data)
        {
            if (data == null)
                return string.Empty;

            return ((decimal)data).To3Digit();
        }

        public static int ToInt(this int? data)
        {
            return data ?? 0;
        }

        public static bool IsInPatient(this int? data)
        {
            return (data ?? 0) == 1;
        }

        public static bool IsNullOrZero(this int? data)
        {
            return data is null or 0;
        }

        public static long ToLong(this long? data)
        {
            return data ?? 0;
        }

        public static string ToSepratedDigit(this double str)
        {
            return str.ToString().DigitSeprate();
        }

        public static string ToFileSizeText(this long len)
        {
            string[] sizes = { "B", "KB", "MB", "GB", "TB" };

            int order = 0;
            while (len >= 1024 && order < sizes.Length - 1)
            {
                order++;
                len /= 1024;
            }

            // Adjust the format string to your preferences. For example "{0:0.#}{1}" would
            // show a single decimal place, and no space.
            string result = string.Format("{0:0.##} {1}", len, sizes[order]);

            return result;
        }
    }
}