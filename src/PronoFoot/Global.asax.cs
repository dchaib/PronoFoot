using Autofac;
using Autofac.Integration.Mvc;
using PronoFoot.Business.Contracts;
using PronoFoot.Business.Services;
using PronoFoot.Configuration;
using PronoFoot.Data;
using PronoFoot.Data.EntityFramework;
using PronoFoot.Data.EntityFramework.Repositories;
using PronoFoot.Logging;
using PronoFoot.Messaging;
using PronoFoot.Models;
using PronoFoot.Security;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;

namespace PronoFoot
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        private static IContainer container;

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            //WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            //AuthConfig.RegisterAuth();

            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new RazorViewEngine());

            InitializeDependecyInjection();
            InitializeAutoMapping();
        }

        private void InitializeDependecyInjection()
        {
            var builder = new ContainerBuilder();
            builder.RegisterControllers(typeof(MvcApplication).Assembly);
            //Data
            builder.RegisterType<PronoFootDbContext>().As<System.Data.Entity.DbContext>();
            builder.RegisterType<PronoFootDbContext>().As<IUnitOfWork>();
            builder.RegisterType<CompetitionRepository>().As<ICompetitionRepository>();
            builder.RegisterType<EditionRepository>().As<IEditionRepository>();
            builder.RegisterType<TeamRepository>().As<ITeamRepository>();
            builder.RegisterType<DayRepository>().As<IDayRepository>();
            builder.RegisterType<FixtureRepository>().As<IFixtureRepository>();
            builder.RegisterType<ForecastRepository>().As<IForecastRepository>();
            builder.RegisterType<UserRepository>().As<IUserRepository>();
            //Business
            builder.RegisterType<UserService>().As<IUserService>().InstancePerLifetimeScope();
            builder.RegisterType<CompetitionService>().As<ICompetitionService>().InstancePerLifetimeScope();
            builder.RegisterType<EditionService>().As<IEditionService>().InstancePerLifetimeScope();
            builder.RegisterType<DayService>().As<IDayService>().InstancePerLifetimeScope();
            builder.RegisterType<FixtureService>().As<IFixtureService>().InstancePerLifetimeScope();
            builder.RegisterType<ForecastService>().As<IForecastService>().InstancePerLifetimeScope();
            builder.RegisterType<TeamService>().As<ITeamService>().InstancePerLifetimeScope();
            builder.RegisterType<ScoringService>().As<IScoringService>().InstancePerLifetimeScope();
            builder.RegisterType<ClassificationService>().As<IClassificationService>().InstancePerLifetimeScope();
            builder.RegisterType<TeamStandingService>().As<ITeamStandingService>().SingleInstance();
            //Framework
            builder.RegisterType<NLogLoggerFactory>().As<ILoggerFactory>().SingleInstance();
            builder.RegisterModule(new LoggerInjectionModule());
            builder.RegisterType<DefaultEncryptionService>().As<IEncryptionService>().SingleInstance();
            builder.RegisterType<EmailMessagingService>().As<IMessagingService>().WithParameter(new NamedParameter("fromAddress", ConfigurationManager.AppSettings["emailAddress"].ToString()));
            builder.RegisterType<FormsAuthenticationService>().As<IAuthenticationService>();
            builder.RegisterType<DefaultMembershipService>().As<IMembershipService>();
            container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }

        private void InitializeAutoMapping()
        {
            AutoMapper.Mapper.CreateMap<PronoFoot.Models.Competition.CompetitionModel, PronoFoot.Business.Models.CompetitionModel>()
                .ForMember(dest => dest.CompetitionId, opt => opt.MapFrom(src => src.Id));
            AutoMapper.Mapper.CreateMap<PronoFoot.Business.Models.CompetitionModel, PronoFoot.Models.Competition.CompetitionModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.CompetitionId));

            AutoMapper.Mapper.CreateMap<PronoFoot.Data.Model.Edition, PronoFoot.Models.Edition.EditionOverviewModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.EditionId));
        }

    }
}