using System;
using System.Collections.Generic;
using System.Text;
using EventApp.Navigation;
using EventApp.Services;
using EventApp.ViewModels;

namespace EventApp
{
    public class ViewModelLocator
    {
        private readonly INavigationService _navigationService;
        private readonly ApiServices _repository;
        private static ViewModelLocator _instance;

        public static ViewModelLocator Instance => _instance ?? (_instance = new ViewModelLocator());

        public LoginPageViewModel LoginViewModel { get; set; }
        public ScanPageViewModel ScanViewModel { get; set; }


        public ViewModelLocator()
        {
            _navigationService = new NavigationService();
            _repository = new ApiServices();

            LoginViewModel = new LoginPageViewModel();
            ScanViewModel = new ScanPageViewModel(_navigationService, _repository);

        }
    }
}
