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
        const string show= "Show Answer";
        const string hide= "Hide Answer";

        PopUpModel model;

        public ICommand DontRememberCommand { get; set; }
        public ICommand VaguelyRememberCommand { get; set; }
        public ICommand DoRememberCommand { get; set; }
        public ICommand ShowAnswerCommand { get; set; }
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

        private bool hasAnswerDisplayed;
        public bool HasAnswerDisplayed
        {
            get { return hasAnswerDisplayed; }
            set { SetField(ref hasAnswerDisplayed, value, "HasAnswerDisplayed");
            if (hasAnswerDisplayed) { ShowHideAnswer = hide; } else { ShowHideAnswer = show; }
            }
        }

        private string collection;
        public string Collection
        {
            get { return collection; }
            set { SetField(ref collection, value, "Collection"); }
        }

        private Brush selectionType;
        public Brush SelectionType
        {
            get { return selectionType; }
            set { SetField(ref selectionType, value, "SelectionType"); }
        }

        private string showHideAnswer;
        public string ShowHideAnswer
        {
            get { return showHideAnswer; }
            set { SetField(ref showHideAnswer, value, "ShowHideAnswer"); }
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
            this.model = model;

            this.ShowHideAnswer = show;

            AnswerHeight = (int)(System.Windows.SystemParameters.PrimaryScreenHeight * 0.7); //Why 70%, why not?

            this.Collection = getCollectionName(model.currentmNeme);

            SelectBrushColour(model.currentmNeme);

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

            this.DontRememberCommand = new DelegateCommand((obj) => true, (obj) =>
            {
                RememberCommand(model, Constants.dontRemember);
                this.RequestClose(obj, new EventArgs());
            });

            this.VaguelyRememberCommand = new DelegateCommand((obj) => true, (obj) =>
            {
                RememberCommand(model, Constants.vaguelyRemember);
                this.RequestClose(obj, new EventArgs());
            });

            this.DoRememberCommand = new DelegateCommand((obj) => true, (obj) =>
            {
                RememberCommand(model, Constants.doRemember);
                this.RequestClose(obj, new EventArgs());
            });

            this.ShowAnswerCommand = new DelegateCommand((o) => true, (o) =>
                {
                    DisplayAnswer.Invoke(this.model.currentmNeme);
                    
                    if (this.HasAnswerDisplayed)
                    {
                        this.HasAnswerDisplayed = false;
                    }
                    else
                    {
                        this.HasAnswerDisplayed = true;
                    }
                });


            this.ShowItem();
        }

        async private void RememberCommand(PopUpModel model, int level)
        {
            DisplayAnswer.Invoke(this.model.currentmNeme);
            await model.DoTheRemembering(level);
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

            if (question.Count() > 0)
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

                this.ShowAnswer = this.ShowAnswer == true ? false : true;
            }
        }

        private void DisplayTextAnswer(mNeme mNeme)
        {
            using (StreamReader sr = new StreamReader(mNeme.Items.Where(x => x.Item2 == FileType.Text
                && !x.Item1.ToLower().Contains("question")).FirstOrDefault().Item1))
            {
                this.Answer = sr.ReadToEnd();

                this.ShowAnswer = this.ShowAnswer == true ? false : true;
            }
        }

        private void SelectBrushColour(mNeme current)
        {

            SelectionType = Brushes.Black;

            if (current.CompletelyRandomlySelected)
            {
                SelectionType = Brushes.Red;
            }

            if (current.PartiallyRandomlySelected)
            {
                SelectionType = Brushes.Green;
            }

        }

        //This assumes that the collection name is the parent directory of the current mNeme
        private string getCollectionName(mNeme current)
        {
            string result = string.Empty;

            result = string.Format("Collection: {0}", new DirectoryInfo(current.Location).Parent.Name);

            return result;
        }

    }
}
