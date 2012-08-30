using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using CachingPreview.Web.Caching;
namespace CachingPreview.Web
{
    public static class Bootstrapper
    {
        public static void Run()
        {
            SetAutofacContainer();            
        }
        private static void SetAutofacContainer()
        {
            var builder = new ContainerBuilder();
            var assembly = Assembly.GetExecutingAssembly();
            builder.RegisterControllers(assembly);
            builder.RegisterAssemblyTypes(assembly)
            .Where(t => t.Name.EndsWith("Repository"))
            .AsImplementedInterfaces().InstancePerHttpRequest();       
            builder.RegisterType<AzureCacheProvider>().As<ICacheProvider>().InstancePerHttpRequest();
            IContainer container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}