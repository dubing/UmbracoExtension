using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using AKQA.Common.Utility;

namespace UmbracoExtension.Web.Utility
{
    public static class WebUtility
    {
        /// <summary>
        /// Check user clientData
        /// </summary>
        /// <param name="clientData"></param>
        public static void CheckClientDataIsSafe(string clientData)
        {
            if (clientData == null) return;
            if (clientData == string.Empty) return;
            if (clientData.IndexOf("'") >= 0)
            {
                throw new Exception("clientData is unsafe");
            }

        }

        /// <summary>
        /// Get user IP
        /// </summary>
        /// <returns></returns>
        public static string GetClientIP()
        {
            if (HttpContext.Current == null)
                return string.Empty;
            var request = HttpContext.Current.Request;

            string ipList = TypeHelper.Parse(request.ServerVariables["HTTP_X_FORWARDED_FOR"], request.ServerVariables["REMOTE_ADDR"]);
            string ipAddress = ipList.Split(',', ';')[0];

            return TypeHelper.Parse(ipAddress, request.UserHostAddress);
        }

        /// <summary>
        /// GetRequestValue
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="name">The name.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        public static string GetRequestValue(HttpRequest request, string name, string defaultValue)
        {
            if (request == null || string.IsNullOrWhiteSpace(name))
                return defaultValue;

            string result = request.Form[name];
            if (string.IsNullOrWhiteSpace(result))
            {
                result = request.QueryString[name];
                if (string.IsNullOrWhiteSpace(result))
                    result = defaultValue;
            }
            return result;
        }

        /// <summary>
        /// Verify string object with specify regix pattern
        /// </summary>
        /// <param name="data"></param>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public static bool VerifyDataWithRegex(string data, string pattern)
        {
            CheckClientData.CheckClientDataIsSafe(data);
            if (!string.IsNullOrWhiteSpace(data))
                return Regex.IsMatch(data.Trim(), pattern);
            return false;
        }

        /// <summary>
        /// Returns the physical file path that corresponds to the specified virtual path on the Web server.
        /// <summary>
        /// GetApplicationPath
        /// </summary>
        public static string GetApplicationPath()
        {
            int requestPort = HttpContext.Current.Request.Url.Port;
            if (requestPort == 80)
            {
                return string.Format("http://{0}{1}/", HttpContext.Current.Request.Url.Host, HttpContext.Current.Request.ApplicationPath.TrimEnd('/'));
            }
            else
            {
                return string.Format("http://{0}:{1}{2}/", HttpContext.Current.Request.Url.Host, requestPort, HttpContext.Current.Request.ApplicationPath.TrimEnd('/'));
            }
        }

        /// </summary>
        /// <param name="virtualPath"></param>
        /// <returns></returns>
        public static string MapServerPath(string virtualPath)
        {
            return HttpContext.Current.Server.MapPath(virtualPath);
        }

        /// <summary>
        /// Get ClientUrl
        /// </summary>
        /// <param name="maxLength">Length of the max.</param>
        /// <returns></returns>
        public static string GetClientUrl(int maxLength = 1024)
        {
            var ctx = HttpContext.Current;
            if (ctx == null)
                return string.Empty;

            if (ctx.Request.UrlReferrer != null)
            {
                return TypeHelper.GetSubString(ctx.Request.UrlReferrer.ToString(), maxLength);
            }
            return TypeHelper.GetSubString(ctx.Request.Url.ToString(), maxLength);
        }


    }
}
