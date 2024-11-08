using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUDAppService.Services.ProductService
{
    public class ProductService : IProductService
    {
        public Task<ServiceReponse<Product>> CreateProductAsync(Product newProduct)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceReponse<bool>> DeleteProductAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceReponse<Product>> GetProductAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceReponse<List<Product>>> GetProductsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ServiceReponse<List<Product>>> SearchProductsAsync(string text, int page, int pageSize)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceReponse<Product>> UpdateProductAsync(Product updatedProduct)
        {
            throw new NotImplementedException();
        }
    }
}
