﻿using System;
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
using PronoFoot.Authentication;

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

        public override void Init()
        {
            this.PostAuthenticateRequest += this.PostAuthenticateRequestHandler;

            base.Init();
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
            builder.RegisterType<UserService>().As<IUserService>();
            builder.RegisterType<DayServices>().As<IDayServices>();
            builder.RegisterType<FixtureService>().As<IFixtureService>();
            builder.RegisterType<ForecastService>().As<IForecastService>();
            builder.RegisterType<TeamService>().As<ITeamService>();
            //Web
            builder.RegisterType<FormsAuthenticationService>().As<IFormsAuthenticationService>();
            container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }

        private void PostAuthenticateRequestHandler(object sender, EventArgs e)
        {
            HttpCookie authCookie = this.Context.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (IsValidAuthCookie(authCookie))
            {
                var formsAuthenticationService = container.Resolve<IFormsAuthenticationService>();
                var ticket = formsAuthenticationService.Decrypt(authCookie.Value);
                var pronoFootIdentity = new PronoFootIdentity(ticket);
                this.Context.User = new GenericPrincipal(pronoFootIdentity, Roles.GetRolesForUser(pronoFootIdentity.Name));
            }
        }

        private static bool IsValidAuthCookie(HttpCookie authCookie)
        {
            return authCookie != null && !String.IsNullOrEmpty(authCookie.Value);
        }
    }
}