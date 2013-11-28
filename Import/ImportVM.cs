using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Import
{
    public class ImportVM : INotifyPropertyChanged
    {
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

        ImportModel model;

        #region Properties
    
        private string windowTitle;
        public string WindowTitle
        {
            get { return windowTitle; }
            set { SetField(ref windowTitle, value, "WindowTitle"); }
        }
        #endregion

        public ImportVM(ImportModel model)
        {
            this.model = model;
            WindowTitle = model.WindowTitle;
        }
    }
}
