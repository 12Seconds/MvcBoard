using MvcBoardAdmin.Models;

namespace MvcBoardAdmin.Controllers.Response
{
    public class ReadBoardTypeResponse : CommonResponse
    {
        public List<BoardType> BoardTypes { get; set; }
    }
}
