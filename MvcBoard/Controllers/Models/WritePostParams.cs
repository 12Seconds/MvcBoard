namespace MvcBoard.Controllers.Models
{
    public class WritePostParams
    {
        public int? PostId { get; set; }
        public int? Category {  get; set; }

        public string jwtToken { get; set; } = ""; // TODO 검토 후 지울 것
    }
}
