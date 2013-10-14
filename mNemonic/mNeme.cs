using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mNemonic
{
    public class mNeme
    {
        public mNemeType Type { get; set; }
        public string Location { get; set; }

        public mNeme(mNemeType type, string location)
        {
            Type = type;
            Location = location;
        }

        public mNeme( string location)
        {
            Location = location;
        }

    }
}
