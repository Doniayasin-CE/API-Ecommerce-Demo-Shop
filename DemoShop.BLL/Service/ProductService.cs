using DemoShop.DAL.DTO.Request;
using DemoShop.DAL.DTO.Response;
using DemoShop.DAL.Models;
using DemoShop.DAL.Repository;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace DemoShop.BLL.Service
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IFileService _fileService;

        public ProductService(IProductRepository productRepository, IFileService fileService)
        {
            _productRepository = productRepository;
            _fileService = fileService;
        }

        public async Task CreateProduct(ProductRequest request)
        {
            var product = request.Adapt<Product>();
            if(request.MainImage != null)
            {
                var imageName = await _fileService.UploadAsync(request.MainImage);
                product.MainImage = imageName!;
            }
            await _productRepository.CreateAsync(product);
        }

        public async Task<List<ProductResponse>> GetAllProducts()
        {
            var products = await _productRepository.GetAllAsync(
                new string[]
                {
                    nameof(Product.Translations),
                    nameof(Product.CreatedBy)
                }
            );
            return products.Adapt<List<ProductResponse>>();
        }

        public async Task<ProductResponse?> GetProduct(Expression<Func<Product, bool>> filter)
        {
            var product = await _productRepository.GetOneAsync(filter, new string[]
                {
                    nameof(Product.Translations),
                    nameof(Product.CreatedBy)
                }
            );
            if(product is null)
                return null;
            return product.Adapt<ProductResponse>();
        }

        public async Task<bool> DeleteProduct(int id)
        {
            var product = await _productRepository.GetOneAsync(c => c.Id == id);
            if (product == null) return false;
            _fileService.DeleteAsync(product.MainImage);
            return await _productRepository.DeleteAsync(product);
        }
    }
}
