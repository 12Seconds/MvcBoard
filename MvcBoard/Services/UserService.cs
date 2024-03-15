using MvcBoard.Controllers.Models;
using MvcBoard.Managers;
using MvcBoard.Utills;

namespace MvcBoard.Services
{
    public class UserService
    {
        private readonly UserDataManager _dataManager;

        public UserService(UserDataManager dataManager)
        {
            _dataManager = dataManager;
        }

        // 로그인
        public void Login()
        {
            _dataManager.Login();
        }

        // 로그아웃
        public void Logout()
        {
            _dataManager.Logout();
        }

        // 회원 가입 (TODO return)
        public void SignUp(SignUpParams _params)
        {
            // 유효성 검증
            (bool IsValid, List<String> Strings) = Utillity.Vaildataion.CheckStrings(
                new List<string>() {
                    _params.Id, 
                    _params.Password, 
                    _params.Name
                });
            // (bool IsValid, List<String> Strings) = Utillity.Vaildataion.CheckStrings(new List<string>() { _params.UserId, _params.Password, _params.Name });

            if (IsValid)
            {
                _dataManager.SignUp(
                    new SignUpParams()
                    {
                        Id = Strings[0],
                        Password = Strings[1],
                        Name = Strings[2]
                    });
                // _dataManager.SignUp(new SignUpParams() { UserId = Strings[0], Password = Strings[1], Name = Strings[2] });
            }
            else
            {
                // TODO 뭐라도 하겠지 회산데
                // return
            }
        }

        // 회원 정보 조회
        public void GetUserInfo()
        {
            _dataManager.GetUserInfo();
        }
    }
}
