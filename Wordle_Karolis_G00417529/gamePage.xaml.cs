using Microsoft.Maui.Controls;
using Microsoft.Maui.Platform;
using System.Diagnostics;
using System.Threading;

namespace Wordle_Karolis_G00417529;
// important notice: ONLY USE ONSCREEN KEYBOARD WHILE USING MOBILE VERSION, as phones don't have keyboards and it can cause glitches.
public partial class gamePage : ContentPage
{
    // class fields
    List<Entry> entries;
    Image refernce;
    int currentEntery, enteryMaxSize;
    bool inputLocked, appOn, enteryLocked, lastInputLock;
    double maxSize;
    wordleAttempt currentWordle;

	public gamePage()
    {
        InitializeComponent();

        // intilizing class fields
        entries = new List<Entry>();
        enteryLocked = false;
        DataHandler.gameFinished = false;
        inputLocked = false;
        appOn = true;
        lastInputLock = true;

        // setting up ui elements
        setupUI();

        // We loop ui focus so that the player never looses track of ui
        // this is done asynchronously
        Thread focusingText = new Thread(focusAllTextBoxs);
        focusingText.Start();

        // we sale all ui
        scaleElements();

        // we create game and wordleAttempt, only save if the game is finished!
        currentWordle = new wordleAttempt();
        currentWordle.setupGame();
    }

    protected override void OnAppearing()
    {
        // this function makes sure loops starts up again
        base.OnAppearing();
        appOn = true;

        // shell workaround
        DataHandler.isInGamePage = true;
    }

    protected override void OnDisappearing()
    {
        // page is being removed, so we stop calling functions from another thread to the main thread to prevent bugs
        base.OnDisappearing();
        appOn = false;
    }

    private void focusAllTextBoxs()
    {
        // this function makes sure user is on the correct entery
        while (true)
        {
            // accessing function from the main thread
            if (appOn)
            {
                if (currentEntery > 29) currentEntery = 29;
                MainThread.InvokeOnMainThreadAsync(() => { entries[currentEntery].Focus(); });
            }
            Thread.Sleep(1000); // slows down multithreaded loop
        }
    }

    private void setupUI()
    {
        // this function setting up ui elements
        // initializing class fields
        currentEntery = 0;
        maxSize = gameGrid.WidthRequest;
        enteryMaxSize = 50;

        // hooking function to every time layout is changed
        LayoutChanged += OnWindowChange;

        // removing navigation buttons if on mobile
        if (DeviceInfo.Current.Idiom == DeviceIdiom.Phone)
        {
            accountBtn.IsVisible = false;
            accountImg.IsVisible = false;

            wordleBtn.IsVisible = false;
            WordleImg.IsVisible = false;

            progressionBtn.IsVisible = false;
            progressionImg.IsVisible = false;

            settingsBtn.IsVisible = false;
            settingsImg.IsVisible = false;

            howToPlayBtn.IsVisible = false;
            howToPlayImg.IsVisible = false;
        }

        // adding grid box's and text box's
        for (int row = 0; row < 6; row++) // 6 attempts
        {
            for (int col = 0; col < 5; col++) // 5 letter word
            { 
                // creatign our image element
                Image newImage = new Image();
                newImage.ZIndex = 1;
                newImage.Source = "candygridimage.png";
                newImage.Aspect = Aspect.Fill;
                newImage.SetValue(Grid.RowProperty, row);
                newImage.SetValue(Grid.ColumnProperty, col);
                gameGrid.Add(newImage);
                // adding referance
                refernce = newImage;

                // creating text box's
                Entry newEntry = new Entry();
                newEntry.ZIndex = 5; // always ontop
                newEntry.MaxLength = 2;
                newEntry.FontSize = enteryMaxSize;
                newEntry.FontFamily = "MerryDeer";
                newEntry.TextColor = new Color(255, 255, 255);
                newEntry.TextTransform = TextTransform.Lowercase;
                newEntry.Rotation = 180;
                newEntry.HorizontalOptions = LayoutOptions.Fill;
                newEntry.VerticalOptions = LayoutOptions.Fill;
                newEntry.HorizontalTextAlignment = TextAlignment.Center;
                newEntry.VerticalTextAlignment = TextAlignment.Center;
                newEntry.IsTextPredictionEnabled = false;
                newEntry.IsSpellCheckEnabled = false;
                newEntry.SetValue(Grid.RowProperty, row);
                newEntry.SetValue(Grid.ColumnProperty, col);
                gameGrid.Add(newEntry);
                entries.Add(newEntry);

                // making sure start entry of each row is length 1
                if (col == 0) newEntry.MaxLength = 1;

                // adding functionality to each entery
                newEntry.TextChanged += NewEntry_TextChanged;
                newEntry.Completed += NewEntry_Completed;
            }
        }

        // focusing on first entery and setting it up
        Entry firstEntery = entries[0];
        firstEntery.MaxLength = 1;
        firstEntery.Focus();
    }

