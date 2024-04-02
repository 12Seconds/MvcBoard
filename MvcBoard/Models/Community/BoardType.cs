namespace MvcBoard.Models.Community
{
    public class BoardType
    {
        public int BoardId { get; set; }
        public string BoardName { get; set; } = "";
        public int Category { get; set; }
        public int ParentCategory { get; set; }
        public bool IsParent { get; set; }
        public int IconType { get; set; }
        public bool IsWritable { get; set; }
        public int ShowOrder { get; set; }
        public bool IsDeleted { get; set; }
        public List<BoardType> Children { get; } = new List<BoardType>();
    }
}
