use [MVC_BOARD_DB]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- TODO 삭제된 게시물 필터링, 프로시저 병합(?)할 수도 있음


-- #####################################################################################################################################

/* SP 페이지 조회 */
ALTER PROCEDURE ReadPost
	@Category int = 0,
	@Page int = 1,
	@Size int = 0 -- 한 페이지에 노출되는 게시물 수 (default: 20)
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @Offset INT
	DECLARE @PageSize INT
	DECLARE @PageIndex INT

	-- 유효하지 않은 값 예외처리
	IF (@Size > 0)
		SET @PageSize = @Size
	ELSE
		SET @PageSize = 20

	IF (@Page < 1) 
		SET @PageIndex = 1
	ELSE 
		SET @PageIndex = @Page

	SET @Offset = (@PageIndex - 1) * @PageSize

	-- 페이지 조회
	/* 탈퇴한 유저의 게시물도 노출해야 할 것 같아서 LEFT OUTER JOIN 사용 */
	IF (@Category = 0)
		SELECT a.*, b.Name, c.BoardName
		FROM [Post] AS a
		LEFT OUTER JOIN [User] AS b ON a.UserId = b.UserId 
		LEFT OUTER JOIN [BoardType] AS c ON a.Category = c.Category
		ORDER BY PostId DESC 
		OFFSET @Offset ROWS 
		FETCH NEXT @PageSize ROWS ONLY
	ELSE
		SELECT a.*, b.Name, c.BoardName
		FROM [Post] AS a
		LEFT OUTER JOIN [User] AS b ON a.UserId = b.UserId
		LEFT OUTER JOIN [BoardType] AS c ON a.Category = c.Category
		WHERE a.Category = @Category
		ORDER BY PostId DESC
		OFFSET @Offset ROWS
		FETCH NEXT @PageSize ROWS ONLY

	-- 총 페이지 수 계산
	DECLARE @PostCount INT
	DECLARE @PageCount INT

	IF (@Category = 0)
		SELECT @PostCount = COUNT(*) FROM Post
	ELSE 
		SELECT @PostCount = COUNT(*) FROM Post WHERE Category = @Category

	IF @PostCount % @PageSize = 0
		SET @PageCount = @PostCount / @PageSize
    ELSE
        SET @PageCount = @PostCount / @PageSize + 1

		SELECT @PageCount AS TotalPageCount
	
END
GO

/* SP 페이지 조회 - 인기 게시판 */
ALTER PROCEDURE ReadHotPost
	@Category int = 0,
	@Page int = 1,
	@Size int = 0 -- 한 페이지에 노출되는 게시물 수 (default: 20)
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @Offset INT
	DECLARE @PageSize INT
	DECLARE @PageIndex INT

	-- 유효하지 않은 값 예외처리
	IF (@Size > 0)
		SET @PageSize = @Size
	ELSE
		SET @PageSize = 20

	IF (@Page < 1) 
		SET @PageIndex = 1
	ELSE 
		SET @PageIndex = @Page

	SET @Offset = (@PageIndex - 1) * @PageSize

	-- 페이지 조회
	/* 탈퇴한 유저의 게시물도 노출해야 할 것 같아서 LEFT OUTER JOIN 사용 */
	IF (@Category = 0)
		SELECT a.*, b.Name, c.BoardName
		FROM [Post] AS a
		LEFT OUTER JOIN [User] AS b ON a.UserId = b.UserId
		LEFT OUTER JOIN [BoardType] AS c ON a.Category = c.Category
		WHERE [Views] > 9 -- 인기 게시물의 조건은 (실시간, 주간, 월간)에 따라, 또 댓글이나 좋아요 수에 따라서도 달라질 수 있다. 지금은 단순하게 9로 하자
		ORDER BY PostId DESC 
		OFFSET @Offset ROWS 
		FETCH NEXT @PageSize ROWS ONLY
	ELSE
		SELECT a.*, b.Name, c.BoardName
		FROM [Post] AS a
		LEFT OUTER JOIN [User] AS b ON a.UserId = b.UserId
		LEFT OUTER JOIN [BoardType] AS c ON a.Category = c.Category
		WHERE a.Category = @Category AND [Views] > 9 -- 인기 게시물 조건
		ORDER BY PostId DESC
		OFFSET @Offset ROWS
		FETCH NEXT @PageSize ROWS ONLY

	-- 총 페이지 수 계산
	DECLARE @PostCount INT
	DECLARE @PageCount INT

	IF (@Category = 0)
		SELECT @PostCount = COUNT(*) FROM Post WHERE [Views] > 9 -- 인기 게시물 조건
	ELSE 
		SELECT @PostCount = COUNT(*) FROM Post WHERE Category = @Category AND [Views] > 9 -- 인기 게시물 조건

	IF @PostCount % @PageSize = 0
		SET @PageCount = @PostCount / @PageSize
    ELSE
        SET @PageCount = @PostCount / @PageSize + 1

		SELECT @PageCount AS TotalPageCount
	
