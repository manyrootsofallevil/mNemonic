using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Import
{
    public class mNemeStorage : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        //TODO: This will need to change if we want to be able to propagate ticks down the hierachy.
        //We'll need to create a family
        private CheckedItem<string> name;

        public mNemeStorage(CheckedItem<string> name, IEnumerable<CheckedItem<string>> subDirectories)
        {
            Name = name;
            SubDirectories = new ObservableCollection<CheckedItem<string>>(subDirectories);
        }

        public CheckedItem<string> Name
        {
            get { return name; }
            set
            {
                name = value;
                if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("Name"));
            }
        }
        public ObservableCollection<CheckedItem<string>> SubDirectories { get; private set; }
    }
}
