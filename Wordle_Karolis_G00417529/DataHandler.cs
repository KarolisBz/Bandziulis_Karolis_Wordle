using System.Diagnostics;
using System.Text.Json;

namespace Wordle_Karolis_G00417529
{
    // this class handles most of loading, saving and storing of data
    public class DataHandler
    {
        // class fields //
        static private String filePath = System.IO.Path.Combine(FileSystem.Current.AppDataDirectory, "wordleUserData.json");
        static private String wordfilePath = System.IO.Path.Combine(FileSystem.Current.AppDataDirectory, "cachedWords.json");
        // player data //
        static public String currentPlayer;
        // settings data //
        static public bool darkMode;
        static public float fontSize;
        static public bool easyMode;
        static public bool timerOn;
        // api cached //
        static HttpClient client;
        static public List<string> wordList;
        // wordle attempt list //
        static public List<wordleAttempt> attemptList;
        // wrapped data //
        static private DataPackage wrappedData = new DataPackage(); // creating data wrapper

        // constructor, don't call more then once in entire program
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

            // creating client for http and wordlist
            client = new HttpClient();
            wordList = new List<string>();

            // requests api call to fetch words if it has not do so before
            if (!File.Exists(wordfilePath)) // if file doesn't exist
            {
                Debug.Print("fetching and saving from api");
                fetchSaveApi();
            }
            else // fetches words from saved file that was downloaded and saved before hand
            {
                Debug.Print("fetching from file");
                fetchLocalWords();
            }
        }

        // creating an internal class that contains all the required data
        // so we can read it and write it all at once while keeping all the varibles in the class static
        // normally storing data like this will cause very easy cheating as all the data is static and public, but since it's a client only game cheats can never be prevented
        // as memory on the client can be changed
        private class DataPackage
        {
            // wrapped variables
            public String currentPlayerPacked { get; set; }
            public bool darkModePacked { get; set; }
            public float fontSizePacked { get; set; }
            public bool easyModePacked { get; set; }
            public bool timerOnPacked { get; set; }
            public List<wordleAttempt> attemptListPacked { get; set; }
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
                        currentPlayer = wrappedData.currentPlayerPacked;
                        darkMode = wrappedData.darkModePacked;
                        fontSize = wrappedData.fontSizePacked;
                        easyMode = wrappedData.easyModePacked;
                        timerOn = wrappedData.timerOnPacked;
                        attemptList = wrappedData.attemptListPacked;
                    }
                    Debug.Print(currentPlayer);
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
                // we wipe old save, as we are overwriting it anyways and it may mess up writing in async
                if (File.Exists(filePath)) // if file exists
                {
                    System.IO.File.Delete(filePath);
                }

                // we attempt to save data
                using FileStream outputStream = System.IO.File.OpenWrite(filePath); // using Filestream for compatability and preformance
                using StreamWriter streamWriter = new StreamWriter(outputStream);
                Debug.WriteLine(filePath);

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

        static public void display()
        {
            Debug.Print("\nCurrent Player: " + currentPlayer + "\n" +
                        "Dark Mode: " + darkMode + "\n" +
                        "Font Size: " + fontSize + "\n" +
                        "Easy Mode: " + easyMode + "\n" +
                        "Timer On: " + timerOn + "\n");
        }

        static private async void fetchSaveApi()
        {
            // varibles
            bool fetchSuccess = false;

            // fetching the data and storing it in a list
            try
            {
                HttpResponseMessage serverResponse = await client.GetAsync("https://raw.githubusercontent.com/DonH-ITS/jsonfiles/main/words.txt");
                if (serverResponse.IsSuccessStatusCode)
                {
                    // requesting data from api
                    string content = await serverResponse.Content.ReadAsStringAsync();

                    // Breaking content into seperate strings and then adding them one by one into list
                    foreach (string word in content.Split('\n'))
                    {
                        wordList.Add(word);
                    }
                }

                fetchSuccess = true;
            }
            catch (Exception ex) // if failed request
            {
                Debug.WriteLine("failed to download words: ", ex.Message);
            }

            // saving data we just fetched
            if (fetchSuccess)
            {
                try
                {
                    // we attempt to save data
                    using FileStream outputStream = System.IO.File.OpenWrite(wordfilePath); // using Filestream for compatability and preformance
                    using StreamWriter streamWriter = new StreamWriter(outputStream);

                    string JsonString = JsonSerializer.Serialize(wordList);
                    using (streamWriter)
                    {
                        await streamWriter.WriteAsync(JsonString);
                    }
                }
                catch (Exception ex) // error saving file
                {
                    Console.WriteLine($"An unexpected error occurred while saving api data : {ex.Message}");
                }
            }

            
        }

        static private void fetchLocalWords()
        {
            bool fetchSuccess = false;

            try
            {
                // we attempt to load data from the file
                using FileStream InputStream = System.IO.File.OpenRead(wordfilePath); // using Filestream for compatability and preformance
                using StreamReader reader = new StreamReader(InputStream);

                using (reader)
                {
                    // loading words
                    string jsonstring = reader.ReadToEnd();
                    wordList = JsonSerializer.Deserialize<List<string>>(jsonstring);
                }

                fetchSuccess = true;
            }
            catch (UnauthorizedAccessException) // don't have premission to open file
            {
                Console.WriteLine($" Access to the file is unauthorized : {filePath}");
            }
            catch (Exception ex) // another error
            {
                Console.WriteLine($"An unexpected error occurred : {ex.Message}");
            }

            if (!fetchSuccess) // if we failed to fetch from file, we will fetch via api
            {
                System.IO.File.Delete(wordfilePath); // deleting currupted data
                fetchSaveApi();
            }
        }

    }
}
