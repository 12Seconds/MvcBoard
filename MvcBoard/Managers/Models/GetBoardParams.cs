namespace MvcBoard.Managers.Models
{
    public class GetBoardParams
    {
        public int Category { get; set; }
        public int Page { get; set; }
        public int Size { get; set; } = 20; // 한 페이지에 노출되는 게시물 수
    }
}