    private void refreshEntrys()
    {
        foreach (var entry in entries)
        {
         
        }
    }

    private void NewEntry_Completed(object sender, EventArgs e)
    {
        // this function handels checking awnser and moving player onto next attempt
        if (!enteryLocked && !DataHandler.gameFinished)
        {
            // class varibales
            Entry castedObj = (Entry)sender;
            bool rowCompleted = false;
            int objRow = (int)castedObj.GetValue(Grid.RowProperty);
            int objCol = (int)castedObj.GetValue(Grid.ColumnProperty);
            int rowStartIndex = objRow * 5;
            string builtUpString = "";

            // checking if all the 5 character slots are filled
            for (int i = 0; i < 5; i++)
            {
                if (entries[rowStartIndex + i].Text != "" && entries[rowStartIndex + i].Text != "\u00A0")
                {
                    // building string
                    if (i > 0)
                    {
                        builtUpString += entries[rowStartIndex + i].Text[1];
                    }
                    else
                    {
                        builtUpString += entries[rowStartIndex + i].Text;
                    }

                    if (i == 4)
                    {
                        rowCompleted = true;
                    }
                }
                else
                {
                    break; // we break without setting value to true
                }
            }

            // check awnsers if row is completed
            if (rowCompleted)
            {
                // we check anwser and prompt animation function
                enteryLocked = true;
                inputLocked = false;
                int[] result = currentWordle.tryAttempt(builtUpString);

                // animating the grid
                animateAttempt(result);

                // moving up a row
                int currentRow = (int)entries[currentEntery].GetValue(Grid.RowProperty);
                int currentCol = (int)entries[currentEntery].GetValue(Grid.ColumnProperty);
                if (currentRow < 6) 
                {
                    currentEntery = ((currentRow + 1) * 5);
                }
                entries[currentEntery-1].Focus();

                // if game is lost or won, we lock inputs
                if (result[0] == 1)
                {
                    setGameIsOver(true);

                }
                else if (currentWordle.NumberOfGuesses == 0) // game lost
                {
                    setGameIsOver(true);
                }
            }
        }
    }

    private void setGameIsOver(bool status)
    {
        // this function locks / unlocks the game mechanics
        DataHandler.gameFinished = status;
        foreach (Entry entry in entries)
        {
            entry.IsReadOnly = status;
        }
    }

    private async void animateAttempt(int[] attempt)
    {
        // this function animates the grid entries
        int startIndex = Math.Abs(currentEntery / 5) * 5;
        int endIndex = startIndex + 5;
        int counter = 0;

        // animating all entries in that row
        for (int i = startIndex; i < endIndex; i++)
        {
            await entries[i].RotateXTo(90, 125); // flip 90 degrees to hide colour change

            // only color change if it's wrong position or correct
            if (attempt[counter + 1] != 0)
            {
                entries[i].BackgroundColor = wordleAttempt.colorArray[attempt[counter + 1]];
                entries[i].Opacity = 0.5;
            }

            await entries[i].RotateXTo(90, 125);
            await entries[i].RotateXTo(360, 250);
            counter++;
        }

        // prompting message
        if (attempt[0] == 1) 
            await DisplayAlert("You won!", $"well done!, progress has been saved, Correct word was: {currentWordle.CorrectWord}.", "Lets go!");
        else if (currentWordle.NumberOfGuesses == 0)
            await DisplayAlert("You lost!", $"Correct word was: {currentWordle.CorrectWord}, better luck next time :)", "alright :(");

        enteryLocked = false;
    }

