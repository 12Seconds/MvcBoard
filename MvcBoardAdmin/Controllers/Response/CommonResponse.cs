namespace MvcBoardAdmin.Controllers.Response
{
    public class CommonResponse
    {
        /// <summary>
        /// HTTP response status codes 와 별개로, 상황 별 ResultCode 에 대한 정의가 필요함
        /// 200: OK
        /// 201: 입력 값 오류
        /// </summary>
        public int ResultCode { get; set; } = 200;
        public string Message { get; set; } = "";
        public string ErrorMessage { get; set; } = "";
    }
}
