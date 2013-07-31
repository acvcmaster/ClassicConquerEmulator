#define CREATE_NEW_ACCOUNTS
using System;
using System.IO;
using System.Text;
using CCO.Networking;
using System.Collections.Generic;

namespace CCO.Data
{
    public class Database
    {
        public static string GameServerIP = "127.0.0.1";
        public static ushort LoginPort = 9958;
        public static ushort GamePort = 5816;
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
    }
}
