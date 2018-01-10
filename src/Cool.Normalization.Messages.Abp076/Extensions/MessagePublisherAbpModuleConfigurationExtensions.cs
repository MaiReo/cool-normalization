using Abp.Configuration.Startup;
using MaiReo.Messages.Abstractions;
using MaiReo.Messages.Publisher;

namespace Abp.Modules
{
    public static class MessagePublisherAbpModuleConfigurationExtensions
    {

        public static IMessageConfiguration Message(
            this IModuleConfigurations modules )
        {
            return modules.AbpConfiguration.Get<IMessageConfiguration>( nameof( IMessageConfiguration ) );
        }

        public static IMessagePublisherModuleConfiguration MessagePublisher(
            this IModuleConfigurations modules )
        {
            return modules.AbpConfiguration.Get<IMessagePublisherModuleConfiguration>( nameof( IMessagePublisherModuleConfiguration ) );
        }
    }
}
