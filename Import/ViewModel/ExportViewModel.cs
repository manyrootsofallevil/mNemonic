using Import.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Import.ViewModel
{
    public class ExportViewModel : INotifyPropertyChanged
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

        ExportModel model;
        
        private ObservableCollection<mNemeStorage> mnemes;
        public ObservableCollection<mNemeStorage> mNemes
        {
            get { return mnemes; }
            set { SetField(ref mnemes, value, "mNemes"); }
        }

        public ExportViewModel(ExportModel model)
        {
            this.model = model;
            mNemes = new ObservableCollection<mNemeStorage>();

            List<CheckedItem<string>> test1 = new List<CheckedItem<string>>();
            test1.Add(new CheckedItem<string>("1Inner1"));
            test1.Add(new CheckedItem<string>("1Inner2"));

            List<CheckedItem<string>> test2 = new List<CheckedItem<string>>();
            test2.Add(new CheckedItem<string>("2Inner1"));
            test2.Add(new CheckedItem<string>("2Inner2"));

            var tolo = new mNemeStorage(new CheckedItem<string>("Test1",false,true), test1);
            var tolito = new mNemeStorage(new CheckedItem<string>("Test2",false,true), test2);

            mNemes.Add(tolo);
            mNemes.Add(tolito);
            
        }

    }
}
