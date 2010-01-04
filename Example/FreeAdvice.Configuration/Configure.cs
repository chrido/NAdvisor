using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac;
using FreeAdvice.Common;
using FreeAdvice.Repositories;
using FreeAdvice.Repositories.Interfaces;

namespace FreeAdvice.Configuration
{
    public class Configure
    {
        public static void ConfigureApplicationContainer()
        {
            var container = new Autofac.Builder.ContainerBuilder();
            
            //Infrastructure
            container.Register(d => new Logger()).As<ILogger>();
            
            //Repositories
            container.Register(d => new AdviceRepository()).As<IAdviceRepository>();

            DIContainer.Container.ApplicationContainer = container.Build();
        }

        public static void ConfigureSessionContainer()
        {
            IContainer sessionContainer = DIContainer.Container.ApplicationContainer.CreateInnerContainer();

            var container = new Autofac.Builder.ContainerBuilder();

            //Register Services


            container.Build(sessionContainer);
            DIContainer.Container.SessionContainer = sessionContainer;
        }
    }
}
