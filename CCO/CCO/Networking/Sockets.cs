using System;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;

namespace CCO.Networking
{
    public class nLink
    {
        public byte[] buffer;
        public Socket _socket;
        public object connector;
    }
    public class HybridSocket
    {
        private Dictionary<string, byte> Connections;
        public event Action<nLink> AnnounceNewConnection;
        public event Action<nLink> AnnounceDisconnection;
        public event Action<byte[], nLink, byte[]> AnnounceReceive;
        private Socket _socket;
        public bool Shutdown()
        {
            try
            {
                if (_socket.Connected)
                {
                    _socket.Shutdown(SocketShutdown.Both);
                    _socket.Disconnect(false);
                }
                _socket.Close();
                _socket.Dispose();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("An error has been yielded by the server's system. Description : " + e.ToString(), "Error!");
                return false;
            }

        }
        public HybridSocket(string IP, ushort port)
        {
            Connections = new Dictionary<string, byte>();
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _socket.Bind(new IPEndPoint(IPAddress.Parse(IP), port));
            _socket.Listen(500);
            _socket.BeginAccept(AcceptConnections, new nLink());
        }
        private void AcceptConnections(IAsyncResult result)
        {
            try
            {
                nLink wr = result.AsyncState as nLink;
                wr._socket = _socket.EndAccept(result);
                #region Invisible
                string IP = wr._socket.RemoteEndPoint.ToString().Split(':')[0].ToString();
                if (!Connections.ContainsKey(IP))
                    Connections.Add(IP, 1);
                else
                    if (Connections[IP] <= 12)
                    {
                        byte connections = Connections[IP];
                        Connections.Remove(IP);
                        Connections.Add(IP, (byte)(connections + 1));
                    }
                    else
                    {
                        wr._socket.Disconnect(false);
                        _socket.BeginAccept(AcceptConnections, new nLink());
                        return;
                    }
                #endregion
                wr.buffer = new byte[65535];
                wr._socket.BeginReceive(wr.buffer, 0, 65535, SocketFlags.None, ReceiveData, wr);
                AnnounceNewConnection.Invoke(wr);
                _socket.BeginAccept(AcceptConnections, new nLink());
            }
            catch
            { }
        }
        private void ReceiveData(IAsyncResult result)
        {
            try
            {
                nLink wr = result.AsyncState as nLink;

                string IP = wr._socket.RemoteEndPoint.ToString().Split(':')[0].ToString();
                if (Connections.ContainsKey(IP))
                {
                    SocketError error = SocketError.Disconnecting;
                    int size = wr._socket.EndReceive(result, out error);
                    if (error == SocketError.Success && size != 0)
                    {
                        byte[] buffer = new byte[size];
                        Buffer.BlockCopy(wr.buffer, 0, buffer, 0, size);
                        byte[] question = new byte[] { 1 };
                        AnnounceReceive.Invoke(buffer, wr, question);
                        if (wr._socket.Connected && question[0] == 1)
                            wr._socket.BeginReceive(wr.buffer, 0, 65535, SocketFlags.None, ReceiveData, wr);
                    }
                    else
                    {
                        if (wr._socket.Connected)
                        {
                            wr._socket.Disconnect(true);
                        }
                        byte connections = Connections[IP];
                        Connections.Remove(IP);
                        Connections.Add(IP, (byte)(connections - 1));
                        try
                        {
                            AnnounceDisconnection.Invoke(wr);
                        }
                        catch { }
                    }
                }
            }
            catch
            {

            }
        }
    }
}
