using Import.Model;
using mNemonic.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;

namespace Import.ViewModel
{
    public class ConfigurationViewModel : NotifyPropertyChangedBase, INotifyPropertyChanged
    {
        ConfigurationModel model;

        public ICommand SaveCommand { get; set; }
        public ICommand CancelCommand { get; set; }
        public ICommand AddNewCollectionCommand { get; set; }
        public ICommand RemoveCollectionCommand { get; set; }
        public EventHandler RequestClose { get; set; }

        private ObservableCollection<mNemeStorage> mnemes;
        public ObservableCollection<mNemeStorage> mNemes
        {
            get { return mnemes; }
            set { SetField(ref mnemes, value, "mNemes"); }
        }

        private bool removeCollection;
        public bool RemoveCollection
        {
            get { return removeCollection; }
            set { SetField(ref removeCollection, value, "RemoveCollection"); }
        }

        public ConfigurationViewModel(ConfigurationModel model)
        {
            this.model = model;
            mNemes = new ObservableCollection<mNemeStorage>(model.mNemesCollection);

            RemoveCollection = mNemes.Where(x => x.IsChecked).Count() > 0;

            this.CancelCommand = new DelegateCommand((o) => true, (o) =>
            {
                this.RequestClose(o, new EventArgs());
            });

            this.SaveCommand = new DelegateCommand((o) => true, (o) =>
            {
                model.mNemesCollection = mNemes.ToList();

                if (model.Save())
                {
                    this.RequestClose(o, new EventArgs());
                }
                else
                {
                    System.Windows.MessageBox.Show("An error occurred");
                }
            });

            this.AddNewCollectionCommand = new DelegateCommand((o) => true, (o) =>
                {
                    FolderBrowserDialog dialog = new FolderBrowserDialog();
                    dialog.SelectedPath = ConfigurationManager.AppSettings["RootDirectory"];

                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        var result = model.AddToCollections(dialog.SelectedPath);

                        if (!result.Item1)
                        {
                            System.Windows.MessageBox.Show(result.Item2);
                        }
                        else
                        {
                            mNemes.Add(result.Item3);
                        }
                    }


                });

            this.RemoveCollectionCommand = new DelegateCommand((o) => true, (o) =>
            {
                //Looks like by converting this into a list we get rid of the pesky problem of deferred execution
                //so we can use foreach below. hurray for laziness.
                var checkedmNemes = mNemes.Where(x => x.IsChecked).ToList();

                foreach (var item in checkedmNemes)
                {
                    var result = model.RemoveCollection(item.Name);

                    if (result.Item1)
                    {
                        mNemes.Remove(item);
                    }
                    else
                    {
                        System.Windows.MessageBox.Show(result.Item2);
                    }
                }
            });

        }
    }
}
