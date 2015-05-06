﻿using Microsoft.Practices.Unity;

using NuClear.Messaging.API.Flows;
using NuClear.Messaging.Transports.ServiceBus;
using NuClear.Messaging.Transports.ServiceBus.API;

namespace NuClear.AdvancedSearch.Replication.EntryPoint.Factories
{
    public class UnityServiceBusMessageFlowReceiverFactory : IServiceBusMessageFlowReceiverFactory
    {
        private readonly IUnityContainer _container;

        public UnityServiceBusMessageFlowReceiverFactory(IUnityContainer container)
        {
            _container = container;
        }

        public IServiceBusMessageFlowReceiver Create(IMessageFlow messageFlow)
        {
            return (IServiceBusMessageFlowReceiver)_container.Resolve(typeof(ServiceBusMessageFlowReceiver<>).MakeGenericType(messageFlow.GetType()));

        }
    }
}