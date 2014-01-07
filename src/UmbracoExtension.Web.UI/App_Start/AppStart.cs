using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;
using UmbracoExtension.Core.Unity;
using UmbracoExtension.Web.Attribute;
using UmbracoExtension.Web.Config;
using Microsoft.Practices.Unity;
using UmbracoExtension.Core.Unity;
using global::Combres;

[assembly: WebActivator.PreApplicationStartMethod(typeof(UmbracoExtension.Web.UI.App_Start.AppStart), "PreStart")]
namespace UmbracoExtension.Web.UI.App_Start
{
    public static class AppStart
    {
        public static void PreStart()
        {
            RouteTable.Routes.AddCombresRoute("Combres");
            GlobalConfiguration.Configuration.Filters.Add(UnityHelper.Container.Resolve<ExceptionHandlingAttribute>());
            AutoMapperWebConfiguration.Configure();
        }
    }
}