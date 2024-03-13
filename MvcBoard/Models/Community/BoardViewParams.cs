using Microsoft.AspNetCore.Mvc;

namespace MvcBoard.Models.Community
{
    public class BoardViewParams
    {
        // [FromQuery]
        public int Category { get; set; } = 0;
        // [FromQuery]
        public int Page { get; set; } = 1;
    }

}