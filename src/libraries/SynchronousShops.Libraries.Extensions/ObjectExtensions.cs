using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using SynchronousShops.Libraries.Extensions.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Xml.Serialization;

namespace SynchronousShops.Libraries.Extensions
{
    public static class ObjectExtensions
    {
        public static object GetPropertyValue(this object source, string propertyName)
        {
            return source.GetType()
                .GetRuntimeProperties()
                .FirstOrDefault(p => string.Equals(p.Name, propertyName, StringComparison.OrdinalIgnoreCase))
                ?.GetValue(source);
        }

        public static bool TryGetPropertyValue<T>(this object source, string propertyName, out T result)
        {
            var property = source.GetType()
                .GetRuntimeProperties()
                .FirstOrDefault(p => string.Equals(p.Name, propertyName, StringComparison.OrdinalIgnoreCase));

            if (property != null)
            {
                result = (T)property.GetValue(source);
                return true;
            }
            result = default;
            return false;
        }

        public static void SetPropertyValue<T>(this object source, string propertyName, T value)
        {
            source.GetType()
                .GetRuntimeProperties()
                .FirstOrDefault(p => string.Equals(p.Name, propertyName, StringComparison.OrdinalIgnoreCase))
                ?.SetValue(source, value);
        }

        public static bool TryInvoke<T>(this object source, string methodName, out T result, params object[] args)
        {
            var method = source.GetType()
                .GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance);
            if (method != null)
            {
                result = (T)method.Invoke(source, args);
                return true;
            }
            result = default;
            return false;
        }

        public static bool IsAssignableToGenericType(this object source, Type genericType)
        {
            return source.GetType().IsAssignableToGenericType(genericType);
        }

        public static string ToJson(this object obj)
        {
            if (obj == null)
            {
                return string.Empty;
            }
            var token = JsonConvert.SerializeObject(
                obj,
                new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    ContractResolver = new CamelCasePropertyNamesContractResolver(),
                }
            );
            return token.ToString();
        }

        public static string ToXML(this object obj)
        {
            using var stringWriter = new UTF8StringWriter();
            var serializer = new XmlSerializer(obj.GetType());
            serializer.Serialize(stringWriter, obj);
            return stringWriter.ToString();
        }

        public static FormUrlEncodedContent ToFormUrlEncodedContent(this object o)
        {
            var json = JsonConvert.SerializeObject(o);
            var dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
            return new FormUrlEncodedContent(dictionary);
        }
    }
}
