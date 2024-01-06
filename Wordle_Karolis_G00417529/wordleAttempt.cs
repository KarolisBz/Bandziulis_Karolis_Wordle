using System;
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
        private List<int[]> attemptVisualData;
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
            attemptVisualData = new List<int[]>();
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

        public List<int[]> AttemptVisualData
        {
            get { return attemptVisualData; }
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

        public int[] tryAttempt(string playerAwnser)
        {
            // first int is true/false for attempt success
            // 1 = true, 0 = false
            // the rest are letters that where correct/false/wrong position
            // 0 = wrong, 1 = correct, 2 = wrong position
            int[] compareResult = {0, 0, 0, 0, 0, 0};
            int correctCounter = 0;
            string positionRecord = correctWord; // copying correct string

            // comparing character by character, removng correct characters from position record
            for (int i = 0; i < 5; i++)
            {
                if (correctWord[i] == playerAwnser[i])
                {
                    positionRecord = removeChar(i, positionRecord); // removing avalible match
                    correctCounter++;
                    compareResult[1 + i] = 1; // awnser is correct
                }
            }

            // comparing each wrong position character
            // we do this in 2 loops instead of 1, so we don't duplicate a found and unfound character
            for (int i = 0; i < 5; i++)
            {
                if (correctWord[i] != playerAwnser[i])
                {
                    // if one of the letters are correct but in wrong position
                    for (int index = 0; index < positionRecord.Length; index++)
                    {
                        if (playerAwnser[i] == positionRecord[index])
                        {
                            positionRecord = removeChar(index, positionRecord); // removing avalible match
                            compareResult[1 + i] = 2; // awnser is wrong position
                            break;
                        }
                        else
                        {
                            // if we didn't find a match
                            compareResult[1 + i] = 0;
                        }
                    }
                }
            }

            // determining if all word is correct
            if (correctCounter == 5)
            {
                compareResult[0] = 1; // setting match as correct
            }

            // printing result
            for (int i = 0; i < 6; i++)
            {
                Debug.Write($"{compareResult[i]} |");
            }
            Debug.Write("\n ");

            // record attempt
            numberOfGuesses -= 1;
            attemptVisualData.Add(compareResult);

            // checks if player looses, or wins
            if (compareResult[0] == 1) // wins
            {
                finished();
            }
            else if (numberOfGuesses == 0)
            {
                finished();
            }
            else // move one row down and reveal result
            {
                currentAttempt++;
            }

            return compareResult;
        }

        private string removeChar(int index, string target)
        {
            string finalString = "";

            // we build the string while ignoring the index to remove the character
            for (int i = 0; i < target.Length; i++)
            {
                if (i != index)
                {
                    finalString += target[i];
                }
                else
                {
                    finalString += ' '; // we add a space to keep the index the same
                }
            }

            return finalString;
        }

        public void setupGame()
        {
            // fetching random word from cached api
            correctWord = DataHandler.wordList[random.Next(DataHandler.wordList.Count)];
            Debug.Print("Chosen word is: " + correctWord); // for cheating / testing
        }
    }
}
