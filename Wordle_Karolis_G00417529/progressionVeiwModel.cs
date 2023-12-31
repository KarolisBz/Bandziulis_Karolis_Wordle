﻿using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Diagnostics;

namespace Wordle_Karolis_G00417529
{
    public class progressionVeiwModel : INotifyPropertyChanged
    {
        // class fields
        private ObservableCollection<wordleAttempt> attemptList;

        // getters and setters
        public ObservableCollection<wordleAttempt> AttemptList
        {
            get { return attemptList; }
            set
            {
                attemptList = value;

                // generating emoji grid data
                // we do this instead of saving it as it saves more space,
                // also saving color values can be tricky and not worth the pros and cons
                foreach (wordleAttempt attempt in attemptList)
                {
                    attempt.createColorGridData();
                }

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
