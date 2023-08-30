using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQueryableFilter.Models
{
    public class PaginationModel<T> where T : class
    {
        public List<T> Data { get; set; } = new();
        public int TotalCount { get; set; } = 0;
    }
}