END
GO

/* SP 게시물 조회 (by PostId) TODO 댓글 데이터 조인? */
ALTER PROCEDURE ReadPostById
	@PostId int
AS
BEGIN
	SET NOCOUNT ON;
	/* 탈퇴한 유저의 게시물도 노출해야 할 것 같아서 LEFT OUTER JOIN 사용 */
	SELECT a.*, b.UserId, b.Id, b.Name, b.Image, c.BoardName
	FROM [Post] AS a 
	LEFT OUTER JOIN [User] AS b ON a.UserId = b.UserId 
	LEFT OUTER JOIN [BoardType] AS c ON a.Category = c.Category
	WHERE PostId = @PostId;
END
GO

/* SP 게시물 작성 */
ALTER PROCEDURE CreatePost
	@Title nvarchar(100),
	@Contents nvarchar(MAX),
	@UserId int,
	--@Likes int,
	--@Views int,
	@Category int = 1,-- default 1: 자유게시판
	@CreateDate datetime2
	--@UpdateDate datetime2,
	--@DeleteDate datetime2
AS
BEGIN
	SET NOCOUNT ON;
	INSERT INTO Post (Title, Contents, UserId, Likes, Views, Category, CreateDate, UpdateDate, DeleteDate) 
	VALUES (@Title, @Contents, @UserId, /*@Likes, @Views */ 0, 0, @Category, @CreateDate, null, null);
END
GO

/* SP 게시물 수정 */
ALTER PROCEDURE UpdatePost
	@PostId int,
	@Title nvarchar(100),
	@Contents nvarchar(MAX),
	@Likes int,
	@Views int,
	@Category int,
	@UpdateDate datetime2 -- TODO 필요 없을 것 같은데
AS
BEGIN
	SET NOCOUNT ON;
	UPDATE Post SET
		Title = @Title,
		Contents = @Contents,
		Likes = @Likes,
		Views = @Views,
		Category = @Category,
		UpdateDate = GETDATE() -- UTC 로 해야 하나? (클라이언트에서 넘길 수도.. 국가별 시차)
	WHERE PostId = @PostId;
END
GO

/* SP 게시물 삭제 */
ALTER PROCEDURE DeletePost
	@PostId int,
	@forceDelete int = 1 -- TODO 0으로 변경
AS
BEGIN
	SET NOCOUNT ON;
	IF (@forceDelete = 1)
		DELETE FROM Post WHERE PostId = @PostId;
	ELSE
		UPDATE Post SET DeleteDate = GETDATE() WHERE PostId = @PostId; -- TODO GETUTCDATE (UTC 로 해야 하나?)
END
GO

-- #####################################################################################################################################

/* SP 댓글 조회 */
/*
CREATE PROCEDURE ReadComment
	@PostId int
AS
BEGIN
	SET NOCOUNT ON;
	-- 댓글도 Page 별로 호출할 필요가 있을까
	SELECT * FROM Comment WHERE PostId = @PostId
END
GO
*/

