using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Autofac;
using Autofac.Core;
using Domain;
using RepositoryLayer;
using ServiceLayer;

namespace DependencyInjection
{
    public class DependencyRegistrar : Autofac.Module
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

            #region old
            /* 
			 * Register Repositories and services
			 //*/
            //        builder
            //            .RegisterType<InfoRequestService>()
            //            .As<IInfoRequestService>()
            //            .InstancePerLifetimeScope();

            //        builder.RegisterType<InfoRequestRepository>()
            //            .As<IInfoRequestRepository>()
            //            .InstancePerLifetimeScope();

            //        builder.RegisterType<BrandService>()
            //            .As<IBrandService>()
            //            .InstancePerLifetimeScope();

            //        builder.RegisterType<BrandRepository>()
            //            .As<IBrandRepository>()
            //            .InstancePerLifetimeScope();

            //        builder.RegisterType<ProductRepository>()
            //            .As<IProductRepository>()
            //            .InstancePerLifetimeScope();

            //        builder.RegisterType<ProductService>()
            //            .As<IProductService>()
            //            .InstancePerLifetimeScope();

            //        builder.RegisterType<ProductCategoryRepository>()
            //            .As<IProductCategoryRepository>()
            //            .InstancePerLifetimeScope();

            //        builder.RegisterType<ProductCategoryService>()
            //            .As<IProductCategoryService>()
            //            .InstancePerLifetimeScope();

            //        builder.RegisterType<CategoryRepository>()
            //            .As<ICategoryRepository>()
            //            .InstancePerLifetimeScope();

            //        builder.RegisterType<CategoryService>()
            //            .As<ICategoryService>()
            //            .InstancePerLifetimeScope();
            #endregion

            builder.RegisterAssemblyTypes(typeof(BrandRepository).Assembly)
                   //.Where(t => t.Name.EndsWith("Repository"))
                   .AsImplementedInterfaces().InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(typeof(BrandService).Assembly)
               //.Where(t => t.Name.EndsWith("Service"))
               .AsImplementedInterfaces().InstancePerLifetimeScope()
               ;
        }
    }
}
