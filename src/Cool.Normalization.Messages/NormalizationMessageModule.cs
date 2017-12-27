using Abp.Dependency;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Castle.MicroKernel.Registration;
using MaiReo.Messages.Abstractions;
using MaiReo.Messages.Abstractions.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;

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
            //Prevent auto starting for pub/sub.
            Configuration.Modules.MessagePublisher().AutoStart = false;
            Configuration.Modules.MessageReceiver().AutoStart = false;
            var messageConfig = Configuration.Modules.Messages();
            AddMissingConfiguration( messageConfig );
        }

        private void AddMissingConfiguration( IMessageConfiguration messageConfig )
        {
            messageConfig.Schema = messageConfig.Schema ?? "tcp";
            messageConfig.ListenAddress =
                messageConfig.ListenAddress
                ?? IPAddress.IPv6Any;
            messageConfig.ListenAddressForPubSub =
                messageConfig.ListenAddressForPubSub
                ?? "coolmessageproxy";
            messageConfig.XPubPort = messageConfig.XPubPort == 0
                ? 5555
                : messageConfig.XPubPort;
            messageConfig.XSubPort = messageConfig.XSubPort == 0
                ? 6666
                : messageConfig.XSubPort;
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
