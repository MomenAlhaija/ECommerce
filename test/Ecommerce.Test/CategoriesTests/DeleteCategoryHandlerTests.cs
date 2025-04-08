namespace ECommerce.CategoriesTests;

public class DeleteCategoryHandlerTests
{


    private Mock<ILogger<DeleteCategoryHandler>> _mockDeleteCategoryLogger;
    private Mock<IDistributedCache> _mockCache;
    private Mock<ICategoryRepository> _mockCategoryRepository;
    private Mock<IMapper> _mockMapper;
    public DeleteCategoryHandlerTests()
    {
        _mockCache = new Mock<IDistributedCache>();
        _mockCategoryRepository = new Mock<ICategoryRepository>();
        _mockMapper = new Mock<IMapper>();
        _mockDeleteCategoryLogger = new Mock<ILogger<DeleteCategoryHandler>>();
    }
    [Fact]
    public async Task DeleteCategoryHandler_ForValiedCategory_ReturnTrue()
    {
        //Arrange
        Guid id = Guid.NewGuid();
        var request = new DeleteCategoryCommand
        {
            Id = id
        };
        var category = new Category { Id = id, Name = "Electronics" };
        var categoryDto = new CategoryDto { Id = id, Name = category.Name };

        _mockCategoryRepository.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
        .ReturnsAsync(category);

        _mockCategoryRepository.Setup(r => r.DeleteAsync(It.IsAny<Category>())).Returns(Task.CompletedTask);


        _mockMapper.Setup(m => m.Map<CategoryDto>(It.IsAny<Category>())).Returns(categoryDto);


        _mockCache.Setup(c => c.GetAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
          .ReturnsAsync((byte[])null);

        _mockCache.Setup(c => c.SetAsync(It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<DistributedCacheEntryOptions>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        //Act

        var deleteCategoryHandler = new DeleteCategoryHandler(_mockCategoryRepository.Object, _mockMapper.Object, _mockDeleteCategoryLogger.Object, _mockCache.Object);
        var actual = await deleteCategoryHandler.Handle(request, CancellationToken.None);

        //
        Assert.True(actual);
    }
    [Fact]
    public async Task DeleteCategoryHandler_ForUnValiedCategory_ThrowNotFoundException()
    {
        //Arrange
        Guid id = Guid.NewGuid();
        var request = new DeleteCategoryCommand
        {
            Id = id
        };
        var category = new Category { Id = id, Name = "Electronics" };
        var categoryDto = new CategoryDto { Id = id, Name = category.Name };

        _mockCategoryRepository.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
        .ReturnsAsync((Category?)null);

        _mockCategoryRepository.Setup(r => r.DeleteAsync(It.IsAny<Category>())).Returns(Task.CompletedTask);


        _mockMapper.Setup(m => m.Map<CategoryDto>(It.IsAny<Category>())).Returns(categoryDto);


        _mockCache.Setup(c => c.GetAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
          .ReturnsAsync((byte[])null);

        _mockCache.Setup(c => c.SetAsync(It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<DistributedCacheEntryOptions>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        //Act

        var deleteCategoryHandler = new DeleteCategoryHandler(_mockCategoryRepository.Object, _mockMapper.Object, _mockDeleteCategoryLogger.Object, _mockCache.Object);
        Func<DeleteCategoryCommand, Task<bool>> func = async (e) => await deleteCategoryHandler.Handle(e, CancellationToken.None);

        //Assert
        Assert.ThrowsAsync<NotFoundException>(async () => await func(request));
    }
}
