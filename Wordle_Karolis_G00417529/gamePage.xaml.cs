using Microsoft.Maui.Controls;
using System.Diagnostics;

namespace Wordle_Karolis_G00417529;

public partial class gamePage : ContentPage
{
    // class fields
    List<Entry> entries;
    bool inputLocked = false;

	public gamePage()
	{
		InitializeComponent();

        // intilizing class fields
        entries = new List<Entry>();

        // setting up ui elements
        setupUI();
    }

    private void setupUI()
    {
        // hooking function to every time layout is changed
        this.LayoutChanged += OnWindowChange;

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

    private void NewEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
        Debug.Print( " lock status: " + inputLocked.ToString());
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

    private void OnWindowChange(object sender, EventArgs e)
    {
        // function varibles
        double windowHeight = this.Height;
        double windowWidth = this.Width;
        double pixelDensity = DeviceDisplay.MainDisplayInfo.Density;
        double gridRatio = 0.65;

        // mobile devices don't have an accurate window, so we use full screen scale for more accurate scaling
        if (DeviceInfo.Current.Idiom == DeviceIdiom.Phone)
        {
            windowHeight = DeviceDisplay.MainDisplayInfo.Height;
            windowWidth = DeviceDisplay.MainDisplayInfo.Width;

            // phone has a larger grid
            gridRatio = 0.7;
        }

        // taking pixelDensity into account
        background.HeightRequest = windowHeight / pixelDensity;
        background.WidthRequest = windowWidth / pixelDensity;

        // scalling fonts
        pageTitle.FontSize = fontManager.scaleFontSize(180, windowHeight, windowWidth);

        // scaling grid, we want it to always be square so we will take the smallest value ( width or height)
        // use it to make a square
        double SmallestLength;

        if (windowWidth > windowHeight)
        {
            SmallestLength = windowHeight / pixelDensity;
        }
        else
        {
            SmallestLength = windowWidth / pixelDensity;
        }

        Debug.WriteLine(SmallestLength.ToString());

        gameGrid.WidthRequest = SmallestLength * gridRatio;
        gameGrid.HeightRequest = (gameGrid.WidthRequest / 5) * 6; // makes images square, as rows and coloumns are not equal so images wouldn't be square
    }
}