/* SP 댓글 조회 (페이지 단위) */
ALTER PROCEDURE ReadComment
	@PostId int = 0,
	@Page int = 1,
	@Size int = 0 -- 한 페이지에 노출되는 게시물 수 (default: 20)
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @Offset INT
	DECLARE @PageSize INT
	DECLARE @PageIndex INT

	-- 유효하지 않은 값 예외처리
	IF @PostId < 1
		RETURN

	IF (@Size > 0)
		SET @PageSize = @Size
	ELSE
		SET @PageSize = 20

	IF (@Page < 1) 
		SET @PageIndex = 1
	ELSE 
		SET @PageIndex = @Page

	SET @Offset = (@PageIndex - 1) * @PageSize

	-- 댓글 조회
	/* 탈퇴한 유저의 댓글도 노출해야 할 것 같아서 LEFT OUTER JOIN 사용 */

	SELECT a.*, b.Name 
	FROM [Comment] AS a LEFT OUTER JOIN [User] AS b ON a.UserId = b.UserId
	WHERE PostId = @PostId
	ORDER BY CommentId ASC
	OFFSET @Offset ROWS
	FETCH NEXT @PageSize ROWS ONLY

	-- 총 페이지 수 계산
	DECLARE @CommentCount INT
	DECLARE @PageCount INT

	SELECT @CommentCount = COUNT(*) FROM Comment WHERE PostId = @PostId

	IF @CommentCount % @PageSize = 0
		SET @PageCount = @CommentCount / @PageSize
    ELSE
        SET @PageCount = @CommentCount / @PageSize + 1

		SELECT @PageCount AS TotalPageCount

		SELECT @CommentCount AS TotalCommentCount
END
GO

/* SP 댓글 생성 */
ALTER PROCEDURE CreateComment
	@PostId int,
	@UserId int,
	@ParentId int = 0,
	@Contents nvarchar(MAX),
	-- @Likes int, -- 따로 댓글 좋아요 테이블 쓸거면 이게 필요한가? count 만 세면 될텐데
	@IsAnonymous bit,
	@CreateDate datetime2
AS
BEGIN
	SET NOCOUNT ON;

	-- TODO 유효하지 않은 값 예외처리
	-- 뭐가 있을까 Contents 말고는 딱히 없을 것 같은데

	IF @PostId > 0 AND @UserId > 0
		INSERT INTO Comment(PostId, UserId, ParentId, Contents, IsAnonymous, CreateDate) 
		VALUES (@PostId, @UserId, @ParentId, @Contents, @IsAnonymous, @CreateDate);
END
GO

/* SP 댓글 수정 */
ALTER PROCEDURE UpdateComment
	@CommentId int,
	@Contents nvarchar(MAX),
	@Likes int, -- todo
	@IsAnonymous bit,
	@UpdateDate datetime2
AS
BEGIN
	SET NOCOUNT ON;
	UPDATE Comment SET
		Contents = @Contents,
		Likes = @Likes,
		IsAnonymous = @IsAnonymous,
		UpdateDate = GETDATE() -- UTC 로 해야 하나? (클라이언트에서 넘길 수도.. 국가별 시차)
	WHERE CommentId = @CommentId;
END
GO

/* SP 댓글 삭제 */
-- 바로 삭제하지 않고, 며칠 보류 후 삭제시키는 방법도 좋음, 이러면 DeleteDate 가 null 인 경우만 Select 하여 출력 필요함
ALTER PROCEDURE DeleteComment
	@CommentId int,	
	@forceDelete int = 1 -- TODO 0으로 변경
AS
BEGIN
	SET NOCOUNT ON;
	IF (@forceDelete = 1)
		DELETE FROM Comment WHERE CommentId = @CommentId;
	ELSE
		UPDATE Comment SET DeleteDate = GETDATE() WHERE CommentId = @CommentId; -- TODO GETUTCDATE (UTC 로 해야 하나?)
END
GO

-- #####################################################################################################################################

/* SP 유저 조회 */
CREATE PROCEDURE ReadUser
	@UserId int
AS
BEGIN 
	SET NOCOUNT ON;
	SELECT * FROM [User] WHERE UserId = @UserId;
