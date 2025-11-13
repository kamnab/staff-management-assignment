using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/staffs")]
public class StaffsController : ControllerBase
{
    private readonly StaffService _staffService;
    private readonly ExcelService _excelService;


    public StaffsController(StaffService staffService, ExcelService excelService)
    {
        _staffService = staffService;
        _excelService = excelService;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] StaffCreateDto dto)
    {
        var existingStaff = await _staffService.GetByIdAsync(dto.StaffId);
        if (existingStaff != null) return Conflict("StaffId already exists.");

        var staff = new Staff
        {
            StaffId = dto.StaffId,
            FullName = dto.FullName,
            Birthday = dto.Birthday,
            Gender = dto.Gender
        };

        await _staffService.AddAsync(staff);
        return CreatedAtAction(nameof(GetById), new { id = staff.StaffId }, new StaffDto
        {
            StaffId = dto.StaffId,
            FullName = dto.FullName,
            Birthday = dto.Birthday,
            Gender = dto.Gender
        });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        var staff = await _staffService.GetByIdAsync(id);
        if (staff == null) return NotFound();
        return Ok(new StaffDto
        {
            StaffId = staff.StaffId,
            FullName = staff.FullName,
            Birthday = staff.Birthday,
            Gender = staff.Gender
        });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] StaffUpdateDto dto)
    {
        var staff = await _staffService.GetByIdAsync(id);
        if (staff == null) return NotFound();
        staff.FullName = dto.FullName;
        staff.Birthday = dto.Birthday;
        staff.Gender = dto.Gender;
        await _staffService.UpdateAsync(staff);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        await _staffService.DeleteAsync(id);
        return NoContent();
    }

    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery] string? staffId, [FromQuery] string? fullName, [FromQuery] int? gender, [FromQuery] DateTime? from, [FromQuery] DateTime? to)
    {
        var staffList = await _staffService.SearchAsync(staffId, fullName, gender, from, to);
        return Ok(staffList.Select(staff => new StaffDto
        {
            StaffId = staff.StaffId,
            FullName = staff.FullName,
            Birthday = staff.Birthday,
            Gender = staff.Gender
        }));
    }

    [HttpGet("export/excel")]
    public async Task<IActionResult> ExportExcel([FromQuery] string? staffId, [FromQuery] string? fullName, [FromQuery] int? gender, [FromQuery] DateTime? from, [FromQuery] DateTime? to)
    {
        var foundItems = await _staffService.SearchAsync(staffId, fullName, gender, from, to);
        var bytes = _excelService.ExportToExcel(foundItems.Select(staff => new StaffDto
        {
            StaffId = staff.StaffId,
            FullName = staff.FullName,
            Birthday = staff.Birthday,
            Gender = staff.Gender
        }));

        return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "staffs.xlsx");
    }
}