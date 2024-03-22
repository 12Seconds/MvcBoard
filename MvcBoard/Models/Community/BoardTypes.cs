namespace MvcBoard.Models.Community
{
    public class BoardType
    {
        public int BoardId { get; set; }
        public string BoardName { get; set; } = "";
        public int Category { get; set; }
        public int ParentCategory { get; set; }
        public int IsParent { get; set; }
        public int IconType { get; set; }
    }
}
