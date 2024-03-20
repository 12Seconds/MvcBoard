namespace MvcBoard.Managers.Models
{
    public class LogInResultParams
    {
        public int ResultCode { get; set; } = 0;
        public string ResultMsg { get; set; } = string.Empty;
        // public string ErrCode { get; set; } = string.Empty;

        // 토큰에 저장할 클레임 정보
        public int UserNumber { get; set; } = 0;
    }
}
