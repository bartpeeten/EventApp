using EventApp.Domain;
using EventApp.Navigation;
using EventApp.Services;
using EventApp.Views;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Windows.Input;
using EventApp.Utility;
using Xamarin.Forms;
using Application = Xamarin.Forms.Application;

namespace EventApp.ViewModels
{
    public class LoginPageViewModel
    {
        private Users             _user;
        private ApiServices       _apiServices;
        private NavigationService _navigationService;

        public Users User
        {
            get => _user;
            set => _user = value;
        }

        // Constructor
        public LoginPageViewModel()
        {
            this._user        = new Users();
            this._apiServices = new ApiServices();
            this._navigationService = new NavigationService();
        }

        // Command for pressing the Register button
        public ICommand RegisterNewUser
        {
            get
            {
                return new Command(async () =>
                {
                    CreateNewUser();
                    _navigationService.NavigateTo(nameof(RegisterPage));
                });
            }
        }

        // Command for pressing the Login button
        public ICommand LoginUser
        {
            get
            {
                return new Command(CheckIfUserExist);
            }
        }

        private async void CheckIfUserExist()
        {
            HttpResponseMessage message = await _apiServices.UserExistsAsync(User);
            if (!(User.Email == null || User.LastName == null || User.FirstName == null || User.Email.Equals("") || User.LastName.Equals("") || User.FirstName.Equals("")))
            {
                HttpStatusCode statusCode = message.StatusCode;
                if (statusCode == HttpStatusCode.OK)
                {
                    LoggingIn(await message.Content.ReadAsStringAsync());
                }
            }
        }

        private void LoggingIn(string json)
        {
            User = JsonConvert.DeserializeObject<Users>(json);
            Application.Current.Properties["myUser"] = json;
            _navigationService.NavigateTo(nameof(ScanPage));
            Messenger.Default.Send<Users>(User);
        }

        private async void CreateNewUser()
        {

            if (!(User.Email == null || User.LastName == null || User.FirstName == null || User.Email.Equals("") || User.LastName.Equals("") || User.FirstName.Equals("")))
            {
                HttpResponseMessage message = await _apiServices.CreateNewUserAsync(User);
                HttpStatusCode statusCode = message.StatusCode;
                switch (statusCode)
                {
                    case HttpStatusCode.OK:
                        LoggingIn(await message.Content.ReadAsStringAsync());
                        break;
                    case HttpStatusCode.NotFound:
                        break;
                    default:
                        break;
                }

            }
        }

    }
}
