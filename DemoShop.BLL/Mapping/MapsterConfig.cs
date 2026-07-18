using DemoShop.DAL.DTO.Request;
using DemoShop.DAL.DTO.Response;
using DemoShop.DAL.Models;
using Mapster;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace DemoShop.BLL.Mapping
{
    public static class MapsterConfig
    {
        public static void RegisterMapsterConfiguration()
        {
            // Syntax: TypeAdapterConfig<Source, Destination>
            // Category Mapster Configuration
            TypeAdapterConfig<Category, CategoryResponse>.NewConfig()
                .Map(dest => dest.CategoryId, src => src.Id)
                // Flattening: Entity.NavigationProperty.Property -> DTO.Property
                .Map(dest => dest.UserCreated, src => src.CreatedBy.UserName)
                .Map(dest => dest.CategoryName, src => src.Translations.Where(
                    land => land.Language == CultureInfo.CurrentCulture.Name)
                    .Select(T => T.Name).FirstOrDefault()
                );

            // Product Mapster Configuration
            TypeAdapterConfig<Product, ProductResponse>.NewConfig()
                .Map(dest => dest.UserCreated, src => src.CreatedBy.UserName)
                .Map(dest => dest.Name, src => src.Translations.Where(
                    lang => lang.Language == CultureInfo.CurrentCulture.Name)
                    .Select(T => T.Name).FirstOrDefault()
                )
                .Map(dest => dest.MainImage, src => $"https://localhost:7043/Images/{src.MainImage}")
                .Map(dest => dest.SubImages, src => src.Images.Select(i => $"https://localhost:7043/Images/{i.ImagePath}"));
            
            // Product Update Mapster Configuration
            TypeAdapterConfig<ProductUpdateRequest, Product>.NewConfig()
                .IgnoreNullValues(true); 

            
            // Brand Mapster Configuration
            TypeAdapterConfig<Brand, BrandResponse>.NewConfig()
                .Map(dest => dest.UserCreated, src => src.CreatedBy.UserName)
                .Map(dest => dest.Name, src => src.Translations.Where(
                    lang => lang.Language == CultureInfo.CurrentCulture.Name)
                    .Select(T => T.Name).FirstOrDefault()
                )
                .Map(dest => dest.MainLogo, src => $"https://localhost:7043/Images/{src.MainLogo}");

            // Cart Mapster Configuration
            TypeAdapterConfig<Cart, CartResponse>.NewConfig()
                .Map(dest => dest.ProductName, src => src.Product.Translations.Where(
                    lang => lang.Language == CultureInfo.CurrentCulture.Name)
                    .Select(T => T.Name).FirstOrDefault()
                )
                .Map(dest => dest.Price, src => src.Product.Price)
                .Map(dest => dest.Discount, src => src.Product.Discount)
                .Map(dest => dest.ProductImg, src => $"https://localhost:7043/Images/{src.Product.MainImage}");

            // Order Mapster Configuration
            TypeAdapterConfig<Order, OrderDetailResponse>.NewConfig()
                .Map(dest => dest.OrderItems, src => src.Orderltems);

            TypeAdapterConfig<Orderltem, OrderItemResponse>.NewConfig()
                .Map(dest => dest.ProductName, src => src.Product.Translations.Where(
                    lang => lang.Language == CultureInfo.CurrentCulture.Name)
                    .Select(T => T.Name).FirstOrDefault()
                );
        }
    }
}
