using Microsoft.AspNetCore.Mvc;

namespace MvcBoard.Controllers.Models
{
    public class BoardViewParams
    {
        // [FromQuery]
        public int Category { get; set; } = 0;
        // [FromQuery]
        public int Page { get; set; } = 1;

        public string jwtToken { get; set; } = ""; // TODO 검토 후 지울 것
    }

}