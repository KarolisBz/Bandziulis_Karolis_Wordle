
namespace Wordle_Karolis_G00417529
{
    // this class holds data for a wordle attempt
    public class wordleAttempt
    {
        // class fields ( keeping them private for preventing bugs )
        private String correctWord;
        private DateTime attemptFinished;
        private int numberOfGuesses;
        private int[,,,,] attemptVisualData;

        // constructor
        public wordleAttempt()
        {
            // initializing fields 
            correctWord = "word test";
            numberOfGuesses = 6;

            // fetching random word from cached api
        }

        // getters and setters
        public String CorrectWord
        {
            get { return CorrectWord; }
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

        public void tryAttempt()
        {

        }
    }
}
