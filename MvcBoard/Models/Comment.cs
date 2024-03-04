using System;
using System.Collections.Generic;

namespace MvcBoard.Models;

public partial class Comment
{
    public int CommentId { get; set; }

    public int PostId { get; set; }

    public int UserId { get; set; }

    public int? ParentId { get; set; }

    public string Contents { get; set; } = null!;

    public int Likes { get; set; }

    public bool IsAnonymous { get; set; }

    public DateTime CreateDate { get; set; }

    public DateTime? UpdateDate { get; set; }

    public DateTime? DeleteDate { get; set; }
}
