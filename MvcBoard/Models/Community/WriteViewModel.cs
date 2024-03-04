namespace MvcBoard.Models.Community
{
    public class WriteViewModel
    {
        public WriteViewModel(int category = 0)
        {
                Category = category;
        }

        public int Category { get; set; }
    }
}
