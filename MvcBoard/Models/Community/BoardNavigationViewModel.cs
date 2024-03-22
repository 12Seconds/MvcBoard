namespace MvcBoard.Models.Community
{
    public class BoardNavigationViewModel
    {
        public List<BoardType> BoardTypes { get; set; } = new List<BoardType>();
        public int SelectedCategory { get; set; }
    }
}
