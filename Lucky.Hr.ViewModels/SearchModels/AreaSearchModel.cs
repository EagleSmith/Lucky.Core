using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Lucky.Hr.Core.Specification;
using Lucky.Hr.Entity;

namespace Lucky.Hr.ViewModels.Models
{
    public class AreaSearchModel
    {
        public int PageIndex { get; set; }
        public string OrderBy { get; set; }
        public string Order { get; set; }
        public string keyword { get; set; }

        public Expression<Func<Area, bool>> Expression(ISpecification<Area> specification)
        {
            if (!string.IsNullOrEmpty(keyword))
                specification.Like(a => a.AreaName, this.keyword);
            return specification.Predicate;
        }
    }
}
