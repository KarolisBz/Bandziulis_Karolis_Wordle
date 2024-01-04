using System.Diagnostics;

namespace Wordle_Karolis_G00417529
{
    // this class holds data for a wordle attempt
    public class wordleAttempt
    {
        // class fields ( keeping them private for preventing bugs )
        private string correctWord;
        private DateTime attemptFinished;
        private int numberOfGuesses;
        private int[,,,,] attemptVisualData;
        private Random random = new Random();
        // public fields
        public int currentAttempt;

        // constructor
        public wordleAttempt()
        {
            // initializing fields 
            correctWord = "word test";
            numberOfGuesses = 6;
            currentAttempt = 0;
        }

        // getters and setters
        public string CorrectWord
        {
            get { return correctWord; }
            // no setter, as correct word is generated only once at object creation
        }

        public int NumberOfGuesses
        {
            get { return numberOfGuesses; }
            set // this will not allow number to be set below 0, or above max number of attempts
            {
                if (value > 0 && value < 7)
                {
                    numberOfGuesses = value;
                }
                else if(value < 0)
                {
                    numberOfGuesses = 0;
                }
                else
                {
                    numberOfGuesses = 6;
                }
            } 
        }

        public DateTime AtemptFinished
        {
            get { return attemptFinished; } 
            // no setter as it's set only when game is finished
        }

        // object methods
        async public void finished()
        {
            // setting finish time of wordle
            attemptFinished = DateTime.Now;

            // saving data
            DataHandler.attemptList.Add(this); // adds itself into list to save
            await DataHandler.saveDataAsync(); // saving game progress so far
        }

        public void inputAttemptTurnData()
        {

        }

        public bool tryAttempt(string playerAwnser)
        {
            bool awnserIsCorrect = false;



            return awnserIsCorrect;
        }

        public void setupGame()
        {
            // fetching random word from cached api
            correctWord = DataHandler.wordList[random.Next(DataHandler.wordList.Count)];
            Debug.Print("Chosen word is: " + correctWord);
        }
    }
}
