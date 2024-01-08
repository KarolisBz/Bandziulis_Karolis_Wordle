namespace Wordle_Karolis_G00417529
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = new AppShell();

            // chacing app
            DataHandler.appCache = this;
        }
    }
}