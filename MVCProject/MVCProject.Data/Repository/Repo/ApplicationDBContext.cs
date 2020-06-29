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
        public ApplicationDBContext() 
        {
            Database.SetInitializer<ApplicationDBContext>(new UserDbContextSeeder<ApplicationDBContext>());
            Database.Initialize(true);
            Database.CreateIfNotExists();
        }
        public DbSet<MemberDetails> MemberDetails { get; set; }

        public DbSet<User> UserDetails { get; set; }

        public DbSet<RelationshipDetails> RelationshipDetails { get; set; }

        public class UserDbContextSeeder<T> : CreateDatabaseIfNotExists<ApplicationDBContext>
        {
            protected override void Seed(ApplicationDBContext context)
            {
                User userDetails = new User()
                {
                   
                    UserName = "Admin",
                    Password = "Admin",
                    UserType = "Admin",
                     Email = "srirenga96@gmail.com"
                };
                context.UserDetails.Add(userDetails);
                base.Seed(context);
            }
        }
    }
}
