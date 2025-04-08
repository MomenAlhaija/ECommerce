namespace ECommerce.CategoriesTests;

public class CreateCategoryHandlerTests
{

    private Mock<ILogger<CreateCategoryHandler>> _mockCreateCategoryLogger;
    private Mock<IDistributedCache> _mockCache;
    private Mock<ICategoryRepository> _mockCategoryRepository;
    private Mock<IMapper> _mockMapper;

    public CreateCategoryHandlerTests()
    {
        _mockCreateCategoryLogger = new Mock<ILogger<CreateCategoryHandler>>();
        _mockCache = new Mock<IDistributedCache>();
        _mockCategoryRepository = new Mock<ICategoryRepository>();
        _mockMapper = new Mock<IMapper>();
        
    }
    [Fact]
    public async Task CreateCategoryHandler_CreateCategoryCommandObject_ReturenCategoryDto()
    {
        //Arrange
        var request = new CreateCategoryCommand
        {
            Name = "Electronics"
        };

        var category = new Category { Id = Guid.NewGuid(), Name = "Electronics" };
        var categoryDto = new CategoryDto { Id = category.Id, Name = category.Name };

        _mockMapper.Setup(m => m.Map<Category>(request)).Returns(category);

        _mockCategoryRepository.Setup(r => r.AddAsync(It.IsAny<Category>()))
            .ReturnsAsync(category);

        _mockMapper.Setup(m => m.Map<CategoryDto>(It.IsAny<Category>())).Returns(categoryDto);

        //Act
        var createCategoryHandler = new CreateCategoryHandler(_mockCategoryRepository.Object, _mockMapper.Object, _mockCreateCategoryLogger.Object, _mockCache.Object);
        var actual = await createCategoryHandler.Handle(request, CancellationToken.None);
        var excpected = categoryDto;

        //Assert
        Assert.Equal(excpected, actual);
    }
}
