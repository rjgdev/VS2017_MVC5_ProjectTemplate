using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Application.Data.Models;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Application.Data.Repository
{
    public class UserRepository : IUserRepository, IDisposable
    {
        private readonly ApplicationUserManager _userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(new ApplicationDbContext()));

        private readonly ApplicationDbContext _db = new ApplicationDbContext();


        public long Add(ApplicationUser obj)
        {
            throw new NotImplementedException();
            //_db.Users.Create(obj, obj.Password);
        }

        public bool Delete(long id)
        {
            throw new NotImplementedException();
        }

        public void Detach(ApplicationUser obj)
        {
            throw new NotImplementedException();
        }

        public ApplicationUser Get(Expression<Func<ApplicationUser, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ApplicationUser> GetAll()
        {
            throw new NotImplementedException();
        }

        public ApplicationUser GetById(long id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ApplicationUser> GetList(Expression<Func<ApplicationUser, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public bool Update(ApplicationUser obj)
        {
            throw new NotImplementedException();
        }

        #region dispose
        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


        /// <summary>
        ///     Dispose
        /// </summary>
        /// <param name="disposing"></param>
        protected void Dispose(bool disposing)
        {
            if (disposing)
                if (_db != null)
                {
                    _db.Dispose();
                }
        }

        #endregion dispose
    }
}
