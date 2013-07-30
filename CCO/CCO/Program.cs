using System;
using CCO.Networking;
using CCO.Data;

namespace CCO
{
    class Program
    {
        public static void Report(string K, ConsoleColor Color = ConsoleColor.White, ReportType Type = ReportType.None)
        {
            ConsoleColor Cache = Console.ForegroundColor;
            Console.ForegroundColor = Color;
            #region Type
            switch (Type)
            {
                case ReportType.Networking:
                    {
                        Console.Write("[Network]");
                        break;
                    }
            }
            #endregion
            Console.WriteLine(K);
            Console.ForegroundColor = Cache;
        }
        static void Main(string[] args)
        {
            Servers.Login = new LoginServer(Database.LoginPort);

            /* Hang */
            for (; ; ) { }
        }
    }
    public enum ReportType
    {
        Networking,
        None
    }
}
