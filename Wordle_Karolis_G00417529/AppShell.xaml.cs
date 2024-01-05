using System.Diagnostics;

namespace Wordle_Karolis_G00417529
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            // disable flyout if on pc
            if (DeviceInfo.Current.Idiom == DeviceIdiom.Desktop)
            {
                Shell.SetFlyoutBehavior(this, Microsoft.Maui.FlyoutBehavior.Disabled);
                Shell.SetFlyoutItemIsVisible(this, false);
            }
        }

        protected override async void OnNavigating(ShellNavigatingEventArgs args)
        {
            // this function prompts mobile players if they want to move pages and loose progress
            // code fetched from https://learn.microsoft.com/en-us/dotnet/maui/fundamentals/shell/navigation?view=net-maui-8.0
            base.OnNavigating(args);

            if (DataHandler.isInGamePage)
            {
                ShellNavigatingDeferral token = args.GetDeferral();

                bool result = await DisplayAlert("Do you want to leave the game?", "If you leave now, your progress will not be saved", "Yes", "No");
                if (!result)
                {
                    args.Cancel();
                }
                DataHandler.isInGamePage = false;
                token.Complete();
            }
        }
    }
}