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
		SELECT a.*, b.Name 
		FROM [Post] AS a LEFT OUTER JOIN [User] AS b ON a.UserId = b.UserId 
		ORDER BY PostId DESC 
		OFFSET @Offset ROWS 
		FETCH NEXT @PageSize ROWS ONLY
	ELSE
		SELECT a.*, b.Name
		FROM [Post] AS a LEFT OUTER JOIN [User] AS b ON a.UserId = b.UserId
		WHERE Category = @Category
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

/* SP 게시물 조회 (by PostId) TODO 댓글 데이터 조인? */
CREATE PROCEDURE ReadPostById
	@PostId int
AS
BEGIN
	SET NOCOUNT ON;
	/* 탈퇴한 유저의 게시물도 노출해야 할 것 같아서 LEFT OUTER JOIN 사용 */
	SELECT a.*, b.Name FROM [Post] AS a LEFT OUTER JOIN [User] AS b ON a.UserId = b.UserId WHERE PostId = @PostId;
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
CREATE PROCEDURE UpdatePost
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
CREATE PROCEDURE DeletePost
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
CREATE PROCEDURE UpdateComment
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
CREATE PROCEDURE DeleteComment
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
CREATE PROCEDURE [LogIn]
	@Id varchar(50),
	@Password varchar(50)
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @Result INT;

	IF LEN(@Id) = 0 OR LEN(@Password) = 0
    BEGIN
        -- 입력값 오류
        SELECT -1 AS Result;
        RETURN;
    END

	SELECT @Result = COUNT(*)
	FROM [User]
	WHERE Id = @Id AND [Password] = @Password;

	IF @Result = 1
		SELECT 1 AS Result; -- 성공
	ELSE
		SELECT 0 AS Result; -- 실패
END

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