END
GO

-- 테스트
EXEC ReadUser 1;
GO

/* SP 유저 생성 */
CREATE PROCEDURE CreateUser
	@Id varchar(50),
	@Password varchar(50),
	@Name nvarchar(50),
	@Image int
AS
BEGIN 
	SET NOCOUNT ON;
	INSERT INTO [User](Id, Password, Name, Image) 
	VALUES (@Id, @Password, @Name, @Image);
END
GO

-- 테스트
EXEC CreateUser 'admin', 'admin132', '운영자', 0;
GO

/* SP 유저 수정 */
CREATE PROCEDURE UpdateUser
	@UserId int,
	@Password varchar(50),
	@Name nvarchar(50),
	@Image int
AS
BEGIN
	SET NOCOUNT ON;
	UPDATE [User] SET
		Password = @Password,
		Name = @Name,
		Image = @Image
	WHERE UserId = @UserId;
END
GO

/* SP 유저 삭제 */
CREATE PROCEDURE DeleteUser
	@UserId int,
	@forceDelete int = 1 -- TODO 0으로 변경
AS
BEGIN
	SET NOCOUNT ON;
	IF (@forceDelete = 1)
		DELETE FROM [User] WHERE UserId = @UserId;
	-- ELSE (TODO)
		-- UPDATE [User] SET DeleteDate = GETDATE() WHERE UserId = @UserId; -- TODO GETUTCDATE (UTC 로 해야 하나?)
END
GO

/* SP 유저 로그인 */
ALTER PROCEDURE UserLogIn
	@Id varchar(50),
	@Password varchar(50)
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @Result INT;

	IF LEN(@Id) = 0 OR LEN(@Password) = 0
    BEGIN
        -- 입력값 오류
        SELECT -1 AS Result, 0 AS UserNumber;
        RETURN;
    END

	SELECT @Result = COUNT(*)
	FROM [User]
	WHERE Id = @Id AND [Password] = @Password;

	IF @Result = 1
	BEGIN
		-- 로그인 정보가 맞으면, 결과와 함께 부가 정보 (Id) 반환
		SELECT 1 AS Result, [User].UserId AS UserNumber
		FROM [User]
		WHERE Id = @Id; -- 성공
	END

	ELSE
		SELECT 0 AS Result, 0 AS UserNumber; -- 실패
END

-- #####################################################################################################################################

/* SP 게시판 타입(메뉴) 조회 */
CREATE PROCEDURE ReadBoardTypes
AS
BEGIN
	SET NOCOUNT ON;
	SELECT * FROM [BoardType]
END
GO

-- 테스트
EXEC ReadBoardTypes

/* SP 게시판 타입(메뉴) 생성 */
ALTER PROCEDURE CreateBoardType
	@BoardName varchar(50),
	@Category int = -1,
	@ParentCategory int = -1,
	@IsParent int = 0,
	@IconType int = 0,
	@IsWritable bit = 0
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO BoardType(BoardName, Category, ParentCategory, IsParent, IconType, IsWritable) 
	VALUES (@BoardName, @Category, @ParentCategory, @IsParent, @IconType, @IsWritable);
END
GO

-- 게시판 테이터 생성 (필수)

EXEC CreateBoardType '전체 게시판', '0', '0', '0', '1', '0'
EXEC CreateBoardType '인기 게시판', '1', '0', '0', '2', '0'
EXEC CreateBoardType '공지 게시판', '2', '0', '0', '3', '0'
EXEC CreateBoardType '팀원 모집', '3', '0', '0', '4', '1'

EXEC CreateBoardType '일반 커뮤니티', '4', '0', '1', '5', '0'
EXEC CreateBoardType '자유 게시판', '41', '4', '0', '0', '1'
EXEC CreateBoardType '질문 답변', '42', '4', '0', '0', '1'
EXEC CreateBoardType '오픈 마이크', '43', '4', '0', '0', '1'
EXEC CreateBoardType '정보 게시판', '44', '4', '0', '0', '1'

