using Microsoft.AspNetCore.Mvc;
using MvcBoardAdmin.Controllers.Params;
using MvcBoardAdmin.Controllers.Response;
using MvcBoardAdmin.Services;

namespace MvcBoardAdmin.Controllers
{
    /* 유저 관리 컨트롤러 */
    public class MemberController : Controller
    {
        private readonly MemberService _memberService;
        public MemberController(MemberService memberService) 
        {
            _memberService = memberService;
        }

        public IActionResult Index()
        {
            // TODO 인증

            return View();
        }

        /* 유저 리스트 PartialView (검색) */
        [HttpGet]
        public IActionResult MemberListPartial(ReadMembersParams _params)
        {
            Console.WriteLine($"## MemberController >> MemberListPartial(SearchFilter: {_params.SearchFilter}, SearchWord: {_params.SearchWord}, Page: {_params.Page})");

            ReadMembersServiceParams ServiceParams = new ReadMembersServiceParams
            {
                ReadMembersParams = _params,
                ModelState = ModelState,
                HttpContext = HttpContext
            };

            ReadMembersResponse Response = _memberService.ReadMembers(ServiceParams);

            if (Response.ResultCode != 200)
            {
                // 필요한 경우 처리
            }

            return PartialView("_MemberList", Response.ViewModel); // TODO Question ViewModel 에 Response 넣기 VS Response 에 ViewModel 넣기
        }

        /* 유저 정보 에디터 PartialView */
        [HttpGet]
        public IActionResult MemberEditorPartial(int UserId = 0)
        {
            ReadMemberDetailServiceParams ServiceParams = new ReadMemberDetailServiceParams
            {
                UserId = UserId,
                ModelState = ModelState,
                HttpContext = HttpContext
            };

            // View 모델을 포함한 Response 객체를 뷰에 넘겨서 클라이언트에서 분기 처리 및 에러메세지 출력
            ReadMemberDetailResponse Response = _memberService.ReadMemberDetail(ServiceParams);
            
            return PartialView("_MemberEditor", Response);
        }

        /* 유저 정보 수정 */
        [HttpPost]
        public IActionResult Update(UpdateMemberParams _params)
        {
            UpdateMemberServiceParams ServiceParams = new UpdateMemberServiceParams
            {
                UpdateParams = _params,
                ModelState = ModelState,
                HttpContext = HttpContext
            };

            CommonResponse Response = _memberService.UpdateMember(ServiceParams);
            
            return Ok(Response);
        }

        /* 유저 삭제 */
        [HttpPost]
        public IActionResult Delete(int UserId = 0)
        {
            DeleteMemberServiceParams ServiceParams = new DeleteMemberServiceParams
            {
                UserId = UserId,
                ModelState = ModelState,
                HttpContext = HttpContext
            };

            CommonResponse Response = _memberService.DeleteMember(ServiceParams);

            return Ok(Response);
        }
    }
}
