using Import.Model;
using mNemonic.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Import.ViewModel
{
    public class ConfigurationViewModel : NotifyPropertyChangedBase, INotifyPropertyChanged
    {
        ConfigurationModel model;

        public ICommand SaveCommand { get; set; }
        public ICommand CancelCommand { get; set; }
        public EventHandler RequestClose { get; set; }

        private ObservableCollection<mNemeStorage> mnemes;
        public ObservableCollection<mNemeStorage> mNemes
        {
            get { return mnemes; }
            set { SetField(ref mnemes, value, "mNemes"); }
        }

        public ConfigurationViewModel(ConfigurationModel model)
        {
            this.model = model;
            mNemes = new ObservableCollection<mNemeStorage>(model.mNemesCollection);

            this.CancelCommand = new DelegateCommand((o) => true, (o) =>
            {
                this.RequestClose(o, new EventArgs());
            });

            this.SaveCommand = new DelegateCommand((o) => true, (o) =>
            {
                model.mNemesCollection = mNemes.Where(x => x.IsChecked).ToList();
                model.Save();
            });
        }
    }
}
