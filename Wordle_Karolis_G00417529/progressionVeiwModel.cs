using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Wordle_Karolis_G00417529
{
    public class progressionVeiwModel : INotifyPropertyChanged
    {
        // class fields
        private ObservableCollection<wordleAttempt> attemptList;
        private double templateFontSize = 15;
        private double templateFontSpacing = 5;

        // getters and setters
        public ObservableCollection<wordleAttempt> AttemptList
        {
            get { return attemptList; }
            set
            {
                attemptList = value;
                OnPropertyChanged();
            }
        }

        public double TemplateFontSize
        {
            get { return templateFontSize; }
            set
            {
                templateFontSize = value;
                OnPropertyChanged();
            }
        }

        public double TemplateFontSpacing
        {
            get { return templateFontSize; }
            set
            {
                templateFontSpacing = value;
                OnPropertyChanged();
            }
        }

        // veiwmodel databinding functions //
        public event PropertyChangedEventHandler PropertyChanged;

        // credits to Donny Hurley for this function
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
