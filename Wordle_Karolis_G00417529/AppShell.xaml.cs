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
    }
}