using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Data.Models;

namespace Application.Data.Repository
{
    public interface IUserRepository : IRepository<ApplicationUser>
    {
    }
}
