#define CREATE_NEW_ACCOUNTS
using System;
using System.IO;
using System.Text;
using CCO.Entities;
using CCO.Networking;
using System.Collections.Generic;

namespace CCO.Data
{
    public class Database
    {
        public static string GameServerIP = "127.0.0.1";
        public static ushort LoginPort = 9958;
        public static ushort GamePort = 5816;
        public static List<char> InvalidCharacters = new List<char> { '[', ']', ' ', '(', ')', '{', '}' };
        public static List<string> InvalidNames = new List<string> { "ADM", "System", "Administrator","Server"};
        public static bool ValidadeLogin(ref string Account, byte[] HashedPassword)
        {
#if CREATE_NEW_ACCOUNTS
            if (Account.StartsWith("new"))
            {
                Account = Account.Substring(3, Account.Length - 3);
                CreateAccount(Account, HashedPassword);
            }
#endif
            if (File.Exists("Database/Accounts/" + Account))
            {
                StreamReader R = new StreamReader("Database/Accounts/" + Account);
                BinaryReader B = new BinaryReader(R.BaseStream);
                byte[] LPassword = B.ReadBytes(16);
                B.Close();
                R.Close();
                return Same(HashedPassword, LPassword);
            }
            
            return false;
        }
        public static void CreateAccount(string Name, byte[] Password)
        {
            if (!File.Exists("Database/Accounts/" + Name))
            {
                StreamWriter S = new StreamWriter("Database/Accounts/" + Name);
                BinaryWriter B = new BinaryWriter(S.BaseStream);
                B.Write(Password);
                B.Write("None"); /* Character name */
                S.Close();
                B.Close();
            }
        }
        public static void SetClient(ref Client Cli, string Account)
        {
            Cli.AccountName = Account;
            StreamReader R = new StreamReader("Database/Accounts/" + Account);
            BinaryReader B = new BinaryReader(R.BaseStream);
            B.ReadBytes(16);
            Cli.CharacterName = B.ReadString();
            B.Close();
            R.Close();
        }
        public static bool Same(byte[] A, byte[] B)
        {
            if (A.Length == B.Length)
            {
                for (int a = 0; a < A.Length; a++)
                    if (A[a] != B[a])
                        return false;
                return true;
            }
            return false;
        }
        public static string CreateCharacter(Client Cli, string CharName, ushort Model, byte Class)
        {
            if (!File.Exists("Database/Characters/" + CharName))
            {
                if (ValidName(CharName))
                {
                    try
                    {
                        ushort Avatar;
                        #region Avatar set up
                        if (IsMale(Model))
                            Avatar = (ushort)Misc.Next(1, 50);
                        else
                            Avatar = (ushort)Misc.Next(201, 225);
                        #endregion

                        /* Time to create file! */
                        Cli.CharacterName = CharName;
                        dynamic S = new StreamWriter("Database/Characters/" + CharName);
                        dynamic B = new BinaryWriter(S.BaseStream);
                        B.Write(Model);
                        B.Write(Class);
                        B.Write((ushort)1010);  // Map
                        B.Write((ushort)61);  // X
                        B.Write((ushort)109);  // Y
                        B.Write((byte)1); // Level
                        B.Write(Misc.Next()); // UID
                        B.Write(Avatar);
                        S.Close();
                        B.Close();

                        S = new StreamReader("Database/Accounts/" + Cli.AccountName);
                        B = new BinaryReader(S.BaseStream);
                        byte[] Password = B.ReadBytes(16);
                        S.Close();
                        B.Close();

                        S = new StreamWriter("Database/Accounts/" + Cli.AccountName);
                        B = new BinaryWriter(S.BaseStream);
                        B.Write(Password);
                        B.Write(Cli.CharacterName);
                        S.Close();
                        B.Close();

                        return "ANSWER_OK";
                    }
                    catch
                    { 
                        return "An unknown error occured. Try again.";
                    }
                }
                else return "The name entered is of invalid length(Not 5~16) or contains invalid characters / forbidden words.";
            }
            else return "A character with this name already exists. Try another one.";
        }
        public static bool ValidName(string Name)
        {
            if (Name.Length >= 5 && Name.Length <= 16)
            {
                for (int a = 0; a < Name.Length; a++)
                    if (InvalidCharacters.Contains(Name[a]))
                        return false;

                foreach (string K in InvalidNames)
                    if (Name.ToLower().Contains(K.ToLower()))
                        return false;

                return true;
            }
            return false;
        }
        public static void SaveCharacter(Character Char)
        {
            try
            {
                StreamWriter S = new StreamWriter("Database/Characters/" + Char.Name);
                BinaryWriter B = new BinaryWriter(S.BaseStream);
                B.Write(Char.Model);
                B.Write(Char.Job);
                B.Write(Char.Map);  // Map
                B.Write(Char.X);  // X
                B.Write(Char.Y);  // Y
                B.Write(Char.Level); // Level
                B.Write(Char.UID);
                B.Write(Char.Avatar);
                S.Close();
                B.Close();
            }
            catch
            {
                Program.Report("Character '" + Char.Name + "' failed to save, an error occured."
                    , ConsoleColor.Red, ReportType.None);
            }
        }
        public static Character LoadCharacter(string Name)
        {
            try
            {
                StreamReader S = new StreamReader("Database/Characters/" + Name);
                BinaryReader B = new BinaryReader(S.BaseStream);
                Character Result = new Character();
                Result.Model = B.ReadUInt16();
                Result.Job = B.ReadByte();
                Result.Map = B.ReadUInt16();
                Result.X = B.ReadUInt16();
                Result.Y = B.ReadUInt16();
                Result.Level = B.ReadByte();
                Result.UID = B.ReadUInt32();
                Result.Avatar = B.ReadUInt16();
                Result.Name = Name;
                S.Close();
                B.Close();
                return Result;
            }
            catch
            {
                Program.Report("Character '" + Name + "' failed to load, an error occured."
                   , ConsoleColor.Red, ReportType.None);
            }
            return null;
        }
        public static bool IsMale(uint Model)
        {
            string Mesh = Convert.ToString(Model);
            if (Mesh[Mesh.Length - 1] == '1' || Mesh[Mesh.Length - 1] == '2')
                return false;
            else return true;
        }

    }
}
