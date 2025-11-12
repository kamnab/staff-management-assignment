using System.ComponentModel.DataAnnotations;

public class Staff
{
    [Key]
    [StringLength(8)]
    public string StaffId { get; set; } = null!;

    [Required]
    [StringLength(100)]
    public string FullName { get; set; } = null!;

    [Required]
    public DateTime Birthday { get; set; }

    [Required]
    public int Gender { get; set; } // 1: Male, 2: Female
}