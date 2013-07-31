using System;
using CCO.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CCO.Packets
{
    public class Chat : iPacket
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
            Writer.WriteUInt16((ushort)_data.Length, 0, ref _data);
            Writer.WriteUInt16(1004, 2, ref _data);
            Writer.WriteUInt32((uint)Color, 4, ref _data);
            Writer.WriteUInt16((ushort)Type, 8, ref _data);
            Writer.WriteUInt32(1523, 12, ref _data);
            Writer.WriteByte(4, 16, ref _data);
            Writer.WriteByte((byte)From.Length, 17, ref _data);
            int Pos = 18;
            Writer.WriteString(From, Pos, ref _data);
            Pos += From.Length;
            Writer.WriteByte((byte)To.Length, Pos, ref _data);
            Pos++;
            Writer.WriteString(To, Pos, ref _data);
            Pos += To.Length;
            Pos++;
            Writer.WriteByte((byte)Message.Length, Pos, ref _data);
            Pos++;
            Writer.WriteString(Message, Pos, ref _data);
        }
    }
    public class CreateCharacter : iPacket
    {
        byte[] _data;
        public byte[] Data
        {
            get { return _data; }
            set { _data = value; }
        }
        public CreateCharacter(byte[] data)
        {
            Data = data;
        }
        public string Name
        {
            get 
            {
                string name = "";

                for (int a = 0; a < 16; a++)
                    if (Data[20 + a] == 0)
                        break;
                    else
                        name += (char)Data[20 + a];

                name.Trim();
                return name;
            }
        }
        public ushort Model
        {
            get { return BitConverter.ToUInt16(Data, 52); }
        }
        public ushort Class
        {
            get { return BitConverter.ToUInt16(Data, 54); }
        }
    }
}
