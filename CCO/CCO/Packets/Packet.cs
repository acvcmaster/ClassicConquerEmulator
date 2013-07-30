using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCO.Packets
{
    public static unsafe class Writer
    {
        public static void WriteByte(byte A, int Offset, ref byte[] Destination)
        {
            Destination[Offset] = A;
        }
        public static void WriteUInt16(ushort A, int Offset, ref byte[] Destination)
        {
            for (int a = 0; a < sizeof(ushort); a++)
                Destination[Offset + a] = *(((byte*)(&A)) + a);
        }
        public static void WriteUInt32(uint A, int Offset, ref byte[] Destination)
        {
            for (int a = 0; a < sizeof(uint); a++)
                Destination[Offset + a] = *(((byte*)(&A)) + a);
        }
        public static void WriteUInt64(ulong A, int Offset, ref byte[] Destination)
        {
            for (int a = 0; a < sizeof(ulong); a++)
                Destination[Offset + a] = *(((byte*)(&A)) + a);
        }
        public static void WriteBytes(byte[] A, int Offset, ref byte[] Destination)
        {
            for (int a = 0; a < A.Length; a++)
                Destination[Offset + a] = A[a];
        }
        public static void WriteString(string A, int Offset, ref byte[] Destination)
        {
            WriteBytes(ASCIIEncoding.ASCII.GetBytes(A), Offset, ref Destination);
        }
    }
    public interface Packet
    {
        byte[] Data { get; set; }
    }
}
