using System.Diagnostics;

namespace Wordle_Karolis_G00417529;

public partial class howToPlayPage : ContentPage
{
	public howToPlayPage()
	{
        InitializeComponent();
        setupUI(); // setting up ui
    }

    private void setupUI()
    {
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

            background.Source = "mobilebackground.png";
            holder.Opacity = 0.25;
            holderShadow.Opacity = 0.25;
        }
    }

    protected override void OnAppearing()
    {
        // handles changes
        base.OnAppearing();
        checkDarkMode();
    }

    private void checkDarkMode()
    {
        // this function handles colour changing between light and dark mode
        if (DataHandler.DataHandlerObject.DarkMode)
        {
            BackgroundColor = Colors.Black;
            background.Opacity = 0.1;
            holder.Color = new Color(77, 77, 77);
            holderShadow.Color = new Color(43, 43, 43);

            // changing font colours
            pageTitle.TextColor = Colors.LightGray;
            titleText0.TextColor = Colors.LightGray;
            text1.TextColor = Colors.LightGray;
            text2.TextColor = Colors.LightGray;
            text3.TextColor = Colors.LightGray;
            text4.TextColor = Colors.LightGray;
            text5.TextColor = Colors.LightGray;
            text6.TextColor = Colors.LightGray;
            text7.TextColor = Colors.LightGray;

            // changing navigation btn colours
            if (DeviceInfo.Current.Idiom != DeviceIdiom.Phone)
            {
                accountBtn.BackgroundColor = new Color(44, 44, 44);
                accountBtn.TextColor = Colors.LightGray;

                wordleBtn.BackgroundColor = new Color(44, 44, 44);
                wordleBtn.TextColor = Colors.LightGray;

                progressionBtn.BackgroundColor = new Color(44, 44, 44);
                progressionBtn.TextColor = Colors.LightGray;

                settingsBtn.BackgroundColor = new Color(44, 44, 44);
                settingsBtn.TextColor = Colors.LightGray;

                howToPlayBtn.BackgroundColor = new Color(44, 44, 44);
                howToPlayBtn.TextColor = Colors.LightGray;
            }
        }
        else
        {
            BackgroundColor = Colors.White;
            background.Opacity = 1;
            holder.Color = new Color(255, 255, 255);
            holderShadow.Color = new Color(0, 0, 0);

            // changing font colours
            pageTitle.TextColor = Colors.Black;
            titleText0.TextColor = Colors.Black;
            text1.TextColor = Colors.Black;
            text2.TextColor = Colors.Black;
            text3.TextColor = Colors.Black;
            text4.TextColor = Colors.Black;
            text5.TextColor = Colors.Black;
            text6.TextColor = Colors.Black;
            text7.TextColor = Colors.Black;

            // changing navigation btn colours
            if (DeviceInfo.Current.Idiom != DeviceIdiom.Phone)
            {
                accountBtn.BackgroundColor = new Color(0, 0, 0);
                accountBtn.TextColor = Colors.White;

                wordleBtn.BackgroundColor = new Color(0, 0, 0);
                wordleBtn.TextColor = Colors.White;

                progressionBtn.BackgroundColor = new Color(0, 0, 0);
                progressionBtn.TextColor = Colors.White;

                settingsBtn.BackgroundColor = new Color(0, 0, 0);
                settingsBtn.TextColor = Colors.White;

                howToPlayBtn.BackgroundColor = new Color(0, 0, 0);
                howToPlayBtn.TextColor = Colors.White;
            }
        }
    }

    private void OnWindowChange(object sender, EventArgs e)
    {
        scaleElements();
    }

    private void scaleElements()
    {
        // this function handles scaling of ui elements
        // function varibles
        double pixelDensity = DeviceDisplay.MainDisplayInfo.Density;
        double windowHeight = this.Height / pixelDensity;
        double windowWidth = this.Width / pixelDensity;
        int BoxSizePreset = 75, baseTextSizePreset = 25, titletTextSizePreset = 35;
        bool isMobile = false;

        // mobile devices don't have an accurate window, so we use full screen scale for more accurate scaling
        if (DeviceInfo.Current.Idiom == DeviceIdiom.Phone)
        {
            windowHeight = DeviceDisplay.MainDisplayInfo.Height / pixelDensity;
            windowWidth = DeviceDisplay.MainDisplayInfo.Width / pixelDensity;
            isMobile = true;
        }

        // taking pixelDensity into account
        background.HeightRequest = windowHeight;
        background.WidthRequest = windowWidth;

        // scalling fonts
        if (!isMobile)
        {
            pageTitle.FontSize = fontManager.scaleFontSize(180, windowHeight, windowWidth);
        }
        else
        {
            pageTitle.FontSize = fontManager.scaleFontSize(360, windowHeight, windowWidth);
            BoxSizePreset = 175;
            baseTextSizePreset = 60;
            titletTextSizePreset = 85;
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

        // scaling assests (from ratio of 2k monitor)
        double titleSize = pageTitle.FontSize * 1.8;
        if (windowWidth < 500 || windowHeight < (700 + titleSize)) // we only scale if screen is smaller then requested size
        {
            // scaling fonts relative to box, not screen
            double relativeWidth = 2560 * (windowWidth / 500);
            double relativeHeight = 1408 * ((windowHeight - titleSize) / 700); // we use titleSize as an offset, so the title always stays above text box

            if (windowWidth < 500) { holder.WidthRequest = windowWidth; } else { holder.WidthRequest = 500; relativeWidth = 2560; }
            if (windowHeight < (700 + titleSize)) { holder.HeightRequest = (windowHeight - titleSize); } else { holder.HeightRequest = 700; relativeHeight = 1408; }

            holderShadow.HeightRequest = holder.HeightRequest + 10;
            holderShadow.WidthRequest = holder.WidthRequest + 10;

            // scaling grid
            contentGrid.HeightRequest = holder.HeightRequest;
            contentGrid.WidthRequest = holder.WidthRequest;

            // scaling boxveiws
            double boxSize = fontManager.scaleFontSize(BoxSizePreset, relativeHeight * 0.25, relativeWidth * 0.5);
            box1.HeightRequest = boxSize;
            box1.WidthRequest = boxSize;
            box2.HeightRequest = boxSize;
            box2.WidthRequest = boxSize;
            box3.HeightRequest = boxSize;
            box3.WidthRequest = boxSize;

            // scalling fonts relative to holder
            double baseTextSize = fontManager.scaleFontSize(baseTextSizePreset, relativeHeight, relativeWidth);
            titleText0.FontSize = fontManager.scaleFontSize(titletTextSizePreset, relativeHeight, relativeWidth);
            text1.FontSize = baseTextSize;
            text2.FontSize = baseTextSize;
            text3.FontSize = baseTextSize;
            text4.FontSize = baseTextSize;
            text5.FontSize = baseTextSize;
            text6.FontSize = baseTextSize;
            text7.FontSize = baseTextSize;

        }
        else // return to original size
        {
            holder.HeightRequest = 700;
            holder.WidthRequest = 500;
            holderShadow.HeightRequest = holder.HeightRequest + 10;
            holderShadow.WidthRequest = holder.WidthRequest + 10;

            // scaling grid
            contentGrid.HeightRequest = holder.HeightRequest;
            contentGrid.WidthRequest = holder.WidthRequest;

            // scaling boxveiws
            double boxSize = 75;
            box1.HeightRequest = boxSize;
            box1.WidthRequest = boxSize;
            box2.HeightRequest = boxSize;
            box2.WidthRequest = boxSize;
            box3.HeightRequest = boxSize;
            box3.WidthRequest = boxSize;

            // scalling fonts relative to holder
            double baseTextSize = 25;
            titleText0.FontSize = 35;
            text1.FontSize = baseTextSize;
            text2.FontSize = baseTextSize;
            text3.FontSize = baseTextSize;
            text4.FontSize = baseTextSize;
            text5.FontSize = baseTextSize;
            text6.FontSize = baseTextSize;
            text7.FontSize = baseTextSize;
        }
    }

    private async void navigationControl(object sender, EventArgs e)
    {
        // this function handles navigation for all devices but phone
        Button castedObj = (Button)sender;
        Debug.Print(castedObj.Text);

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
        }
    }
}