using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Trader.Common;
using System.Collections.Concurrent;
using System.Threading;
using Trader.Server.Util;
using Trader.Server.Serialization;
using Trader.Server.Bll;
using Trader.Server.Core.Request;
using Trader.Server.TypeExtension;
namespace Trader.Server.Ssl
{
	public class ReceiveAgent
	{
        private ConcurrentQueue<ReceiveData> _Queue = new ConcurrentQueue<ReceiveData>();
        private volatile  bool _IsStoped = true;
        private ReceiveData _Current;

        public void Send(ReceiveData data)
        {
            _Queue.Enqueue(data);
            if (_IsStoped)
            {
                _IsStoped = false;
                ProcessData();
            }
        }

        private void ProcessData()
        {
            if (_Queue.TryDequeue(out _Current))
            {
                ThreadPool.QueueUserWorkItem(ProcessCallback,null);
            }
            else
            {
                _IsStoped = true;
            }
        }

        private void ProcessCallback(object state)
        {
            SerializedInfo request = PacketParser.Parse(_Current.Data);
            if (request != null)
            {
                var sender = Application.Default.AgentController.GetSender(_Current.ClientId);
                var remoteIp = _Current.RemoteIp;
                request.ClientInfo.Initialize(_Current.ClientId, sender, remoteIp);
                if (request.ClientInfo.Session == Session.InvalidValue)
                    request.ClientInfo.UpdateSession(_Current.ClientId);
                ProcessRequest(request);
            }
            ProcessData();
        }

        private void ProcessRequest(SerializedInfo request)
        {
            PacketContent responseContent=null;
            try
            {
                ContentType contentType = request.Content.ContentType;
                IRequestProcessor requestProcessor;
                if (contentType == ContentType.KeepAlivePacket)
                {
                    requestProcessor = KeepAliveProcessor.Default;
                }
                else if (contentType == ContentType.Xml)
                {
                    requestProcessor = XmlProcessor.Default;
                }
                else if (contentType == ContentType.Json)
                {
                    requestProcessor = JsonProcessor.Default;
                }
                else
                {
                    throw new NotSupportedException();
                }
                responseContent = requestProcessor.Process(request);
            }
            catch (Exception ex)
            {
                responseContent = XmlResultHelper.NewErrorResult(ex.ToString()).ToPacketContent();
            }
            finally
            {
                Application.Default.SessionMonitor.Update(request.ClientInfo.Session);
                if (responseContent != null)
                {
                    request.UpdateContent(responseContent);
                    SendCenter.Default.Send(request);
                }
            }
        }
	}
}