    private void NewEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (!DataHandler.gameFinished) // locks input if game is over
        {
            // creating dynamic entery box moving
            Entry castedObj = (Entry)sender;
            int objRow = (int)castedObj.GetValue(Grid.RowProperty);
            int objCol = (int)castedObj.GetValue(Grid.ColumnProperty);
            int entreyIndex = (objRow * 5) + objCol;

            if ((currentWordle.currentAttempt * 5) + 5 > entreyIndex && (currentWordle.currentAttempt * 5) - 1 < entreyIndex)
            {
                switchFocus(castedObj);

                // only unlock if not last col in row
                if (objCol < 4 || !lastInputLock) inputLocked = false;
                lastInputLock = !lastInputLock; // alternating lock
            }
        }
    }

    private void switchFocus(Entry sender)
    {
        // calss varibales
        int objRow = (int)sender.GetValue(Grid.RowProperty);
        int objCol = (int)sender.GetValue(Grid.ColumnProperty);
        int entreyIndex = (objRow * 5) + objCol;

        if (!inputLocked)
        {
            inputLocked = true;

            // use went back 1 slot
            if ((sender.Text == "\u00A0" || sender.Text == "") && entreyIndex > 0)
            {
                // moving elmenet 1 slot down
                entreyIndex --;
            }
            else if (sender.Text != "" && entreyIndex < 30) // user went foward 1 slot
            {
                // moving elmenet 1 slot up
                entreyIndex++;
            }

            // finding next entery element while making sure player doesnt pass further then the word row
            if (entreyIndex < 30 && (currentWordle.currentAttempt * 5) + 5 > entreyIndex && (currentWordle.currentAttempt * 5) - 1 < entreyIndex)
            {
                Entry nextEntery = entries[entreyIndex];
                currentEntery = entreyIndex;

                // adding invisible character, so you can go foward and backwords with textchanged which
                // is alot more userfriendly then having to press enter every time
                if (entreyIndex != currentWordle.currentAttempt*5) // people don't need to be able to go back, on the last entery of the board
                {
                    nextEntery.Text = "\u00A0";
                }
                else
                {
                    nextEntery.Text = "";
                }

                // focusing on entery 
                nextEntery.Focus();
            }
        }
    }

    private async void startGameBtn_Clicked(object sender, EventArgs e)
    {
        // this function handels restarting the game on button click
        bool answer = true;

        if (!DataHandler.gameFinished)
        {
            answer = await DisplayAlert("Do you want to restart the game?", "data will not be saved for uncompleted wordles", "Yes", "No");
        }
        
        if (answer) 
        {
            resetGame();
        }
    }

    private void resetGame()
    {
        // reseting all class fields back to default
        DataHandler.gameFinished = true; // Locks loop
        appOn = true;
        enteryLocked = false;
        inputLocked = false;
        lastInputLock = true;
        currentEntery = 0;
        entries[currentEntery].Focus();

        // wiping grid
        foreach (Entry entry in entries)
        {
            entry.Text = "";
            entry.CancelAnimations();
            entry.Rotation = 180;
            entry.BackgroundColor = new Color(0,0,0,0);
            entry.Background = Brush.Transparent;
            entry.Opacity = 1;
        }

        // spawning new game
        currentWordle = new wordleAttempt();
        currentWordle.setupGame();
        setGameIsOver(false);
    }

    private string strReverse(string toReverse)
    {
        // this function reverses a string
        string reversedString = "";

        for (int i = toReverse.Length-1; i > 0; i--)
        {
            reversedString += toReverse[i];
        }

        return reversedString;
    }

    private void scaleElements()
    {
        // this function handles scaling of ui elements
        // function varibles
        double pixelDensity = DeviceDisplay.MainDisplayInfo.Density;
        double windowHeight = this.Height / pixelDensity;
        double windowWidth = this.Width / pixelDensity;
        double gridRatio = 0.65;
        bool isMobile = false;

        // mobile devices don't have an accurate window, so we use full screen scale for more accurate scaling
        if (DeviceInfo.Current.Idiom == DeviceIdiom.Phone)
        {
            windowHeight = DeviceDisplay.MainDisplayInfo.Height / pixelDensity;
            windowWidth = DeviceDisplay.MainDisplayInfo.Width / pixelDensity;
            isMobile = true;

            // phone has a larger grid
            gridRatio = 0.7;
        }

        // taking pixelDensity into account
        background.HeightRequest = windowHeight;
        background.WidthRequest = windowWidth;

        // scalling fonts
        pageTitle.FontSize = fontManager.scaleFontSize(180, windowHeight, windowWidth);
        startGameBtn.FontSize = fontManager.scaleFontSize(100, windowHeight, windowWidth) * 0.95;

        // scaling start new game button
        if (!isMobile) // windows scaling
        {
            startGameBtn.WidthRequest = windowWidth * 0.2;
            startGameBtn.HeightRequest = windowHeight * 0.1;
        }
        else // mobile scaling
        {
            pageTitle.FontSize = fontManager.scaleFontSize(360, windowHeight, windowWidth);
            startGameBtn.WidthRequest = windowWidth * 0.5;
            startGameBtn.HeightRequest = windowHeight * 0.1;
        }

        // scaling navigation buttons that are only visible on pc
        if (!isMobile)
        {
            double btnFontSize = fontManager.scaleFontSize(100, windowHeight, windowWidth) * 0.95;
            double scaledHeight = (windowHeight / 1408) * 150;
            double scaledWidth = (windowWidth / 2560) * 500;
            accountBtn.FontSize = btnFontSize;
            accountBtn.HeightRequest = scaledHeight;
            accountBtn.WidthRequest = scaledWidth;
            accountImg.HeightRequest = scaledHeight;
            accountImg.WidthRequest = accountBtn.WidthRequest / 5;

            wordleBtn.FontSize = btnFontSize;
            wordleBtn.HeightRequest = scaledHeight;
            wordleBtn.WidthRequest = scaledWidth;
            WordleImg.HeightRequest = scaledHeight;
            WordleImg.WidthRequest = wordleBtn.WidthRequest / 5;

            progressionBtn.FontSize = btnFontSize;
            progressionBtn.HeightRequest = scaledHeight;
            progressionBtn.WidthRequest = scaledWidth;
            progressionImg.HeightRequest = scaledHeight;
            progressionImg.WidthRequest = progressionBtn.WidthRequest / 5;

            settingsBtn.FontSize = btnFontSize;
            settingsBtn.HeightRequest = scaledHeight;
            settingsBtn.WidthRequest = scaledWidth;
            settingsImg.HeightRequest = scaledHeight;
            settingsImg.WidthRequest = settingsBtn.WidthRequest / 5;

            howToPlayBtn.FontSize = btnFontSize;
            howToPlayBtn.HeightRequest = scaledHeight;
            howToPlayBtn.WidthRequest = scaledWidth;
            howToPlayImg.HeightRequest = scaledHeight;
            howToPlayImg.WidthRequest = howToPlayBtn.WidthRequest / 5;

            // scaling btn spacing
            btnHolder.ColumnSpacing = (windowWidth / 2560) * 1555;
        }
      
        // scaling grid, we want it to always be square so we will take the smallest value ( width or height)
        // use it to make a square
        double SmallestLength;

        if (windowWidth > windowHeight)
        {
            SmallestLength = windowHeight;
        }
        else
        {
            SmallestLength = windowWidth;
        }

        gameGrid.WidthRequest = SmallestLength * gridRatio;
        gameGrid.HeightRequest = (gameGrid.WidthRequest / 5) * 6; // makes images square, as rows and coloumns are not equal so images wouldn't be square

        // scaling grid fonts relative to any selection of the grid
        // PS: There is a weird glitch that stops you from scaling entries correctly
        double percentChanged = gameGrid.WidthRequest / maxSize;

        // overshadowed "currentEntery"
        foreach (Entry currentEntery in entries)
        {
            currentEntery.FontSize = (enteryMaxSize * percentChanged) * 0.85;
            currentEntery.ScaleX = refernce.WidthRequest * 0.85;
            currentEntery.ScaleY = refernce.HeightRequest * 0.85;
        }

        // moving grid up on mobile
        if (isMobile)
        {
            gameGrid.TranslationY -= windowHeight * 0.1;
            startGameBtn.TranslationY -= (windowHeight * 0.1) + (gameGrid.HeightRequest * 0.5) - startGameBtn.HeightRequest;
            startGameBtn.FontSize = fontManager.findFontSizeToConstraint(startGameBtn.HeightRequest * 3);
        }  
    }

    private void OnWindowChange(object sender, EventArgs e)
    {
        scaleElements();
    }

    private async void navigationControl(object sender, EventArgs e)
    {
        // this function handles navigation for all devices but phone
        Button castedObj = (Button)sender;
        bool answer = true; // allow to pass by default

        // prompt player if they want to leave their current game (if one is running)
        if (!DataHandler.gameFinished)
        {
            answer = await DisplayAlert("Do you want to leave the game?", "If you leave now, your progress will not be saved", "Yes", "No");
        }

        if (answer)
        {
            DataHandler.isInGamePage = false;
            switch (castedObj.Text)
            {
                case "Account":
                    await Navigation.PushAsync(new MainPage());
                    break;
                case "Wordle":
                    await Navigation.PushAsync(new gamePage());
                    break;
                case "Progression":
                    await Navigation.PushAsync(new progressionPage());
                    break;
                case "Settings":
                    await Navigation.PushAsync(new SettingsPage());
                    break;
                case "How to play":
                    await Navigation.PushAsync(new howToPlayPage());
                    break;
            }
        }
    }
}