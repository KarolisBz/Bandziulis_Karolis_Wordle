using Microsoft.Maui.Controls;
using Microsoft.Maui.Platform;
using System.Diagnostics;

namespace Wordle_Karolis_G00417529;

public partial class gamePage : ContentPage
{
    // class fields
    List<Entry> entries;
    int currentEntery;
    bool inputLocked, appOn;

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

        // page is being removed, so we stop calling functions from another thread to the main thread to prevent bugs
        //this.Window.Destroying += Window_Destroying;
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
            Debug.Print("looping");
            // accessing function from the main thread
            MainThread.InvokeOnMainThreadAsync(() => { entries[currentEntery].Focus(); });
        }
    }

    private void setupUI()
    {
        // initializing class fields
        currentEntery = 0;
        inputLocked = false;
        appOn = true;

        // hooking function to every time layout is changed
        LayoutChanged += OnWindowChange;

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

                // creating text box's
                Entry newEntry = new Entry();
                newEntry.ZIndex = 5; // always ontop
                newEntry.MaxLength = 2;
                newEntry.FontSize = 80;
                newEntry.FontFamily = "MerryDeer";
                newEntry.TextColor = new Color(255, 255, 255);
                newEntry.HorizontalTextAlignment = TextAlignment.Center;
                newEntry.VerticalTextAlignment = TextAlignment.Center;
                newEntry.HorizontalOptions = LayoutOptions.Center;
                newEntry.VerticalOptions = LayoutOptions.Center;
                newEntry.IsTextPredictionEnabled = false;
                newEntry.IsSpellCheckEnabled = false;
                //newEntry.Unfocused += NewEntry_Unfocused;
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
        double maxSelectionSize = 390 / 5;
        double currentSelectionSize = gameGrid.WidthRequest / 5;
        double relativeWidth = 2560 * (currentSelectionSize / maxSelectionSize);
        double relativeHeight = 1408 * (currentSelectionSize / maxSelectionSize);

        foreach (Entry currentEntery in entries)
        {
            currentEntery.FontSize = fontManager.scaleFontSize(80, relativeHeight, relativeWidth);
            currentEntery.TranslationX = windowWidth / 2;
            //currentEntery.TranslationY = (currentSelectionSize / 2) + gameGrid.Y;
        }
        Debug.Print(entries[0].FontSize.ToString() + ", maxSize: " + maxSelectionSize.ToString() + ", currentSize: " + currentSelectionSize.ToString());
    }

    private void OnWindowChange(object sender, EventArgs e)
    {
        scaleElements();
    }
}