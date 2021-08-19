using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;

namespace STC.Models
{
   public class FAQModel : ObservableCollection<Answer>, INotifyPropertyChanged
    {
        public ObservableCollection<Answer> Answers = new ObservableCollection<Answer>();

       // public ObservableCollection<Answer> Answers { get; set; }
        public FAQModel(FAQModel questins, bool expanded = false)
        {
           // this.Hotel = hotel;
            this._expanded = expanded;

            foreach (Answer ans in questins.Answers)
            {
                Answers.Add(ans);

                if (expanded)
                {
                    this.Add(ans);
                }
            }

        }

        public FAQModel(string question)
        {
            Question = question;
        }
        public FAQModel()
        {
        }
        public string Question { get; set; }

        public Answer Answer { get; set; }

        private bool _expanded;
        public bool Expanded
        {
            get { return _expanded; }
            set
            {
                if (_expanded != value)
                {
                    _expanded = value;
                   // OnPropertyChanged(new PropertyChangedEventArgs("Expanded"));
                    OnPropertyChanged(new PropertyChangedEventArgs("StateIcon"));
                    if (_expanded)
                    {
                        foreach (Answer ans in Answers)
                        {
                            this.Add(ans);
                        }
                    }
                    else
                    {
                        this.Clear();
                    }
                }
            }
        }

        public string StateIcon
        {
            get
            {
                if (Expanded)
                {
                    return "UpArrow.png";
                }
                else
                { return "DownArrow.png"; }
            }
        }

    }

    public class Answer
    {
        public Answer(string text)
        {
            Text = text;
        }
        public Answer()
        {

        }
        public string Text { get; set; }
    }
}
