using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;

namespace SynchronousShops.Libraries.Settings.Extensions
{
    internal static class ObjectExtensions
    {
        internal static IEnumerable<AzureSetting> ToAzureSettings(this object obj, string baseName = null)
        {
            var t = obj.GetType();
            var result = new List<AzureSetting>();
            foreach (var propertyInfo in t.GetProperties())
            {
                var propValue = propertyInfo.GetValue(obj, null);
                if (propertyInfo.PropertyType.IsPrimitive
                    || propertyInfo.PropertyType == typeof(string)
                    || propertyInfo.PropertyType == typeof(Uri)
                    || propertyInfo.PropertyType == typeof(TimeSpan)
                    || propertyInfo.PropertyType == typeof(TimeSpan?)
                    || propertyInfo.PropertyType == typeof(DateTime)
                    || propertyInfo.PropertyType == typeof(DateTime?)
                    || propertyInfo.PropertyType == typeof(DateTimeOffset)
                    || propertyInfo.PropertyType == typeof(DateTimeOffset?)
                )
                {
                    result.Add(
                        new AzureSetting()
                        {
                            Name = $"{baseName}:{propertyInfo.Name}",
                            Value = $"{propValue}"
                        }
                    );
                }
                else if (typeof(IEnumerable).IsAssignableFrom(propertyInfo.PropertyType))
                {
                    var enumerable = (IEnumerable)propValue;
                    var i = 0;
                    foreach (object child in enumerable)
                    {
                        result.AddRange(child.ToAzureSettings($"{baseName}:{propertyInfo.Name}:{i}"));
                        i++;
                    }
                }
                else if (propValue != null)
                {
                    result.AddRange(propValue.ToAzureSettings($"{baseName}{propertyInfo.Name}:"));
                }
            }
            return result;
        }
    }

    internal class AzureSetting
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "value")]
        public string Value { get; set; }
        [JsonProperty(PropertyName = "slotSetting")]
        public bool SlotSetting { get; set; } = false;
    }
}
