using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GainVocab.API.Core.Models.Pager
{
    public class PagerParams
    {
        private int _pageSize = 10;

        public int PageNumber { get; set; }
        public string? SortDirection { get; set; }
        public string? SortBy { get; set; }

        public int PageSize
        {
            get
            {
                return _pageSize;
            }
            set
            {
                _pageSize = value;
            }
        }
    }
}
