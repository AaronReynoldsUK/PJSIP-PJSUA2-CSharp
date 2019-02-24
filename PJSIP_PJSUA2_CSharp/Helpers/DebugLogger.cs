using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PJSIP_PJSUA2_CSharp
{
    public static class DebugLogger
    {
        public static void LogEvent(object eventArgs)
        {
            var __actualType = eventArgs.GetType();

            System.Diagnostics.Debug.WriteLine(String.Format("Event={0}", __actualType.Name));

            var __properties = eventArgs.GetType().GetProperties();
            foreach (var __property in __properties)
            {
                var __propertyTypeFullName = __property.PropertyType.FullName;
                var __propertyTypeName = __property.PropertyType.Name;
                var __propertyName = __property.Name;

                switch (__propertyTypeName)
                {
                    case "String":
                        System.Diagnostics.Debug.WriteLine(String.Format("{0}={1}", __propertyName, __property.GetValue(eventArgs) as String));
                        break;
                    case "DateTime":
                        System.Diagnostics.Debug.WriteLine(String.Format("{0}={1:yyyy-MM-dd HH-mm-ss}", __propertyName, __property.GetValue(eventArgs) as DateTime?));
                        break;
                    case "Double":
                        System.Diagnostics.Debug.WriteLine(String.Format("{0}={1}", __propertyName, __property.GetValue(eventArgs) as Double?));
                        break;
                    case "Int32":
                        System.Diagnostics.Debug.WriteLine(String.Format("{0}={1}", __propertyName, __property.GetValue(eventArgs) as Int32?));
                        break;
                    case "Boolean":
                        System.Diagnostics.Debug.WriteLine(String.Format("{0}={1}", __propertyName, GetBooleanAsYN(__property.GetValue(eventArgs))));
                        break;

                    default:
                        break;
                }
            }
        }

        private static string GetBooleanAsYN(object value)
        {
            var __boolValue = value as Boolean?;

            if (__boolValue.HasValue)
            {
                return __boolValue.Value ? "True" : "False";
            }

            return "No Value";
        }
    }
}
