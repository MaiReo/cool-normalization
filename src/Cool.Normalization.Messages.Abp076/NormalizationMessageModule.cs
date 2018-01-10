using Abp.Modules;
using MaiReo.Messages.Abstractions;
using MaiReo.Messages.Publisher;

namespace Cool.Normalization.Messages
{
    public class NormalizationMessageModule : AbpModule
    {
        private IMessagePublisherWrapper _messagePublisherWrapper;
        public override void PreInitialize()
        {
            IocManager.Register<IMessageConfiguration, MessageConfiguration>();
            IocManager.Register<IMessagePublisherModuleConfiguration,
                MessagePublisherModuleConfiguration>();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention( typeof( NormalizationMessageModule ).Assembly );
        }

        public override void PostInitialize()
        {
            RegisterIfNot<IKafkaProducerOption, KafkaProducerOption>();
            RegisterIfNot<IKafkaProducerBuilder, KafkaProducerBuilder>();
            RegisterIfNot<IMessagePublisherWrapper, KafkaProducerWrapper>();
            RegisterIfNot<IMessagePublisher, MessagePublisher>();

            var messageConfiguration = IocManager.Resolve<IMessageConfiguration>();

            messageConfiguration.BrokerAddress = "test.baishijiaju.com";
            messageConfiguration.BrokerPort = MessageConfiguration.Default.BrokerPort;

            var messagePublisherWrapper = _messagePublisherWrapper = IocManager.Resolve<IMessagePublisherWrapper>();

            var config = IocManager.Resolve
                <IMessagePublisherModuleConfiguration>();
            if (!config.AutoStart)
            {
                return;
            }
            messagePublisherWrapper.Connect();

        }

        private void RegisterIfNot<TService, TImpl>(
            Abp.Dependency.DependencyLifeStyle lifeStyle
            = Abp.Dependency.DependencyLifeStyle.Singleton )
            where TImpl : class, TService
            where TService : class
        {
            if (IocManager.IsRegistered<TService>())
            {
                return;
            }
            IocManager.Register<TService, TImpl>( lifeStyle );
        }
        public override void Shutdown()
        {
            _messagePublisherWrapper?.Disconnect();
        }
    }
}
