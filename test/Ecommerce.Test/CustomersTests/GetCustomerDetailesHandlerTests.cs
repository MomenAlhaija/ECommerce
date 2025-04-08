namespace ECommerce.CustomersTests;

public class GetCustomerDetailesHandlerTests
{

    private Mock<ILogger<GetCustomerDetailsQueryHandler>> _mockGetCustomerDetailesLogger;
    private Mock<IDistributedCache> _mockCache;
    private Mock<ICustomerRepository> _mockCustomerRepository;
    private Mock<IMapper> _mockMapper;

    public GetCustomerDetailesHandlerTests()
    {
        _mockGetCustomerDetailesLogger = new Mock<ILogger<GetCustomerDetailsQueryHandler>>();
        _mockCache = new Mock<IDistributedCache>();
        _mockCustomerRepository = new Mock<ICustomerRepository>();
        _mockMapper = new Mock<IMapper>();
    }
    [Fact]
    public async Task GetCustomerDetailesHandler_GetCustomerDetaielsCommandObject_ReturnCustoemrDto()
    {
        //Arrange
        Guid id = Guid.NewGuid();
        var request = new GetCustomerDetailsQuery
        {
            Id = id,

        };

        var customer = new Customer { Id = id, Name = "Momen", Email = "haija@gmail.com" };
        var customerDto = new CustomerDto { Id = customer.Id, Name = customer.Name, Email = customer.Email };

        _mockMapper.Setup(m => m.Map<Customer>(request)).Returns(customer);


        _mockCustomerRepository.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
           .ReturnsAsync(customer);

        _mockMapper.Setup(m => m.Map<CustomerDto>(It.IsAny<Customer>())).Returns(customerDto);

        //Act
        var deleteCustomerHandler = new GetCustomerDetailsQueryHandler(_mockMapper.Object,_mockCustomerRepository.Object, _mockGetCustomerDetailesLogger.Object, _mockCache.Object);
        var actual = await deleteCustomerHandler.Handle(request, CancellationToken.None);
        var expected = customerDto;
        //Assert
        Assert.Equal(expected,actual);
    }
    [Fact]
    public async Task GetCustomerDetailesHandler_ForEmptyCustomerObject_ThrowNotFoundException()
    {
        //Arrange
        Guid id = Guid.NewGuid();
        var request = new GetCustomerDetailsQuery
        {
            Id = id,

        };

        var customer = new Customer { Id = id, Name = "Momen", Email = "haija@gmail.com" };
        var customerDto = new CustomerDto { Id = customer.Id, Name = customer.Name, Email = customer.Email };

        _mockMapper.Setup(m => m.Map<Customer>(request)).Returns(customer);

  
        _mockCustomerRepository.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
           .ReturnsAsync((Customer?)null);

        _mockMapper.Setup(m => m.Map<CustomerDto>(It.IsAny<Customer>())).Returns(customerDto);

        //Act
        var deleteCustomerHandler = new GetCustomerDetailsQueryHandler(_mockMapper.Object, _mockCustomerRepository.Object, _mockGetCustomerDetailesLogger.Object, _mockCache.Object);
        Func<GetCustomerDetailsQuery, Task<CustomerDto>> func = async (e) => await deleteCustomerHandler.Handle(e, CancellationToken.None);


        //Assert
        Assert.ThrowsAsync<NotFoundException>(async () => await func(request));
    }
}
