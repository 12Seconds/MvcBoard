namespace MvcBoardAdmin.Models
{
    public class AdmUser
    {
        public int UserNo { get; set; }
        public string Id { get; set; } = "";
        public string Password { get; set; } = "";
        public string Name { get; set; } = "";
        public int Image { get; set; }
        public string AuthorityGroup { get; set; } = "";
        public bool IsDeleted { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? DeleteDate { get; set; }

    }
}
