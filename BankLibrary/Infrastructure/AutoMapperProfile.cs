using AutoMapper;
using BankLibrary.Models;
using BankLibrary.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankLibrary.Infrastructure
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Customer, CRUDCustomerViewModel>()
                        .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => Enum.Parse(typeof(Gender), char.ToUpper(src.Gender[0]) + src.Gender.Substring(1))))
                        .ForMember(dest => dest.Country, opt => opt.MapFrom(src => Enum.Parse(typeof(Country), src.Country)));

            CreateMap<CRUDCustomerViewModel, Customer>()
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender.ToString()))
                .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.Country.ToString()));
        }
    }
}
