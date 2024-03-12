# MvcBoard

-----
#####  2024.03.13
+ [CI/CD] AWS RDS MS SQL Server 생성 및 연동
  - 빌드 환경(상용/개발)에 따라 ConnectionString 을 다르게 구성하고 사용할 수 있도록 처리 필요
+ 앱 구조 변경
  - 뷰 요청 처리 및 비즈니스 로직과 데이터 관련 코드를 분리
  - Controller - Service - Manager(DAO) - DB Manager(DBHelper) 계층 구조로 변경

-----
##### 2024.03.12
+ [CI/CD] AWS EC2 Instance 생성 및 상용 배포   
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
