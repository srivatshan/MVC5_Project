using MVCProject.Data.Models;
using MVCProject.Data.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCProject.Data.Repository.Repo
{
    public class UserDetails : IUserDetails
    {
        private readonly ApplicationDBContext _db;
        public UserDetails(ApplicationDBContext applicationDBContext)
        {
            _db = applicationDBContext;
        }
        public void Add(User user)
        {
            _db.UserDetails.Add(user);
            _db.SaveChanges();
        }

        public User GetUser(string UserName, string Password)
        {
            return _db.UserDetails.Where(x => x.UserName == UserName && x.Password == Password).FirstOrDefault();
        }
    }
}
