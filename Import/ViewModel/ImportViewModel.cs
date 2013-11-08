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
    public class ImportViewModel : INotifyPropertyChanged, IDataErrorInfo
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

        ImportModel model;

        public ICommand SaveCommand { get; set; }
        public ICommand CancelCommand { get; set; }
        public ICommand InsertCommand { get; set; }
        public EventHandler RequestClose { get; set; }

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
            else
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
                SetField(ref rootdirectory, value, "Directory");
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

        public ImportViewModel(ImportModel model)
        {
            this.model = model;
            WindowTitle = model.WindowTitle;
            RootDirectory = model.RootDirectory;

            this.SaveCommand = new DelegateCommand((obj) => true, (obj) =>
                {
                    if (model.WriteToFile())
                    {
                        this.RequestClose(obj, new EventArgs());
                    }
                    else
                    {
                        MessageBox.Show("An error occurred while saving files.");
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
}
