using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using UmbracoExtension.Core.Common;
using umbraco;
using umbraco.NodeFactory;

namespace UmbracoExtension.Core.Umbraco
{
    public class UmbracoUtility : IUmbracoUtility
    {

        private static readonly object _lock = new object();

        public Node RootNode
        {
            get
            {
                string cacheKey = typeof(UmbracoUtility).FullName;
                if (!string.IsNullOrWhiteSpace(cacheKey))
                {
                    if (!MemoryCache.Default.Contains(cacheKey))
                    {
                        lock (_lock)
                        {
                            if (!MemoryCache.Default.Contains(cacheKey))
                            {
                                MemoryCache.Default.Add(cacheKey, uQuery.GetRootNode(),
                                                        new DateTimeOffset(DateTime.Now.AddMinutes(5)));
                            }
                        }
                    }

                    return MemoryCache.Default[cacheKey] as Node;
                }
                return null;
            }
        }

        



    }
}
