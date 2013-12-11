using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mNemonic
{
    public class StoredmNeme : IEquatable<StoredmNeme>
    {
        public string Location { get; set; }
        public int Coefficient { get; set; }
        public long Time { get; set; }
        public int Remembered { get; set; }

        public StoredmNeme(string location, int coefficient, long time, int remembered)
        {
            Location = location;
            Coefficient = coefficient;
            Time = time;
            Remembered = remembered;
        }

        public StoredmNeme(string location)
        {
            Location = location;            
        }


        public bool Equals(StoredmNeme other)
        {
            bool result = false;

            if (this.Location.Equals(other.Location, StringComparison.InvariantCultureIgnoreCase))
            {
                result = true;
            }

            return result;

        }
    }
}
