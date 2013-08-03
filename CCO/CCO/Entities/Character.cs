using System;
using CCO.Networking;

namespace CCO.Entities
{
    public class Character : iEntity
    {
        ushort x, y, map,
            model, avatar;
        byte level, job, hairColor, hairStyle;
        uint uid;
        string name;
        public ushort X
        {
            get { return x; }
            set { x = value; }
        }
        public ushort Y
        {
            get { return y; }
            set { y = value; }
        }
        public ushort Map
        {
            get { return map; }
            set { map = value; }
        }
        public ushort Model
        {
            get { return model; }
            set { model = value; }
        }
        public byte HairColor
        {
            get { return hairColor; }
            set { hairColor = value; }
        }
        public byte HairStyle
        {
            get { return hairStyle; }
            set { hairStyle = value; }
        }
        public ushort Hair
        {
            get { return ushort.Parse(HairColor.ToString() + HairStyle.ToString()); }
        }
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public byte Level
        {
            get { return level; }
            set { level = value; }
        }
        public byte Job
        {
            get { return job; }
            set { job = value; }
        }
        public uint UID
        {
            get { return uid; }
            set { uid = value; }
        }
        public ushort Avatar
        {
            get { return avatar; }
            set { avatar = value; }
        }
        public uint Mesh
        {
            get { return uint.Parse(Avatar.ToString() + Model.ToString()); }
        }
        public Client CClient;

    }
}