EXEC CreateBoardType '전공 커뮤니티', '6', '0', '1', '6', '0'
EXEC CreateBoardType '입시', '61', '6', '0', '0', '1'
EXEC CreateBoardType '실용음악과', '62', '6', '0', '0', '1'


-- #####################################################################################################################################

-- 테스트 --

-- Post
EXEC CreatePost '아이유 신곡 Love wins all 너무 좋네요', '너무 취향 저격이라 매일 듣고 있어요', 1, 0, 0, 1, '2024-02-27 16:30:27.0000000', null, null;
EXEC CreatePost '같이 밴드하실 분 (보컬) 찾습니다', '저는 기타를 치고 있고 미디 작업이 가능합니다. 지역은 서울 입니다.', 2, 0, 0, 1, '2024-02-27 16:35:27.0000000', null, null;
EXEC CreatePost '비비 밤양갱 커버해봤어요!!', '유튜브 링크 @@ 듣고 마음에 드시면 좋아요 및 피드백 부탁드려요!', 3, 0, 0, 1, '2024-02-27 16:40:27.0000000', null, null;
EXEC CreatePost '작곡/ 미디 레슨 합니다 (합정)', '1회 무료 상담 해드립니다. 취미반/입시반 모두 운영하니 편하게 연락주세요. 010-4xxx-8xxx', 4, 0, 0, 1, '2024-02-27 16:45:27.0000000', null, null;
EXEC CreatePost '[공지] 뮤직 커뮤니티 게시판 이용 안내', '공지사항 입니다.', 999, 100, 1000, 99, '2024-02-27 16:20:00.0000000', null, null;
EXEC CreatePost '아이유 신곡 Love wins all 너무 좋네요', '너무 취향 저격이라 매일 듣고 있어요', 1, 0, 0, 1, '2024-02-27 16:30:27.0000000', null, null;

DECLARE @currentTime DATETIME
SET @currentTime = GETDATE()
EXEC CreatePost '페이지 처리 테스트용 데이터', '테스트용', 1, 1, @currentTime;

EXEC ReadPost;
EXEC ReadPost 0;
EXEC ReadPost 99;

EXEC UpdatePost 6, '어이유 신곡 Love wins all 너무 좋네요', '너무 취향 저격이라 매일 듣고 있어요', 1, 0, 0, 1, '2024-02-27 16:30:27.0000000', null, null;
EXEC DeletePost 6, 1;

-- Comment
EXEC CreateComment 1, 1, 0, "저도 매일 들어요", 0, 0, '2024-02-28 16:30:27.0000000', null, null;
EXEC ReadComment 1;
EXEC UpdateComment 1, '저도 매일 들어요 (수정됨)', 1, 0, '2024-02-28 16:30:27.0000000';
EXEC DeleteComment 6, 1;

-- #####################################################################################################################################

/* MvcBoardAdmin 관련 SP */

--TODO 
/* SP 관리자 로그인 */
/*
ALTER PROCEDURE UserLogIn
	@Id varchar(50),
	@Password varchar(50)
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @Result INT;

	IF LEN(@Id) = 0 OR LEN(@Password) = 0
    BEGIN
        -- 입력값 오류
        SELECT -1 AS Result, 0 AS UserNumber;
        RETURN;
    END

	SELECT @Result = COUNT(*)
	FROM [User]
	WHERE Id = @Id AND [Password] = @Password;

	IF @Result = 1
	BEGIN
		-- 로그인 정보가 맞으면, 결과와 함께 부가 정보 (Id) 반환
		SELECT 1 AS Result, [User].UserId AS UserNumber
		FROM [User]
		WHERE Id = @Id; -- 성공
	END

	ELSE
		SELECT 0 AS Result, 0 AS UserNumber; -- 실패
END
*/

