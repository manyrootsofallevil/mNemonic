using mNemonic.Commands;
using mNemonic.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace mNemonic.ViewModel
{
    public class PopUpVM : INotifyPropertyChanged
    {
        PopUpModel model;

        //public string Answer { get; set; }
       // public bool ShowAnswer { get; set; }
        public ICommand DontRememberCommand { get; set; }
        public ICommand VaguelyRememberCommand { get; set; }
        public ICommand DoRememberCommand { get; set; }
        public EventHandler RequestClose { get; set; }

        public PopUpVM(PopUpModel model)
        {
            this.model = model;

            this.DontRememberCommand = new DelegateCommand((obj) => true, (obj) =>
            {
                this.Answer = DateTime.Now.ToString();
                this.ShowAnswer = true;
                model.DoTheRemembering();
                //this.RequestClose(obj, new EventArgs());
            });

            this.VaguelyRememberCommand = new DelegateCommand((obj) => true, (obj) =>
            {
                Answer = "Answer";
                ShowAnswer = true;
                model.DoTheRemembering();
                this.RequestClose(obj, new EventArgs());
            });

            this.DoRememberCommand = new DelegateCommand((obj) => true, (obj) =>
            {
                model.DoTheRemembering();
                this.RequestClose(obj, new EventArgs());
            });
        }
        
        // boiler-plate
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T field, T value, string propertyName)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
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
       


    }
}
