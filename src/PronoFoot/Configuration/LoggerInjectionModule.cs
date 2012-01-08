using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Autofac;
using Autofac.Core;
using PronoFoot.Logging;

namespace PronoFoot.Configuration
{
    class LoggerInjectionModule : Module
    {
        protected override void AttachToComponentRegistration(IComponentRegistry registry, IComponentRegistration registration)
        {
            registration.Preparing += OnComponentPreparing;
        }

        static void OnComponentPreparing(object sender, Autofac.Core.PreparingEventArgs e)
        {
            var t = e.Component.Activator.LimitType;
            e.Parameters = e.Parameters.Union(new[]
                    {
                        new Autofac.Core.ResolvedParameter(
                            (p, i) => p.ParameterType == typeof(ILogger),
                            (p, i) => e.Context.Resolve<ILoggerFactory>().CreateLogger(t))
                    });
        }
    }
}