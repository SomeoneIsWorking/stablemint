using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace stablemint.Data;

[Table("User")]
public class User : IdentityUser
{
}