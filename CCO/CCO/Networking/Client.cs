﻿using System;
using CCO.Packets;
using CCO.Cryptography;
using System.Net.Sockets;
using System.Text;

namespace CCO.Networking
{
    public class Client
    {
        public Socket InnerSocket;
        public AuthCryptography AuthCrypt = new AuthCryptography();
        public void SendAuth(Packet P)
        {
            byte[] Data = new byte[P.Data.Length];
            Buffer.BlockCopy(P.Data, 0, Data, 0, P.Data.Length);
            AuthCrypt.Encrypt(Data);
            InnerSocket.Send(Data, 0, Data.Length, SocketFlags.None);
        }
        public void SendGame(Packet P)
        {
            byte[] Data = new byte[P.Data.Length];
            Buffer.BlockCopy(P.Data, 0, Data, 0, P.Data.Length);
            PacketCrypt.Encrypt(ref Data);
            InnerSocket.Send(Data, 0, Data.Length, SocketFlags.None);
        }
        public string AccountName = "";
        public string CharacterName = "";
        public GameCryptography PacketCrypt;
    }
}
