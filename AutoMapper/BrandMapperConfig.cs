using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using System.Linq;
using Domain;

namespace Mapper
{
    public class BrandMapperConfig : Profile
    {
        public BrandMapperConfig()
        {

            CreateMap<Brand, BrandProjectionBasic>()
                .ForMember(d => d.BrandName, o => o.MapFrom(s => s.BrandName))
                ;

            CreateMap<Brand, BrandAccountProjection>()
                .IncludeBase<Brand, BrandProjectionBasic>()
                .ForSourceMember(d => d.Account, o => o.DoNotValidate());

            CreateMap<Brand, BrandSelect>()
                .IncludeBase<Brand, BrandProjectionBasic>()
                .ForMember(d => d.ProductIds, o => o.MapFrom(s => s.Products.Select(p => p.Id)))
                ;

            //da rivedere
            CreateMap<Brand, BrandDetail>()
                .ForMember(d => d.NumberRequests, o => o.MapFrom(s => s.Products.SelectMany(x => x.InfoRequests).Count()))
                .ForMember(d => d.ListProducts, o => o.MapFrom(s => s.Products))
                ;





        }


    }
}
