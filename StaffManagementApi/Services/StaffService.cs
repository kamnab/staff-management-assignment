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

    public async Task<IEnumerable<Staff>> SearchAsync(string? staffId, int? gender, DateTime? from, DateTime? to)
    {
        var q = _db.Staffs.AsQueryable();
        if (!string.IsNullOrWhiteSpace(staffId))
            q = q.Where(s => s.StaffId.Contains(staffId));
        if (gender.HasValue)
            q = q.Where(s => s.Gender == gender.Value);
        if (from.HasValue)
            q = q.Where(s => s.Birthday >= from.Value.Date);
        if (to.HasValue)
            q = q.Where(s => s.Birthday <= to.Value.Date);


        return await q.OrderBy(s => s.StaffId).ToListAsync();
    }
}