namespace ECommerce.CustomersTests;

public class UpdateCustomerHandlerTests
{
    private Mock<ILogger<UpdateCustomerHandler>> _mockUpdateCustomerLogger;
    private Mock<IDistributedCache> _mockCache;
    private Mock<ICustomerRepository> _mockCustomerRepository;
    private Mock<IMapper> _mockMapper;
    public UpdateCustomerHandlerTests()
    {
        _mockUpdateCustomerLogger = new Mock<ILogger<UpdateCustomerHandler>>();
        _mockCache = new Mock<IDistributedCache>();
        _mockCustomerRepository = new Mock<ICustomerRepository>();
        _mockMapper = new Mock<IMapper>();
    }
    [Fact]
    public async Task UpdateCustomerHandler_UpdataValiedCustomerCommandObject_ReturenCustomerDto()
    {
        //Arrange
        Guid id = Guid.NewGuid();
        var request = new UpdateCustomerCommand
        {
            Id=id,
            Name = "Momen",
            Email = "haija@gmail.com"
        };

        var customer = new Customer { Id =id, Name = "Momen", Email = "haija@gmail.com" };
        var customerDto = new CustomerDto { Id = customer.Id, Name = customer.Name, Email = customer.Email };

        _mockMapper.Setup(m => m.Map<Customer>(request)).Returns(customer);

        _mockCustomerRepository.Setup(r => r.UpdateAsync(It.IsAny<Customer>()))
            .Returns(Task.CompletedTask);

        _mockCustomerRepository.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
           .ReturnsAsync(customer);

        _mockMapper.Setup(m => m.Map<CustomerDto>(It.IsAny<Customer>())).Returns(customerDto);

        //Act
        var updateCustomerHandler = new UpdateCustomerHandler( _mockCustomerRepository.Object, _mockMapper.Object, _mockUpdateCustomerLogger.Object, _mockCache.Object);
        var actual = await updateCustomerHandler.Handle(request, CancellationToken.None);
        var excpected = customerDto;
        
        //Assert
        Assert.Equal(excpected, actual);
    }
    [Fact]
    public async Task UpdateCustomerHandler_ForEmptyCustomerObject_ThrowNotFoundException()
    {
        //Arrange
        Guid id = Guid.NewGuid();
        var request = new UpdateCustomerCommand
        {
            Id = id,
            Name = "Momen",
            Email = "haija@gmail.com"
        };

        var customer = new Customer { Id = id, Name = "Momen", Email = "haija@gmail.com" };
        var customerDto = new CustomerDto { Id = customer.Id, Name = customer.Name, Email = customer.Email };

        _mockMapper.Setup(m => m.Map<Customer>(request)).Returns(customer);

        _mockCustomerRepository.Setup(r => r.UpdateAsync(It.IsAny<Customer>()))
            .Returns(Task.CompletedTask);

        _mockCustomerRepository.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
           .ReturnsAsync((Customer?)null);

        _mockMapper.Setup(m => m.Map<CustomerDto>(It.IsAny<Customer>())).Returns(customerDto);

        //Act
        var updateCustomerHandler = new UpdateCustomerHandler(_mockCustomerRepository.Object, _mockMapper.Object, _mockUpdateCustomerLogger.Object, _mockCache.Object);
        Func<UpdateCustomerCommand,Task<CustomerDto>> func =async (e)=> await updateCustomerHandler.Handle(e, CancellationToken.None);


        //Assert
        Assert.ThrowsAsync<NotFoundException>(async () => await func(request));
    }
}
