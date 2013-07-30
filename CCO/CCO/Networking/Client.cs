using System;
using CCO.Packets;
using CCO.Cryptography;
using System.Net.Sockets;

namespace CCO.Networking
{
    public class Client
    {
        public Socket InnerSocket;
        public AuthCryptography AuthCrypt = new AuthCryptography();
        public void SendAuth(Packet P)
        {
            byte[] Data = P.Data;
            AuthCrypt.Encrypt(Data);
            InnerSocket.Send(Data, 0, Data.Length, SocketFlags.None);
        }
    }
}
