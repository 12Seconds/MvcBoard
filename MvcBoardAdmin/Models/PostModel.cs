using System.ComponentModel.DataAnnotations;

namespace MvcBoardAdmin.Models;

public partial class PostModel // Post
{
    public int PostId { get; set; }

    [Required(ErrorMessage = "제목이 올바르지 않습니다.")] // errorMessage 출력하려면 input tag 뒤에 <span asp-validation-for="필드명"></span> 필요
    public string Title { get; set; } = null!;

    [Required(ErrorMessage = "내용을 입력해주세요.")]
    public string? Contents { get; set; }

    /*[Required]*/
    public int UserId { get; set; } // TODO UserNumber

    public int Likes { get; set; }

    public int Views { get; set; }

    [Required(ErrorMessage = "게시판을 선택해주세요.")]
    public int Category { get; set; }

    public DateTime? CreateDate { get; set; }

    public DateTime? UpdateDate { get; set; }

    public DateTime? DeleteDate { get; set; }

    public bool IsDeleted { get; set; }
    public bool IsBlinded { get; set; }
    public bool IsNotice { get; set; }
}
