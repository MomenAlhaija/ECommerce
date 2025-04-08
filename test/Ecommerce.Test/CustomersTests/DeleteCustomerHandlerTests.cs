namespace ECommerce.CustomersTests;

public class DeleteCustomerHandlerTests
{
    private Mock<ILogger<DeleteCustomerHandler>> _mockDeleteCustomerLogger;
    private Mock<IDistributedCache> _mockCache;
    private Mock<ICustomerRepository> _mockCustomerRepository;
    private Mock<IMapper> _mockMapper;
    public DeleteCustomerHandlerTests()
    {
        _mockDeleteCustomerLogger = new Mock<ILogger<DeleteCustomerHandler>>();
        _mockCache = new Mock<IDistributedCache>();
        _mockCustomerRepository = new Mock<ICustomerRepository>();
        _mockMapper = new Mock<IMapper>();
    }
    [Fact]
    public async Task DeleteCustomerHandler_RemoveCustomerCommandObject_ReturnTrue()
    {
        //Arrange
        Guid id = Guid.NewGuid();
        var request = new DeleteCustomerCommand
        {
            Id = id,
          
        };

        var customer = new Customer { Id = id, Name = "Momen", Email = "haija@gmail.com" };
        var customerDto = new CustomerDto { Id = customer.Id, Name = customer.Name, Email = customer.Email };

        _mockMapper.Setup(m => m.Map<Customer>(request)).Returns(customer);

        _mockCustomerRepository.Setup(r => r.DeleteAsync(It.IsAny<Customer>()))
            .Returns(Task.CompletedTask);

        _mockCustomerRepository.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
           .ReturnsAsync(customer);

        _mockMapper.Setup(m => m.Map<CustomerDto>(It.IsAny<Customer>())).Returns(customerDto);

        //Act
        var deleteCustomerHandler = new DeleteCustomerHandler(_mockCustomerRepository.Object, _mockMapper.Object, _mockDeleteCustomerLogger.Object, _mockCache.Object);
        var actual = await deleteCustomerHandler.Handle(request, CancellationToken.None);

        //Assert
        Assert.True(actual);
    }
    [Fact]
    public async Task DeleteCustomerHandler_ForEmptyCustomerObject_ThrowNotFoundException()
    {
        //Arrange
        Guid id = Guid.NewGuid();
        var request = new DeleteCustomerCommand
        {
            Id = id,
          
        };

        var customer = new Customer { Id = id, Name = "Momen", Email = "haija@gmail.com" };
        var customerDto = new CustomerDto { Id = customer.Id, Name = customer.Name, Email = customer.Email };

        _mockMapper.Setup(m => m.Map<Customer>(request)).Returns(customer);

        _mockCustomerRepository.Setup(r => r.DeleteAsync(It.IsAny<Customer>()))
            .Returns(Task.CompletedTask);

        _mockCustomerRepository.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
           .ReturnsAsync((Customer?)null);

        _mockMapper.Setup(m => m.Map<CustomerDto>(It.IsAny<Customer>())).Returns(customerDto);

        //Act
        var deleteCustomerHandler = new DeleteCustomerHandler(_mockCustomerRepository.Object, _mockMapper.Object, _mockDeleteCustomerLogger.Object, _mockCache.Object);
        Func<DeleteCustomerCommand, Task<bool>> func = async (e) => await deleteCustomerHandler.Handle(e, CancellationToken.None);


        //Assert
        Assert.ThrowsAsync<NotFoundException>(async () => await func(request));
    }
}
  