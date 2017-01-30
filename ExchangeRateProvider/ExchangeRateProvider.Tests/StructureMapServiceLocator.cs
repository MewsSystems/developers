using System;
using System.Collections.Generic;
using Microsoft.Practices.ServiceLocation;
using StructureMap;

namespace ExchangeRateProvider.Tests
{
    /// <summary>
    /// StructureMapServiceLocator adapter for Microsoft <c>CommonServiceLocator</c>
    /// </summary>
    public class StructureMapServiceLocator : ServiceLocatorImplBase
    {
        private readonly IContainer container;

        public StructureMapServiceLocator(IContainer container)
        {
            this.container = container;
        }

        /// <summary>
        ///  When implemented by inheriting classes, this method will do the actual work of resolving
        ///  the requested service instance.
        /// </summary>
        /// <param name="serviceType">Type of instance requested.</param>
        /// <param name="key">Name of registered service you want. May be null.</param>
        /// <returns>
        /// The requested service instance.
        /// </returns>
        protected override object DoGetInstance(Type serviceType, string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return container.GetInstance(serviceType);
            }
            else
            {
                return container.GetInstance(serviceType, key);
            }
        }

        /// <summary>
        ///             When implemented by inheriting classes, this method will do the actual work of
        ///             resolving all the requested service instances.
        /// </summary>
        /// <param name="serviceType">Type of service requested.</param>
        /// <returns>
        /// Sequence of service instance objects.
        /// </returns>
        protected override IEnumerable<object> DoGetAllInstances(Type serviceType)
        {
            foreach (object obj in container.GetAllInstances(serviceType))
            {
                yield return obj;
            }
        }
    }


}