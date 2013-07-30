using CCO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCO.Networking
{
    public static class Servers
    {
        public static LoginServer Login;
    }
    public class LoginServer
    {
        ushort port = 0;
        public ushort Port
        {
            get { return port; }
            set { port = value; }
        }
        HybridSocket Listener;
        public LoginServer(ushort _port)
        {
            Port = _port;
            Listener = new HybridSocket(Port);
            Program.Report("Login server listening in all network interfaces on port " + Port + ".",
                ConsoleColor.White, ReportType.Networking);
        }
    }
}
