namespace ECommerce.CategoriesTests;

public class UpdateCategoryHandlerTests
{

    private Mock<ILogger<UpdateCategoryHandler>> _mockUpdateCategoryLogger;
    private Mock<IDistributedCache> _mockCache;
    private Mock<ICategoryRepository> _mockCategoryRepository;
    private Mock<IMapper> _mockMapper;
    public UpdateCategoryHandlerTests()
    {
        _mockCache = new Mock<IDistributedCache>();
        _mockCategoryRepository = new Mock<ICategoryRepository>();
        _mockMapper = new Mock<IMapper>();
        _mockUpdateCategoryLogger = new Mock<ILogger<UpdateCategoryHandler>>();
    }
    [Fact]
    public async Task UpdateCategoryHandler_UpdateCategoryCommandObject_ReturenCategoryDto()
    {

        var id = Guid.NewGuid();
        //Arrange
        var request = new UpdateCategoryCommand
        {
            Id = id,
            Name = "Electronics"
        };

        var category = new Category { Id = id, Name = "Electronics" };
        var categoryDto = new CategoryDto { Id = category.Id, Name = category.Name };

        _mockMapper.Setup(m => m.Map<Category>(request)).Returns(category);

        _mockCategoryRepository.Setup(r => r.UpdateAsync(It.IsAny<Category>())).Returns(Task.CompletedTask);

        _mockCategoryRepository.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(category);

        _mockMapper.Setup(m => m.Map<CategoryDto>(It.IsAny<Category>())).Returns(categoryDto);

        // Mock GetAsync to return cached data
        _mockCache.Setup(c => c.GetAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((byte[])null); // Simulate cache miss

        // Mock SetAsync to store data in cache
        _mockCache.Setup(c => c.SetAsync(It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<DistributedCacheEntryOptions>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        //Act
        var ypdateCategoryHandler = new UpdateCategoryHandler(_mockCategoryRepository.Object, _mockMapper.Object, _mockUpdateCategoryLogger.Object, _mockCache.Object);
        var actual = await ypdateCategoryHandler.Handle(request, CancellationToken.None);
        var excpected = categoryDto;

        //Assert
        Assert.Equal(excpected, actual);
    }
    [Fact]
    public async Task UpdateCategoryHandler_UpdateCategoryCommandNotFoundCategory_ThrowNotFoundException()
    {

        //Arrange
        var id = Guid.NewGuid();
        var request = new UpdateCategoryCommand
        {
            Id = id,
            Name = "Electronics"
        };

        var category = new Category { Id = id, Name = "Electronics" };
        var categoryDto = new CategoryDto { Id = category.Id, Name = category.Name };

        _mockMapper.Setup(m => m.Map<Category>(request)).Returns(category);

        _mockCategoryRepository.Setup(r => r.UpdateAsync(It.IsAny<Category>())).Returns(Task.CompletedTask);

        _mockCategoryRepository.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync((Category?)null);

        _mockMapper.Setup(m => m.Map<CategoryDto>(It.IsAny<Category>())).Returns(categoryDto);

        _mockCache.Setup(c => c.GetAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((byte[])null);

        _mockCache.Setup(c => c.SetAsync(It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<DistributedCacheEntryOptions>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        //Act
        var ypdateCategoryHandler = new UpdateCategoryHandler(_mockCategoryRepository.Object, _mockMapper.Object, _mockUpdateCategoryLogger.Object, _mockCache.Object);
        Func<UpdateCategoryCommand, Task<CategoryDto>> func =
            async (e) => await ypdateCategoryHandler.Handle(e, CancellationToken.None);

        //Assert
        await Assert.ThrowsAsync<NotFoundException>(async () => await func(request));
    }
}
