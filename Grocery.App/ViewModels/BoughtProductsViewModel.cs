using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Grocery.Core.Interfaces.Services;
using Grocery.Core.Models;
using System;
using System.Collections.ObjectModel;


namespace Grocery.App.ViewModels
{
    public partial class BoughtProductsViewModel : BaseViewModel
    {
        private readonly IBoughtProductsService _boughtProductsService;
        private readonly GlobalViewModel _global;

        [ObservableProperty]
        Product selectedProduct;
        public ObservableCollection<BoughtProducts> BoughtProductsList { get; set; } = [];
        public ObservableCollection<Product> Products { get; set; }

        public BoughtProductsViewModel(IBoughtProductsService boughtProductsService, IProductService productService, GlobalViewModel global)
        {
            _boughtProductsService = boughtProductsService;
            Products = new(productService.GetAll());
            _global = global;
        }

        partial void OnSelectedProductChanged(Product? oldValue, Product newValue)
        {
            BoughtProductsList.Clear();

            if (newValue == null)
                return;

            List<BoughtProducts> boughtProducts = _boughtProductsService.Get(newValue.Id);

            foreach (BoughtProducts item in boughtProducts)
                BoughtProductsList.Add(item);
        }

        public void ShowBoughtProducts()
        {
            Client currentClient = _global.Client;
            if (currentClient.Role == Role.Admin)
                Shell.Current.GoToAsync(nameof(Views.BoughtProductsView));
            else
                throw new InvalidNavigationException();
        }

        [RelayCommand]
        public void NewSelectedProduct(Product product)
        {
            SelectedProduct = product;
        }
    }
}
