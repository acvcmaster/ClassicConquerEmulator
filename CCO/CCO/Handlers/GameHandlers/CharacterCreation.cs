using CCO;
using System;
using CCO.Data;
using CCO.Packets;
using CCO.Networking;
using CCO.Cryptography;


namespace CCO.Handlers
{
    public class CharacterCreation
    {
        public static void Handle(Client Cli, byte[] Data)
        {
            CreateCharacter FormatedData = new CreateCharacter(Data);
            string CreationString = Database.CreateCharacter
                (Cli, FormatedData.Name, FormatedData.Model, FormatedData.Class);
#if DEBUG
            Program.Report("Account '" + Cli.AccountName + "' trying to create character named '" +
                FormatedData.Name + "' using model '" + FormatedData.Model + "' and class '" + FormatedData.Class + "'."
                , ConsoleColor.Blue, ReportType.None);
#endif
            Cli.SendGame(new Packets.Chat("SYSTEM", "ALLUSERS", CreationString, ChatColor.Default, ChatType.Dialog));
        }
    }
}
