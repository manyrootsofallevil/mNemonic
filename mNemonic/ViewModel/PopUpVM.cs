using mNemonic.Commands;
using mNemonic.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace mNemonic.ViewModel
{
    public class PopUpVM : INotifyPropertyChanged
    {
        PopUpModel model;
        public ICommand DontRememberCommand { get; set; }
        public ICommand VaguelyRememberCommand { get; set; }
        public ICommand DoRememberCommand { get; set; }
        public EventHandler RequestClose { get; set; }

        // boiler-plate
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T field, T value, string propertyName)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        //// props
        //private string name;
        //public string Name
        //{
        //    get { return name; }
        //    set { SetField(ref name, value, "Name"); }
        //}

        public PopUpVM(PopUpModel model)
        {
            this.model = model;

            this.DontRememberCommand = new DelegateCommand((obj) => true,
                (obj) =>
                {
                    this.RequestClose(obj, new EventArgs());
                });
        }


    }
}
