using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Newtonsoft.Json;

namespace Framework
{
    public static class ObjectExtension
    {
        public static string TryToString(this object obj)
        {
            if (obj == null) return string.Empty;
            return obj.ToString() ?? string.Empty;
        }

        public static int TryToInt(this object obj)
        {
            if (obj == null) return 0;

            bool v = int.TryParse(obj.ToString(), out var value);
            return v ? value : 0;
        }

        public static long TryToLong(this object obj)
        {
            if (obj == null) return 0;

            bool v = long.TryParse(obj.ToString(), out var value);
            return v ? value : 0;
        }

        public static string ToJson(this object obj)
        {
            var stringValue = obj.TryToString();
            if (stringValue == string.Empty) return string.Empty;
            return JsonConvert.SerializeObject(obj);
        }

        public static T? MapTo<T>(this object obj)
        {
            var stringValue = obj.TryToString();
            if (stringValue == string.Empty) return default;
            return JsonConvert.DeserializeObject<T>(obj.ToJson());
        }

        public static string GetStringProperty(this object value, string propertyName)
           => Convert.ToString(value.GetType().GetProperty(propertyName)?.GetValue(value) ?? string.Empty) ?? string.Empty;

        public static bool GetBooleanProperty(this object value, string propertyName)
            => Convert.ToBoolean(value.GetType().GetProperty(propertyName)?.GetValue(value) ?? false);

        public static int GetIntProperty(this object value, string propertyName)
        {
            var propertyValue = value.GetType().GetProperty(propertyName)?.GetValue(value);
            if (propertyValue is null) return 0;
            return propertyValue.TryToString().TryToInt();
        }

        public static void SetStringProperty(this object value, string propertyName, string setValue)
        {
            var property = value.GetType().GetProperty(propertyName);

            if (property?.PropertyType == typeof(string))
                property.SetValue(value, setValue);
        }

        public static void SetIntProperty(this object value, string propertyName, int? setValue)
        {
            var property = value.GetType().GetProperty(propertyName);

            if (property?.PropertyType == typeof(int) || property?.PropertyType == typeof(int?))
                property.SetValue(value, setValue);
        }

        public static void SetLongProperty(this object value, string propertyName, long setValue)
        {
            var property = value.GetType().GetProperty(propertyName);

            if (property?.PropertyType == typeof(long))
                property.SetValue(value, setValue);
        }

        public static IDictionary<string, object> ToDictionaryData(this object source)
        {
            return HtmlHelper.AnonymousObjectToHtmlAttributes(source);
        }

        public static bool ToTryAny(this List<object>? list)
        {
            return (list != null && list.Any());
        }

        public static bool ToTryAny(this IEnumerable<object>? list)
        {
            return (list != null && list.Any());
        }
    }
}