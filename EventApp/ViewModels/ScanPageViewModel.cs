using EventApp.Domain;
using EventApp.Navigation;
using EventApp.Services;
using EventApp.Views;
using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Net.Http;
using System.Windows.Input;
using EventApp.Utility;
using Xamarin.Forms;

namespace EventApp.ViewModels
{
    public class ScanPageViewModel : INotifyPropertyChanged
    {
        private INavigationService _navigationService;
        private ApiServices        _repository;
        public CustomCommand       QRScanResultCommand { get; set; }
        public CustomCommand       SwitchUserCommand   { get; set; }


        public ScanPageViewModel(INavigationService navigationService, ApiServices repository)
        {
            _navigationService = navigationService;
            _repository        = repository;
            Messenger.Default.Register<Users>(this, OnUserReceived);
            LoadCommands();
            ResetScanner();
        }



        private void OnUserReceived(Users myUser)
        {
            User = myUser;
        }

        private Users _user;
        public Users User
        {
            get
            {
                return _user;
            }
            set
            {
                _user = value;
                RaisePropertyChanged(nameof(User));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public ZXing.Result Result { get; set; }

        private bool _isAnalyzing = true;
        public bool IsAnalyzing
        {
            get { return _isAnalyzing; }
            set
            {
                if (!bool.Equals(_isAnalyzing, value))
                {
                    _isAnalyzing = value;
                    RaisePropertyChanged(nameof(IsAnalyzing));
                }
            }
        }

        private bool _isScanning = true;
        public bool IsScanning
        {
            get { return _isScanning; }
            set
            {
                if (!bool.Equals(_isScanning, value))
                {
                    _isScanning = value;
                    RaisePropertyChanged(nameof(IsScanning));
                }
            }
        }

        private void LoadCommands()
        {
            QRScanResultCommand = new CustomCommand((obj) =>
            {
                QRScanResult();
            }, null);

            SwitchUserCommand = new CustomCommand((obj) =>
            {
                BackToLoginScreen();
            }, null);
        }

        private void BackToLoginScreen()
        {
            Application.Current.Properties.Clear();
            _navigationService.NavigateTo(nameof(LoginPage));

        }

        private void QRScanResult()
        {
            IsAnalyzing = false;
            IsScanning = false;

            Device.BeginInvokeOnMainThread(async () =>
            {
                Participations participation = new Participations();
                participation.UserId = _user.UserId;
                participation.EventId = Int32.Parse(Result.Text);
                HttpResponseMessage httpResponseMessage = await _repository.CreateNewParticipationAsync(participation);
                switch (httpResponseMessage.StatusCode)
                {
                    case System.Net.HttpStatusCode.OK:
                        Events myEvent = JsonConvert.DeserializeObject<Events>(await httpResponseMessage.Content.ReadAsStringAsync());

                        Device.BeginInvokeOnMainThread(() =>
                        {
                            App.Current.MainPage.DisplayAlert("You are registrated!", "Thank you to participate to: " + myEvent.Description, "OK");
                        });
                        break;
                    case System.Net.HttpStatusCode.NotFound:
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            App.Current.MainPage.DisplayAlert("Event was not found!", "Please try again", "OK");
                        });
                        break;
                    default:
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            App.Current.MainPage.DisplayAlert("Something went wrong!", "Please try again", "OK");
                        });
                        break;
                }
                ResetScanner();


            });
        }

        private void ResetScanner()
        {
            IsAnalyzing = true;
            IsScanning = true;
        }
    }
}
