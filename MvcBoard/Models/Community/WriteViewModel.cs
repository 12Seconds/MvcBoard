using Microsoft.AspNetCore.Identity;

namespace MvcBoard.Models.Community
{
    public class WriteViewModel
    {
        public Post PostData { get; set; } = new Post();
        public List<BoardType> BoardTypes { get; set; } = new List<BoardType>();

        // public int SelectedCategory { get; set; }
    }
}
