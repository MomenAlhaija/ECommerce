using ECommerce.Application.Models.Orders;
using ECommerce.Domain.Enum;

namespace ECommerce.OrdersTests;

public class CreateOrderHandler
{
    private Mock<ILogger<CreateOrderCommandHandler>> _mockCreateOrderLogger;
    private Mock<IDistributedCache> _mockCache;
    private Mock<IOrderRepository> _mockOrderRepository;
    private Mock<ICustomerRepository> _mockCustomerRepo;

    private Mock<IMapper> _mockMapper;

    public CreateOrderHandler()
    {
        _mockCreateOrderLogger = new Mock<ILogger<CreateOrderCommandHandler>>();
        _mockCache = new Mock<IDistributedCache>();
        _mockOrderRepository = new Mock<IOrderRepository>();
        _mockMapper = new Mock<IMapper>();
        _mockCustomerRepo = new Mock<ICustomerRepository>();

    }
    [Fact]
    public async Task CreateCategoryHandler_CreateCategoryCommandObject_ReturenCategoryDto()
    {
        //Arrange
        Guid orderId = Guid.NewGuid();
        Guid customerId = Guid.NewGuid();
        var order = new Order
        {

            CustomerId = customerId,
            ShippingAddress = "Amman",
            OrderStatus = OrderStatus.Completed,
            OrderItems = new List<OrderItem>
            {
                new OrderItem
                {
                    ProductId=Guid.NewGuid(),
                    Price=20m,
                    Quantity=2
                }
            }
        };

        var request = new CreateOrderCommand
        {
          CustomerId=customerId,
          ShippingAddress=order.ShippingAddress,
          OrderItems= order.OrderItems.Select(orIt => new CreateOrderItemsDto
          {
              Price = orIt.Price,
              Quantity = orIt.Quantity,
              ProductId = orIt.ProductId
          }).ToList()
        };

        var customer = new Customer
        {
            Id = customerId,
            Name = "Momen",
            Email = "HAija@gmail.com"
        };
        var orderDto = new OrderDto
        {
            Id = order.Id,
            CustomerId = order.CustomerId,
            ShippingAddress = order.ShippingAddress,
            Payment = order.Payment,
            OrderStatus = order.OrderStatus,
            OrderItems = order.OrderItems.Select(orIt => new OrderItemDto
            {
                Price=orIt.Price,
                Quantity=orIt.Quantity,
                ProductId=orIt.ProductId
            }).ToList()
        };

        _mockMapper.Setup(m => m.Map<Order>(request)).Returns(order);

        _mockOrderRepository.Setup(r => r.AddAsync(It.IsAny<Order>()))
            .ReturnsAsync(order);

        _mockCustomerRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
          .ReturnsAsync(customer);

        _mockMapper.Setup(m => m.Map<OrderDto>(It.IsAny<Order>())).Returns(orderDto);

        //Act
        var handler = new CreateOrderCommandHandler(_mockOrderRepository.Object, _mockMapper.Object,_mockCreateOrderLogger.Object,_mockCustomerRepo.Object, _mockCache.Object);
        var actual = await handler.Handle(request, CancellationToken.None);
        var excpected = orderDto;

        //Assert
        Assert.Equal(excpected, actual);
    }
}
