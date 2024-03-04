using System;
using System.Collections.Generic;

namespace MvcBoard.Models;

public partial class Post
{
    public int PostId { get; set; }

    public string Title { get; set; } = null!;

    public string? Contents { get; set; }

    public int UserId { get; set; }

    public int Likes { get; set; }

    public int Views { get; set; }

    public int Category { get; set; }

    public DateTime CreateDate { get; set; }

    public DateTime? UpdateDate { get; set; }

    public DateTime? DeleteDate { get; set; }
}
