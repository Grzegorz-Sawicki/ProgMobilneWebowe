using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CRUDAppService.MessageBox;
using CRUDAppService.Services.ProductService;
using CRUDAppService;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAUIAppCRUD.ViewModels
{
    public partial class ProductsViewModel : ObservableObject
    {
        private readonly IProductService _productService;
        private readonly IMessageDialogService _messageDialogService;


        [ObservableProperty]
        private ObservableCollection<Product> _products;


        [ObservableProperty]
        private Product _selectedProduct;

        public string SearchText { get; set; }

        public ProductsViewModel(IProductService productService, IMessageDialogService messageDialogService)
        {
            _productService = productService;
            _messageDialogService = messageDialogService;

            GetProductsAsync();
        }

        public async Task GetProductsAsync()
        {
            var result = await _productService.GetProductsAsync();
            if (result.Success)
            {
                Products = new ObservableCollection<Product>(result.Data);
            }
        }



        [RelayCommand]
        public async Task Search()
        {
            var result = await _productService.SearchProductsAsync(SearchText);
            if (result.Success)
            {
                Products = new ObservableCollection<Product>(result.Data);
            }
        }



        [RelayCommand]
        public async Task ShowDetails(Product product)
        {

            SelectedProduct = product;

            await Shell.Current.GoToAsync(nameof(ProductDetailsView), true, new Dictionary<string, object>
            {
                {"Product",product },
                {nameof(ProductsViewModel), this }
            });
        }

        [RelayCommand]
        public async Task New()
        {

            SelectedProduct = new Product();

            await Shell.Current.GoToAsync(nameof(ProductDetailsView), true, new Dictionary<string, object>
            {
                {"Product",SelectedProduct },
                {nameof(ProductsViewModel), this }
            });
        }


    }
}
