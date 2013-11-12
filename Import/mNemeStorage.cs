using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Import
{
    public class mNemeStorage : NotifyPropertyChangedBase, INotifyPropertyChanged
    {

        private string name;
        private bool isChecked;
        private mNemeStorage parent;

        public mNemeStorage(string name)
        {
            Name = name;
        }

        public mNemeStorage(string name, mNemeStorage parent)
        {
            Name = name;
            Parent = parent;
        }

        public mNemeStorage(string name, mNemeStorage parent, IEnumerable<mNemeStorage> subDirectories)
        {
            Name = name;
            Parent = parent;
            SubDirectories = new ObservableCollection<mNemeStorage>(subDirectories);
        }

        public string Name
        {
            get { return name; }
            set { SetField(ref name, value, "Name"); }
        }

        public bool IsChecked
        {
            get { return isChecked; }
            set {
                SetField(ref isChecked, value, "IsChecked");
                
                //This means that this is a top item
                if (Parent == null && SubDirectories!=null)
                {
                    SubDirectories.ToList().ForEach((x) => x.IsChecked = !x.IsChecked);
                }
            }
        }

        public mNemeStorage Parent { get; set; }


        public ObservableCollection<mNemeStorage> SubDirectories { get; set; }
    }
}
