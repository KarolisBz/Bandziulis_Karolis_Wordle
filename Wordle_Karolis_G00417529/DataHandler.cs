using Foundation;
using System.Text.Json;

namespace Wordle_Karolis_G00417529
{
    // this class handles most of loading and saving of data
    public class DataHandler
    {
        // class fields //
        static String filePath = "wordle_UserData.json";
        // player data
        static String currentPlayer;
        // settings data
        static bool darkMode;
        static float fontSize;
        static bool easyMode;
        static bool timerOn;
        // wordle attempt list
        static List<wordleAttempt> attemptList;
        // wrapped data
        static DataPackage wrappedData = new DataPackage(); // creating data wrapper

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

        // creating an internal class that containts all the required data
        // so we can read it and write it all at once while keeping as a static class
        private class DataPackage
        {
            public String currentPlayerPacked;
            public bool darkModePacked;
            public float fontSizePacked;
            public bool easyModePacked;
            public bool timerOnPacked;
            public List<wordleAttempt> attemptListPacked;
        }

        static private bool loadData()
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
                        dataStored = JsonSerializer.Deserialize<List<DataPackage>>(jsonstring);
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

        static private async Task<bool> saveDataAsync()
        {
            bool status = false; // false if failed, true if success in loading data

            try
            {
                if (File.Exists(filePath)) // if file exists
                {
                    // we attempt to load data from the file
                    using FileStream outputStream = System.IO.File.OpenWrite(filePath); // using Filestream for compatability and preformance
                    using StreamWriter streamWriter = new StreamWriter(outputStream);

                    // wrapping all data in current class into wrappedData, overwriting old dataStore
                    wrappedData.currentPlayerPacked = currentPlayer;
                    wrappedData.darkModePacked = darkMode;
                    wrappedData.fontSizePacked = fontSize;
                    wrappedData.easyModePacked = easyMode;
                    wrappedData.timerOnPacked = timerOn;
                    wrappedData.attemptListPacked = attemptList;

                    string JsonString = JsonSerializer.Serialize(wrappedData);
                    using (streamWriter)
                    {
                        await streamWriter.WriteAsync(JsonString);
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
    }
}
