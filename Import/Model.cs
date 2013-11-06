using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Import
{

    public class Model
    {
        public string WindowTitle { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }

        public Model(string title)
        {
            WindowTitle = title;
        }

        public bool WriteToFile(string input, bool isQuestion=false)
        {

            return true;


        }

    }
}
