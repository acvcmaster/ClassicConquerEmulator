using System;
using CCO.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCO.Packets
{
    public class AuthResponseOK : iPacket
    {
        byte[] _data;
        public byte[] Data
        {
            get { return _data; }
            set { _data = value; }
        }
        public AuthResponseOK(uint LoginToken)
        {
            _data = new byte[32];
            Writer.WriteUInt16(32, 0, ref _data);
            Writer.WriteUInt16(1055, 2, ref _data);
            Writer.WriteUInt32(LoginToken, 4, ref _data); /* Actually, account ID */
            Writer.WriteUInt32(LoginToken, 8, ref _data);
            Writer.WriteString(Database.GameServerIP, 12, ref _data);
            Writer.WriteUInt16(Database.GamePort, 28, ref _data);
        }
    }
}
