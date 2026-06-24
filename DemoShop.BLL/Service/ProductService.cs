using DemoShop.BLL.Extensions;
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
            if(request.SubImages != null)
            {
                foreach(var img in request.SubImages)
                {
                    var imagePath = await _fileService.UploadAsync(img);
                    product.Images.Add(new ProductImage { ImagePath = imagePath! });
                }
            }
            await _productRepository.CreateAsync(product);
        }

        public async Task<PaginationResponse<ProductResponse>> GetAllProducts(PaginationRequest request)
        {
            var query =  _productRepository.GetQueryable(
                p => p.Status == EntityStatus.Active,
                new string[]
                {
                    nameof(Product.Translations),
                    nameof(Product.CreatedBy),
                    nameof(Product.Images)
                }
            );

            var paginated = await query.ToPaginationAsync(request.Page, request.Limit);
            return new PaginationResponse<ProductResponse>
            {
                Data = paginated.Data.Adapt<List<ProductResponse>>(),
                Page = paginated.Page,
                Limit = paginated.Limit,
                TotalCount = paginated.TotalCount,
            };
        }

        //public async Task<List<ProductResponse>> GetAllProducts()
        //{
        //    var products = await _productRepository.GetAllAsync(
        //        p => p.Status == EntityStatus.Active,
        //        new string[]
        //        {
        //            nameof(Product.Translations),
        //            nameof(Product.CreatedBy),
        //            nameof(Product.Images)
        //        }
        //    );
        //    return products.Adapt<List<ProductResponse>>();
        //}

        public async Task<ProductResponse?> GetProduct(Expression<Func<Product, bool>> filter)
        {
            var product = await _productRepository.GetOneAsync(filter, new string[]
                {
                    nameof(Product.Translations),
                    nameof(Product.CreatedBy),
                    nameof(Product.Images)
                }
            );
            if(product is null)
                return null;
            return product.Adapt<ProductResponse>();
        }

        public async Task<bool> DeleteProduct(int id)
        {
            var product = await _productRepository.GetOneAsync(c => c.Id == id,
                new string[]
                {
                    nameof(Product.Images)
                });

            if (product == null) return false;
            _fileService.DeleteAsync(product.MainImage);

            foreach(var image in product.Images)
            {
                _fileService.DeleteAsync(image.ImagePath);
            }
            return await _productRepository.DeleteAsync(product);
        }

        public async Task<bool> UpdateProduct(int id, ProductUpdateRequest request)
        {
            var existingProduct = await _productRepository.GetOneAsync(
                p => p.Id == id,
                new string[] {
                    nameof(Product.Translations),
                    nameof(Product.Images)
                }
            );
            if(existingProduct == null) return false;

            var oldImage = existingProduct.MainImage;
            request.Adapt(existingProduct);

            if(request.Translations != null)
            {
                foreach(var DTO in request.Translations)
                {
                    var existingTranslation = existingProduct.Translations
                        .FirstOrDefault(t=>t.Language == DTO.Language);
                    if(existingTranslation != null)
                    {
                        if(DTO.Name != null)
                            existingTranslation.Name = DTO.Name;
                        //existingTranslation.Name = existingProduct.Translations.Select(t => t.Name).FirstOrDefault()!;
                        if(DTO.Description != null)
                            existingTranslation.Description = DTO.Description;
                        //existingTranslation.Description = existingProduct.Translations.Select(t => t.Description).FirstOrDefault()!;
                    }
                    else
                    {
                        return false; //to prevent adding a new languag
                    }
                }
            }

            if (request.MainImage != null)
            {
                _fileService.DeleteAsync(oldImage);
                existingProduct.MainImage = await _fileService.UploadAsync(request.MainImage);
            }
            else
            {
                existingProduct.MainImage = oldImage;
            }

            if(request.SubImages != null)
            {
                //delete old SubImages
                foreach (var image  in existingProduct.Images)
                {
                    _fileService.DeleteAsync(image.ImagePath);
                }
                existingProduct.Images.Clear();
                //add new SubImages
                foreach(var image in request.SubImages)
                {
                    var imagePath = await _fileService.UploadAsync(image);
                    existingProduct.Images.Add(new ProductImage { ImagePath = imagePath! });
                }
            }

            if(request.NewImages != null)
            {
                //add new SubImages
                foreach (var image in request.NewImages)
                {
                    var imagePath = await _fileService.UploadAsync(image);
                    existingProduct.Images.Add(new ProductImage { ImagePath = imagePath! });
                }
            }
            return await _productRepository.UpdateAsync(existingProduct);
        }

        public async Task<bool> ToggleStatus(int id)
        {
            var product = await _productRepository.GetOneAsync(p => p.Id == id);
            if(product == null) return false;

            product.Status = product.Status == EntityStatus.Active 
                ? EntityStatus.InActive
                : EntityStatus.Active;

            return await _productRepository.UpdateAsync(product);
        }
    }
}
