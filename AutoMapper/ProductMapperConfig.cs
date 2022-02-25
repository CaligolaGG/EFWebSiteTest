using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using System.Linq;
using Domain;

namespace Mapper
{
    public class ProductMapperConfig : Profile
    {
        public ProductMapperConfig()
        {
            CreateMap<Product, ProductBasic>()
                .ForMember(d => d.ProductName, o => o.MapFrom(s => s.Name))
                ;

            CreateMap<Product, ProductBasicWithBrand>()
                .IncludeBase<Product, ProductBasic>()
                .ForMember(d => d.BrandName, o => o.MapFrom(s => s.Brand.BrandName))
                ;

            CreateMap<Product, ProductSelect>()
                .IncludeBase<Product,ProductBasicWithBrand>()
                .ForMember(d => d.Description, o => o.MapFrom(s => s.ShortDescription))
                .ForMember(d => d.Categories, o => o.MapFrom(s => s.ProductCategory.Select(x => x.Category.Name)))
                ;

            CreateMap<Product, ProductDetail>()
               .IncludeBase<Product, ProductBasicWithBrand>()
               .ForMember(d => d.BrandId, o => o.MapFrom(s => s.Brand.Id))
               .ForMember(d => d.GuestUsersRequestsNumber, o => o.MapFrom(s => s.InfoRequests.Where(u => u.UserId == null).Count()))
               .ForMember(d => d.LoggedUsersRequestsNumber, o => o.MapFrom(s => s.InfoRequests.Where(u => u.UserId != null).Count()))
               .ForMember(d => d.Categories, o => o.MapFrom(p => p.ProductCategory.Select(pc => pc.Category))
               );
            //.ForMember(d => d.Requests, o => o.MapFrom(p => p.InfoRequests));


            //da rivedere
            CreateMap<Product, ProductAndCategories>()
               .ForMember(d => d.Product, o => o.MapFrom(s => s))
               .ForMember(d => d.BrandName, o => o.MapFrom(s => s.Brand.BrandName))
               .ForMember(d => d.Categories, o => o.MapFrom(s => s.ProductCategory.Select(pc => pc.Category)))
               ;

            CreateMap<Product, ProductTemp>()
                .ForMember(d => d.ProductName, o => o.MapFrom(s => s.Name))
                .ForMember(d => d.ProductRequestNumber, o => o.MapFrom(s => s.InfoRequests.Count))
                ;

            CreateMap<ProductAndCategoryModel2, Product >()
                .ForMember(d => d.Name, o => o.MapFrom(s => s.Name))
                .ForMember(d => d.Id, o => o.MapFrom(s => s.Id))
                .ForMember(d => d.Description, o => o.MapFrom(s => s.Description))
                .ForMember(d => d.ShortDescription, o => o.MapFrom(s => s.ShortDescription))
                .ForMember(d => d.Price, o => o.MapFrom(s => s.Price))
                .ForMember(d => d.BrandId, o => o.MapFrom(s => s.BrandId))

                .ForMember(d => d.ProductCategory, o => o.MapFrom(s => s.Categories.Select(x => new ProductCategory
                { IdCategory = x}).ToList()))
            ;


            //CreateMap<Product, Product>();
            //CreateMap<ProductAndCategoryModel, Product>()
            //    .ForMember(d => d, o => o.MapFrom(s => s.Product))
            //    .ForMember(d => d.ProductCategory, o => o.MapFrom(s => s.Categories.Select(x => new ProductCategory
            //    { IdCategory = x }).ToList()))
            //;


        }
    }
}
