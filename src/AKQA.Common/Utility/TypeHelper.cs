using System;
using System.Linq;

namespace AKQA.Common.Utility
{
    /// <summary>
    /// Summary description for TypeHelper.
    /// </summary>
    public static class TypeHelper
    {
        /// <summary>
        /// string ==&gt; int
        /// </summary>
        /// <param name="inputText">The input text.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        public static int Parse(string inputText, int defaultValue)
        {
            if (string.IsNullOrWhiteSpace(inputText))
                return defaultValue;
            int result;
            return int.TryParse(inputText, out result) ? result : defaultValue;
        }

        /// <summary>
        /// bool ==&gt; bool
        /// </summary>
        /// <param name="inputText">The input text.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        public static bool Parse(bool? inputText, bool defaultValue)
        {
            if (inputText == null)
                return defaultValue;
            return (bool)inputText;
        }


        /// <summary>
        /// string ==&gt; long
        /// </summary>
        /// <param name="inputText">The input text.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        public static long Parse(string inputText, long defaultValue)
        {
            if (string.IsNullOrWhiteSpace(inputText))
                return defaultValue;
            long result;
            return long.TryParse(inputText, out result) ? result : defaultValue;
        }

        /// <summary>
        /// string ==&gt; bool
        /// </summary>
        /// <param name="inputText">The input text.</param>
        /// <param name="defaultValue">if set to <c>true</c> [default value].</param>
        /// <returns></returns>
        public static bool Parse(string inputText, bool defaultValue)
        {
            if (string.IsNullOrWhiteSpace(inputText))
                return defaultValue;
            bool result;
            return bool.TryParse(inputText, out result) ? result : defaultValue;
        }

        /// <summary>
        /// string ==&gt; DateTime
        /// </summary>
        /// <param name="inputText">The input text.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        public static DateTime Parse(string inputText, DateTime defaultValue)
        {
            if (string.IsNullOrWhiteSpace(inputText))
                return defaultValue;
            DateTime result;
            return DateTime.TryParse(inputText, out result) ? result : defaultValue;
        }

        /// <summary>
        /// string ==&gt; int[]
        /// </summary>
        /// <param name="inputText">The input text.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        public static int[] Parse(string inputText, int[] defaultValue)
        {
            if (string.IsNullOrWhiteSpace(inputText))
                return defaultValue;
            try
            {
                return inputText.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
            }
            catch
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// string ==&gt; Guid
        /// </summary>
        /// <param name="inputText">The input text.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        public static Guid Parse(string inputText, Guid defaultValue)
        {
            if (string.IsNullOrWhiteSpace(inputText))
                return defaultValue;
            try
            {
                return new Guid(inputText);
            }
            catch
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// string ==&gt; Enum
        /// </summary>
        /// <param name="inputText">The input text.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        public static T Parse<T>(string inputText, T defaultValue)
        {
            if (string.IsNullOrWhiteSpace(inputText))
                return defaultValue;
            try
            {
                return (T)Enum.Parse(defaultValue.GetType(), inputText, true);
            }
            catch
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// string ==&gt; double
        /// </summary>
        /// <param name="inputText">The input text.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        public static double Parse(string inputText, double defaultValue)
        {
            if (string.IsNullOrWhiteSpace(inputText))
            {
                return defaultValue;
            }

            double result;
            return double.TryParse(inputText, out result) ? result : defaultValue;
        }

        /// <summary>
        /// string ==&gt; string
        /// </summary>
        /// <param name="inputText">The input text.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        public static string Parse(string inputText, string defaultValue)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(inputText))
                    return defaultValue;
                return inputText;
            }
            catch (Exception)
            {
                return defaultValue;
            }

        }

        /// <summary>
        /// GetSubString
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="maxLength">Length of the max.</param>
        /// <returns></returns>
        public static string GetSubString(string value, int maxLength)
        {
            if (string.IsNullOrWhiteSpace(value))
                return string.Empty;
            return value.Length > maxLength ? value.Substring(0, maxLength) : value;
        }

        /// <summary>
        /// try to return a appropriate type
        /// </summary>
        /// <param name="typeName">Name of the type.</param>
        /// <param name="defaultType">The default type.</param>
        /// <param name="ignoreCase">if set to <c>true</c> [ignore case].</param>
        /// <returns></returns>
        public static Type GetType(string typeName, Type defaultType, bool ignoreCase)
        {
            Type ret = null;
            if (!string.IsNullOrWhiteSpace(typeName))
                ret = Type.GetType(typeName, false, ignoreCase);

            return ret ?? defaultType;
        }

        /// <summary>
        /// try to return a appropriate type
        /// </summary>
        /// <param name="typeName">Name of the type.</param>
        /// <param name="defaultType">The default type.</param>
        /// <returns></returns>
        public static Type GetType(string typeName, Type defaultType)
        {
            return GetType(typeName, defaultType, false);
        }

        /// <summary>
        /// whether is type inherit from baseType
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="baseType">Type of the base.</param>
        /// <returns>
        /// 	<c>true</c> if [is inherit base] [the specified type]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsInheritBase(Type type, Type baseType)
        {
            if (baseType == null)
                return false;
            if (type == null)
                return false;
            if (type == baseType)
                return true;
            if (type.BaseType == baseType)
                return true;

            if (type.GetInterfaces().Any(interfaceType => interfaceType == baseType))
            {
                return true;
            }

            return IsInheritBase(type.BaseType, baseType);
        }

        /// <summary>
        /// GetArrayItemType
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static Type GetArrayItemType(Type type)
        {
            if (!type.IsArray)
                return null;
            return type.GetElementType();
        }

    }
}
