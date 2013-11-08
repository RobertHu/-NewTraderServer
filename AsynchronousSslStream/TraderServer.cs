using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using log4net;
using System.Configuration;
using Trader.Server.Service;
using Trader.Server.Bll;
using Trader.Server.Util;
using Trader.Common;
using Trader.Server.Ssl;
using Trader.Server.ValueObjects;
[assembly: log4net.Config.XmlConfigurator(ConfigFile = "Config/Log4Net.config",Watch = true)]
namespace Trader.Server
{
    public class TraderServer
    {
        private ILog _Log = LogManager.GetLogger(typeof(TraderServer));
        private CommandCollectorHost _CommandCollectorHost = new CommandCollectorHost();
        private SecureTcpServer server = null;

        public void Start()
        {
            try
            {
                AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
                _Log.InfoFormat("{0} certificate path", SettingManager.Default.CertificatePath);
                X509Certificate serverCert = X509Certificate.CreateFromCertFile(SettingManager.Default.CertificatePath);
                server = new SecureTcpServer(SettingManager.Default.ServerPort, serverCert, OnServerConnectionAvailable, null);
                _Log.Info("Server Start");
                server.StartListening();
                _CommandCollectorHost.Start();
                Application.Default.Start();

            }
            catch (Exception ex)
            {
                _Log.Error(ex);
            }
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            this._Log.Error(e.ExceptionObject);
        }

        public void Stop()
        {
            try
            {
                if (server != null)
                {
                    server.StopListening();
                    server.Dispose();
                }

                if (_CommandCollectorHost != null)
                {
                    _CommandCollectorHost.Stop();
                }
                if (Application.Default != null)
                {
                    Application.Default.Stop();
                }
            }
            catch (Exception ex)
            {
                this._Log.Error(ex);
            }
        }


        private void OnServerConnectionAvailable(object sender, SecureConnectionResults args)
        {
            try
            {
                if (args.AsyncException != null)
                {
                    _Log.ErrorFormat("Client connection failed {0}", args.AsyncException);
                    return;
                }
                SslInfo sslInfo = args.SecureInfo;
                Session session = SessionMapping.Get();
                Client client = new Client(sslInfo.SslStream, session, sslInfo.NetworkStream.BufferInUsed,sslInfo.Socket);
                SenderReceiverPair relation = new SenderReceiverPair(client, new ReceiveAgent());
                Application.Default.AgentController.Add(session, relation.Receiver, relation.Sender);
            }
            catch (Exception ex)
            {
                _Log.Error(ex);
            }
        }
    }
}