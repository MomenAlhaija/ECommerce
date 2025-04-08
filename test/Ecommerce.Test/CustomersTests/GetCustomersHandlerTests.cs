namespace ECommerce.CustomersTests;

public class GetCustomersHandlerTests
{
    private Mock<ILogger<GetCustomersQueryHandler>> _mockGetCustomerLogger;
    private Mock<IDistributedCache> _mockCache;
    private Mock<ICustomerRepository> _mockCustomerRepository;
    private Mock<IMapper> _mockMapper;

    public GetCustomersHandlerTests()
    {
        _mockGetCustomerLogger = new Mock<ILogger<GetCustomersQueryHandler>>();
        _mockCache = new Mock<IDistributedCache>();
        _mockCustomerRepository = new Mock<ICustomerRepository>();
        _mockMapper = new Mock<IMapper>();
    }
    [Fact]
    public async Task GetCustomersHandler_ForCustomersQuery_ReturnCustomersList()
    {
        //Arrange
        var request = new GetCustomersQuery
        {

        };
        var customers = new List<Customer>
        {
            new Customer{ Id = Guid.NewGuid(), Name = "Momenm",Email="ex01@gmail.com" },
            new Customer{ Id = Guid.NewGuid(), Name = "Ahmad",Email = "ex02@gmail.com" },
            new Customer{ Id = Guid.NewGuid(), Name = "Saleh",Email= "ex03@gmail.com" },

        };
        var customersDto = new List<CustomerDto>();
        foreach (var customer in customers)
        {
            customersDto.Add(new CustomerDto
            {
                Id = customer.Id,
                Name = customer.Name,
                Email=customer.Email
            });
        }

        _mockCustomerRepository.Setup(r => r.ListAllAsync()).ReturnsAsync(customers);


        _mockMapper.Setup(m => m.Map<List<CustomerDto>>(It.IsAny<List<Customer>>())).Returns(customersDto);


        _mockCache.Setup(c => c.GetAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
          .ReturnsAsync((byte[])null);

        _mockCache.Setup(c => c.SetAsync(It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<DistributedCacheEntryOptions>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        //Act

        var deleteCategoryHandler = new GetCustomersQueryHandler(_mockCustomerRepository.Object, _mockMapper.Object, _mockGetCustomerLogger.Object, _mockCache.Object);
        var actual = await deleteCategoryHandler.Handle(request, CancellationToken.None);
        var expected = customersDto;
        //Assert
        Assert.Equal(expected, actual);
    }
}
