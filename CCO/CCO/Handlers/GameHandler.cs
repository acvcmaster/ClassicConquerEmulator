using CCO;
using System;
using CCO.Data;
using CCO.Networking;
using CCO.Cryptography;

namespace CCO.Handlers
{
    public class GameHandler
    {
        public static void Handle(Client Cli, byte[] Data)
        {
            Cli.PacketCrypt.Decrypt(ref Data);
            ushort Length = BitConverter.ToUInt16(Data, 0);
            ushort Type = BitConverter.ToUInt16(Data, 2);
            #region Handler
            switch (Type)
            {
                #region Character creation
                case 1001:
                    {
                        CharacterCreation.Handle(Cli, Data);
                        break;
                    }
                #endregion
            }
            #endregion
        }
    }
}
