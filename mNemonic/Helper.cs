using Hardcodet.Wpf.TaskbarNotification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mNemonic
{
    public static class Helper
    {

        public static void UpdateToolTip(TaskbarIcon tb, int interval)
        {
            tb.ToolTipText = string.Format("Next mNeme to be displayed @ {0:HH:mm:ss}", DateTime.Now.AddMilliseconds(interval));
        }
    }
}
