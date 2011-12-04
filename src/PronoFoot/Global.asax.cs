using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using Autofac;
using Autofac.Integration.Mvc;
using PronoFoot.Data;
using PronoFoot.Data.EntityFramework;
using PronoFoot.Data.EntityFramework.Repositories;
using System.Security.Principal;
using PronoFoot.Business.Contracts;
using PronoFoot.Business.Services;
using PronoFoot.Models;
using PronoFoot.Security;

namespace PronoFoot
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        private static IContainer container;

        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            InitializeDependecyInjection();
        }

        private void InitializeDependecyInjection()
        {
            var builder = new ContainerBuilder();
            builder.RegisterControllers(typeof(MvcApplication).Assembly);
            //Data
            builder.RegisterType<PronoFootDbContext>().As<IUnitOfWork>();
            builder.RegisterType<CompetitionRepository>().As<ICompetitionRepository>();
            builder.RegisterType<TeamRepository>().As<ITeamRepository>();
            builder.RegisterType<DayRepository>().As<IDayRepository>();
            builder.RegisterType<FixtureRepository>().As<IFixtureRepository>();
            builder.RegisterType<ForecastRepository>().As<IForecastRepository>();
            builder.RegisterType<UserRepository>().As<IUserRepository>();
            //Business
            builder.RegisterType<UserService>().As<IUserService>().InstancePerLifetimeScope();
            builder.RegisterType<DayService>().As<IDayService>().InstancePerLifetimeScope();
            builder.RegisterType<FixtureService>().As<IFixtureService>().InstancePerLifetimeScope();
            builder.RegisterType<ForecastService>().As<IForecastService>().InstancePerLifetimeScope();
            builder.RegisterType<TeamService>().As<ITeamService>().InstancePerLifetimeScope();
            builder.RegisterType<ScoringService>().As<IScoringService>().InstancePerLifetimeScope();
            //Web
            builder.RegisterType<FormsAuthenticationService>().As<IAuthenticationService>();
            builder.RegisterType<DefaultMembershipService>().As<IMembershipService>();
            container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}