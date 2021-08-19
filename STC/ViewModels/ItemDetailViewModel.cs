using System;
using Prism.Navigation;
using STC.Models;

namespace STC.ViewModels
{
    public class ItemDetailViewModel : BaseViewModel
    {
        public Item Item { get; set; }


        public ItemDetailViewModel(INavigationService navigationService) : base(navigationService)
        {

        }
    }
}
