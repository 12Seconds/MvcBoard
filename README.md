# MvcBoard

#####  2024.03.21
+ 게시물 작성, 수정에 인증 로직 추가
+ 게시물 삭제 구현
+ 댓글 삭제 구현
+ 처음 JWT 토큰 인증 적용했을 때 미들웨어를 이용해서 자동으로 인증하는 방식을 사용하려 했는데 이해가 부족했어서   
  수동으로 인증하는 방법으로 고쳐서 작업했는데.. 미들웨어 사용했어도 됐을 것 같다.
+ 만들어진 토큰을 클라이언트에서 저장한 후에, 모든 요청 시에 직접 Script 영역에서 헤더에 토큰을 담아 Ajax call을 해야되는줄 알았다..
  Controller 가 ControllerBase 를 상속받기 때문에 HttpContext, Request 객체에 접근할 수 있어 Cookie 를 얻어 인증 로직을 구현하였다.
-----
#####  2024.03.16
+ Cookie, Session, JSON Web Token
  사용자 인증을 구현하기 위해 여러 개념 학습하고, JWT 패키지 설치하여 적용함   
+ 토큰 생성 및 쿠키 저장, 로그인 및 인증 구현 (추가 개발 및 소스 정리 필요)

-----
#####  2024.03.15
+ Html.AjaxBeginForm (MVC 5 Ajax.BeginForm 대체)사용을 위한 종속성 패키지 설치
  - 컨트롤러의 액션 메서드를 Ajax Post 방식으로 호출하고 처리하기 위해 Ajax.BeginForm 를 사용하려 시도해보았지만 실패   
       1번) Microsoft.jQuery.Unobtrusive.Ajax (실패)   
       2번) Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation (실패)
  - Html.AjaxBeginForm 라는 이름으로 사용할 수 있는 패키지를 찾아서 설치 하였다. (설치 후 몇가지 세팅 필요)   
       3번) AspNetCore.Unobtrusive.Ajax (성공)
  - https://github.com/mjebrahimi/AspNetCore.Unobtrusive.Ajax/blob/master/README.md (세팅 참고할 것)
  - jquery.unobtrusive-ajax.js 파일은 1번 패키지를 설치했을 때 생긴 파일이었는데..   
  아마 3번 패키지만 설치해도 동작 될 것 같다. (파일 생길 듯, 안되면 1번도 같이 설치할 것)
+ 게시물 수정 기능 구현
+ 일반 계정 로그인, 회원가입 UI 구현
+ 일반 계정 회원가입 기능 구현

-----
#####  2024.03.14
+ 게시물 상세화면 댓글 작성 및 노출 구현 1/2
+ Enterprise Architecture 개념 학습
+ 의존성 주입 (DI & IoT Container) 개념 학습 및 적용
+ PartialView 개념 학습 및 적용
  
-----
#####  2024.03.13
+ [CI/CD] AWS RDS MS SQL Server 생성 및 연동
  - 빌드 환경(상용/개발)에 따라 ConnectionString 을 다르게 구성하고 사용할 수 있도록 처리 필요
+ 앱 구조 변경
  - 뷰 요청 처리 및 비즈니스 로직과 데이터 관련 코드를 분리
  - Controller - Service - Manager(DAO) - DB Manager(DBHelper) 계층 구조로 변경

-----
##### 2024.03.12
+ [CI/CD] AWS EC2 Instance 생성 및 웹서버 구동 (nginx 리버스 프록시)
  - Amazon Linux AMI 에서 .NET SDK 설치가 잘 진행되지 않아 Red Hat Enterprise Linux 9 (HVM) 으로 변경

-----
##### 2024.03.08
+ 게시판 페이지 인디케이터 UI 및 동작 구현

-----
##### 2024.03.07
+ 라우팅 및 파라미터 수정
+ 게시물 상세화면 페이지 구현 (댓글 및 일부 제외)
+ 게시판 페이징 처리 구현

-----
##### ....
