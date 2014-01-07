using System;
using System.Threading;
using System.Web;
using UmbracoExtension.Core.Logging;
using UmbracoExtension.Core.Unity;
using Microsoft.Practices.Unity;

namespace UmbracoExtension.Web.Module
{
    public class ErrorModule : IHttpModule
    {
        private readonly ILogger _logger = UnityHelper.Container.Resolve<ILogger>();

        public void Init(HttpApplication application)
        {
            application.Error += application_Error;
        }

        public void Dispose() { }


        public void application_Error(object sender, EventArgs e)
        {
            HttpContext ctx = HttpContext.Current;

            Exception exception = ctx.Server.GetLastError();

            if (exception != null)
            {
                _logger.Error(exception.Message, exception);

                Thread.Sleep(100);
            }
        }

    }
}
