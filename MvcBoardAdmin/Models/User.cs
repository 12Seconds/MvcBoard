namespace MvcBoardAdmin.Models
{
    public class User
    {
        public int UserId { get; set; }

        public string Id { get; set; } = null!;

        public string Password { get; set; } = null!;

        public string Name { get; set; } = null!;

        public int? Image { get; set; }
        public string Authority { get; set; } = "";
    }
}
