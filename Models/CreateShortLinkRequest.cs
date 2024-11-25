
using System.ComponentModel.DataAnnotations;

namespace stablemint.Models;

public class CreateShortLinkModel
{
    [Required]
    [Url]
    public string Url { get; set; }
}