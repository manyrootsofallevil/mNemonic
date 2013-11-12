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
            //mNemes = new ObservableCollection<mNemeStorage>();
            mNemes = new ObservableCollection<mNemeStorage>(model.mNemes);

            mNemeStorage tolo = new mNemeStorage("Test1");

            mNemeStorage tolo1 = new mNemeStorage("1-1", tolo);
            mNemeStorage tolo2 = new mNemeStorage("1-2", tolo);

            List<mNemeStorage> various = new List<mNemeStorage>();
            various.Add(tolo1);
            various.Add(tolo2);

            tolo.SubDirectories = new ObservableCollection<mNemeStorage>(various);
            

            mNemes.Add(tolo);
            //mNemes.Add(tolito);
            
        }

    }
}
