using System;
using System.IO;
using CCO.Data;
using CCO.Entities;


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
        public byte Class
        {
            get { return (byte)BitConverter.ToUInt16(Data, 54); }
        }
    }
    public class CharacterInformation : iPacket
    {
        byte[] _data;
        public byte[] Data
        {
            get { return _data; }
            set { _data = value; }
        }
        public CharacterInformation(Character A)
        {
            string Spouse = "None";
            _data = new byte[66 + A.Name.Length + Spouse.Length];
            Writer.WriteUInt16((ushort)_data.Length, 0, ref _data);
            Writer.WriteUInt16(1006, 2, ref _data);
            Writer.WriteUInt32(A.UID, 4, ref _data);
            Writer.WriteUInt32(A.Mesh, 8, ref _data); 
            Writer.WriteUInt16(0, 12, ref _data); // Hair Style
            Writer.WriteUInt32(0, 16, ref _data); // Gold
            Writer.WriteUInt32(0, 20, ref _data); // EXP
            /* 
               40	 Strength Stats	 ushort
               42	 Dexterity Stats ushort
               44	 Vitality Stats	 ushort
               46	 Spirit Stats	 ushort
               48	 Unspent Stats	 ushort
               50	 Current HP	 ushort	 1624	
               52	 Current MP	 ushort	 0	
               54	 PK Points	 ushort	 0
            */
            Writer.WriteByte(A.Level, 56, ref _data);
            Writer.WriteByte(A.Job, 57, ref _data);
            Writer.WriteByte(0, 59, ref _data); // Reborn
            Writer.WriteByte(1, 60, ref _data); // bool : display names
            Writer.WriteByte(2, 61, ref _data);
            Writer.WriteByte((byte)A.Name.Length, 62, ref _data);
            int pos = 63;
            Writer.WriteString(A.Name, pos, ref _data);
            pos += A.Name.Length;
            Writer.WriteByte((byte)Spouse.Length, pos, ref _data);
            pos++;
            Writer.WriteString(Spouse, pos, ref _data);
        }
    }
    public class GeneralData : iPacket
    {
        byte[] _data;
        public byte[] Data
        {
            get { return _data; }
            set { _data = value; }
        }
        public GeneralData(byte[] Packet)
        {
            _data = Packet;
        }
        public GeneralData(uint UID, ushort a, ushort b, ushort c, ushort d, GeneralDataType type)
        {
            _data = new byte[28];
            Writer.WriteUInt16(28, 0, ref _data);
            Writer.WriteUInt16(1010, 2, ref _data);
            Writer.WriteUInt32(UID, 8, ref _data);
            A = a;
            B = b;
            C = c;
            D = d;
            Type = type;
        }
        public GeneralDataType Type
        {
            get { return (GeneralDataType)BitConverter.ToUInt32(_data, 24); }
            set { Writer.WriteUInt32((uint)value, 24, ref _data); }
        }
        public ushort A
        {
            get { return BitConverter.ToUInt16(_data, 12); }
            set { Writer.WriteUInt16(value, 12, ref _data); }
        }
        public ushort B
        {
            get { return BitConverter.ToUInt16(_data, 14); }
            set { Writer.WriteUInt16(value, 14, ref _data); }
        }
        public ushort C
        {
            get { return BitConverter.ToUInt16(_data, 16); }
            set { Writer.WriteUInt16(value, 16, ref _data); }
        }
        public uint D
        {
            get { return BitConverter.ToUInt32(_data, 20); }
            set { Writer.WriteUInt32(value, 20, ref _data); }
        }
    }
}