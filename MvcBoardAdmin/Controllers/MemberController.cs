using Microsoft.AspNetCore.Mvc;
using MvcBoardAdmin.Controllers.Params;
using MvcBoardAdmin.Controllers.Response;
using MvcBoardAdmin.Models;
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
            Console.WriteLine($"## MemberController >> MemberListPartial() SearchFilter: {_params.SearchFilter}, SearchWord: {_params.SearchWord}");

            ReadMembersServiceParams serviceParams = new ReadMembersServiceParams
            {
                ReadMembersParams = _params,
                ModelState = ModelState,
                HttpContext = HttpContext
            };

            ReadMembersResponse response = _memberService.ReadMembers(serviceParams);

            if (response.ResultCode != 200)
            {
                // 필요한 경우 처리
            }

            return PartialView("_MemberList", response.ViewModel);
        }
    }
}
