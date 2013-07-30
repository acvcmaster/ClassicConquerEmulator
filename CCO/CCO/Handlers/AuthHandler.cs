using System;
using CCO.Data;
using CCO.Packets;
using System.Text;
using CCO.Networking;
using CCO.Cryptography;
using System.Security.Cryptography;


namespace CCO.Handlers
{
    public class AuthHandler
    {
        public static void Handle(Client Cli, byte[] Data)
        {
            Cli.AuthCrypt.Decrypt(Data);
            ushort Length = BitConverter.ToUInt16(Data, 0);
            ushort Type = BitConverter.ToUInt16(Data, 2);
#if DEBUG
            Program.Report("Auth packet of type '" + Type + "' and size '" + Length + "(" + Data.Length + ")' was " +
                "recieved.", ConsoleColor.Red, ReportType.Networking);
#endif

            switch (Type)
            {
                #region Login Request
                case 1051:
                    {
                        string Account = Encoding.ASCII.GetString(Data, 4, 16).Trim('\0');
                        byte[] Password = new byte[16];
                        Buffer.BlockCopy(Data, 20, Password, 0, 16);
                        /* OK so here's the thing : I don't need to know people's       */
                        /* password in order to validate them, so what I'm basically    */
                        /* going to do is : I'm not decrypting this password. This      */
                        /* will save me some time and will make things safer for        */
                        /* everyone. Not only I'm not going to decrypt it, but I'm also */
                        /* going to hash this password. Of course, a generic password   */
                        /* mechanism will have to be created to allow game master to    */
                        /* access other people's account if should needed.              */
#if DEBUG
                        Program.Report("Login data recieved! Attempt to login on account " +
                            "'" + Account + "'. Awaiting database validation.", ConsoleColor.Red,
                            ReportType.Networking);
#endif
                        HashAlgorithm Hasher = MD5.Create();
                        Password = Hasher.ComputeHash(Password);
                        if (Database.ValidadeLogin(ref Account, Password))
                        {
                            /* Send valid 'Valid Auth' response */
                            Program.Report("Login validated for account '"+Account+"'.", ConsoleColor.Green,
                                ReportType.Networking);

                            AuthResponseOK Response = new AuthResponseOK(1);
                            Cli.SendAuth(Response);
                            
                        }
                        else
                        {
                            Program.Report("Database rejected login attempt of account '" + Account + "'.", ConsoleColor.Green,
                                ReportType.Networking);
                            Servers.Login.ConnectedClients.Remove(Cli.InnerSocket);
                            /* And then send 'Invalid Auth' response */
                        }
                        break;
                    }
                #endregion
            }
        }
    }
}
