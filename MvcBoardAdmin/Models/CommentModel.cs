using System.ComponentModel.DataAnnotations;

namespace MvcBoardAdmin.Models;

public partial class CommentModel
{
    public int CommentId { get; set; }

    [Required]
    public int PostId { get; set; }

    [Required]
    public int UserId { get; set; }

    public int? ParentId { get; set; }

    [Required]
    public string Contents { get; set; } = null!;

    public int Likes { get; set; }

    public bool IsAnonymous { get; set; }

    public DateTime CreateDate { get; set; }

    public DateTime? UpdateDate { get; set; }

    public DateTime? DeleteDate { get; set; }

    public bool IsDeleted { get; set; }
    public bool IsBlinded { get; set; }

}
