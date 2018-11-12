using System;
using System.Collections.Generic;
using System.Text;

namespace EventApp.Navigation
{
    public interface INavigationService
    {
        void NavigateTo(string key);
    }
}
