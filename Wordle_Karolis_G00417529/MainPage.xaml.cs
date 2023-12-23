
namespace Wordle_Karolis_G00417529
{
    public partial class MainPage : ContentPage
    {
        int count = 0;
        DataHandler data = new DataHandler(); // loads data once when program is opened

        public MainPage()
        {
            InitializeComponent();
        }

        private void OnCounterClicked(object sender, EventArgs e)
        {
            count++;

            if (count == 1)
                CounterBtn.Text = $"Clicked {count} time";
            else
                CounterBtn.Text = $"Clicked {count} times";

            SemanticScreenReader.Announce(CounterBtn.Text);
        }
    }
}