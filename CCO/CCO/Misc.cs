using System;
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
    }
}
