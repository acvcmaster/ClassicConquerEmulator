using CCO;
using CCO.Entities;
using CCO.Data;
using System;
using System.IO;
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
        public Dictionary<uint, Client> ConnectedClients = new Dictionary<uint, Client>();
        public Dictionary<Socket, Client> ConnectedClients2 = new Dictionary<Socket, Client>();
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
            byte[] Data = new byte[arg1.Length];
            Buffer.BlockCopy(arg1, 0, Data, 0, arg1.Length);
            GameCryptography GameCrypt = new GameCryptography();
            GameCrypt.Decrypt(ref Data);

            if (BitConverter.ToUInt16(Data, 2) == 1052)
            {
                /* Login request */
                Client Cli;
                if (ConnectedClients.TryGetValue(BitConverter.ToUInt32(Data, 8), out Cli))
                {
                    Cli.PacketCrypt = GameCrypt;
                    Cli.PacketCrypt.GenerateKeys((int)BitConverter.ToUInt32(Data, 4), (int)BitConverter.ToUInt32(Data, 8));
                    ConnectedClients2.Add(arg2._socket, Cli);
                    Cli.InnerSocket = arg2._socket;
                roleback:
                    if (Cli.CharacterName == "None")
                    {
#if DEBUG
                        Program.Report("Character creation sequence triggered for account '" + Cli.AccountName + "'."
                            , ConsoleColor.Magenta, ReportType.Networking);
#endif
                        Cli.SendGame(new Packets.Chat("SYSTEM", "ALLUSERS", "NEW_ROLE",
                            ChatColor.Default, ChatType.LoginInformation));

                    }
                    else
                    {
                        if (!File.Exists("Database/Characters/"+Cli.CharacterName))
                        {
                            Cli.CharacterName = "None";
                            goto roleback;
                        }
                        /* Continue login */
                        Cli.Player = Database.LoadCharacter(Cli.CharacterName);
                        Cli.SendGame(new Packets.Chat("SYSTEM", "ALLUSERS", "ANSWER_OK",
                            ChatColor.Default, ChatType.LoginInformation));
                        Cli.SendGame(new Packets.CharacterInformation(Cli.Player));
                    }
                }
            }
            else
                GameHandler.Handle(ConnectedClients2[arg2._socket], arg1);            
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
