using System.Diagnostics;
using System.Text.Json;

namespace Wordle_Karolis_G00417529
{
    public partial class MainPage : ContentPage
    {
        int count = 0;
        DataHandler data = new DataHandler(); // setup data ONCE, when program is opened

        public MainPage()
        {
            InitializeComponent();
        }

        private void setupUI()
        {
            // hooking function to every time layout is changed
            this.LayoutChanged += OnWindowChange;

            // changing background if it's a mobile device, as they have a different screen ratio
            if (DeviceInfo.Current.Idiom == DeviceIdiom.Phone)
            {
                background.Source = "mobilebackground.png";
            }
        }

        private void OnWindowChange(object sender, EventArgs e)
        {
            // function varibles
            double windowHeight = this.Height;
            double windowWidth = this.Width;
            double pixelDensity = DeviceDisplay.MainDisplayInfo.Density;

            // window size has changed, so we scale all instances to fit
            background.HeightRequest = windowHeight * pixelDensity;
            background.WidthRequest = windowWidth * pixelDensity;
        }

        private async void toPage2(object sender, EventArgs e)
        {
            string name = await DisplayPromptAsync("Login", "What's your username?");
            if (name != null && name != "") // if the user doesn't cancel and enters a valid name, push them onto the gamepage
            {
                await Navigation.PopAsync();
                await Navigation.PushAsync(new gamePage()); // going to new page
            }
        }

        async private void saveTest()
        {
            await DataHandler.saveDataAsync();
        }
    }
}