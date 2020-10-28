using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Entity;
using System.Web.Mvc;

using Autofac;
using Autofac.Builder;
using Autofac.Integration.Mvc;
using System.Reflection;

namespace t6
{
    public class AutofacRegister
    {
        static IContainer container = null;
        public static IContainer Container
        {
            get
            {
                if (container == null)
                {
                    container = Register();
                }

                return container;
            }
        }
        public static IContainer Register()
        {
            var builder = new ContainerBuilder();
            /*builder.RegisterAssemblyTypes(System.Reflection.Assembly.GetExecutingAssembly()).Where(
                type => typeof(DbContext).IsAssignableFrom(type)
                ).InstancePerRequest();
            */
            builder.RegisterType<Test1>().InstancePerRequest();
            builder.RegisterControllers(Assembly.GetExecutingAssembly());

             container = builder.Build();
            
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            return container;
        }
    }
}