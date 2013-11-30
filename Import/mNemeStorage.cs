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
        private string directory;

        public mNemeStorage(string directoryName)
        {
            Directory = directoryName;
            Name = new System.IO.FileInfo(directoryName).Name;
        }

        public mNemeStorage(string directoryName, bool isChecked)
        {
            Directory = directoryName;
            Name = new System.IO.FileInfo(directoryName).Name;
            this.isChecked = isChecked;
        }


        public mNemeStorage(string directoryName, mNemeStorage parent)
        {
            Directory = directoryName;
            Name = new System.IO.FileInfo(directoryName).Name;
            Parent = parent;
        }

        public mNemeStorage(string directoryName, mNemeStorage parent, IEnumerable<mNemeStorage> subDirectories)
        {
            Directory = directoryName;
            Name = new System.IO.FileInfo(directoryName).Name;
            Parent = parent;
            SubDirectories = new ObservableCollection<mNemeStorage>(subDirectories);
        }

        public string Name
        {
            get { return name; }
            set
            {
                SetField(ref name, value, "Name");

            }
        }

        public bool IsChecked
        {
            get { return isChecked; }
            set
            {
                SetField(ref isChecked, value, "IsChecked");

                //This means that this is a top item
                if (Parent == null && SubDirectories != null)
                {
                    SubDirectories.ToList().ForEach((x) => x.IsChecked = !x.IsChecked);
                }
            }
        }

        public string Directory
        {
            get { return directory; }
            set
            {
                SetField(ref directory, value, "Directory");
            }
        }

        public mNemeStorage Parent { get; set; }


        public ObservableCollection<mNemeStorage> SubDirectories { get; set; }
    }
}
