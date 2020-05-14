using MVCProject.Data.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCProject.Data.Repository.Repo
{
    public class ApplicationDBContext : DbContext
    {
        public DbSet<User> UserDetails { get; set; }
    }
}
