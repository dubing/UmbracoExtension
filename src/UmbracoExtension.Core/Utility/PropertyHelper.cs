using System;
using AKQA.Common.Utility;
using umbraco.interfaces;

namespace UmbracoExtension.Core.Utility
{
    public static class PropertyHelper
    {
        public static string PropertyDefaultValue(this IProperty xProperty)
        {
            return xProperty != null ? xProperty.Value : string.Empty;
        }

        public static int PropertyDefaultIntValue(this IProperty xProperty)
        {
            if (xProperty != null)
            {
                return TypeHelper.Parse(xProperty.Value, 0);
            }
            return 0;
        }

        public static DateTime PropertyDefaultDateTimeValue(this IProperty xProperty)
        {
            if (xProperty != null)
            {
                return TypeHelper.Parse(xProperty.Value, DateTime.Now);
            }
            return DateTime.Now;
        }

        public static T PropertyDefaultEnumValue<T>(this IProperty xProperty, bool ignoreCase = true) where T : struct
        {
            if (xProperty != null)
            {
                T type;
                if (Enum.TryParse(xProperty.Value, true, out type))
                {
                    return type;
                }
            }
            return default(T);
        }

        public static double PropertyDefaultDoubleValue(this IProperty xProperty)
        {
            if (xProperty != null)
            {
                return TypeHelper.Parse(xProperty.Value, double.NaN);
            }
            return double.NaN;
        }

        public static DateTime? PropertyDefaultTimeValue(this IProperty xProperty)
        {
            if (xProperty != null)
            {
                if (!string.IsNullOrWhiteSpace(xProperty.Value))
                {
                    try
                    {
                        return Convert.ToDateTime(xProperty.Value);
                    }
                    catch (Exception)
                    {
                        return null;
                    }

                }
            }
            return null;
        }
    }
}
