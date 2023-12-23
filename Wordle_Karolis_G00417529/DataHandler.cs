using System.Diagnostics;
using System.Text.Json;

namespace Wordle_Karolis_G00417529
{
    // this class handles most of loading, saving and storing of data
    public class DataHandler
    {
        // class fields //
        static private String filePath = System.IO.Path.Combine(FileSystem.Current.AppDataDirectory, "wordle_UserData.json");
        // player data
        static public String currentPlayer;
        // settings data
        static public bool darkMode;
        static public float fontSize;
        static public bool easyMode;
        static public bool timerOn;
        // wordle attempt list
        static public List<wordleAttempt> attemptList;
        // wrapped data
        static private DataPackage wrappedData = new DataPackage(); // creating data wrapper

        // constructor
        public DataHandler()
        {
            // requests loading of data when created, however data is static and can be accessed anywhere without object
            if (!loadData())
            {
                // if we failed to load data, then we will give it default values
                currentPlayer = "Default_User";
                darkMode = false;
                fontSize = 20;
                easyMode = false;
                timerOn = true;
                attemptList = new List<wordleAttempt>();
            }
        }

        // creating an internal class that contains all the required data
        // so we can read it and write it all at once while keeping all the varibles in the class static
        // normally storing data like this will cause very easy cheating as all the data is static and public, but since it's a client only game cheats can never be prevented
        // as memory on the client can be changed
        private class DataPackage
        {
            // wrapped variables
            private String currentPlayerPacked;
            private bool darkModePacked;
            private float fontSizePacked;
            private bool easyModePacked;
            private bool timerOnPacked;
            private List<wordleAttempt> attemptListPacked;

            // getters and setters
            public String CurrentPlayerPacked
            {
                get { return currentPlayerPacked; }
                set { currentPlayerPacked = value; }
            }

            public bool DarkModePacked
            {
                get { return darkMode; }
                set { darkMode = value; }
            }

            public float FontSizePacked
            {
                get { return fontSizePacked; }
                set { fontSizePacked = value; }
            }

            public bool EasyModePacked
            {
                get { return easyModePacked; }
                set { easyModePacked = value; }
            }

            public bool TimerOnPacked
            {
                get { return timerOnPacked; }
                set { timerOnPacked = value; }
            }

            public List<wordleAttempt> AttemptListPacked
            {
                get { return attemptListPacked; }
                set { attemptListPacked = value; }
            }
        }

        static public bool loadData()
        {
            bool status = false; // false if failed, true if success in loading data

            try
            {
               if (File.Exists(filePath)) // if file exists
                {
                    // we attempt to load data from the file
                    using FileStream InputStream = System.IO.File.OpenRead(filePath); // using Filestream for compatability and preformance
                    using StreamReader reader = new StreamReader(InputStream);

                    using (reader)
                    {
                        // loading Data package
                        string jsonstring = reader.ReadToEnd();
                        wrappedData = JsonSerializer.Deserialize<DataPackage>(jsonstring);

                        // moving data from wrapped package to class
                        currentPlayer = wrappedData.CurrentPlayerPacked;
                        darkMode = wrappedData.DarkModePacked;
                        fontSize = wrappedData.FontSizePacked;
                        easyMode = wrappedData.EasyModePacked;
                        timerOn = wrappedData.TimerOnPacked;
                        attemptList = wrappedData.AttemptListPacked;
                    }
                    status = true;
                }
               else
                {
                    status = false;
                }
            }
            catch (UnauthorizedAccessException) // don't have premission to open file
            {
                Console.WriteLine($" Access to the file is unauthorized : {filePath}");
            }
            catch (Exception ex) // another error
            {
                Console.WriteLine($"An unexpected error occurred : {ex.Message}");
            }

            return status;
        }

        static public async Task<bool> saveDataAsync()
        {
            bool status = false; // false if failed, true if success in loading data

            try
            {
                // we attempt to load data from the file
                using FileStream outputStream = System.IO.File.OpenWrite(filePath); // using Filestream for compatability and preformance
                using StreamWriter streamWriter = new StreamWriter(outputStream);

                // wrapping all data in current class into wrappedData, overwriting old dataStore
                wrappedData.CurrentPlayerPacked = currentPlayer;
                wrappedData.DarkModePacked = darkMode;
                wrappedData.FontSizePacked = fontSize;
                wrappedData.EasyModePacked = easyMode;
                wrappedData.TimerOnPacked = timerOn;
                wrappedData.AttemptListPacked = attemptList;

                string JsonString = JsonSerializer.Serialize(wrappedData);
                using (streamWriter)
                {
                    await streamWriter.WriteAsync(JsonString);
                }
                status = true;
                Debug.Print("Data is saved");
            }
            catch (UnauthorizedAccessException) // don't have premission to open file
            {
                Console.WriteLine($" Access to the file is unauthorized : {filePath}");
            }
            catch (Exception ex) // another error
            {
                Console.WriteLine($"An unexpected error occurred : {ex.Message}");
            }

            return status;
        }

        static public void Display()
        {
            Debug.Print("\nCurrent Player: " + currentPlayer + "\n" +
                        "Dark Mode: " + darkMode + "\n" +
                        "Font Size: " + fontSize + "\n" +
                        "Easy Mode: " + easyMode + "\n" +
                        "Timer On: " + timerOn + "\n");
        }
    }
}
