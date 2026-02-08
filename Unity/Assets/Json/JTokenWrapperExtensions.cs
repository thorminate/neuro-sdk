#nullable enable

using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace NeuroSdk.Json
{
    public static class JTokenWrapperExtensions
    {
        public static T? GetValue<T>(this IJTokenWrapper data, string key, T? defaultValue)
        {
            var jToken = data.Data?[key];
            return jToken != null ? jToken.Value<T>() : defaultValue;
        }

        public static T? GetValue<T>(this IJTokenWrapper data, string key) where T : class
        {
            var jToken = data.Data?[key];
            return jToken?.Value<T>();
        }

        public static IEnumerable<T?> GetArray<T>(this IJTokenWrapper data, string key)
        {
            return data.Data?[key]?.Values<T?>() ?? Enumerable.Empty<T?>();
        }

        public static T? Get<T>(this IJTokenWrapper data, string path, T? defaultValue = default)
        {
            return data.TryGet(path, out T? value) ? value : defaultValue;
        }

        public static bool TryGet<T>(this IJTokenWrapper data, string path, out T? value)
        {
            value = default;

            var token = data.Data?.SelectToken(path);
            if (token is null || token.Type == JTokenType.Null)
                return false;

            try
            {
                value = token.ToObject<T>();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static string? GetString(this IJTokenWrapper data, string path, string? defaultValue = null)
        {
            return data.Get(path, defaultValue);
        }

        public static bool TryGetString(this IJTokenWrapper data, string path, out string? value)
        {
            return data.TryGet(path, out value);
        }

        public static int GetInt(this IJTokenWrapper data, string path, int defaultValue = 0)
        {
            return data.Get(path, defaultValue);
        }

        public static bool TryGetInt(this IJTokenWrapper data, string path, out int value)
        {
            value = 0;
            if (!data.TryGet(path, out int? val)) return false;

            value = val.GetValueOrDefault();
            return true;
        }

        public static float GetFloat(this IJTokenWrapper data, string path, float defaultValue = 0f)
        {
            return data.Get(path, defaultValue);
        }

        public static bool TryGetFloat(this IJTokenWrapper data, string path, out float value)
        {
            value = 0f;
            if (!data.TryGet(path, out float? val)) return false;

            value = val.GetValueOrDefault();
            return true;
        }

        public static bool GetBool(this IJTokenWrapper data, string path, bool defaultValue = false)
        {
            return data.Get(path, defaultValue);
        }

        public static bool TryGetBool(this IJTokenWrapper data, string path, out bool value)
        {
            value = false;
            if (!data.TryGet(path, out bool? val)) return false;

            value = val.GetValueOrDefault();
            return true;
        }

        public static JObject? GetObject(this IJTokenWrapper data, string path)
        {
            return data.Get<JObject>(path);
        }

        public static bool TryGetObject(this IJTokenWrapper data, string path, out JObject? obj)
        {
            obj = null;
            if (data.Data?.SelectToken(path) is not JObject token) return false;

            obj = token;
            return true;
        }

        public static JArray? GetArray(this IJTokenWrapper data, string path)
        {
            return data.Get<JArray>(path);
        }

        public static bool TryGetArray(this IJTokenWrapper data, string path, out JArray? array)
        {
            array = null;
            if (data.Data?.SelectToken(path) is not JArray token) return false;

            array = token;
            return true;
        }

        public static bool Contains(this IJTokenWrapper data, string path)
        {
            return data.Data?.SelectToken(path) != null;
        }

        public static bool HasValue(this IJTokenWrapper data, string path)
        {
            return data.Data?.SelectToken(path) is { Type: not JTokenType.Null };
        }

        public static bool IsNull(this IJTokenWrapper data, string path)
        {
            return data.Data?.SelectToken(path)?.Type == JTokenType.Null;
        }

        public static Dictionary<string, object>? ToDictionary(this IJTokenWrapper data)
        {
            return data.Data?.ToObject<Dictionary<string, object>>();
        }

        public static JToken? DeepCopy(this IJTokenWrapper data)
        {
            return data.Data?.DeepClone();
        }

        public static T? ToObject<T>(this IJTokenWrapper data)
        {
            if (data.Data == null) return default;

            try
            {
                return data.Data.ToObject<T>();
            }
            catch
            {
                return default;
            }
        }
    }
}