/* SP 사용자 조회 (검색 및 페이지 처리) */
ALTER PROCEDURE adm_ReadUser
	@SearchFilter NVARCHAR(50) = '',
	@SearchWord NVARCHAR(50) = '',
	@PageIndex int = 1,
	@RowPerPage int = 10 -- 한 페이지에 노출되는 행 수 (default: 10)
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @Offset INT
	DECLARE @ResultCount INT
	DECLARE @PageCount INT

	-- 유효하지 않은 값 예외처리
	IF (@RowPerPage < 1)
		SET @RowPerPage = 10

	IF (@PageIndex < 1) 
		SET @PageIndex = 1

	SET @Offset = (@PageIndex - 1) * @RowPerPage

	-- 검색
	IF @SearchFilter = 'UserId'
    BEGIN
		-- String -> Int 형변환
		SET @SearchWord = TRY_CAST(@SearchWord AS INT);
		IF @SearchWord IS NULL
			SET @SearchWord = 0;

		SELECT [UserId], [Id], [Name], [Image], [Authority] -- 결과에서 비밀번호 제외
		FROM [dbo].[User]
		WHERE [UserId] = @SearchWord
		ORDER BY [UserId] ASC
		OFFSET @Offset ROWS
		FETCH NEXT @RowPerPage ROWS ONLY;
		-- 검색된 항목 수
		SELECT @ResultCount = COUNT(*) FROM [dbo].[User] WHERE [UserId] = @SearchWord;
    END
    ELSE IF @SearchFilter = 'Id'
    BEGIN
        SELECT [UserId], [Id], [Name], [Image], [Authority] -- 결과에서 비밀번호 제외
        FROM [dbo].[User]
        WHERE [Id] LIKE '%' + @SearchWord + '%'
		ORDER BY [Id] ASC
		OFFSET @Offset ROWS
		FETCH NEXT @RowPerPage ROWS ONLY;
		-- 검색된 항목 수
		SELECT @ResultCount = COUNT(*) FROM [dbo].[User] WHERE [Id] LIKE '%' + @SearchWord + '%';
    END
    ELSE IF @SearchFilter = 'Name'
    BEGIN
        SELECT [UserId], [Id], [Name], [Image], [Authority] -- 결과에서 비밀번호 제외
        FROM [dbo].[User]
        WHERE [Name] LIKE '%' + @SearchWord + '%'
		ORDER BY [Name] ASC
		OFFSET @Offset ROWS
		FETCH NEXT @RowPerPage ROWS ONLY;
		-- 검색된 항목 수
		SELECT @ResultCount = COUNT(*) FROM [dbo].[User] WHERE [Name] LIKE '%' + @SearchWord + '%';
    END
    ELSE IF @SearchFilter = 'Authority'
    BEGIN
        SELECT [UserId], [Id], [Name], [Image], [Authority] -- 결과에서 비밀번호 제외
        FROM [dbo].[User]
        WHERE [Authority] LIKE '%' + @SearchWord + '%'
		ORDER BY [Authority] ASC
		OFFSET @Offset ROWS
		FETCH NEXT @RowPerPage ROWS ONLY;
		-- 검색된 항목 수
		SELECT @ResultCount = COUNT(*) FROM [dbo].[User] WHERE [Authority] LIKE '%' + @SearchWord + '%';
    END
    ELSE
    BEGIN
        SELECT [UserId], [Id], [Name], [Image], [Authority] -- 결과에서 비밀번호 제외
		FROM [dbo].[User]
		ORDER BY [UserId] ASC
		OFFSET @Offset ROWS
		FETCH NEXT @RowPerPage ROWS ONLY;
		-- 검색된 항목 수
		SELECT @ResultCount = COUNT(*) FROM [dbo].[User];
    END

	-- 총 페이지 수 계산
	IF @ResultCount % @RowPerPage = 0
		SET @PageCount = @ResultCount / @RowPerPage
    ELSE
        SET @PageCount = @ResultCount / @RowPerPage + 1

		SELECT @PageCount AS TotalPageCount

		SELECT @ResultCount AS TotalResultCount

END

-- 테스트
EXEC adm_ReadUser @SearchFilter = 'UserId', @SearchWord = '2';
EXEC adm_ReadUser @SearchFilter = 'Authority', @SearchWord = 'normal';
EXEC adm_ReadUser @SearchFilter = 'Id', @SearchWord = 'a';