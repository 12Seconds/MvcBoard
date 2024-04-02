using MvcBoard.Models.Community;

namespace MvcBoard.Controllers.Response
{
    public class ReadBoardTypeResponse : CommonResponse
    {
        public List<BoardType> BoardTypes { get; set; }
    }
}
