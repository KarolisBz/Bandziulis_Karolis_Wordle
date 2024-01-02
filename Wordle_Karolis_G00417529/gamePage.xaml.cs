using Microsoft.Maui.Controls;
using Microsoft.Maui.Platform;
using System.Diagnostics;

namespace Wordle_Karolis_G00417529;

public partial class gamePage : ContentPage
{
    // class fields
    List<Entry> entries;
    Image refernce;
    int currentEntery, enteryMaxSize;
    bool inputLocked, appOn;
    double maxSize;
    wordleAttempt currentWordle;

	public gamePage()
    {
        InitializeComponent();

        // intilizing class fields
        entries = new List<Entry>();

        // setting up ui elements
        setupUI();

        // We loop ui focus so that the player never looses track of ui
        // this is done asynchronously
        Thread focusingText = new Thread(focusAllTextBoxs);
        focusingText.Start();

        // we sale all ui
        scaleElements();
    }

    protected override void OnDisappearing()
    {
        // page is being removed, so we stop calling functions from another thread to the main thread to prevent bugs
        appOn = false;
    }

    private void focusAllTextBoxs()
    {
        while (true && appOn)
        {
            Thread.Sleep(1000); // slows down multithreaded loop
            // accessing function from the main thread
            MainThread.InvokeOnMainThreadAsync(() => { entries[currentEntery].Focus(); });
        }
    }

    private void setupUI()
    {
        // initializing class fields
        currentEntery = 0;
        maxSize = gameGrid.WidthRequest;
        inputLocked = false;
        appOn = true;
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
                //newEntry.BackgroundColor = new Color(255, 0, 0);
                newEntry.Rotation = 180;
                newEntry.HorizontalOptions = LayoutOptions.Center;
                newEntry.VerticalOptions = LayoutOptions.Center;
                newEntry.HorizontalTextAlignment = TextAlignment.Center;
                newEntry.VerticalTextAlignment = TextAlignment.Center;
                newEntry.IsTextPredictionEnabled = false;
                newEntry.IsSpellCheckEnabled = false;
                newEntry.SetValue(Grid.RowProperty, row);
                newEntry.SetValue(Grid.ColumnProperty, col);
                gameGrid.Add(newEntry);
                entries.Add(newEntry);

                // adding functionality to each entery
                newEntry.TextChanged += NewEntry_TextChanged;
            }
        }

        // focusing on first entery and setting it up
        Entry firstEntery = entries[0];
        firstEntery.MaxLength = 1;
        firstEntery.Focus();
    }

    private void NewEntry_Unfocused(object sender, FocusEventArgs e)
    {
        // ensuring player does not click off entery
        Debug.WriteLine("refocused \n");
        entries[currentEntery].Focus();
    }

    private void NewEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
        // creating dynamic entery box moving
        switchFocus((Entry)sender);
        inputLocked = false;
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

            // finding next entery element
            if (entreyIndex < 30)
            {
                Entry nextEntery = entries[entreyIndex];
                currentEntery = entreyIndex;

                // adding invisible character, so you can go foward and backwords with textchanged which
                // is alot more userfriendly then having to press enter every time
                if (entreyIndex != 0) // people don't need to be able to go back, on the last entery of the board
                {
                    nextEntery.Text = "\u00A0";
                }

                // focusing on entery 
                nextEntery.Focus();
            }
        }
    }

    private string strReverse(string toReverse)
    {
        string reversedString = "";

        for (int i = toReverse.Length-1; i > 0; i--)
        {
            reversedString += toReverse[i];
        }

        return reversedString;
    }

    private void scaleElements()
    {
        // function varibles
        double pixelDensity = DeviceDisplay.MainDisplayInfo.Density;
        double windowHeight = this.Height / pixelDensity;
        double windowWidth = this.Width / pixelDensity;
        double gridRatio = 0.65;

        // mobile devices don't have an accurate window, so we use full screen scale for more accurate scaling
        if (DeviceInfo.Current.Idiom == DeviceIdiom.Phone)
        {
            windowHeight = DeviceDisplay.MainDisplayInfo.Height / pixelDensity;
            windowWidth = DeviceDisplay.MainDisplayInfo.Width / pixelDensity;

            // phone has a larger grid
            gridRatio = 0.7;
        }

        // taking pixelDensity into account
        background.HeightRequest = windowHeight;
        background.WidthRequest = windowWidth;

        // scalling fonts
        pageTitle.FontSize = fontManager.scaleFontSize(180, windowHeight, windowWidth);

        // scaling navigation buttons that are only visible on pc
        if (DeviceInfo.Current.Idiom != DeviceIdiom.Phone)
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

        foreach (Entry currentEntery in entries)
        {
            currentEntery.FontSize = (enteryMaxSize * percentChanged) * 0.85;
            currentEntery.ScaleX = refernce.WidthRequest * 0.85;
            currentEntery.ScaleY = refernce.HeightRequest * 0.85;
        }
        //Debug.Print("Grid height: " + (gameGrid.HeightRequest  / 5).ToString() + ",Font Size: " + entries[0].FontSize.ToString() + ", Desired Size: " + entries[0].DesiredSize.ToString());
    }

    private void OnWindowChange(object sender, EventArgs e)
    {
        scaleElements();
    }
}