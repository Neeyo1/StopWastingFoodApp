using System.ComponentModel.DataAnnotations;

namespace API.DTOs;

public class RegisterDto
{
    [Required]
    [StringLength(20, MinimumLength = 6)]
    public required string Username { get; set; }

    [Required]
    [StringLength(20, MinimumLength = 8)]
    public required string Password { get; set; }

    [Required]
    [StringLength(20, MinimumLength = 3)]
    public required string KnownAs { get; set; }
}
