using CCO;
using System;
using CCO.Handlers;
using CCO.Cryptography;
using System.Net.Sockets;
using System.Collections.Generic;

namespace CCO.Networking
{
    public static class Servers
    {
        public static LoginServer Login;
        public static GameServer Game;
    }
    public class LoginServer
    {
        public Dictionary<Socket, Client> ConnectedClients = new Dictionary<Socket, Client>();
        ushort port = 0;
        public ushort Port
        {
            get { return port; }
            set { port = value; }
        }
        HybridSocket Listener;
        public LoginServer(ushort _port)
        {
            AuthCryptography.PrepareAuthCryptography();
            Port = _port;
            Listener = new HybridSocket(Port);
            Program.Report("Login server listening in all network interfaces on port " + Port + ".",
                ConsoleColor.White, ReportType.Networking);
            Listener.AnnounceNewConnection += Listener_AnnounceNewConnection;
            Listener.AnnounceDisconnection += Listener_AnnounceDisconnection;
            Listener.AnnounceReceive += Listener_AnnounceReceive;
        }

        void Listener_AnnounceReceive(byte[] arg1, nLink arg2, byte[] arg3)
        {
#if DEBUG
            Program.Report("Data recieved!", ConsoleColor.Green, ReportType.Networking);
#endif
            AuthHandler.Handle(ConnectedClients[arg2._socket], arg1);
        }

        void Listener_AnnounceDisconnection(nLink obj)
        {
        }

        void Listener_AnnounceNewConnection(nLink obj)
        {
#if DEBUG
            Program.Report("Connection recieved!", ConsoleColor.Green, ReportType.Networking);
#endif
            if (!ConnectedClients.ContainsKey(obj._socket))
            {
                Client Connecting = new Client();
                Connecting.InnerSocket = obj._socket;
                ConnectedClients.Add(obj._socket, Connecting);
            }
        }
    }
    public class GameServer
    {
        ushort port = 0;
        public ushort Port
        {
            get { return port; }
            set { port = value; }
        }
        HybridSocket Listener;
        public GameServer(ushort _port)
        {
            Port = _port;
            Listener = new HybridSocket(Port);
            Program.Report("Game server listening in all network interfaces on port " + Port + ".",
                ConsoleColor.White, ReportType.Networking);
            Listener.AnnounceNewConnection += Listener_AnnounceNewConnection;
            Listener.AnnounceDisconnection += Listener_AnnounceDisconnection;
            Listener.AnnounceReceive += Listener_AnnounceReceive;
        }

        void Listener_AnnounceReceive(byte[] arg1, nLink arg2, byte[] arg3)
        {
#if DEBUG
            Program.Report("Data recieved! (Game server)", ConsoleColor.Green, ReportType.Networking);
#endif
        }

        void Listener_AnnounceDisconnection(nLink obj)
        {
        }

        void Listener_AnnounceNewConnection(nLink obj)
        {
#if DEBUG
            Program.Report("Connection recieved! (Game server)", ConsoleColor.Green, ReportType.Networking);
#endif
        }
    }
}
