using Microsoft.EntityFrameworkCore;

public class StaffService
{
    private readonly AppDbContext _db;
    public StaffService(AppDbContext db) => _db = db;

    public async Task AddAsync(Staff staff)
    {
        _db.Staffs.Add(staff);
        await _db.SaveChangesAsync();
    }

    public async Task DeleteAsync(string staffId)
    {
        var s = await _db.Staffs.FindAsync(staffId);
        if (s != null)
        {
            _db.Staffs.Remove(s);
            await _db.SaveChangesAsync();
        }
    }

    public async Task<Staff?> GetByIdAsync(string staffId)
    {
        return await _db.Staffs.FindAsync(staffId);
    }

    public async Task UpdateAsync(Staff staff)
    {
        _db.Staffs.Update(staff);
        await _db.SaveChangesAsync();
    }

    public async Task<IEnumerable<Staff>> SearchAsync(string? staffId, string? fullName, int? gender, DateTime? from, DateTime? to)
    {
        var queryableStaffs = _db.Staffs.AsQueryable();
        if (!string.IsNullOrWhiteSpace(staffId))
            queryableStaffs = queryableStaffs.Where(s => s.StaffId.Contains(staffId));
        if (!string.IsNullOrWhiteSpace(fullName))
            queryableStaffs = queryableStaffs.Where(s => s.FullName.Contains(fullName));
        if (gender.HasValue)
            queryableStaffs = queryableStaffs.Where(s => s.Gender == gender.Value);
        if (from.HasValue)
            queryableStaffs = queryableStaffs.Where(s => s.Birthday >= from.Value.Date);
        if (to.HasValue)
            queryableStaffs = queryableStaffs.Where(s => s.Birthday <= to.Value.Date);


        return await queryableStaffs.OrderBy(s => s.StaffId).ToListAsync();
    }
}