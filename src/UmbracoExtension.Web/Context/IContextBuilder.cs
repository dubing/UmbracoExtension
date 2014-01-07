using umbraco.cms.businesslogic.member;

namespace UmbracoExtension.Web.Context
{
    public interface IContextBuilder
    {
        Member GetCurrentMember();
    }
}
