namespace MvcBoard.Models.Proifle
{
    // TODO User class 를 Models/Database 로 옮기고, User 폴더 만들어서 하위에 놓기.. User 네임 스페이스 겹침
    public class ProfileInfo
    {
        public int UserNumber { get; set; } = 0; // UserId
        public string Name { get; set; } = "";
        public int Image { get; set; } = 0;

        public int Point { get; set; } = 0;
        public int PostsCnt { get; set; } = 0;
        public int CommentsCnt { get; set; } = 0;
        public int BookmarksCnt { get; set; } = 0;
    }
}
