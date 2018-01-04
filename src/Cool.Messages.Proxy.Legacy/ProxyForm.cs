using NetMQ;
using NetMQ.Sockets;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cool.Messages.Proxy.Legacy
{
    public partial class ProxyForm : Form
    {
        public ProxyForm()
        {
            InitializeComponent();
            _cancellationTokenSource = new CancellationTokenSource();
        }

        private NetMQ.Proxy _proxy;

        private class ProxyConfig
        {
            public string Addr { get; set; }
            public int? XSub { get; set; }
            public int? XPub { get; set; }
        }

        private CancellationTokenSource _cancellationTokenSource;

        private ProxyConfig MakeConfig()
        {
            var address = tb_addr.Text?.Trim();
            if (string.IsNullOrWhiteSpace( address ))
                return null;
            if (!int.TryParse( tb_xpub.Text, out var xpub ))
                return null;
            if (!int.TryParse( tb_xsub.Text, out var xsub ))
                return null;
            return new ProxyConfig
            {
                Addr = address,
                XPub = xpub,
                XSub = xsub
            };
        }

        private void btn_onoff_Click( object sender, EventArgs e )
        {
            if (_proxy != null)
            {
                _proxy.Stop();
                _proxy = null;
                return;
            }
            if (!_cancellationTokenSource.IsCancellationRequested)
                _cancellationTokenSource.Cancel();
            var cancellationTokenSource = _cancellationTokenSource = new CancellationTokenSource();
            Task.Run( () =>
            {
                var config = MakeConfig();
                if (config == null)
                {
                    InvokeFor( rtb_out, x => x.AppendText( $"配置错误{Environment.NewLine}" ) );
                    return;
                }
                var pubAddress = $"@tcp://{config.Addr}:{config.XPub}";
                var subAddress = $"@tcp://{config.Addr}:{config.XSub}";
                InvokeFor( rtb_out, x => x.AppendText( $"[{nameof( ProxyForm )}]XPub listening on: {pubAddress}{Environment.NewLine}" ) );
                InvokeFor( rtb_out, x => x.AppendText( $"[{nameof( ProxyForm )}]XSub listening on: {subAddress}{Environment.NewLine}" ) );
                using (var xpubSocket = new XPublisherSocket( pubAddress ))
                using (var xsubSocket = new XSubscriberSocket( subAddress ))
                using (var p2sControlOut = new DealerSocket( "@inproc://control-p2s" ))
                using (var p2sControlIn = new DealerSocket( ">inproc://control-p2s" ))
                using (var s2pControlOut = new DealerSocket( "@inproc://control-s2p" ))
                using (var s2pControlIn = new DealerSocket( ">inproc://control-s2p" ))
                {
                    //proxy messages between frontend / backend
                    var proxy = _proxy = new NetMQ.Proxy( xsubSocket, xpubSocket, p2sControlOut, s2pControlOut );
                    Task.Run( () =>
                     {
                         while (!cancellationTokenSource.IsCancellationRequested)
                         {
                             try
                             {
                                 var topic = p2sControlIn.ReceiveFrameString();
                                 var timestamp_bytes = p2sControlIn.ReceiveFrameBytes();
                                 var timestamp_ticks = BitConverter.ToInt64( timestamp_bytes, 0 );
                                 var timestamp = new DateTimeOffset( timestamp_ticks, TimeSpan.FromMinutes( 0 ) );
                                 var message = p2sControlIn.ReceiveFrameString();
                                 InvokeFor( rtb_out, x =>
                                 {
                                     x.AppendText( $"[{DateTimeOffset.Now}]MESSAGE-RECEIVED:{Environment.NewLine}" );
                                     x.AppendText( $"\t[Topic]{topic}{Environment.NewLine}" );
                                     x.AppendText( $"\t[Timestamp]{timestamp.ToLocalTime()}{Environment.NewLine}" );
                                     x.AppendText( $"\t[Message]{message?.Replace( "\r", "" )?.Replace( "\n", "" ) ?? "NULL"}{Environment.NewLine}" );
                                 } );
                             }
                             catch (System.ObjectDisposedException)
                             {
                                 return;
                             }
                             catch (Exception)
                             {
                                 continue;
                             }
                         }
                     }, cancellationTokenSource.Token );
                    //WARNING:blocks indefinitely.
                    InvokeFor( rtb_out, x => x.AppendText( $"[{nameof( ProxyForm )}]Proxy Start.{Environment.NewLine}" ) );
                    proxy.Start();
                    InvokeFor( rtb_out, x => x.AppendText( $"[{nameof( ProxyForm )}]Proxy Stop.{Environment.NewLine}" ) );
                }
            }, cancellationTokenSource.Token );

        }

        public static void InvokeFor<T>( T control, Action<T> action ) where T : Control
        {
            if (control.InvokeRequired)
            {
                control.Invoke( action, control );
            }
            else
            {
                action( control );
            }
        }

        private void ProxyForm_FormClosing( object sender, FormClosingEventArgs e )
        {
            e.Cancel = MessageBox.Show( "Quit?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2 )
                != DialogResult.Yes;
            if (!e.Cancel)
            {
                _cancellationTokenSource.Cancel();
                Task.Delay( 200 ).GetAwaiter().GetResult();
                _proxy?.Stop();
            }
        }
    }
}
