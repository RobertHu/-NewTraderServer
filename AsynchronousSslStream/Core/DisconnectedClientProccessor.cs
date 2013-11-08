using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Trader.Common;
using System.Threading;
using System.Collections.Concurrent;

namespace Trader.Server.Core
{
   public class DisconnectedClientProccessor:IDisconnectedClientProccessor
    {
        private readonly AutoResetEvent _DisconnectEvent = new AutoResetEvent(false);
        private readonly ConcurrentQueue<Session> _DisconnectQueue = new ConcurrentQueue<Session>();
        private volatile bool _Started;
        private volatile bool _Stopped;
        private IClientsProcessor _ClientsProcessor;

        public DisconnectedClientProccessor(IClientsProcessor clientsProccessor)
        {
            _ClientsProcessor = clientsProccessor;
        }


        public void Add(Session session)
        {
            _DisconnectQueue.Enqueue(session);
            _DisconnectEvent.Set();
        }

        private void DisconnectHandle()
        {
            while (true)
            {
                if (_Stopped) break;
                _DisconnectEvent.WaitOne();
                while (_DisconnectQueue.Count != 0)
                {
                    if (_Stopped) break;
                    Session session;
                    if (_DisconnectQueue.TryDequeue(out session))
                    {
                        _ClientsProcessor.Remove(session);
                    }
                }
            }
        }


        public bool Start()
        {
            if (_Started)
                return _Started;
            var thread = new Thread(DisconnectHandle) { IsBackground = true };
            thread.Start();
            _Started = true;
            return _Started;
        }

        public bool Stop()
        {
            _Stopped = true;
            return _Stopped;
        }
    }
}
