using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mNemonic.Model
{
    public class TaskBarIconModel
    {
        string toolTipText;
        int timerInterval;

        public TaskBarIconModel(string toolTipText, int interval)
        {
            this.toolTipText = toolTipText;
            this.timerInterval = interval;
        }

        public string UpdateToolTip()
        {
            return string.Format("Next mNeme to be displayed @ {0:HH:mm:ss}", DateTime.Now.AddMilliseconds(timerInterval));
        }
    }
}
