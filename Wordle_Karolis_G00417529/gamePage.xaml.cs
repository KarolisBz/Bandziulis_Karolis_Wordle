using Microsoft.Maui.Controls;
using System.Diagnostics;

namespace Wordle_Karolis_G00417529;

public partial class gamePage : ContentPage
{
	public gamePage()
	{
		InitializeComponent();
        setupUI(); // setting up ui elements
    }

    private void setupUI()
    {
        // hooking function to every time layout is changed
        this.LayoutChanged += OnWindowChange;

        // adding grid box's
        for (int row = 0; row < 6; row++) // 6 attempts
        {
            for (int col = 0; col < 5; col++) // 5 letter word
            { 
                // creatign our image element
                Image newImage = new Image();
                newImage.ZIndex = 1;
                newImage.Source = "candygridimage.png";
                newImage.Aspect = Aspect.AspectFit;
                newImage.SetValue(Grid.RowProperty, row);
                newImage.SetValue(Grid.ColumnProperty, col);
                gameGrid.Add(newImage);
            }
        }
    }

    private void OnWindowChange(object sender, EventArgs e)
    {
        // function varibles
        double windowHeight = this.Height;
        double windowWidth = this.Width;
        double pixelDensity = DeviceDisplay.MainDisplayInfo.Density;

        // mobile devices don't have an accurate window, so we use full screen scale for more accurate scaling
        if (DeviceInfo.Current.Idiom == DeviceIdiom.Phone)
        {
            windowHeight = DeviceDisplay.MainDisplayInfo.Height;
            windowWidth = DeviceDisplay.MainDisplayInfo.Width;
        }

        // taking pixelDensity into account
        background.HeightRequest = windowHeight / pixelDensity;
        background.WidthRequest = windowWidth / pixelDensity;

        // scalling fonts
        pageTitle.FontSize = fontManager.scaleFontSize(180, windowHeight, windowWidth);
    }
}