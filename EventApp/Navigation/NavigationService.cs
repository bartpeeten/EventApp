using EventApp.Views;
using Xamarin.Forms;

namespace EventApp.Navigation
{
    class NavigationService : INavigationService
    {
        public void NavigateTo(string pageKey)
        {
            var navigationPage = (NavigationPage)App.Current.MainPage;

            switch (pageKey)
            {
                case nameof(ScanPage):
                    App.Current.MainPage = new NavigationPage(new ScanPage());
                    break;
                case nameof(LoginPage):
                    App.Current.MainPage = new NavigationPage(new LoginPage());
                    break;
                case nameof(RegisterPage):
                    App.Current.MainPage = new NavigationPage(new LoginPage());
                    break;
            }
        }
    }
}
