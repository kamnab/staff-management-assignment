using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class StaffsControllerTests
{
    private readonly StaffsController _controller;
    private readonly StaffService _staffService;
    private readonly ExcelService _excelService;
    private readonly AppDbContext _dbContext;

    private AppDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString()) // unique DB per test
            .Options;
        return new AppDbContext(options);
    }

    public StaffsControllerTests()
    {
        _dbContext = CreateDbContext();
        _staffService = new StaffService(_dbContext);
        _excelService = new ExcelService(); // real implementation
        _controller = new StaffsController(_staffService, _excelService);
    }

    [Fact]
    public async Task Create_ReturnsCreated_WhenStaffDoesNotExist()
    {
        var dto = new StaffCreateDto
        {
            StaffId = "ST001",
            FullName = "Alice",
            Birthday = new DateTime(1990, 1, 1),
            Gender = 2
        };

        var result = await _controller.Create(dto);

        var createdResult = Assert.IsType<CreatedAtActionResult>(result);
        var returnDto = Assert.IsType<StaffDto>(createdResult.Value);
        Assert.Equal("Alice", returnDto.FullName);
    }

    [Fact]
    public async Task Create_ReturnsConflict_WhenStaffExists()
    {
        var staff = new Staff { StaffId = "ST002", FullName = "Bob", Birthday = DateTime.Now, Gender = 1 };
        await _staffService.AddAsync(staff);

        var dto = new StaffCreateDto
        {
            StaffId = "ST002",
            FullName = "Bob",
            Birthday = DateTime.Now,
            Gender = 1
        };

        var result = await _controller.Create(dto);

        var conflictResult = Assert.IsType<ConflictObjectResult>(result);
        Assert.Equal("StaffId already exists.", conflictResult.Value);
    }

    [Fact]
    public async Task GetById_ReturnsOk_WhenStaffExists()
    {
        var staff = new Staff { StaffId = "ST003", FullName = "Charlie", Birthday = DateTime.Now, Gender = 1 };
        await _staffService.AddAsync(staff);

        var result = await _controller.GetById("ST003");

        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnDto = Assert.IsType<StaffDto>(okResult.Value);
        Assert.Equal("Charlie", returnDto.FullName);
    }

    [Fact]
    public async Task GetById_ReturnsNotFound_WhenStaffDoesNotExist()
    {
        var result = await _controller.GetById("NON_EXIST");
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Update_ReturnsNoContent_WhenStaffUpdated()
    {
        var staff = new Staff { StaffId = "ST004", FullName = "Dana", Birthday = DateTime.Now, Gender = 2 };
        await _staffService.AddAsync(staff);

        var dto = new StaffUpdateDto { FullName = "Dana Updated", Birthday = staff.Birthday, Gender = 2 };

        var result = await _controller.Update("ST004", dto);

        Assert.IsType<NoContentResult>(result);
        var updated = await _staffService.GetByIdAsync("ST004");
        Assert.Equal("Dana Updated", updated!.FullName);
    }

    [Fact]
    public async Task Delete_ReturnsNoContent()
    {
        var staff = new Staff { StaffId = "ST005", FullName = "Eve", Birthday = DateTime.Now, Gender = 2 };
        await _staffService.AddAsync(staff);

        var result = await _controller.Delete("ST005");

        Assert.IsType<NoContentResult>(result);
        var deleted = await _staffService.GetByIdAsync("ST005");
        Assert.Null(deleted);
    }

    [Fact]
    public async Task Search_ReturnsFilteredResults()
    {
        await _staffService.AddAsync(new Staff { StaffId = "S1", FullName = "Anna", Birthday = DateTime.Now, Gender = 2 });
        await _staffService.AddAsync(new Staff { StaffId = "S2", FullName = "Ben", Birthday = DateTime.Now, Gender = 1 });

        var getStaffId = await _controller.Search("S1", null, null, null, null);
        var okResult = Assert.IsType<OkObjectResult>(getStaffId);
        var list = Assert.IsAssignableFrom<IEnumerable<StaffDto>>(okResult.Value);
        Assert.Equal("S1", list.First().StaffId);

        var getFullName = await _controller.Search(null, "Anna", null, null, null);
        okResult = Assert.IsType<OkObjectResult>(getFullName);
        list = Assert.IsAssignableFrom<IEnumerable<StaffDto>>(okResult.Value);
        Assert.Equal("Anna", list.First().FullName);

        var getGender = await _controller.Search(null, null, 2, null, null);
        okResult = Assert.IsType<OkObjectResult>(getGender);
        list = Assert.IsAssignableFrom<IEnumerable<StaffDto>>(okResult.Value);
        Assert.Equal("Anna", list.First().FullName);

        var allStaff = await _controller.Search(null, null, null, null, null);
        okResult = Assert.IsType<OkObjectResult>(allStaff);
        list = Assert.IsAssignableFrom<IEnumerable<StaffDto>>(okResult.Value);
        Assert.Equal(3, list.Count());
        Assert.Equal("Anna", list.First().FullName);
    }

    [Fact]
    public async Task ExportExcel_ReturnsFile()
    {
        await _staffService.AddAsync(new Staff { StaffId = "S10", FullName = "Alice", Birthday = DateTime.Now, Gender = 2 });

        var result = await _controller.ExportExcel(null, null, null, null, null);

        var fileResult = Assert.IsType<FileContentResult>(result);
        Assert.Equal("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileResult.ContentType);
        Assert.Equal("staffs.xlsx", fileResult.FileDownloadName);
        Assert.True(fileResult.FileContents.Length > 0);
    }
}
