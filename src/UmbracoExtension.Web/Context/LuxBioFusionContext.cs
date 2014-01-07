using System.Web;
using UmbracoExtension.Core.Unity;
using Microsoft.Practices.Unity;
using umbraco.cms.businesslogic.member;


namespace UmbracoExtension.Web.Context
{
    public sealed class LuxBioFusionContext
    {
        private readonly HttpContext _httpContext;
        private static readonly object _locker = new object();

        public Member Member { get; set; }

        private LuxBioFusionContext(HttpContext httpContext)
        {
            _httpContext = httpContext;
            // set current member info in context
            var contextManager = UnityHelper.Container.Resolve<IContextBuilder>();
            Member = contextManager.GetCurrentMember();
        }

        public static LuxBioFusionContext Current
        {
            get
            {
                HttpContext httpContext = HttpContext.Current;

                if (httpContext == null)
                {
                    return null;
                }

                if (!httpContext.Items.Contains(typeof(LuxBioFusionContext).FullName))
                {
                    lock (_locker)
                    {
                        if (!httpContext.Items.Contains(typeof(LuxBioFusionContext).FullName))
                        {
                            var context = new LuxBioFusionContext(httpContext);
                            httpContext.Items.Add(typeof(LuxBioFusionContext).FullName, context);
                        }
                    }
                }

                return httpContext.Items[typeof(LuxBioFusionContext).FullName] as LuxBioFusionContext;
            }
        }

    }
}
