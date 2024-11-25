
using System.ComponentModel.DataAnnotations;

namespace stablemint.Models;

public class LoginModel
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
}