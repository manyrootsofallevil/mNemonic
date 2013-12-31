using Import.Model;
using Microsoft.Win32;
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
    public class ExportViewModel :NotifyPropertyChangedBase, INotifyPropertyChanged
    {        
        ExportModel model;
        string defaultFileName = "backup";

        public ICommand ExportCommand { get; set; }
        public ICommand CancelCommand { get; set; }
        public EventHandler RequestClose { get; set; }
        
        private ObservableCollection<mNemeStorage> mnemes;
        public ObservableCollection<mNemeStorage> mNemes
        {
            get { return mnemes; }
            set { SetField(ref mnemes, value, "mNemes"); }
        }

        public ExportViewModel(ExportModel model)
        {
            this.model = model;
            mNemes = new ObservableCollection<mNemeStorage>(model.mNemes);

            this.CancelCommand = new DelegateCommand((o) => true, (o) =>
            {
                this.RequestClose(o, new EventArgs());
            });

            this.ExportCommand = new DelegateCommand((o) => true, (o) =>
            {
                SaveFileDialog dlg = new SaveFileDialog();
               
                var mNemeName = this.mNemes.Where(x => x.IsChecked).FirstOrDefault();

                if (mNemeName != null)
                {
                    defaultFileName = mNemeName.Name;
                }

                dlg.FileName = defaultFileName;
                dlg.DefaultExt = ".zip";
                dlg.Filter = "Zip Files|*.zip";

                bool? result = dlg.ShowDialog();

                if (result == true)
                {
                    model.DestinationFile = dlg.FileName;
                }

                model.mNemes = mNemes.Where(x => x.IsChecked).ToList();

                if (model.ExportmNemes())
                {
                    System.Windows.Forms.MessageBox.Show("Successfully Exported");
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("An Error Ocurred. Try Again.");
                }
            });
        }

    }
}
