using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UmbracoExtension.Core.Common
{
    public class PagingData<T>
    {
        public IEnumerable<T> Items { get; set; }

        public int TotalCount { get; set; }
    }

    public sealed class PagingData<T, T1> : PagingData<T>
    {
        public T1 ExtensionObj { get; set; }
    }
}
