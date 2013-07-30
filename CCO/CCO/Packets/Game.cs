using System;
using CCO.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CCO.Packets
{
    public class Chat : Packet
    {
        byte[] _data;
        public byte[] Data
        {
            get { return _data; }
            set { _data = value; }
        }
        public Chat(string From, string To, string Message, ChatColor Color, ChatType Type)
        {
            _data = new byte[From.Length + To.Length + Message.Length + 24];
            Writer.WriteUInt16(55, 0, ref _data);
            Writer.WriteUInt16(1004, 2, ref _data);
            Writer.WriteUInt32((uint)Color, 4, ref _data);
            Writer.WriteUInt32((uint)Type, 8, ref _data);
            Writer.WriteUInt32(1523, 12, ref _data);
            Writer.WriteByte(3, 16, ref _data);
            Writer.WriteByte((byte)From.Length, 17, ref _data);
            int Pos = 18;
            Writer.WriteString(From, Pos, ref _data);
            Pos += From.Length;
            Writer.WriteByte((byte)To.Length, Pos, ref _data);
            Pos++;
            Writer.WriteString(To, Pos, ref _data);
            Pos += To.Length;
            Writer.WriteByte(0, Pos, ref _data); /* Suffix length.. WTF */
            Pos++;
            Writer.WriteByte((byte)Message.Length, Pos, ref _data);
            Pos++;
            Writer.WriteString(Message, Pos, ref _data);
        }
    }
}
