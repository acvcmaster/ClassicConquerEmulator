using System;
using System.Drawing;

namespace CCO.Entities
{
    public interface iEntity
    {
        ushort X { get; set; }
        ushort Y { get; set; }
        ushort Map { get; set; }
    }
}
