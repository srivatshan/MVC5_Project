using Autofac;
using Autofac.Integration.Mvc;
using MVCProject.Data.Repository.Interface;
using MVCProject.Data.Repository.Repo;
using System.Web.Mvc;

namespace MVCProject
{
    public class ContainerConfig
    {
        internal static void RegisterContainer()
        {
            var builder = new ContainerBuilder();
            builder.RegisterControllers(typeof(MvcApplication).Assembly);

            builder.RegisterType<MemberDetailsRepository>()
                .As<IMemberDetailsRepository>()
                .InstancePerRequest();
            builder.RegisterType<RelationshipRepository>()
               .As<IRelationshipRepository>()
               .InstancePerRequest();
            builder.RegisterType<UserDetails>()
               .As<IUserDetails>()
               .InstancePerRequest();
            builder.RegisterType<ApplicationDBContext>().InstancePerRequest();

            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}