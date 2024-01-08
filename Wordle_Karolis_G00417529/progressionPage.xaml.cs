using System.Diagnostics;

namespace Wordle_Karolis_G00417529;

public partial class progressionPage : ContentPage
{
    // class Fields
    List<Grid> generatedGrids = new List<Grid>();

    public progressionPage()
	{
		InitializeComponent();
        this.Resources["dynamicTextColour"] = Colors.Black; // setting text color
        BindingContext = DataHandler.cachedProgressViewModel;
        setupUI(); // setting up ui
    }

    private void checkDarkMode()
    {
        // this function handles colour changing between light and dark mode
        if (DataHandler.DataHandlerObject.DarkMode)
        {
            // setting to dark mode
            BackgroundColor = Colors.Black;
            background.Opacity = 0.1;
            holder.Color = new Color(77, 77, 77);
            holderShadow.Color = new Color(43, 43, 43);
            pageTitle.TextColor = Colors.LightGray;
            this.Resources["dynamicTextColour"] = Colors.LightGray;

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
            // setting to light mode
            BackgroundColor = Colors.White;
            background.Opacity = 1;
            holder.Color = new Color(255, 255, 255);
            holderShadow.Color = new Color(0, 0, 0);
            pageTitle.TextColor = Colors.Black;
            this.Resources["dynamicTextColour"] = Colors.Black;

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

            holder.Opacity = 0.25;
            holderShadow.Opacity = 0.25;
            holder.CornerRadius = 5;
            holderShadow.CornerRadius = 0;
        }

        checkDarkMode();
    }

    protected override void OnAppearing()
    {
        // handles dark mode switching
        base.OnAppearing();
        checkDarkMode();
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
        double mobileExtra = 0, boarderSize = 10;
        bool isMobile = false;

        // mobile devices don't have an accurate window, so we use full screen scale for more accurate scaling
        if (DeviceInfo.Current.Idiom == DeviceIdiom.Phone)
        {
            windowHeight = DeviceDisplay.MainDisplayInfo.Height / pixelDensity;
            windowWidth = DeviceDisplay.MainDisplayInfo.Width / pixelDensity;
            mobileExtra = 10;
            boarderSize = 5;
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
            pageTitle.FontSize = fontManager.scaleFontSize(450, windowHeight, windowWidth); 
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

        // scaling scrolling page (from ratio of 2k monitor)
        if (windowWidth < 630) // we only scale if screen is smaller then requested size
        {
            holder.WidthRequest = windowWidth - (boarderSize * 2);
            holder.HeightRequest = windowHeight - (pageTitle.FontSize * 1.1) - mobileExtra;
            scroller.HeightRequest = windowHeight - (pageTitle.FontSize * 1.1) - mobileExtra;

            holderShadow.HeightRequest = holder.HeightRequest + boarderSize;
            holderShadow.WidthRequest = holder.WidthRequest + (boarderSize * 2);

            // scaling data template fonts relative to box width
            this.Resources["templateFontSize"] = 20 * (windowWidth / 630);
        }
        else // return to original size
        {
            holder.HeightRequest = windowHeight - (pageTitle.FontSize * 1.1) - mobileExtra;
            scroller.HeightRequest = windowHeight - (pageTitle.FontSize * 1.1) - mobileExtra;
            holder.WidthRequest = 630;
            holderShadow.HeightRequest = holder.HeightRequest + boarderSize;
            holderShadow.WidthRequest = holder.WidthRequest + (boarderSize * 2);
            this.Resources["templateFontSize"] = 20;
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
            case "How to play":
                await Navigation.PushAsync(new howToPlayPage());
                break;
        }
    }
}