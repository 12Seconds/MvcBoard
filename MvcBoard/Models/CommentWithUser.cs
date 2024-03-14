namespace MvcBoard.Models
{
    public class CommentWithUser : Comment
    {
        public string UserName { get; set; }

        // TODO 날짜 형식 변환하여 string 으로 저장하는 필드 추가할 것
    }
}
