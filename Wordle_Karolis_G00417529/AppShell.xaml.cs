namespace Wordle_Karolis_G00417529
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            // enable tabs if on phone
             Shell.SetTabBarIsVisible(this, DeviceInfo.Current.Idiom == DeviceIdiom.Phone);
        }
    }
}