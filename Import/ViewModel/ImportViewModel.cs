using mNemonic.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        public ICommand Cancel { get; set; }
        public ICommand Insert { get; set; }
        public EventHandler RequestClose { get; set; }

        public string Error
        {
            get { throw new NotImplementedException(); }
        }
        
        int validationErrors = 0;

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
                    case "directory": result = ValidateString(Directory); break;
                }

                SaveMeNow = validationErrors == 0;

                return result;
            }
        }

        #region Properties

        private string windowTitle;
        public string WindowTitle
        {
            get { return windowTitle; }
            set { SetField(ref windowTitle, value, "WindowTitle"); }
        }

        private string directory;
        public string Directory
        {
            get { return directory; }
            set
            {
                SetField(ref directory, value, "Directory");
                model.Directory = Directory; 
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

            this.SaveCommand = new DelegateCommand((obj) => true, (obj) =>
                {
                    if (model.WriteToFile())
                    {
                        this.RequestClose(obj, new EventArgs());
                    }
                    else
                    {
                        MessageBox.Show("An Error Occurred");
                    }
                });


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
    }
}
