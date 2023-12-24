﻿using System.Diagnostics;
using System.Text.Json;

namespace Wordle_Karolis_G00417529
{
    public partial class MainPage : ContentPage
    {
        bool loggedIn = false;
        DataHandler data = new DataHandler(); // setup data ONCE, when program is opened

        public MainPage()
        {
            InitializeComponent();
            setupUI(); // setting up ui
            swapPageContent(); // making sure we display the right content
        }

        private void swapPageContent()
        {
            // swaps page content depending if logged in or not
            // checking if user is not logged in
            if (DataHandler.currentPlayer == "Default_User")
            {
                userDisplay.Text = "You're currently logged out";
                actionDisplay.Text = "Press login to login!";
                startBtn.Text = "Login";
            }
            else
            {
                userDisplay.Text = ("Welcome back " + DataHandler.currentPlayer + "!");
                actionDisplay.Text = "Press start to play!";
                startBtn.Text = "Start";
                loggedIn = true;
            }
        }
        private void setupUI()
        {
            // hooking function to every time layout is changed
            this.LayoutChanged += OnWindowChange;

            // changing background more suited for mobile
            if (DeviceInfo.Current.Idiom == DeviceIdiom.Phone)
            {
                background.Source = "mobilebackground.png";
                holder.Opacity = 0.25;
                holderShadow.Opacity = 0.25;
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

            // window size has changed, so we scale all instances to fit
            background.HeightRequest = windowHeight / pixelDensity;
            background.WidthRequest = windowWidth / pixelDensity;

            // scalling fonts
            fontPageTitle.FontSize = scaleFontSize(180, windowHeight, windowWidth);

            // scaling assests (from ratio of 2k monitor)
            if (windowWidth < 500 || windowHeight < 700) // we only scale if screen is smaller then requested size
            {
                // scaling fonts relative to box, not screen
                double relativeWidth = 2560 * (windowWidth / 500);
                double relativeHeight = 1408 * (windowHeight / 700);

                if (windowWidth < 500) { holder.WidthRequest = windowWidth; } else {  holder.WidthRequest = 500; relativeWidth = 2560; }
                if (windowHeight < 500) { holder.HeightRequest = windowHeight; } else { holder.HeightRequest = 700; relativeHeight = 1408; }

                startBtn.WidthRequest = holder.WidthRequest / 2;
                holderShadow.HeightRequest = holder.HeightRequest + 10;
                holderShadow.WidthRequest = holder.WidthRequest + 10;

                startBtn.FontSize = scaleFontSize(75, relativeHeight, relativeWidth);
                actionDisplay.FontSize = scaleFontSize(50, relativeHeight, relativeWidth);
                userDisplay.FontSize = scaleFontSize(50, relativeHeight, relativeWidth);
                subTitle.FontSize = scaleFontSize(100, relativeHeight, relativeWidth);
            }
            else // return to original size
            {
                holder.HeightRequest = 700;
                holder.WidthRequest = 500;
                startBtn.WidthRequest = holder.WidthRequest / 2;
                holderShadow.HeightRequest = holder.HeightRequest + 10;
                holderShadow.WidthRequest = holder.WidthRequest + 10;
                // scaling fonts
                startBtn.FontSize = 75;
                actionDisplay.FontSize = 50;
                userDisplay.FontSize = 50;
                subTitle.FontSize = 100;
            }

        }

        private double scaleFontSize(double fullSize, double windowHeight, double windowWidth)
        {
            // function varibles 
            double pixelDensity = DeviceDisplay.MainDisplayInfo.Density;

            // originally made at 2560p x 1440p, so we scale to that (minus the task bar ect)
            double scaledHeight = (windowHeight / pixelDensity) / 1408;
            double scaledWidth = (windowWidth / pixelDensity) / 2560;
            double scaledFontSize = ((scaledHeight + scaledWidth) / 2) * fullSize;

            return scaledFontSize;
        }

        private async void login()
        {
            string name = await DisplayPromptAsync("Login", "What's your username?");
            if (name != null && name != "") // if the user doesn't cancel and enters a valid name, swap content
            {
                DataHandler.currentPlayer = name;
                saveData(); // saves new data now that user logged in
                swapPageContent(); // swaps to logged in page
                loggedIn = true;
            }
        }

        private async void toPage2()
        {
            await Navigation.PopAsync();
            await Navigation.PushAsync(new gamePage()); // going to new page
        }

        private void startBtn_Clicked(object sender, EventArgs e)
        {
            if (loggedIn)
            {
                toPage2(); // directs to new page
            }
            else
            {
                login(); // prompts login
            }
        }

        async private void saveData()
        {
            await DataHandler.saveDataAsync();
        }
    }
}