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
    public class BrandService : IBrandService
    {
        private readonly IBrandRepository _brandRepository;
        private readonly IFileService _fileService;
        public BrandService(IBrandRepository brandRepository, IFileService fileService)
        {
            _brandRepository = brandRepository;
            _fileService = fileService;
        }

        public async Task CreateBrand(BrandRequest request)
        {
            var brand = request.Adapt<Brand>();
            if(request.MainLogo != null)
            {
                var logoName = await _fileService.UploadAsync(request.MainLogo);
                brand.MainLogo = logoName!;
            }
            await _brandRepository.CreateAsync(brand);
        }

        public async Task<List<BrandResponse>> GetAllBrands()
        {
            var brands = await _brandRepository.GetAllAsync(
                b => b.Status == EntityStatus.Active,
                new string[]
                {
                    nameof(Brand.Translations),
                    nameof(Brand.CreatedBy)
                }
            );
            return brands.Adapt<List<BrandResponse>>();
        }

        public async Task<BrandResponse?> GetBrand(Expression<Func<Brand, bool>> filter)
        {
            var brand = await _brandRepository.GetOneAsync(filter, 
                new string[] 
                {
                    nameof(Brand.Translations),
                    nameof(Brand.CreatedBy)
                }
            );
            if(brand is null)
                return null;
            return brand.Adapt<BrandResponse>();
        }

        public async Task<bool> DeleteBrand(int id)
        {
            var brand = await _brandRepository.GetOneAsync(b => b.Id == id);
            if(brand is null) return false;
            _fileService.DeleteAsync(brand.MainLogo);
            return await _brandRepository.DeleteAsync(brand);
        }

        public async Task<bool> ToggleStatus(int id)
        {
            var brand = await _brandRepository.GetOneAsync(p => p.Id == id);
            if (brand == null) return false;

            brand.Status = brand.Status == EntityStatus.Active
                ? EntityStatus.InActive
                : EntityStatus.Active;

            return await _brandRepository.UpdateAsync(brand);
        }
    }
}
