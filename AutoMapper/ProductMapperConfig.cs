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
                //.ForMember(d => d.ProductId, o => o.MapFrom(s => s.Id))
                .ForMember(d => d.ProductName, o => o.MapFrom(s => s.Name))
                .ForMember(d => d.ProductRequestNumber, o => o.MapFrom(s => s.InfoRequests.Count))
                ;






            CreateMap<Product, ProductAndCategoryModel>()
                .ForMember(d => d.Product, o => o.MapFrom(s => s))
                .ForMember(d => d.Categories, o => o.MapFrom(s => s.ProductCategory.Select(x => x.Category)))
                //.ReverseMap()
                //.ForPath(d=> d.ProductCategory , o=>o.MapFrom(s=>s.Categories.Select(x => new ProductCategory 
                //{ IdCategory = x, IdProduct = s.Product.Id})))
                ;

            //CreateMap<Product, Product>();



        }
    }
}
