using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Trader.Server.Core
{
    public class ComponentFactory
    {
        private static IClientsProcessor _ClientsProccessor;
        private static IDisconnectedClientProccessor _DisconnectedClientProccessor;
        private static ICommandTransfer _CommandTransfer;
        private static AgentController _AgentController;

        public static IClientsProcessor CreateClientsProcessor()
        {
            if (_ClientsProccessor == null)
                _ClientsProccessor = new ClientsCenter();
            return _ClientsProccessor;
        }

        public static IDisconnectedClientProccessor CreateDisconnectedClientProccessor(IClientsProcessor clientsProccessor)
        {
            if (_DisconnectedClientProccessor == null)
                _DisconnectedClientProccessor = new DisconnectedClientProccessor(clientsProccessor);
            return _DisconnectedClientProccessor;
        }

        public static ICommandTransfer CreateCommandTransfer(IClientsProcessor clientsProccessor)
        {
            if (_CommandTransfer == null)
                _CommandTransfer = new CommandTransfer(clientsProccessor);
            return _CommandTransfer;
        }

        public static AgentController CreateAgentController()
        {
            if (_AgentController == null)
            {
                var clientsProcessor = CreateClientsProcessor();
                var disconnectedClientProccessor = CreateDisconnectedClientProccessor(clientsProcessor);
                var commandTransfer = CreateCommandTransfer(clientsProcessor);
                _AgentController=new AgentController(clientsProcessor, commandTransfer, disconnectedClientProccessor);
            }
            return _AgentController;
        }

    }
}
