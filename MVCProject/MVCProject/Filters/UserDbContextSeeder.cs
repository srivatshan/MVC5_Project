using MVCProject.Data.Models;
using MVCProject.Data.Repository.Repo;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MVCProject.Filters
{
    public class UserDbContextSeeder : DropCreateDatabaseIfModelChanges<ApplicationDBContext>
    {
        protected override void Seed(ApplicationDBContext context)
        {
            User userDetails = new User()
            {
               
                UserName="Admin",
                Password="Admin",
                UserType="Admin",
                Email="srirenga96@gmail.com"
            };
            context.UserDetails.Add(userDetails);
            base.Seed(context);
        }
    }
}