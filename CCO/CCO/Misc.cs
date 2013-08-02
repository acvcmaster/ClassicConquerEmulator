using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCO
{
    public class Misc
    {
        static Random R = new Random();
        public static uint Next()
        {
            return (uint)R.Next(0, int.MaxValue);
        }
        public static int Next(int a, int b)
        {
            return R.Next(a, b);
        }
        public static void BinaryDump(byte[] Data)
        {
            StreamWriter S = new StreamWriter("packetdump");
            BinaryWriter B = new BinaryWriter(S.BaseStream);
            B.Write(Data);
            B.Close();
            S.Close();
        }
    }
}
