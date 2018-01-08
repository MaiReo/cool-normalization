using Abp.Dependency;
using Abp.Modules;
using MaiReo.Messages.Abstractions;
using System.Net;
using System;

namespace Cool.Normalization.Messages
{
    [DependsOn(
        typeof( MessageAbstractionsModule ),
        typeof( MessagePublisherModule ),
        typeof( MessageReceiverModule )
        )]
    public class NormalizationMessageModule : AbpModule
    {
        private IMessageReceiverWrapper _receiver;
        public override void PreInitialize()
        {
            IocManager.AddConventionalRegistrar( new MessageHandlerRegistrar() );
        }
        public override void Initialize()
        {
            //Prevent auto starting for receiver.
            Configuration.Modules.MessageReceiver().AutoStart = false;
            AddMissingConfiguration( IocManager.Resolve<IMessageConfiguration>() );
        }

        private void AddMissingConfiguration( IMessageConfiguration messageConfiguration )
        {
            messageConfiguration.BrokerAddress = messageConfiguration.BrokerAddress
                ?? MessageConfiguration.Default.BrokerAddress;
            if (messageConfiguration.BrokerPort > 65535
                || messageConfiguration.BrokerPort < 1)
            {
                messageConfiguration.BrokerPort =  MessageConfiguration.Default.BrokerPort;
            }
        }

        public override void PostInitialize()
        {
            var messageConfig = Configuration.Modules.Messages();

            RegisterIfNot<IMessageResolver, MessageResolver>();
            RegisterIfNot<IMessageHandlerResolver, MessageHandlerResolver>();
            RegisterIfNot<IMessageHandlerCallExpressionBuilder, MessageHandlerCallExpressionBuilder>();
            RegisterIfNot<IMessageLogFormatter, MessageLogFormatter>();
            RegisterIfNot<IMessageHandlerBinder, MessageHandlerBinder>();

            using (var messageBinder = IocManager.ResolveAsDisposable<IMessageHandlerBinder>())
            {
                var bindingSucceed = messageBinder.Object.Binding( IocManager, messageConfig );
                if (bindingSucceed)
                {
                    _receiver = IocManager.Resolve<IMessageReceiverWrapper>();
                    _receiver.Connect();
                }
            }
        }


        private void RegisterIfNot<IService, IImpl>(
            DependencyLifeStyle lifeStyle = DependencyLifeStyle.Singleton )
            where IService : class
            where IImpl : class, IService
        {
            if (IocManager.IsRegistered<IService>())
            {
                return;
            }
            IocManager.Register<IService, IImpl>( lifeStyle );
        }


        public override void Shutdown()
        {
            _receiver?.Disconnect();
        }
    }
}
