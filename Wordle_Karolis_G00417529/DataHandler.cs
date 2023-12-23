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

        // constructor
        public DataHandler()
        {
            // requests loading of data when created, however data is static and can be accessed anywhere
            if (!loadData())
            {
                // if we failed to load data, then we will give it default values
                currentPlayer = "Default_User";
                darkMode = false;
                fontSize = 20;
                easyMode = false;
                timerOn = true;
            }
        }

        private bool loadData()
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
                        string jsonstring = reader.ReadToEnd();
                        List<Entry> peoplelist = JsonSerializer.Deserialize<List<Entry>>(jsonstring);
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
