# MvcBoard    

(2024.03 ~ )    
본 프로젝트는 C# ASP.NET MVC 및 MS-SQL Server 학습을 위한 게시판 클론 코딩 1인 프로젝트 입니다.   

+ 게시판 프로젝트 (MvcBoard)
  - Live URL: http://43.200.65.126/Community
  - 테스트 계정
    - ID: test / PW: test (또는 가입 가능)
  - 구현 기능
    - 일반 회원가입/로그인/로그아웃 (SNS 로그인 미구현)
    - 게시물 작성/수정/삭제 (페이지 처리)
    - 댓글 작성/삭제 (페이지 처리)

+ 관리 페이지 프로젝트 (MvcBoardAdmin)
  - Live URL: http://43.200.65.126:8080/
  - 테스트 계정
    - <span style='background-color: #fff5b1'>링크 참고 부탁드립니다.</span>   
      (https://www.notion.so/C-ASP-NET-MVC-82f4c047bb574e8d801f8e98a9602931?pvs=4#c21751954c984f2cbce2e49b94b2c5a1)
  - 구현 기능
    - 일반 유저 관리 (검색 및 조회, 수정, 삭제)
    - 관리자 계정 관리 (검색 및 조회, 수정, 삭제)
    - 게시판 관리 (조회 및 생성, 수정, 삭제, 순서 변경, 미리보기)
    - 게시물 관리 (검색 및 조회, 숨김, 삭제, 영구삭제, 게시판 이동)
    - 댓글 관리 (검색 및 조회, 숨김, 삭제, 영구삭제)

-----   

##### ( ↓ 개발 로그 ↓)   
  
#####  2024.05.08
[MvcBoardAdmin]
+ JWT Token 관련 수정
  - MvcBoard 와 MvcBoardAdmin 의 JWT 인증 토큰명 중복 문제 수정
  - AWS EC2(nginx) 배포 환경에서 쿠키가 저장되지 않던 문제 수정 (http 연결이므로 secure 옵션 값 false 설정)

-----
#####  2024.04.13
[MvcBoardAdmin]
+ 관리자 계정을 관리하는 페이지 추가 및 구현 (리스트 조회(검색), 수정, 삭제, 권한 변경)

-----
#####  2024.04.12
[MvcBoardAdmin]
+ 로그인 인증 필터 구현 및 적용 (ActionFilterAttribute, IAuthorizationFilter)
+ 권한 필터 구현 및 적용 (ActionFilterAttribute, IAuthorizationFilter)

-----
#####  2024.04.11
[MvcBoardAdmin]
+ 권한, 권한그룹, 매핑 테이블 추가
+ 관리자 계정 테이블 추가
+ 관리자 로그인/로그아웃 구현(JWT)

-----
#####  2024.04.10
[MvcBoardAdmin]
+ 댓글 관리 페이지
  - 댓글 수정 기능 구현 (숨김, 삭제/복구, 영구삭제 일괄 수정), 숨김, 삭제 버튼 개별 기능 구현
  - 댓글 상세 UI 수정
  - 유저, 게시물 관리에서 댓글 수 클릭 시 페이지 이동 처리
  
[MvcBoard]
+ 삭제 또는 숨김(블라인드) 처리된 댓글 표시 UI 적용
  - '삭제된 댓글입니다' / '블라인드 처리된 댓글입니다'

-----
#####  2024.04.09
[MvcBoardAdmin]
+ 댓글 관리 페이지
  - 댓글 리스트 조회(검색) 구현
  - 댓글 상세 화면 PartialView 추가, 댓글 상세 조회 기능 구현

-----
#####  2024.04.08
[MvcBoardAdmin]
+ 게시물 관리 페이지
  - 게시물 상세 화면 추가 (내용 볼 수 있도록)
+ 게시판 관리 페이지
  - UI 수정 (새 게시판 생성 버튼 위치)

-----
#####  2024.04.05
[MvcBoardAdmin]
+ 게시물 관리 페이지
  - 숨김, 삭제 버튼 및 동작 추가 (단일 수정)
+ 유저-게시물 관리 페이지 간 이동 (게시물 수 클릭 또는 유저 클릭)
+ PartialView 의 뷰모델 클래스 관련 리팩토링
  - 뷰를 구성하기 위해 데이터를 반환 받아야 하는 Get 요청의 경우 ViewModel 클래스에 Response 와 데이터를 포함하도록 구조 변경   
    기존엔 CommonResponse 를 상속 받은 Response 클래스 객체에 VIewModel 을 담는 구조였음   
    DataManager 에서도 Response 가 아닌 Result 라는 클래스 객체를 반환 하도록 수정함   
  - 결과만 반환하는 경우는 Response 반환 (UPDATE, DELETE 와 같은 DB 쿼리를 수행하는 메소드)
    
-----
#####  2024.04.04
[MvcBoardAdmin]
+ 유저, 게시물 관리 페이지 UI 수정 (게시물 제목에 공지 및 댓글 수 노출)
+ 게시물 관리 페이지
  - 게시물 검색 SP 쿼리 오류 수정
  - 수정 기능 구현 (게시판 이동, 숨김, 삭제/복구, 영구삭제 일괄 수정 기능)
  - UI 수정
  
[MvcBoard]
+ 게시물 UI 수정 (게시물 제목에 공지 및 댓글 수 노출)

-----
#####  2024.04.03
[MvcBoardAdmin]
+ 게시물 관리 페이지
  - 게시물 검색 (동적 쿼리) 및 리스트 출력 구현
  - 검색 기능이 2개의 조건을 필요로 하여서 IF 처리만으론 쿼리가 너무 길어져 동적 쿼리를 사용해 보았다. (그래도 길고 복잡하긴 하다)
  - (1) 게시판 필터(Category), (2) 검색어 필터 (글번호, 글제목, 내용, 아이디, 닉네임)

+ 레이아웃 Nav 탭 UI 수정
  - 현재 페이지 표시하도록 함

-----
#####  2024.04.02
[MvcBoardAdmin]
+ SQL 파일 코드가 너무 길어져서 분리 (SP, SP_Admin)
+ 게시판 관리 페이지   
  - 수정, 삭제 구현 및 UI 일부 수정
  - 게시판 데이터 획득 함수 수정 (Response 리턴으로), 게시판 메뉴 UI 수정

[MvcBoard]
+ Admin 개발하면서 게시판 메뉴 (부모-자식) 구조로 변경한 것 MvcBoard 에도 적용

-----
#####  2024.03.31
[MvcBoardAdmin]
+ 게시판 관리 페이지 추가   
  - 게시판 조회 (BoardType) 및 노출, 생성 구현
+ 게시판 카테고리 메뉴데이터를 DB에서 정렬하여 보내주는 SP 신규 작성   
  - 부모-자식(1-2Depth) 관계로 노출할 수 있도록 처리하였다.
  
-----
#####  2024.03.30
[MvcBoardAdmin]
+ 유저 테이블 수정 및 삭제된 유저 관련 처리

[MvcBoard]
+ 삭제된 유저 로그인 불가 처리

[공통]
+ 공지 게시판 조회 분리 (SP 추가)
+ Post 테이블 수정 (IsDeleted, IsBlinded, IsNotice 컬럼 추가)

-----
#####  2024.03.29
[MvcBoardAdmin]
+ 유저 관리 페이지 - 유저 리스트 페이지 이동 구현
+ 유저 관리 페이지 - 유저 수정, 삭제 구현

-----
#####  2024.03.28
[MvcBoardAdmin]
+ 관리자 프로젝트 로그인 페이지 UI 작업 (로그인 및 인증 구현중)
+ 유저 관리 페이지 추가 및 유저 리스트를 PartailView 로 만들고 DB 연동함
+ 유저 리스트 조회(검색) 구현 및 UI 작업
+ 다른 페이지에서도 공통으로 사용할 페이지 인디케이터 로직을 Utility 로 작성

-----
#####  2024.03.27
[MvcBoardAdmin]
+ 관리자 페이지 프로젝트 (MvcBoardAdmin) 생성 및 초기 설정 (Unobtrusive.Ajax 등)
+ 초기 파일 생성 및 설계

[MvcBoard]   
MvcBoard 에서 아직 구현 못한 자잘한 기능들도 많으나 기본 기능들은 거의 구현한 것 같다.
백엔드도 그렇고 C# .NET MVC 를 처음 접하여서 공부를 하며 만들다 보니 구조적으로 이상한 부분도 많고 아쉬운 부분도 많다.
만들어나가면서 설계도 계속 변하다 보니 수정량도 많아져서 손을 대기가 힘든 부분도 있었다. 아쉬운 부분들은 차차 수정해 나가기로 하고
코드적으로 지저분하고 구조적으로 아쉬웠던 부분들을 정리하며 새 프로젝트인 관리자 페이지를 만들기로 하자.

-----
#####  2024.03.26
+ 게시판을 구분(지정)하여 게시물을 작성할 수 있도록 함 (게시판 선택 항목에 DB 데이터 연동)
+ 현재 게시판 이름이 정상적으로 노출되도록 수정 (하드코딩 -> 데이터 연동)
+ UI 디자인 수정

-----
#####  2024.03.23
+ 댓글 UI, 로그인 유저 정보 UI(일부) 구현
+ 댓글 총 갯수가 아닌 현재 댓글 페이지의 댓글 수가 노출되던 것 수정
+ 로그인하지 않은 경우 댓글을 작성하지 않도록 처리

-----
#####  2024.03.22
+ 좌측 로그인 유저 정보 Partail View 로 작성
+ 로그아웃 구현
+ 게시판 카테고리(메뉴)를 하드코딩하지 않고 DB에 작성, 읽어서 메뉴를 구성하도록 함
  좌측 게시판 네비게이션 메뉴를 Partial View 로 작성
+ 게시판 카테고리(메뉴) 데이터는 자주 바뀌는 데이터가 아니므로 CommunityService.cs 에서 캐시 로직을 적용하여 DB 접근을 최소화 함
+ 인기 게시판 조회 SP를 새로 작성하여 기존 게시판 조회 SP와 분리   
  -> 현재는 로직이 거의 같지만, 추후 실시간/주간/월간 등 인기 게시물 분류 기준 및 필터링 기능 구현 시 많이 달라질 것
+ 댓글 삭제 후, 페이지 전체가 댓글 Partial View 로 갱신 되는 문제 수정 (하지만 아직 제대로 업데이트 되지 않음)

< Partial View >
1. 페이지를 구성하는 요소들 중 재사용 빈도가 높은 부분들을 Partial View 로 작성하고
2. 페이지 내부에 @Html.AjaxBeginForm 및 렌더 타겟 div 를 만들어 놓고
3. 페이지가 로드 되면 스크립트 영역에서 이를 subbmit -> Controller/ActionMethod -> 필요한 모델 데이터와 함께 return PartialView
4. 지정한 타겟 div 에 Partial View 렌더링 됨

  그런데 스크립트 영역에서 바닐라로 submit 을 하니까 전체 페이지가 PartialView 로 로드 되는 문제가 있었음.   
  JQeury 를 이용해서 form.submit 하니까 정상적으로 작동함 -> 원인은 아직 못찾았다.   

   $('#UserInformationForm').submit();  (정상 작동)   
   document.getElementById('UserInformationForm').submit(); (비정상 작동)   
   
< 중첩 레이아웃 (?) >   
로그인 정보 패널과 게시판 카테고리 메뉴를 Partial View 로 변경하였지만
그래도 해당 Partial View 를 로드하는 코드를 중복해서 작성해야 하는 것은 여전하다.
기본 _Layout.cshtml 내에 또 다른 레이아웃을 작성해서 중첩 레이아웃 구조(?)로 처음에 설계를 했어야 했던 것 같다.

-----
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
