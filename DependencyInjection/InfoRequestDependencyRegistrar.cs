using System;
using System.Collections.Generic;
using System.Text;
using Autofac;
using Autofac.Core;
using Domain;
using RepoLayer;

namespace DependencyInjection
{
    public class InfoRequestDependencyRegistrar : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            /* 
			 * Register Db
			 */
            builder
                .RegisterType<MyDbContext>()
                .AsSelf()
                .InstancePerLifetimeScope();

            /* 
			 * Register Repositories
			 */
            builder.RegisterGeneric(typeof(IInfoRequestRepository))
                .WithParameter(
                    new ResolvedParameter(
                        (pi, ctx) => pi.Name == "dbContext",
                        (pi, ctx) => ctx.Resolve<MyDbContext>()
                    ));


        }
    }
}
