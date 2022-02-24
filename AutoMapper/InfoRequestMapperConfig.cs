using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using Domain;

namespace Mapper
{
    public class InfoRequestMapperConfig : Profile
    {

        public InfoRequestMapperConfig()
        {
            CreateMap<InfoRequest, InfoRequestBasic>()
                .ForMember(d => d.UserFullName, o => o.MapFrom(s => s.Name + " " + s.LastName));

            CreateMap<InfoRequest, InfoRequestSelect>()
                .IncludeBase<InfoRequest,InfoRequestBasic>()
                .ForMember(d => d.ProductName, o => o.MapFrom(s => s.Product.Name))
                .ForMember(d => d.Date, o => o.MapFrom(s => s.InsertDate));

            CreateMap<InfoRequest, InfoRequestDetail>()
                .IncludeBase<InfoRequest, InfoRequestBasic>()
                .ForMember(d => d.ProductName, o => o.MapFrom(s => s.Product.Name))
                .ForMember(d => d.BrandName, o => o.MapFrom(s => s.Product.Brand.BrandName))
                .ForMember(d => d.InfoUser, o => o.MapFrom(s => "City: " + s.City + " Cap: " + s.Cap + " Nation: " + s.Nation.Name))
                .ForMember(d => d.Replies, o => o.MapFrom(s => s.InfoRequestReplies))
                ;

            CreateMap<InfoRequestReply, RepliesTemp>()
                .ForMember(d => d.ReplyId, o => o.MapFrom(s => s.Id))
                .ForMember(d => d.Date, o => o.MapFrom(s => s.InsertDate))
                .ForMember(d => d.AccountName, o => o.MapFrom(s => s.Account.AccountType == 1 ? "User" + s.Account.User.Name : " Brand" + s.Account.Brand.BrandName))
                ;

        }

    }
}
