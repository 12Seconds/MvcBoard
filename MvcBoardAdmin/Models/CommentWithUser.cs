namespace MvcBoardAdmin.Models
{
    public class CommentWithUser : CommentModel
    {
        public string UserName { get; set; }
        public bool IsPostWriter { get; set; } = false;
        public int Category { get; set; }
        public string BoardName { get; set; } = "";
    }
}
