using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Trader.Server.ValueObjects;
using System.Threading;
using System.Collections.Concurrent;
using Trader.Common;

namespace Trader.Server.Core
{
    public class CommandTransfer:ICommandTransfer
    {
        private readonly AutoResetEvent _SendQuotationEvent = new AutoResetEvent(false);
        private readonly ConcurrentQueue<CommandForClient> _Commands = new ConcurrentQueue<CommandForClient>();
        private CommandForClient _Current;
        private volatile bool _Started;
        private volatile bool _Stopped;
        private IClientsProcessor _ClientsProcessor;
        public CommandTransfer(IClientsProcessor clientsProccessor)
        {
            _ClientsProcessor = clientsProccessor;
        }

        public void Send(CommandForClient command)
        {
            _Commands.Enqueue(command);
            _SendQuotationEvent.Set();
        }

        public bool Start()
        {
            if (_Started)
                return _Started; 
            Thread thread = new Thread(Dispatch) {IsBackground=true };
            thread.Start();
            _Started = true;
            return _Started;
        }

        public bool Stop()
        {
            _Stopped = true;
            return _Stopped;
        }

        private void Dispatch()
        {
            while (true)
            {
                if (_Stopped) break;
                _SendQuotationEvent.WaitOne();
                while (_Commands.TryDequeue(out _Current))
                {
                    _ClientsProcessor.ParallelForEach(SendCommandHandler);
                }
            }
        }

        private void SendCommandHandler(KeyValuePair<Session, SenderReceiverPair> p)
        {
            p.Value.Sender.Send(_Current);
        }


      
    }
}
