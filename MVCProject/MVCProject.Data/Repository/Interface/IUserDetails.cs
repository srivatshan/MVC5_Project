using MVCProject.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCProject.Data.Repository.Interface
{
   public interface IUserDetails
    {
        void Add(User user);
        User GetUser(string UserName, string Password);
    }
}
