﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CRUDAppService.MessageBox;
using CRUDAppService.Services.ProductService;
using CRUDAppService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAUIAppCRUD.ViewModels
{
    [QueryProperty(nameof(Product), nameof(Product))]
    [QueryProperty(nameof(ProductsViewModel), nameof(ProductsViewModel))]
    public partial class ProductDetailsViewModel : ObservableObject
    {
        private readonly IProductService _productService;
        private readonly IMessageDialogService _messageDialogService;
        private ProductsViewModel _productsViewModel;
        public ProductDetailsViewModel(IProductService productService, IMessageDialogService messageDialogService)
        {
            _productService = productService;
            _messageDialogService = messageDialogService;
        }

        [ObservableProperty]
        private Product product;

        public ProductsViewModel ProductsViewModel
        {
            get { return _productsViewModel; }
            set { _productsViewModel = value; }
        }

        [RelayCommand]
        public async Task Delete()
        {
            await DeleteProduct();
            await Shell.Current.GoToAsync("../", true);
        }

        public async Task DeleteProduct()
        {
            var result = await _productService.DeleteProductAsync(product.Id);
            if (result.Success)
            {
                await _productsViewModel.GetProductsAsync();
            }
            else
            {
                _messageDialogService.ShowMessage(result.Message);
            }

        }


        [RelayCommand]
        public async Task Save()
        {
            if (product.Id == 0)
            {
                // tworzymy nowy produkt 
                await CreateProductAsync();

            }
            else
            {
                // aktualizujemy produkt
                await UpdateProductAsync();
            }

            await Shell.Current.GoToAsync("../", true);
        }

        public async Task CreateProductAsync()
        {
            var result = await _productService.CreateProductAsync(product);
            if (result.Success)
            {
                await _productsViewModel.GetProductsAsync();
            }
            else
            {
                _messageDialogService.ShowMessage(result.Message);
            }

        }



        public async Task UpdateProductAsync()
        {
            var result = await _productService.UpdateProductAsync(product);
            if (result.Success)
            {
                await _productsViewModel.GetProductsAsync();
            }
            else
            {
                _messageDialogService.ShowMessage(result.Message);
            }
        }
    }
}
