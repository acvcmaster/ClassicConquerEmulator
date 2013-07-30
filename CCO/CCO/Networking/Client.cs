using System;
using CCO.Cryptography;
using System.Net.Sockets;

namespace CCO.Networking
{
    public class Client
    {
        public Socket InnerSocket;
        public AuthCryptography AuthCrypt = new AuthCryptography();
    }
}
