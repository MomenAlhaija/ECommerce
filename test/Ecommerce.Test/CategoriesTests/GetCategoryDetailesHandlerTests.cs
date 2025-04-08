namespace ECommerce.CategoriesTests;

public class GetCategoryDetailesHandlerTests
{


    private Mock<ILogger<GetCategoryDetailsHandler>> _mockGetCategoryDetailesLogger;
    private Mock<IDistributedCache> _mockCache;
    private Mock<ICategoryRepository> _mockCategoryRepository;
    private Mock<IMapper> _mockMapper;
    public GetCategoryDetailesHandlerTests()
    {
        _mockCache = new Mock<IDistributedCache>();
        _mockCategoryRepository = new Mock<ICategoryRepository>();
        _mockMapper = new Mock<IMapper>();
        _mockGetCategoryDetailesLogger = new Mock<ILogger<GetCategoryDetailsHandler>>();
    }
    [Fact]
    public async Task GetCategoryDetailsHandler_ForCategoryDetailesQuery_ReturnCategoreyDto()
    {
        //Arrange
        Guid id = Guid.NewGuid();
        var request = new GetCategoryDetailsQuery
        {
            Id = id
        };
        var category = new Category { Id = id, Name = "Computers" };


        var categoryDto = new CategoryDto
        {
            Id = category.Id,
            Name = category.Name
        };

        _mockCategoryRepository.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(category);

        _mockCategoryRepository.Setup(r => r.DeleteAsync(It.IsAny<Category>())).Returns(Task.CompletedTask);


        _mockMapper.Setup(m => m.Map<CategoryDto>(It.IsAny<Category>())).Returns(categoryDto);


        _mockCache.Setup(c => c.GetAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
          .ReturnsAsync((byte[])null);

        _mockCache.Setup(c => c.SetAsync(It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<DistributedCacheEntryOptions>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        //Act

        var deleteCategoryHandler = new GetCategoryDetailsHandler(_mockCategoryRepository.Object, _mockMapper.Object, _mockGetCategoryDetailesLogger.Object, _mockCache.Object);
        var actual = await deleteCategoryHandler.Handle(request, CancellationToken.None);
        var expected = categoryDto;
        //Assert
        Assert.Equal(expected, actual);
    }
    [Fact]
    public async Task GetCategoryDetailsHandler_IfNotFoundCategoryDetailes_ThrowNotFoundException()
    {
        //Arrange
        Guid id = Guid.NewGuid();
        var request = new GetCategoryDetailsQuery
        {
            Id = id
        };
        Category category = null;


        var categoryDto = new CategoryDto
        {
            Id = id,
            Name = "Computers"
        };

        _mockCategoryRepository.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(category);

        _mockCategoryRepository.Setup(r => r.DeleteAsync(It.IsAny<Category>())).Returns(Task.CompletedTask);


        _mockMapper.Setup(m => m.Map<CategoryDto>(It.IsAny<Category>())).Returns(categoryDto);


        _mockCache.Setup(c => c.GetAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
          .ReturnsAsync((byte[])null);

        _mockCache.Setup(c => c.SetAsync(It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<DistributedCacheEntryOptions>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        //Act

        var deleteCategoryHandler = new GetCategoryDetailsHandler(_mockCategoryRepository.Object, _mockMapper.Object, _mockGetCategoryDetailesLogger.Object, _mockCache.Object);
        Func<GetCategoryDetailsQuery, Task<CategoryDto>> func = async (e) => await deleteCategoryHandler.Handle(e, CancellationToken.None);

        //Assert
        Assert.ThrowsAsync<NotFoundException>(async () => await func(request));
    }
}
