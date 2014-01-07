using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Web.Http.Filters;
using UmbracoExtension.Core.Exceptions;
using UmbracoExtension.Core.Logging;

namespace UmbracoExtension.Web.Attribute
{
    public class ExceptionHandlingAttribute : ExceptionFilterAttribute
    {
        #region Private Members

        private ILogger _logger;

        #endregion

        #region Constructors

        public ExceptionHandlingAttribute(ILogger logger)
        {
            _logger = logger;
        }

        #endregion

        #region Overrides

        public override void OnException(HttpActionExecutedContext context)
        {
            _logger.Error(context.Exception.Message, context.Exception);

            if (context.Exception is BusinessException)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent(context.Exception.Message),
                    ReasonPhrase = "Exception"
                });

            }

            throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError)
            {
                Content = new StringContent("An error occurred, please try again or contact the administrator."),
                ReasonPhrase = "Critical Exception"
            });
        }

        #endregion
    }
}
