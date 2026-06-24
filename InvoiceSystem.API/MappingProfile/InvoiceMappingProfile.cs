using AutoMapper;
using InvoiceSystem.API.Dtos.Request;
using InvoiceSystem.API.Dtos.Response;
using InvoiceSystem.API.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace InvoiceSystem.API.MappingProfile
{
    public class InvoiceMappingProfile : Profile
    {
        public InvoiceMappingProfile()
        {
            // Request DTO => Model


            CreateMap<RegisterRequestDto, ApplicationUser>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email))
          .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
          .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName));


            CreateMap<CreateInvoiceRequestDto, Invoice>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Serial, opt => opt.Ignore())
                .ForMember(dest => dest.InvoiceDate, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.TotalPrice, opt => opt.Ignore())
                .ForMember(dest => dest.Store, opt => opt.Ignore())
                .ForMember(dest => dest.Customer, opt => opt.Ignore())
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));

            CreateMap<UpdateInvoiceRequestDto, Invoice>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Serial, opt => opt.Ignore())
                .ForMember(dest => dest.InvoiceDate, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.TotalPrice, opt => opt.Ignore())
                .ForMember(dest => dest.Store, opt => opt.Ignore())
                .ForMember(dest => dest.Customer, opt => opt.Ignore())
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));

            CreateMap<CreateInvoiceItemRequestDto, InvoiceItem>()
    .ForMember(dest => dest.Id, opt => opt.Ignore())
    .ForMember(dest => dest.InvoiceId, opt => opt.Ignore())
    .ForMember(dest => dest.Invoice, opt => opt.Ignore())
    .ForMember(dest => dest.Product, opt => opt.Ignore())
    .ForMember(dest => dest.Price, opt => opt.Ignore())
    .ForMember(dest => dest.Total, opt => opt.Ignore());

            // Model => Response DTO


            CreateMap<ApplicationUser, LoginResponseDto>()
                .ForMember(dest => dest.Token, opt => opt.Ignore())
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName));

            CreateMap<ApplicationUser, RegisterResponseDto>()
    .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
    .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
    .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName));


            CreateMap<Invoice, InvoiceListResponseDto>()
                .ForMember(dest => dest.StoreName, opt => opt.MapFrom(src => src.Store.Name))
                .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.Customer.Name));

            CreateMap<Invoice, InvoiceResponseDto>()
                .ForMember(dest => dest.StoreName, opt => opt.MapFrom(src => src.Store.Name))
                .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.Customer.Name))
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));

            CreateMap<InvoiceItem, InvoiceItemResponseDto>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name));

            CreateMap<Product, ProductResponseDto>();

            CreateMap<Store, StoreResponseDto>();

            CreateMap<Customer, CustomerResponseDto>();
        }
    }
}