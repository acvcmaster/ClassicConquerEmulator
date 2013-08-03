using System;
using System.Linq;
using CCO.Packets;
using CCO.Cryptography;
using System.Net.Sockets;
using System.Text;
using CCO.Entities;

namespace CCO.Networking
{
    public class Client
    {
        public Socket InnerSocket;
        public bool InGame = false;
        public AuthCryptography AuthCrypt = new AuthCryptography();
        public void SendAuth(iPacket P)
        {
            byte[] Data = new byte[P.Data.Length];
            Buffer.BlockCopy(P.Data, 0, Data, 0, P.Data.Length);
            AuthCrypt.Encrypt(Data);
            InnerSocket.Send(Data, 0, Data.Length, SocketFlags.None);
        }
        public void SendGame(iPacket P)
        {
            byte[] Data = new byte[P.Data.Length];
            Buffer.BlockCopy(P.Data, 0, Data, 0, P.Data.Length);
            PacketCrypt.Encrypt(ref Data);
            InnerSocket.Send(Data, 0, Data.Length, SocketFlags.None);
        }
        public string AccountName = "";
        public string CharacterName = "";
        public Character Player;
        public GameCryptography PacketCrypt;
        public void Disconnect()
        {
            Servers.Game.ConnectedClients2.Remove(InnerSocket);
            InnerSocket.Disconnect(false);
        }
    }
}
