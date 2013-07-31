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
            bool CanCreate = Database.CreateCharacter
                (FormatedData.Name, FormatedData.Model, FormatedData.Class);
#if DEBUG
            Program.Report("Account '" + Cli.AccountName + "' trying to create character named '" +
                FormatedData.Name + "' using model '" + FormatedData.Model + "' and class '" + FormatedData.Class + "'."
                , ConsoleColor.Blue, ReportType.None);
#endif
            if (!CanCreate)
            {
                Cli.SendGame(new Packets.Chat("SYSTEM", "ALLUSERS",
                    "The database rejected this character. Please check if this name is already taken. Also, check if the length is " +
              "between 5~16 characters and also if it doesn't contain symbols like '[' or ']'.", ChatColor.Default, ChatType.Dialog));
            }
            else
            {
                Cli.SendGame(new Packets.Chat("SYSTEM", "ALLUSERS", "ANSWER_OK", ChatColor.Default, ChatType.Dialog));
                //Cli.Disconnect();
            }
        }
    }
}
