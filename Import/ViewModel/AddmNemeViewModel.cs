using Microsoft.Win32;
using mNemonic.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Import
{
    public class AddmNemeViewModel : NotifyPropertyChangedBase, INotifyPropertyChanged, IDataErrorInfo
    {

        AddmNemeModel model;

        #region Commands

        public ICommand SaveCommand { get; set; }
        public ICommand SaveAndNewCommand { get; set; }
        public ICommand CancelCommand { get; set; }
        public ICommand InsertCommand { get; set; }
        public ICommand SelectDirectoryCommand { get; set; }
        public EventHandler RequestClose { get; set; }
        public EventHandler SaveAndNew { get; set; }

        #endregion

        #region Validation

        public string Error
        {
            get { throw new NotImplementedException(); }
        }

        int validationErrors = 0;
        bool validDirectory = false;//I don't like this

        public string this[string columnName]
        {
            get
            {
                string result = null;

                switch (columnName.ToLower())
                {
                    case "title": result = ValidateString(Title); break;
                    case "question": result = ValidateString(Question); break;
                    case "answer": result = ValidateString(Answer); break;
                    case "rootdirectory": result = ValidateDirectory(RootDirectory); break;
                    default: validationErrors++; break;
                }

                SaveMeNow = validationErrors == 0 && validDirectory;

                return result;
            }
        }

        private string ValidateString(string input)
        {
            string result = string.Empty;

            if (string.IsNullOrEmpty(input))
            {
                result = "Empty Filed";
                validationErrors++;
            }
            else if (validationErrors > 0)
            {
                validationErrors--;
            }

            return result;
        }

        private string ValidateDirectory(string input)
        {
            string result = string.Empty;

            //Using this on the basis that attempting to solve a problem with a regular expressions is a sure fire way
            //of increasing the number of problems by one. will probably end up using regex as this seems extremely ugly.
            try
            {
                Path.GetFullPath(input);
                validDirectory = true;
            }
            catch (Exception)
            {
                result = "Empty Filed";
                validDirectory = false;
            }

            return result;
        }
        #endregion

        #region Properties

        private string windowTitle;
        public string WindowTitle
        {
            get { return windowTitle; }
            set { SetField(ref windowTitle, value, "WindowTitle"); }
        }

        private string rootdirectory;
        public string RootDirectory
        {
            get { return rootdirectory; }
            set
            {
                SetField(ref rootdirectory, value, "RootDirectory");
                model.RootDirectory = RootDirectory;
            }
        }
        private string title;
        public string Title
        {
            get { return title; }
            set
            {
                SetField(ref title, value, "Title");
                model.Title = Title;
            }
        }
        private string answer;
        public string Answer
        {
            get { return answer; }
            set
            {
                SetField(ref answer, value, "Answer");
                model.Answer = Answer;
            }
        }

        private string question;
        public string Question
        {
            get { return question; }
            set
            {
                SetField(ref question, value, "Question");
                model.Question = Question;
            }
        }

        private bool saveMeNow;

        public bool SaveMeNow
        {
            get { return saveMeNow; }
            set { SetField(ref saveMeNow, value, "SaveMeNow"); }
        }

        #endregion

        public AddmNemeViewModel(AddmNemeModel model)
        {
            this.model = model;
            WindowTitle = model.WindowTitle;
            RootDirectory = model.RootDirectory;

            this.SelectDirectoryCommand = new DelegateCommand((o) => true, (o) =>
            {
                System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog();
                dialog.RootFolder = Environment.SpecialFolder.Desktop;

                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    RootDirectory = dialog.SelectedPath;
                }

            });

            this.SaveCommand = new DelegateCommand((obj) => true, (obj) =>
                {
                    var saved = model.WriteToFile();

                    if (saved.Item1)
                    {
                        this.RequestClose(obj, new EventArgs());
                    }
                    else
                    {
                        MessageBox.Show(saved.Item2);
                    }
                });


            this.SaveAndNewCommand = new DelegateCommand((obj) => true, (obj) =>
            {
                var saved = model.WriteToFile();

                if (saved.Item1)
                {
                    this.SaveAndNew(obj, new AddmNemeEventArgs(RootDirectory));
                }
                else
                {
                    MessageBox.Show(saved.Item2);
                }
            });


            this.CancelCommand = new DelegateCommand((obj) => true, (o) =>
            {
                this.RequestClose(o, new EventArgs());
            });

            this.InsertCommand = new DelegateCommand((obj) => true, (o) =>
                {
                    //Using a openfile dialog as we are only interested in getting the name of the file
                    //so that it can be saved later.
                    OpenFileDialog dlg = new OpenFileDialog();
                    dlg.FileName = "Image";
                    dlg.DefaultExt = ".jpg";
                    dlg.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.tiff";

                    bool? result = dlg.ShowDialog();

                    if (result == true)
                    {
                        model.Image = dlg.FileName;
                    }
                });
        }
    }

    public class AddmNemeEventArgs : EventArgs
    {
        public AddmNemeEventArgs(string currentRootDirectory)
        {
            this.CurrentRootDirectory = currentRootDirectory;
        }

        public string CurrentRootDirectory { get; private set; }
    }


}
