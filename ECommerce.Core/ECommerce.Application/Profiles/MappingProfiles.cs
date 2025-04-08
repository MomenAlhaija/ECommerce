using AutoMapper;
using ECommerce.Application.Features;
using ECommerce.Application.Models;
using ECommerce.Application.Models.Orders;
using ECommerce.Application.Models.Prodcuts;
using ECommerce.Domain.Entities;

namespace ECommerce.Application.Profiles
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            #region Category
            CreateMap<CreateCategoryCommand, Category>().ReverseMap();
            CreateMap<Category, CategoryDto>().ReverseMap();
            CreateMap<UpdateCategoryCommand, Category>().ReverseMap();
            #endregion

            #region Customer
            CreateMap<CreateCustomerCommand, Customer>().ReverseMap();
            CreateMap<Customer, CustomerDto>().ReverseMap();
            CreateMap<UpdateCustomerCommand, Customer>().ReverseMap();
            #endregion

            #region Order
            CreateMap<UpdateOrderItemDto, OrderItem>().ReverseMap();
            CreateMap<OrderItem, OrderItemDto>().ReverseMap();
            CreateMap<CreateOrderItemsDto, OrderItem>().ReverseMap();
            CreateMap<CreateOrderCommand, Order>().ReverseMap()
                .ForMember(des => des.OrderItems, opt => opt.MapFrom(src => src.OrderItems));
            CreateMap<Order, OrderDto>().ReverseMap();
            CreateMap<UpdateOrderCommand, Order>().ReverseMap()
                .ForMember(des => des.OrderItems, opt => opt.MapFrom(src => src.OrderItems));
            #endregion

            #region Product
            CreateMap<CreateProcductCommnd, Product>().ReverseMap();
            CreateMap<UpdateProductCommand, Product>().ReverseMap();
            CreateMap<ProductDto, Product>().ReverseMap();
            #endregion

       
        }
    }
}
