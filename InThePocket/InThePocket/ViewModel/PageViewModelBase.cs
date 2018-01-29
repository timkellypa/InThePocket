using System;
using System.Collections.Generic;
using System.Text;

namespace InThePocket.ViewModel
{
    abstract public class PageViewModelBase : ViewModelBase
    {
        /// <summary>
        /// Default behavior of back button in page view model is to use the router to close the current page.
        /// Can be overridden per page.
        /// </summary>
        /// <returns>true, overridden function can return false if it wants (probably in the case that the back functionality is cancelled due to form validation, etc.).</returns>
        public virtual bool BackButtonPressed ()
        {
            NotifyPropertyChanged("ROUTE/Close");
            return true;
        }
    }
}
