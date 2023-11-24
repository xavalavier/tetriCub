using Frac.Pages;

namespace Frac
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(GamePage), typeof(GamePage));
            Routing.RegisterRoute(nameof(EndGamePage), typeof(EndGamePage));
        }
    }
}