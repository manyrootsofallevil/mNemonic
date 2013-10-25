using mNemonic.Commands;
using mNemonic.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace mNemonic.ViewModel
{
    public class PopUpVM : INotifyPropertyChanged
    {
        PopUpModel model;

        public ICommand DontRememberCommand { get; set; }
        public ICommand VaguelyRememberCommand { get; set; }
        public ICommand DoRememberCommand { get; set; }
        public EventHandler RequestClose { get; set; }
        private Action<mNeme> DisplayAnswer;

        private ImageSource imageSource;

        public ImageSource ImageSource
        {
            get { return imageSource; }
            set { SetField(ref imageSource, value, "ImageSource"); }
        }

        private bool showAnswer;
        public bool ShowAnswer
        {
            get { return showAnswer; }
            set { SetField(ref showAnswer, value, "ShowAnswer"); }
        }

        private string answer;
        public string Answer
        {
            get { return answer; }
            set { SetField(ref answer, value, "Answer"); }
        }

        private string question;
        public string Question
        {
            get { return question; }
            set { SetField(ref question, value, "Question"); }
        }

        private int height;
        public int Height
        {
            get { return height; }
            set { SetField(ref height, value, "Height"); }
        }

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

        public PopUpVM(PopUpModel model)
        {
            DisplayAnswer = (x) =>
            {
                if (x.Type == mNemeType.Image)
                {
                    DisplayImageAnswer(x);
                }
                else if (x.Type == mNemeType.Text)
                {
                    DisplayTextAnswer(x);
                }
            };

            Height = (int)(System.Windows.SystemParameters.PrimaryScreenHeight *0.7); //Why 70%, why not?

            this.model = model;

            this.DontRememberCommand = new DelegateCommand((obj) => true, (obj) =>
            {
                DisplayAnswer.Invoke(this.model.currentmNeme);
                model.DoTheRemembering(PopUpModel.dontRemember);
                //this.RequestClose(obj, new EventArgs());
            });

            this.VaguelyRememberCommand = new DelegateCommand((obj) => true, (obj) =>
            {
                DisplayAnswer.Invoke(this.model.currentmNeme);
                model.DoTheRemembering(PopUpModel.vaguelyRemember);
                //this.RequestClose(obj, new EventArgs());
            });

            this.DoRememberCommand = new DelegateCommand((obj) => true, (obj) =>
            {
                model.DoTheRemembering(PopUpModel.doRemember);
                this.RequestClose(obj, new EventArgs());
            });

            this.ShowItem();
        }

        private void ShowItem()
        {
            switch (this.model.currentmNeme.Type)
            {
                case mNemeType.Image:
                    ImageSource =
                       new BitmapImage(new Uri(this.model.currentmNeme.Items.Where(x => x.Item2 == FileType.Image).FirstOrDefault().Item1));
                    break;
                case mNemeType.Text:
                    DisplayQuestion();
                    break;

            }
        }

        private void DisplayQuestion()
        {
            using (StreamReader sw = new StreamReader(this.model.currentmNeme.Items.
                Where(x => x.Item2 == FileType.Text && x.Item1.ToLower().Contains("question")).FirstOrDefault().Item1))
            {
                Question = sw.ReadToEnd();
            }
        }

        private void DisplayImageAnswer(mNeme mNeme)
        {
            using (StreamReader sr
                = new StreamReader(mNeme.Items.Where(x => x.Item2 == FileType.Text).FirstOrDefault().Item1))
            {
                this.Answer = sr.ReadToEnd();
                this.ShowAnswer = true;
            }
        }

        private void DisplayTextAnswer(mNeme mNeme)
        {
            using (StreamReader sr = new StreamReader(mNeme.Items.Where(x => x.Item2 == FileType.Text
                && !x.Item1.ToLower().Contains("question")).FirstOrDefault().Item1))
            {
                this.Answer = sr.ReadToEnd();
                this.ShowAnswer = true;
            }
        }

    }
}
