using System.ComponentModel.DataAnnotations;

public class StaffUpdateDto
{
    [Required, StringLength(100)]
    public string FullName { get; set; } = null!;

    [Required]
    public DateTime Birthday { get; set; }

    [Required]
    [Range(1, 2)]
    public int Gender { get; set; }
}