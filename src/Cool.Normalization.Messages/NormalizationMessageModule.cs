using Abp.Dependency;
using Abp.Modules;
using MaiReo.Messages.Abstractions;
using System.Net;
using System;
using Cool.Normalization.Messages;

namespace Abp.Modules
{
    [DependsOn(
        typeof( NormalizationAbstractionModule ),
        typeof( MessageAbstractionsModule ),
        typeof( MessagePublisherModule ),
        typeof( MessageReceiverModule )
        )]
    public class NormalizationMessageModule : AbpModule
    {
        private IMessageReceiverWrapper _receiver;

        public override void Initialize()
        {
            //Prevent auto starting for receiver.
            Configuration.Modules.MessageReceiver().AutoStart = false;

            if (!Configuration.Modules.Normalization().IsMessageEnabled)
            {
                return;
            }
            AddMissingConfiguration( IocManager.Resolve<IMessageConfiguration>() );
        }

        private void AddMissingConfiguration(IMessageConfiguration messageConfiguration)
        {
            messageConfiguration.BrokerAddress = messageConfiguration.BrokerAddress
                ?? MessageConfiguration.Default.BrokerAddress;
            if (messageConfiguration.BrokerPort > 65535
                || messageConfiguration.BrokerPort < 1)
            {
                messageConfiguration.BrokerPort = MessageConfiguration.Default.BrokerPort;
            }
        }

        public override void PostInitialize()
        {
            IocManager.RegisterIfNot<NullMessageResolver,
                NullMessageResolver>();
            IocManager.RegisterIfNot<NullMessageHandlerResolver,
                NullMessageHandlerResolver>();
            IocManager.RegisterIfNot<NullMessageHandlerCallExpressionBuilder,
                NullMessageHandlerCallExpressionBuilder>();
            IocManager.RegisterIfNot<IMessageHandlerCodeResolver,
                NullMessageHandlerCodeResolver>();
            IocManager.RegisterIfNot<IMessageLogFormatter,
                NullMessageLogFormatter>();
            IocManager.RegisterIfNot<NullMessageHandlerInvoker,
                NullMessageHandlerInvoker>();
            IocManager.RegisterIfNot<IMessageHandlerBinder,
                NullMessageHandlerBinder>();

            if (!Configuration.Modules.Normalization().IsMessageEnabled)
            {
                return;
            }

            var messageConfig = Configuration.Modules.Messages();
            //TODO:及时更新kafka地址并打包发布
            messageConfig.BrokerAddress = "test.baishijiaju.com";

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


        public override void Shutdown()
        {
            _receiver?.Disconnect();
        }
    }
}
