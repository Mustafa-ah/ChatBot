using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

using Xamarin.Forms;

using STC.Models;
using STC.Services;
using Prism.Mvvm;
using Prism.Navigation;

namespace STC.ViewModels
{
    public class BaseViewModel : BindableBase, INavigationAware
    {

        string _title;
        bool _isBusy;
        int _rtl;
        string _busyMessage;

        public INavigationService NavigationService  { get; set; }


        public BaseViewModel(INavigationService navigationService)
        {
            NavigationService = navigationService;
        }

        //https://www.c-sharpcorner.com/article/xamarin-forms-getting-starting-with-prism/
        public IDataStore<Item> DataStore => DependencyService.Get<IDataStore<Item>>();


        string title = string.Empty;
        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }

        public bool IsBusy
        {
            get => this._isBusy;
            set => this.SetProperty(ref this._isBusy, value);
        }
        public string BusyMessage
        {
            get => this._busyMessage;
            set => this.SetProperty(ref this._busyMessage, value);
        }
        public int Rtl
        {
            get => this._rtl;
            set => this.SetProperty(ref this._rtl, value);
        }
        //protected bool SetProperty<T>(ref T backingStore, T value,
        //    [CallerMemberName] string propertyName = "",
        //    Action onChanged = null)
        //{
        //    if (EqualityComparer<T>.Default.Equals(backingStore, value))
        //        return false;

        //    backingStore = value;
        //    onChanged?.Invoke();
        //    OnPropertyChanged(propertyName);
        //    return true;
        //}

        //#region INotifyPropertyChanged
        //public event PropertyChangedEventHandler PropertyChanged;
        //protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        //{
        //    var changed = PropertyChanged;
        //    if (changed == null)
        //        return;

        //    changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        //}


        //#endregion

        public void HideLoading()
        {
            this.IsBusy = false;
            this.BusyMessage = null;
        }
        public void ShowLoading(string message = null)
        {
            this.IsBusy = true;
            this.BusyMessage = message;
        }

        public void OnNavigatedFrom(INavigationParameters parameters)
        {
            //throw new NotImplementedException();
        }

        public void OnNavigatedTo(INavigationParameters parameters)
        {
            //throw new NotImplementedException();
        }
    }
}
