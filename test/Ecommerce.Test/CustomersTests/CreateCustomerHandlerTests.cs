namespace ECommercec.CustomersTests;

public class CreateCustomerHandlerTests
{
    private Mock<ILogger<CreateCustomerHandler>> _mockCreateCustomerLogger;
    private Mock<IDistributedCache> _mockCache;
    private Mock<ICustomerRepository> _mockCustomerRepository;
    private Mock<IMapper> _mockMapper;
    public CreateCustomerHandlerTests()
    {
        _mockCreateCustomerLogger = new Mock<ILogger<CreateCustomerHandler>>();
        _mockCache = new Mock<IDistributedCache>();
        _mockCustomerRepository = new Mock<ICustomerRepository>();
        _mockMapper = new Mock<IMapper>();
    }
    [Fact]
    public async Task CreateCustomerHandler_CreateCustomerCommandObject_ReturenCustomerDto()
    {
        //Arrange
        var request = new CreateCustomerCommand
        {
            Name = "Momen",
            Email="haija@gmail.com"
        };

        var customer = new Customer { Id = Guid.NewGuid(), Name = "Momen",Email= "haija@gmail.com" };
        var customerDto = new CustomerDto { Id = customer.Id, Name = customer.Name,Email=customer.Email };

        _mockMapper.Setup(m => m.Map<Customer>(request)).Returns(customer);

        _mockCustomerRepository.Setup(r => r.AddAsync(It.IsAny<Customer>()))
            .ReturnsAsync(customer);

        _mockMapper.Setup(m => m.Map<CustomerDto>(It.IsAny<Customer>())).Returns(customerDto);

        //Act
        var createcustomerHandler = new CreateCustomerHandler(_mockMapper.Object,_mockCustomerRepository.Object, _mockCreateCustomerLogger.Object, _mockCache.Object);
        var actual = await createcustomerHandler.Handle(request, CancellationToken.None);
        var excpected = customerDto;

        //Assert
        Assert.Equal(excpected, actual);
    }

}
