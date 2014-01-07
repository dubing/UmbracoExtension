using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using umbraco.cms.businesslogic.member;

namespace UmbracoExtension.Web.Context
{
    public class ContextBuilder : IContextBuilder
    {
        Member IContextBuilder.GetCurrentMember()
        {
            return Member.GetCurrentMember();
        }
    }
}
