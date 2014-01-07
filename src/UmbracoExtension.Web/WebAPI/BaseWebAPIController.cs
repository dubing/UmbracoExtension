using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using AKQA.Common.Utility;
using UmbracoExtension.Web.Context;
using Umbraco.Web.WebApi;
using umbraco.cms.businesslogic.member;

namespace UmbracoExtension.Web.WebAPI
{
    public class BaseWebAPIController : UmbracoApiController
    {
        protected Member CurrentMember
        {
            get
            {
                var currentMember = LuxBioFusionContext.Current.Member;
                return currentMember;
            }
        }


        protected void ValidatePageNumber(int pageNumber)
        {
            if (pageNumber < 1)
            {
                throw new ArgumentException(string.Format("The pargeNumber, {0}, cannot be smaller than 1.", pageNumber), "pageNumber");
            }
        }

        protected void ValidatePageSize(int pageSize)
        {
            if (pageSize < 0)
            {
                throw new ArgumentException(string.Format("The pageSize, {0}, cannot be smaller than 0.", pageSize), "pageSize");
            }
        }

        protected string GetRequestFormValue(string key)
        {
            string requestFormValue = HttpContext.Current.Request.Form[key];
            if (!string.IsNullOrWhiteSpace(requestFormValue))
            {
                CheckClientData.CheckClientDataIsSafe(requestFormValue);
                requestFormValue = HttpUtility.UrlDecode(requestFormValue);
            }
            return requestFormValue;
        }

        protected string GetNotNullRequestFormValue(string key)
        {
            string requestFormValue = HttpContext.Current.Request.Form[key];
            if (!string.IsNullOrWhiteSpace(requestFormValue))
            {
                CheckClientData.CheckClientDataIsSafe(requestFormValue);
                requestFormValue = HttpUtility.UrlDecode(requestFormValue);
            }
            else
            {
                throw new ArgumentNullException(key);
            }
            return requestFormValue;
        }
    }
}
