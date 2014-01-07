using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Http;

namespace UmbracoExtension.Web.WebAPI
{
    public class DemoApiController :BaseWebAPIController
    {
        [HttpGet]
        public string GetDemo()
        {
            return "hello world";
        }



    }
}
