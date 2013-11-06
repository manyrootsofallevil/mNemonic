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
        public ICommand NextCommand { get; set; }
        public EventHandler RequestClose { get; set; }
        private Action<mNeme> DisplayAnswer;

        #region Properties
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

        private int answerheight;
        public int AnswerHeight
        {
            get { return answerheight; }
            set { SetField(ref answerheight, value, "AnswerHeight"); }
        }

        private int questionheight;
        public int QuestionHeight
        {
            get { return questionheight; }
            set { SetField(ref questionheight, value, "QuestionHeight"); }
        }
        private bool rankedmNeme;
        public bool RankedmNeme
        {
            get { return rankedmNeme; }
            set { SetField(ref rankedmNeme, value, "RankedmNeme"); }
        }
        #endregion

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

            AnswerHeight = (int)(System.Windows.SystemParameters.PrimaryScreenHeight *0.7); //Why 70%, why not?

            this.model = model;

            this.DontRememberCommand = new DelegateCommand((obj) => true, (obj) =>
            {
                RememberCommand(model, PopUpModel.dontRemember);
            });

            this.VaguelyRememberCommand = new DelegateCommand((obj) => true, (obj) =>
            {
                RememberCommand(model, PopUpModel.vaguelyRemember);
            });

            this.DoRememberCommand = new DelegateCommand((obj) => true, (obj) =>
            {
                RememberCommand(model, PopUpModel.doRemember);
            });

            this.NextCommand = new DelegateCommand((obj) => true, (obj) =>
                {
                    this.RequestClose(obj, new EventArgs());
                });

            this.ShowItem();
        }

        private void RememberCommand(PopUpModel model, int level)
        {
            DisplayAnswer.Invoke(this.model.currentmNeme);
            model.DoTheRemembering(level);
            this.RankedmNeme = true;
        }

        private void ShowItem()
        {
            switch (this.model.currentmNeme.Type)
            {
                case mNemeType.Image:
                    ImageSource =
                       new BitmapImage(new Uri(this.model.currentmNeme.Items.Where(x => x.Item2 == FileType.Image).FirstOrDefault().Item1));
                    DisplayQuestion(0.097);//This value might seem arbitrary but it was arrived at via a long process of deliberation                    
                    break;
                case mNemeType.Text:
                    DisplayQuestion(0.7);//same for this one.
                    break;

            }
        }

        private void DisplayQuestion(double factor)
        {
            var question = this.model.currentmNeme.Items.
                Where(x => x.Item2 == FileType.Text && x.Item1.ToLower().Contains("question"));

            if (question.Count() >0)
            {
                using (StreamReader sw = new StreamReader(question.First().Item1))
                {
                    Question = sw.ReadToEnd();
                }
            }

            QuestionHeight = (int)(System.Windows.SystemParameters.PrimaryScreenHeight * factor); 
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
