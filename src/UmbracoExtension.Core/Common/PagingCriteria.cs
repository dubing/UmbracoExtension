using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UmbracoExtension.Core.Common
{
    public sealed class PagingCriteria
    {
        private int _pageNumber;
        private int _pageSize;

        public int PageNumber
        {
            get
            {
                return _pageNumber;
            }
            set
            {
                _pageNumber = (value < 1) ? 0 : value;
            }
        }

        public int PageSize
        {
            get
            {
                return _pageSize;
            }
            set
            {
                _pageSize = (value < 1) ? 1 : value;
            }
        }

        /// <summary>
        /// Page number is 1 and page size is 10
        /// </summary>
        public static PagingCriteria Default
        {
            get
            {
                return new PagingCriteria() { PageNumber = 1, PageSize = 10 };
            }
        }
    }
}
