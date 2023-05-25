using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;

namespace Framework
{
    public static class CommonExtention
    {
        public static string ToBase64Image(this byte[] bytes)
        {
            string base64String = Convert.ToBase64String(bytes, 0, bytes.Length);
            return $"data:image/png;base64,{base64String}";
        }

        public static string TryGetValue(this JObject obj, string propertyName)
        {
            try
            {
                return obj[propertyName]?.ToString() ?? string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }

        public static bool ToBool(this bool? item) => item ?? false;

        public static string ToStringBool(this bool? item) => item == true ? "true" : "false";

        public static void ToSessionSet(this ISession session, string key, object? value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T? ToSessionGet<T>(this ISession session, string key)
        {
            var value = session.GetString(key);

            return value == null ? default : JsonConvert.DeserializeObject<T>(value);
        }

        public static void SessionSet(this ISession session, string key, string value)
        {
            session.SetString(key, value);
        }

        public static string SessionGet<T>(this ISession session, string key)
        {
            var value = session.GetString(key);

            return value ?? string.Empty;
        }

        public static bool ToAnyTry<T>(this List<T>? obj)
        {
            if (obj == null)
                return false;

            return obj.Any();
        }
    }
}