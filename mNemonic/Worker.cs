using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mNemonic
{
    public class Worker
    {
        public Task<string> GetNextItemAsync()
        {
            return Task.Run(() => "hello");
        }
    }
}
