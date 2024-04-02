using MvcBoard.Controllers.Response;

namespace MvcBoard.Models.Community
{
    public class BoardNavigationViewModel
    {
        public CommonResponse Response { get; set; } = new CommonResponse();
        public List<BoardType> BoardTypes { get; set; } = new List<BoardType>();
        public int SelectedCategory { get; set; }
    }
}
