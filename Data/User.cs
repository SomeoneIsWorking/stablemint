using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace stablemint.Data;

public class User
{
    public int Id { get; set; }
    public string Email { get; set; }
}