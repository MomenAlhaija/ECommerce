namespace ECommerce.CategoriesTests;

public class GetCategoriesHandlerTests
{


    private Mock<ILogger<GetCategoriesHandler>> _mockGetCategoriesLogger;
    private Mock<ILogger<GetCategoryDetailsHandler>> _mockGetCategoryDetailesLogger;
    private Mock<IDistributedCache> _mockCache;
    private Mock<ICategoryRepository> _mockCategoryRepository;
    private Mock<IMapper> _mockMapper;
    public GetCategoriesHandlerTests()
    {
        _mockCache = new Mock<IDistributedCache>();
        _mockCategoryRepository = new Mock<ICategoryRepository>();
        _mockMapper = new Mock<IMapper>();
        _mockGetCategoriesLogger = new Mock<ILogger<GetCategoriesHandler>>();
    }
    [Fact]
    public async Task GetCategoriesHandler_ForCategoriesQuery_ReturnCategoriesList()
    {
        //Arrange
        var request = new GetCategoriesQuery
        {

        };
        var categories = new List<Category>
        {
            new Category{ Id = Guid.NewGuid(), Name = "Computers" },
            new Category{ Id = Guid.NewGuid(), Name = "Phones" },
            new Category{ Id = Guid.NewGuid(), Name = "Watches" },

        };
        var categoriesDto = new List<CategoryDto>();
        foreach (var category in categories)
        {
            categoriesDto.Add(new CategoryDto
            {
                Id = category.Id,
                Name = category.Name
            });
        }

        _mockCategoryRepository.Setup(r => r.ListAllAsync()).ReturnsAsync(categories);

        _mockCategoryRepository.Setup(r => r.DeleteAsync(It.IsAny<Category>())).Returns(Task.CompletedTask);


        _mockMapper.Setup(m => m.Map<List<CategoryDto>>(It.IsAny<List<Category>>())).Returns(categoriesDto);


        _mockCache.Setup(c => c.GetAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
          .ReturnsAsync((byte[])null);

        _mockCache.Setup(c => c.SetAsync(It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<DistributedCacheEntryOptions>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        //Act

        var deleteCategoryHandler = new GetCategoriesHandler(_mockCategoryRepository.Object, _mockMapper.Object, _mockGetCategoriesLogger.Object, _mockCache.Object);
        var actual = await deleteCategoryHandler.Handle(request, CancellationToken.None);
        var expected = categoriesDto;
        //Assert
        Assert.Equal(expected, actual);
    }
}
