using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mNemonic
{
    public class State
    {
        public int IntervalTimer { get; set; }
        public bool Paused { get; set; }

        public State(int timer, bool paused)
        {
            IntervalTimer = timer;
            Paused = paused;
        }
        public State()
        { }

    }
}
