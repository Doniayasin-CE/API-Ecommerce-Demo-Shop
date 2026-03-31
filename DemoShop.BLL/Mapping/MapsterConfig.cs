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
            TypeAdapterConfig<Category, CategoryResponse>.NewConfig()
                .Map(dest => dest.CategoryId, src => src.Id)
                // Flattening: Entity.NavigationProperty.Property -> DTO.Property
                .Map(dest => dest.UserCreated, src => src.CreatedBy.UserName)
                .Map(dest => dest.CategoryName, src => src.Translations.Where(
                    land => land.Language == CultureInfo.CurrentCulture.Name)
                    .Select(T => T.Name).FirstOrDefault()
                );
        }
    }
}
