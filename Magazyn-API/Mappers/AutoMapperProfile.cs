using AutoMapper;
using Magazyn_API.Model.Excel;
using Magazyn_API.Model.Order;
using Magazyn_API.Model.Order.FromExcelDto;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Magazyn_API.Mappers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<IExcelComponent, ComponentModel>()
                .ForMember(dest => dest.ArticleNumber,
                opt => opt.MapFrom(src => src.ArticleNumber))
                .ForMember(dest => dest.OrderingNumber,
                opt => opt.MapFrom(src => src.OrderingNumber))
                .ForMember(dest => dest.Supplier,
                opt => opt.MapFrom(src => src.Supplier))
                .ForMember(dest => dest.SAP,
                opt => opt.MapFrom(src => src.SAP))
                .ForMember(dest => dest.Description,
                opt => opt.MapFrom(src => src.Description));

            CreateMap<IExcelComponent, ComponentModelFromExcelDto>();
        }
    }
}
