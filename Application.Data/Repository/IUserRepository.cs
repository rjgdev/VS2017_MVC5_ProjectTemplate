using CenGts.Data.Models;
using CenGts.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CenGts.Data.Repository
{
    public interface IUserRepository
    {
        ApplicationUser GetUser(int id);

        void AddUser(ApplicationUser user);

        void DeleteUser(ApplicationUser user);

        void UpdateUser(ApplicationUser user);
    }
}
