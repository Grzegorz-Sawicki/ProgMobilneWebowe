using CRUDAppService.Services.ProductService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CRUDAppService.Services.ProductService
{
    public class ProductService : IProductService
    {
        private readonly IFileService _fileService;
        private const string _fileName = "products.json";

        public ProductService(IFileService fileService)
        {
            _fileService = fileService;
        }

        private async Task<List<Product>> LoadProductsAsync()
        {
            var json = await _fileService.ReadFileAsync(_fileName);
            return string.IsNullOrEmpty(json)
                ? new List<Product>()
                : JsonSerializer.Deserialize<List<Product>>(json) ?? new List<Product>();
        }

        private async Task SaveProductsAsync(List<Product> products)
        {
            var json = JsonSerializer.Serialize(products);
            await _fileService.WriteFileAsync(_fileName, json);
        }

        public async Task<ServiceResponse<List<Product>>> GetProductsAsync()
        {
            var products = await LoadProductsAsync();
            return new ServiceResponse<List<Product>> { Data = products, Success = true };
        }

        public async Task<ServiceResponse<Product>> CreateProductAsync(Product newProduct)
        {
            var products = await LoadProductsAsync();
            newProduct.Id = products.Any() ? products.Max(p => p.Id) + 1 : 1;
            products.Add(newProduct);
            await SaveProductsAsync(products);

            return new ServiceResponse<Product> { Data = newProduct, Success = true, Message = "Product added!" };
        }

        public async Task<ServiceResponse<bool>> DeleteProductAsync(int id)
        {
            var products = await LoadProductsAsync();
            var product = products.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return new ServiceResponse<bool> { Data = false, Success = false, Message = "Product not found" };
            }

            products.Remove(product);
            await SaveProductsAsync(products);

            return new ServiceResponse<bool> { Data = true, Success = true };
        }

        public async Task<ServiceResponse<Product>> UpdateProductAsync(Product updatedProduct)
        {
            var products = await LoadProductsAsync();
            var product = products.FirstOrDefault(p => p.Id == updatedProduct.Id);
            if (product == null)
            {
                return new ServiceResponse<Product> { Data = null, Success = false, Message = "Product not found" };
            }

            product.Title = updatedProduct.Title;
            product.Description = updatedProduct.Description;
            product.Price = updatedProduct.Price;
            product.Barcode = updatedProduct.Barcode;
            product.ReleaseDate = updatedProduct.ReleaseDate;

            await SaveProductsAsync(products);

            return new ServiceResponse<Product> { Data = product, Success = true };
        }

        public async Task<ServiceResponse<Product>> GetProductAsync(int id)
        {
            var products = await LoadProductsAsync();
            var product = products.FirstOrDefault(p => p.Id == id);

            if (product == null)
            {
                return new ServiceResponse<Product> { Data = null, Success = false, Message = "Product not found" };
            }

            return new ServiceResponse<Product> { Data = product, Success = true };
        }

        public async Task<ServiceResponse<List<Product>>> SearchProductsAsync(string text)
        {
            var products = await LoadProductsAsync();
            if (text == null || text == "") return new ServiceResponse<List<Product>> { Data = products, Success = true };

            var filteredProducts = products
                .Where(p => (p.Title != null && p.Title.Contains(text, StringComparison.OrdinalIgnoreCase)) ||
                (p.Description != null && p.Description.Contains(text, StringComparison.OrdinalIgnoreCase)))
                .ToList();

            return new ServiceResponse<List<Product>> { Data = filteredProducts, Success = true };
        }
    }
}
