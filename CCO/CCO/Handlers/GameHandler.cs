using CCO;
using System;
using CCO.Data;
using CCO.Networking;
using CCO.Cryptography;
using System.IO;

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
                #region General data
                case 1010:
                    {
                        Packets.GeneralData ThisPacket = new Packets.GeneralData(Data);
                        switch (ThisPacket.Type)
                        {
                            #region Complete login
                            case GeneralDataType.CompleteLogin:
                                {
                                    break;
                                }
                            #endregion
                            #region Set location
                            case GeneralDataType.SetLocation:
                                {
                                    Cli.SendGame(new Packets.GeneralData(Cli.Player.UID, Cli.Player.X,
                                        Cli.Player.Y, 0, Cli.Player.Map, GeneralDataType.SetLocation));

                                    Cli.SendGame(new Packets.GeneralData(Cli.Player.UID, Cli.Player.X,
                                        Cli.Player.Y, 0xFFFF, 0xFFFF, GeneralDataType.ConfirmMap));

                                    break;
                                }
                            #endregion
                            #region Delete char
                            case GeneralDataType.DeleteCharacter:
                                {
                                    Cli.Disconnect();
                                    if (File.Exists("Database/Characters/" + Cli.Player.Name))
                                        File.Delete("Database/Characters/" + Cli.Player.Name);
                                    break;
                                }
                            #endregion
                        }
                        break;
                    }
                #endregion
                default:
                    {
#if DEBUG
                        Program.Report("Packet of type '" + Type + "' was recieved but was not recognized.",
                            ConsoleColor.Yellow, ReportType.Networking);
#endif
                        Cli.SendGame(new Packets.Chat("SYSTEM", Cli.Player.Name, "Unhandled packet '" + Type + "'",
    ChatColor.Green, ChatType.Talk));
                        break;
                    }
            }
            #endregion
        }
    }
}
