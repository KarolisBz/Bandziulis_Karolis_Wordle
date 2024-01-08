using System.Diagnostics;

namespace Wordle_Karolis_G00417529;

public partial class SettingsPage : ContentPage
{
	public SettingsPage()
	{
		InitializeComponent();
        BindingContext = DataHandler.DataHandlerObject;
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

    private void OnWindowChange(object sender, EventArgs e)
    {
        scaleElements();
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

            pageTitle.TextColor = Colors.LightGray;
            lbl1.TextColor = Colors.LightGray;
            lbl2.TextColor = Colors.LightGray;
            lbl3.TextColor = Colors.LightGray;
            lbl4.TextColor = Colors.LightGray;

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

            pageTitle.TextColor = Colors.Black;
            lbl1.TextColor = Colors.Black;
            lbl2.TextColor = Colors.Black;
            lbl3.TextColor = Colors.Black;
            lbl4.TextColor = Colors.Black;

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

    private void scaleElements()
    {
        // this function handles scaling of ui elements
        // function varibles
        double pixelDensity = DeviceDisplay.MainDisplayInfo.Density;
        double windowHeight = this.Height / pixelDensity;
        double windowWidth = this.Width / pixelDensity;
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
        if(!isMobile)
        {
            pageTitle.FontSize = fontManager.scaleFontSize(180, windowHeight, windowWidth);
        }
        else
        {
            pageTitle.FontSize = fontManager.scaleFontSize(360, windowHeight, windowWidth);
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

                // scaling checkboxs
                double CheckboxSize = fontManager.scaleFontSize(25, relativeHeight, relativeWidth);

                // scaling slider
                slider1.WidthRequest = holder.WidthRequest * 0.5;

                // scalling fonts relative to holder
                double baseTextSize = fontManager.scaleFontSize(25, relativeHeight, relativeWidth);
                lbl1.FontSize = baseTextSize;
                lbl2.FontSize = baseTextSize;
                lbl3.FontSize = baseTextSize;
                lbl4.FontSize = baseTextSize;

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

                // scaling checkboxs

                // scaling slider
                slider1.WidthRequest = 250;

                // scalling fonts relative to holder
                double baseTextSize = 25;
                lbl1.FontSize = baseTextSize;
                lbl2.FontSize = baseTextSize;
                lbl3.FontSize = baseTextSize;
                lbl4.FontSize = baseTextSize;
            }
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

    private void checkBox2_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        // changing to new value
        checkDarkMode();
    }
}