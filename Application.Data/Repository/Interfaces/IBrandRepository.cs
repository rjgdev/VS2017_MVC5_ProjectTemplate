using Application.Model;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Application.Data.Repository
{
    public interface IBrandRepository : IRepository<Brand>
    {
        //Brand Get(Expression<Func<Brand, bool>> predicate);
        //IEnumerable<Brand> GetList(Expression<Func<Brand, bool>> predicate);
        IEnumerable<Brand> GetByProductId(long id);
        IEnumerable<Brand> GetByProductCode(string code);
        IEnumerable<Brand> GetByItemCode(string code);
        dynamic GetByBrandCode(string code);
    }
}
