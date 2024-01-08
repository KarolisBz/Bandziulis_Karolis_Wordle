using Microsoft.Maui;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace Wordle_Karolis_G00417529
{
    // this class handles most of loading, saving and storing of data
    public class DataHandler : INotifyPropertyChanged
    {
        // class fields //
        static private string filePath = System.IO.Path.Combine(FileSystem.Current.AppDataDirectory, "wordleUserData2.json");
        static private string wordfilePath = System.IO.Path.Combine(FileSystem.Current.AppDataDirectory, "cachedWords.json");
        // player data //
        static public string currentPlayer;
        // settings data //
        static private bool darkMode;
        static private bool easyMode;
        static private bool cheats;
        static private double animationSpeed;
        // api cached //
        static private HttpClient client;
        static public List<string> wordList;
        // shell workaround
        static public bool isInGamePage;
        static public bool gameFinished;
        static public ShellVeiwModel shellVeiwModel = new ShellVeiwModel(); // creating shellViewmodel
        // wrapped data //
        static public DataPackage wrappedData = new DataPackage(); // creating data wrapper
        static public progressionVeiwModel cachedProgressViewModel = new progressionVeiwModel();
        static public DataHandler DataHandlerObject;

        // constructor, don't call more then once in entire program
        public DataHandler()
        {
            // we cache ourselves to be visible to everyone
            DataHandlerObject = this;

            // requests loading of data when created, however data is static and can be accessed anywhere without object
            if (!loadData())
            {
                // if we failed to load data, then we will give it default values
                currentPlayer = "Default_User";
                animationSpeed = 1;
                darkMode = false;
                easyMode = false;
                cheats = false;
                gameFinished = false;
                cachedProgressViewModel.AttemptList = new ObservableCollection<wordleAttempt>();
            }

            // initializing shellworkaround
            isInGamePage = false;

            // creating client for http and wordlist
            client = new HttpClient();
            wordList = new List<string>();
            Debug.Print("data directory: " + filePath);

            // requests api call to fetch words if it has not do so before
            if (!File.Exists(wordfilePath)) // if file doesn't exist
            {
                Debug.Print("fetching and saving from api");
                fetchSaveApi();
            }
            else // fetches words from saved file that was downloaded and saved before hand
            {
                Debug.Print("fetching from file - " + wordfilePath);
                fetchLocalWords();
            }
        }

        // creating an internal class that contains all the required data
        // so we can read it and write it all at once while keeping all the varibles in the class static, and use it as a veiwmodel in the future
        // normally storing data like this will cause very easy cheating as all the data is static and public, but since it's a client only game cheats can never be prevented
        // as memory on the client can be changed
        public class DataPackage
        {
            // wrapped variables
            public string currentPlayerPacked { get; set; }
            public bool darkModePacked { get; set; }
            public double animationSpeedPacked { get; set; }
            public bool easyModePacked { get; set; }
            public bool cheatsPacked { get; set; }
            public ObservableCollection<wordleAttempt> attemptListPacked { get; set; }
        }

        // is the veiwModel for shell, it's a very small class so we will just keep it in dataHandler
        public class ShellVeiwModel : INotifyPropertyChanged
        {
            FlyoutBehavior flyoutPageStatus = FlyoutBehavior.Disabled;

            // get and set
            public FlyoutBehavior FlyoutPageStatus
            {
                get { return flyoutPageStatus; }
                set 
                { 
                    flyoutPageStatus = value;
                    OnPropertyChanged();
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;

            // credits to Donny Hurley for this function
            protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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
                        currentPlayer = wrappedData.currentPlayerPacked;
                        animationSpeed = wrappedData.animationSpeedPacked;
                        darkMode = wrappedData.darkModePacked;
                        easyMode = wrappedData.easyModePacked;
                        cheats = wrappedData.cheatsPacked;
                        cachedProgressViewModel.AttemptList = wrappedData.attemptListPacked;
                    }
                    status = true;
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
                wrappedData.animationSpeedPacked = animationSpeed;
                wrappedData.darkModePacked = darkMode;
                wrappedData.easyModePacked = easyMode;
                wrappedData.cheatsPacked = cheats;
                wrappedData.attemptListPacked = cachedProgressViewModel.AttemptList;

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
            Debug.Print("\nCurrent Player: " + currentPlayer.ToString() + "\n" +
                        "Dark Mode: " + darkMode.ToString() + "\n" +
                        "Animation Speed: " + animationSpeed.ToString() + "\n" +
                        "Easy Mode: " + easyMode.ToString() + "\n" +
                        "Cheats: " + cheats.ToString() + "\n");
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

                    // last part of api list is a blank, so we will remove this
                    wordList.Remove("");
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

        // DATA veiwmodel - used for settings page
        // getters and setters
        public bool DarkMode
        {
            get { return darkMode; }
            set { darkMode = value; OnPropertyChanged(); savingExtender(); }
        }
        public bool EasyMode
        {
            get { return easyMode; }
            set { easyMode = value; OnPropertyChanged(); savingExtender(); }
        }
        public bool Cheats
        {
            get { return cheats; }
            set { cheats = value; OnPropertyChanged(); savingExtender(); }
        }
        public double AnimationSpeed // allows for up to x10 animation speed
        {
            get { return animationSpeed; }
            set 
            {
                if (value == 0) value = 0.001; // we don't allow a value of 0
                animationSpeed = value * 10;
                OnPropertyChanged();
                savingExtender(); 
            }
        }

        public double SliderValue // makes sure slide stays between 0 - 1
        {
            get { return animationSpeed / 10; }
            set { AnimationSpeed = value; }
        }
        // veiwModel functions
        private async void savingExtender()
        {
            await saveDataAsync();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        // credits to Donny Hurley for this function
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
