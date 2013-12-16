using mNemonic.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mNemonic.ViewModel
{
    public class TaskBarIconVM : INotifyPropertyChanged
    {
        TaskBarIconModel model;

        #region movetoabstractclass
        // boiler-plate
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        protected bool SetField<T>(ref T field, T value, string propertyName)
        {
            if (EqualityComparer<T>.Default.Equals(field, value))
            {
                return false;
            }

            field = value;
            OnPropertyChanged(propertyName);

            return true;
        }
        #endregion

        private string toolTipText;
        public string ToolTipText
        {
            get { return toolTipText; }
            set { SetField(ref toolTipText, value, "ToolTipText"); }
        }

        public TaskBarIconVM(TaskBarIconModel model)
        {
            this.model = model;

            ToolTipText = model.UpdateToolTip();
        }
    }
}
