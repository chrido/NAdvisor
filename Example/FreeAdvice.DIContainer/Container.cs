
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web;
using Autofac;
using IContainer=Autofac.IContainer;

namespace FreeAdvice.DIContainer
{
    public class Container
    {
        private const string applicationContainerKey = "________ApplicationDependencyInjection";
        private static IContainer _applicationContainer;

        public static IContainer ApplicationContainer
        {
            get
            {
                if (HttpContext.Current == null)
                    return _applicationContainer;

                return HttpContext.Current.Application[applicationContainerKey] as IContainer;
            }
            set
            {
                if (HttpContext.Current == null)
                {
                    _applicationContainer = value;
                    return;
                }

                HttpContext.Current.Application.Lock();
                HttpContext.Current.Application[applicationContainerKey] = value;
                HttpContext.Current.Application.UnLock();
            }
        }

        public static IContainer CreateInnerContainer()
        {
            if (HttpContext.Current != null) HttpContext.Current.Application.Lock();
            IContainer innercontainer = ApplicationContainer.CreateInnerContainer();
            if (HttpContext.Current != null) HttpContext.Current.Application.UnLock();

            return innercontainer;
        }

        /// <summary>
        /// Session Container
        /// </summary>
        private const string sessionContainerKey = "________SessionDependencyInjection";
        private static IContainer sessionContainer;

        public static IContainer SessionContainer
        {
            get
            {
                if (HttpContext.Current == null)
                    return sessionContainer;

                if (HttpContext.Current.Session[sessionContainerKey] == null)
                {
                    IContainer innerContainer = CreateInnerContainer();
                    HttpContext.Current.Session[sessionContainerKey] = innerContainer;
                    return innerContainer;
                }
                return HttpContext.Current.Session[sessionContainerKey] as IContainer;
            }
            set
            {
                if (HttpContext.Current != null)
                {
                    HttpContext.Current.Session[sessionContainerKey] = value;
                    return;
                }
                sessionContainer = value;
            }
        }
    }
}
