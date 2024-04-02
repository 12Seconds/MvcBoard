using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace MvcBoard.Controllers.Response
{
    public class CommonResponse
    {
        /// <summary>
        /// HTTP response status codes 와 별개로, 상황 별 ResultCode 에 대한 정의가 필요함
        /// 200: OK
        /// 201: 입력 값 오류
        /// 202: DB 오류
        /// 203: DB 실패 (결과가 없는 경우 - ex.수행 결과 영향을 받은 행이 없음)
        /// </summary>
        public int ResultCode { get; set; } = 200;
        public string Message { get; set; } = "";
        public List<string> ErrorFields { get; set; } = new List<string>();
        public List<string> ErrorMessages { get; set; } = new List<string>();

        public string ErrorSummary = "";
        public ModelStateDictionary ModelState { get; set; }
    }
}
