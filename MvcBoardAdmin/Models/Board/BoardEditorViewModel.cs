namespace MvcBoardAdmin.Models.Board
{
    public class BoardEditorViewModel
    {
        public bool IsNew { get; set; }
        public BoardType Board { get; set; }

        // 현재 존재하는 부모 게시판들의 정보
        public List<BoardType> Parents { get; set; } = new List<BoardType>();
    }
}
