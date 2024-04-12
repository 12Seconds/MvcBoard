using Microsoft.AspNetCore.Mvc;
using MvcBoardAdmin.Controllers.Params;
using MvcBoardAdmin.Controllers.Response;
using MvcBoardAdmin.Filters;
using MvcBoardAdmin.Models.Member;
using MvcBoardAdmin.Services;

namespace MvcBoardAdmin.Controllers
{
    // TODO 일반 계정 (멤버 컨트롤러) 과 공통으로 사용하는 클래스 객체들 있음, 추후 분리 필요 (ReadMembersParams, ReadMembersServiceParams 등)

    /* 관리자 계정 관리 컨트롤러 */
    [AuthenticationFilter]
    [AuthorizationFilter]
    public class AdminMemberController : Controller
    {
        private readonly AdminMemberService _adminMemberService;
        public AdminMemberController(AdminMemberService adminMemberService)
        {
            _adminMemberService = adminMemberService;
        }

        /* 관리자 계정 관리 페이지 진입 */
        public IActionResult Index(MemberManageViewParams _params)
        {
            MemberManageViewModel Model = _adminMemberService.GetAdmMemberManageViewModel(/* _params */);

            return View(Model);
        }

        /* 관리자 계정 리스트 PartialView (검색) */
        [HttpGet]
        public IActionResult AdminMemberListPartial(ReadMembersParams _params)
        {
            Console.WriteLine($"## AdminMemberController >> AdminMemberListPartial(SearchFilter: {_params.SearchFilter}, SearchWord: {_params.SearchWord}, Page: {_params.Page})");

            ReadMembersServiceParams ServiceParams = new ReadMembersServiceParams
            {
                ReadMembersParams = _params,
                ModelState = ModelState,
                HttpContext = HttpContext
            };

            ReadAdminMembersResponse Response = _adminMemberService.ReadAdmMembers(ServiceParams);

            if (Response.ResultCode != 200)
            {
                // 필요한 경우 처리
            }

            return PartialView("_AdminMemberList", Response.ViewModel); // TODO ViewModel 에 Response 넣는 구조로 변경
        }

        /* 관리자 계정 정보 에디터 PartialView */
        [HttpGet]
        public IActionResult AdminMemberEditorPartial(int UserNo = 0)
        {
            ReadMemberDetailServiceParams ServiceParams = new ReadMemberDetailServiceParams
            {
                UserId = UserNo,
                ModelState = ModelState,
                HttpContext = HttpContext
            };

            // View 모델을 포함한 Response 객체를 뷰에 넘겨서 클라이언트에서 분기 처리 및 에러메세지 출력
            ReadAdminMemberDetailResponse Response = _adminMemberService.ReadAdmMemberDetail(ServiceParams);

            return PartialView("_AdminMemberEditor", Response);
        }

        /* 관리자 계정 정보 수정 */
        [HttpPost]
        public IActionResult Update(UpdateMemberParams _params)
        {
            UpdateMemberServiceParams ServiceParams = new UpdateMemberServiceParams
            {
                UpdateParams = _params,
                ModelState = ModelState,
                HttpContext = HttpContext
            };

            CommonResponse Response = _adminMemberService.UpdateAdmMember(ServiceParams);

            return Ok(Response);
        }

        /* 관리자 계정 삭제 (영구삭제가 아닌 보류삭제) */
        [HttpPost]
        public IActionResult Delete(int UserId = 0)
        {
            DeleteMemberServiceParams ServiceParams = new DeleteMemberServiceParams
            {
                UserId = UserId,
                ModelState = ModelState,
                HttpContext = HttpContext
            };

            CommonResponse Response = _adminMemberService.DeleteAdmMember(ServiceParams);

            return Ok(Response);
        }
    }
}
