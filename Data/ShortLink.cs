
using System;

namespace stablemint.Data;

public class ShortLink
{
    public int Id { get; set; }
    public string ShortId { get; set; }
    public string LongUrl { get; set; }
    public int Clicks { get; set; }
    public DateTime CreatedAt { get; set; }
    public string UserId { get; set; }
    public User User { get; set; }
}