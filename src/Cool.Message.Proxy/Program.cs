using MaiReo.Messages.Abstractions;
using MaiReo.Messages.Broker;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Cool.Message.Proxy
{
    class Program
    {
        static Program()
        {
            _cancellationTokenSource = new CancellationTokenSource();
        }
        private static CancellationTokenSource _cancellationTokenSource;
        private static MessageBroker _broker;

        static async Task Main( string[] args )
        {
            if (!CheckArgs( args ))
            {
                PrintUsage();
                return;
            }
            var configuration = ParseConfiguration( args[0] );
            if (configuration != null && configuration.ListenAddress == null)
            {

                configuration.ListenAddress = IPAddress.Any;
            }
            if (!configuration.IsValid())
            {
                PrintUsage();
                return;
            }

            var broker = _broker = new MessageBroker( configuration )
            {
                Logger = new ConsoleMessageBrokerLogger()
            };

            broker.Startup();
            await Task.Delay( -1, _cancellationTokenSource.Token );
        }

        private static IMessageConfiguration ParseConfiguration( string fileNameOrText )
        {
            var configuration = default( IMessageConfiguration );
            if (File.Exists( fileNameOrText ))
            {
                try
                {
                    var text = File.ReadAllText( fileNameOrText );
                    configuration = JsonConvert.DeserializeObject<MessageConfiguration>( text );
                }
                catch (Exception ex)
                {
                    Console.WriteLine( ex.Message );
                    configuration = null;
                }
            }
            else
            {
                try
                {
                    configuration = JsonConvert.DeserializeObject<MessageConfiguration>( fileNameOrText );
                }
                catch (Exception ex)
                {
                    Console.WriteLine( ex.Message );
                    configuration = null;
                }
            }
            return configuration;
        }

        static void PrintUsage()
        {
            Console.WriteLine( "Must pass a file path string or json fomatting string as configuration." );
        }

        static bool CheckArgs( string[] args )
        {
            if (args == null)
                return false;
            if (args.Length != 1)
                return false;
            return true;
        }

        ~Program()
        {
            _broker?.Shutdown();
            _cancellationTokenSource?.Cancel();
        }
    }
}
