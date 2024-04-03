USE [MVC_BOARD_DB]
GO

/* MvcBoardAdmin 관련 SP */

-- 게시판 관리 #########################################################################################################################

/* SP 게시판 생성 */
-- Result  1 : DB Success
-- Result  0 : DB Fail
-- Result -1: Category Number 중복 // 비즈니스 로직에 따라 중복이 필요한 경우 수정
-- Result -2: 입력값 오류
CREATE PROCEDURE adm_CreateBoardType
	@BoardName varchar(50),
	@Category int = -1,
	@ParentCategory int = -1,
	@IsParent bit = 0,
	@IconType int = 0,
	@IsWritable bit = 0,
	@ShowOrder int = 0
AS
BEGIN
	
	-- 입력값 검증
	IF (@Category <= 0)
		BEGIN
		SELECT -2 AS 'Result';
		RETURN;
		END
	IF (@IsParent = 1 AND @IsWritable = 1) -- 부모 게시판인 경우 작성 불가
		BEGIN
		SELECT -2 AS 'Result';
		RETURN;
		END
	IF (@IsParent = 1 AND @ParentCategory > 0) -- 2depth 까지만 허용
		BEGIN
		SELECT -2 AS 'Result';
		RETURN;
		END

	-- Category Nubmer 중복 확인
	IF EXISTS (SELECT 1 FROM BoardType WHERE Category = @Category)
	BEGIN 
		SELECT -1 AS 'Result';
		RETURN;
	END

	-- 중복되지 않는 경우, 새 게시판 생성
	INSERT INTO BoardType(BoardName, Category, ParentCategory, IsParent, IconType, IsWritable, ShowOrder) 
	VALUES (@BoardName, @Category, @ParentCategory, @IsParent, @IconType, @IsWritable, @ShowOrder);

	-- @ROWCOUNT 는 마지막에 실행된 SQL 문에 영향을 받은 행의 수를 반환하는 시스템 함수 (INSERT, UPDATE, DELETE 등)
	SELECT @@ROWCOUNT AS 'Result';

END
GO

/* SP 게시판 수정 */
-- Result  1 : DB Success
-- Result  0 : DB Fail
-- Result -1: Category Number 중복 // 비즈니스 로직에 따라 중복이 필요한 경우 수정
-- Result -2: 입력값 오류
ALTER PROCEDURE adm_UpdateBoardType
	@BoardId int = 0,
	@BoardName varchar(50),
	@Category int = -1,
	@PrevCategory int = -1,
	@ParentCategory int = -1,
	@IsParent bit = 0,
	@IconType int = 0,
	@IsWritable bit = 0,
	@ShowOrder int = 0
AS
BEGIN

	-- 입력값 검증
	IF (@Category <= 0 OR @PrevCategory <= 0)
		BEGIN
		SELECT -2 AS 'Result';
		RETURN;
		END
	IF (@IsParent = 1 AND @IsWritable = 1) -- 부모 게시판인 경우 작성 불가
		BEGIN
		SELECT -2 AS 'Result';
		RETURN;
		END
	IF (@IsParent = 1 AND @ParentCategory > 0) -- 2depth 까지만 허용
		BEGIN
		SELECT -2 AS 'Result';
		RETURN;
		END

	-- Category Nubmer 중복 확인
	IF @PrevCategory != @Category AND EXISTS (SELECT 1 FROM BoardType WHERE Category = @Category)
	BEGIN 
		SELECT -1 AS 'Result';
		RETURN;
	END

	-- 중복되지 않는 경우, 게시판 정보 수정
	UPDATE BoardType SET
		BoardName = @BoardName,
		Category = @Category,
		ParentCategory = @ParentCategory,
		IsParent = @IsParent,
		IconType = @IconType,
		IsWritable = @IsWritable,
		ShowOrder = @ShowOrder
	WHERE BoardId = @BoardId;

	-- @ROWCOUNT 는 마지막에 실행된 SQL 문에 영향을 받은 행의 수를 반환하는 시스템 함수 (INSERT, UPDATE, DELETE 등)
	SELECT @@ROWCOUNT AS 'Result';

END
GO

/* SP 게시판 삭제 */
-- Result  1 : DB Success
-- Result  0 : DB Fail
-- Result -1: 존재하지 않는 게시판 (BoardId)
-- Result -2: 입력값 오류
ALTER PROCEDURE adm_DeleteBoardType
	@BoardId int
	-- @forceDelete int = 0
AS
BEGIN

	-- 입력값 검증
	IF (@BoardId < 3) -- (전체, 인기, 공지 제외)
		BEGIN
		SELECT -2 AS 'Result';
		RETURN;
		END

	-- 존재 확인
	/*
	IF NOT EXISTS (SELECT 1 FROM BoardType WHERE BoardId = @BoardId)
	BEGIN 
		SELECT -1 AS 'Result';
		RETURN;
	END
	*/
	DELETE FROM BoardType WHERE BoardId = @BoardId;

	-- @ROWCOUNT 는 마지막에 실행된 SQL 문에 영향을 받은 행의 수를 반환하는 시스템 함수 (INSERT, UPDATE, DELETE 등)
	SELECT @@ROWCOUNT AS 'Result';

END


/* SP 게시판 조회 - 부모/자식 관계 및 순서 정렬 */
-- 정렬 쿼리 1
ALTER PROCEDURE GetSortedBoards
AS
BEGIN 
	SELECT *
	FROM BoardType AS b1
	ORDER BY
			CASE 
				WHEN ParentCategory = 0 THEN b1.ShowOrder 
				ELSE (SELECT MAX(ShowOrder) -- MAX: 행이 2개 이상인 경우 예외처리
						FROM BoardType AS b2
						WHERE b2.Category= b1.ParentCategory) 
				END ASC,
			CASE 
				WHEN ParentCategory = 0 THEN NULL 
				ELSE ShowOrder 
			END ASC
END

EXEC GetSortedBoards

-- 정렬 쿼리 2 (개선?)
/*
ALTER PROCEDURE GetSortedBoards
AS
BEGIN
	SELECT b1.*
	FROM BoardType AS b1
	ORDER BY
		CASE 
			WHEN b1.ParentCategory = 0 THEN b1.ShowOrder 
			ELSE (SELECT b2.ShowOrder 
				  FROM BoardType AS b2 
				  WHERE b2.Category = b1.ParentCategory) 
		END ASC,
		CASE 
			WHEN b1.ParentCategory = 0 THEN NULL 
			ELSE b1.ShowOrder 
		END ASC;
END
*/

-- 정렬 쿼리 3 (이건 실패)
/*
ALTER PROCEDURE GetSortedBoards
AS
BEGIN
    WITH SortedBoards AS (
        SELECT *, 0 AS Level
        FROM BoardType
        WHERE ParentCategory = 0 -- 부모 게시판인 경우

        UNION ALL

		SELECT b.*, sb.Level + 1
        FROM BoardType b
        INNER JOIN SortedBoards sb ON b.ParentCategory = sb.Category
		WHERE b.ParentCategory > 0 -- 자식 게시판인 경우
    )
    SELECT *
    FROM SortedBoards
    --ORDER BY ParentCategory, [ShowOrder];
END;
*/

-- 테스트 및 초기 데이터 생성 ----------------------------------------------------------------------------------------------------------

-- 게시판 데이터 생성 (필수)
EXEC amd_CreateBoardType '전체 게시판', '0', '0', '0', '1', '0', '0'
EXEC amd_CreateBoardType '인기 게시판', '1', '0', '0', '2', '0', '1'
EXEC amd_CreateBoardType '공지 게시판', '2', '0', '0', '3', '0', '2'
EXEC amd_CreateBoardType '팀원 모집', '3', '0', '0', '4', '1', '3'

EXEC amd_CreateBoardType '일반 커뮤니티', '4', '0', '1', '5', '0', '4'
EXEC amd_CreateBoardType '자유 게시판', '41', '4', '0', '0', '1', '1'
EXEC amd_CreateBoardType '질문 답변', '42', '4', '0', '0', '1', '2'
EXEC amd_CreateBoardType '오픈 마이크', '43', '4', '0', '0', '1', '3'
EXEC amd_CreateBoardType '정보 게시판', '44', '4', '0', '0', '1', '4'

EXEC amd_CreateBoardType '전공 커뮤니티', '6', '0', '1', '6', '0', '5'
EXEC amd_CreateBoardType '입시', '61', '6', '0', '0', '1', '1'
EXEC amd_CreateBoardType '실용음악과', '62', '6', '0', '0', '1', '2'

-- 추가 무질서 데이터 (정렬 테스트)
EXEC amd_CreateBoardType '자유 - 3', '47', '4', '0', '0', '1', '13'
EXEC amd_CreateBoardType '자유 - 1', '45', '4', '0', '0', '1', '11'
EXEC amd_CreateBoardType '자유 - 2', '46', '4', '0', '0', '1', '12'

-- 생성 확인 (조회)
EXEC GetSortedBoards

-- 유저 관리 ###########################################################################################################################

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
CREATE PROCEDURE adm_ReadUsers
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

		SELECT [UserId], [Id], [Name], [Image], [Authority], [IsDeleted] -- 결과에서 비밀번호 제외
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
        SELECT [UserId], [Id], [Name], [Image], [Authority], [IsDeleted] -- 결과에서 비밀번호 제외
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
        SELECT [UserId], [Id], [Name], [Image], [Authority], [IsDeleted] -- 결과에서 비밀번호 제외
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
        SELECT [UserId], [Id], [Name], [Image], [Authority], [IsDeleted] -- 결과에서 비밀번호 제외
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
        SELECT [UserId], [Id], [Name], [Image], [Authority], [IsDeleted] -- 결과에서 비밀번호 제외
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
EXEC adm_ReadUsers @SearchFilter = 'UserId', @SearchWord = '2';
EXEC adm_ReadUsers @SearchFilter = 'Authority', @SearchWord = 'normal';
EXEC adm_ReadUsers @SearchFilter = 'Id', @SearchWord = 'a';

/* SP 사용자 상세 조회 */
ALTER PROCEDURE adm_UserDetail
	@UserId int
AS
BEGIN
	SET NOCOUNT ON;

    SELECT 
        U.[UserId],
        U.[Id],
		-- U.[Password], -- 불필요시 제거
		U.[Name],
		U.[Image],
		U.[Authority],
		U.[IsDeleted],
		U.[CreateDate],
		U.[DeleteDate],
        COUNT(DISTINCT P.PostId) AS PostCount, -- 사용자가 작성한 게시물 수
        COUNT(DISTINCT C.CommentId) AS CommentCount -- 사용자가 작성한 댓글 수
    FROM 
        [User] U
    LEFT JOIN 
		[Post] P ON U.UserId = P.UserId -- 게시물 테이블과 조인하여 게시물 수 계산
    LEFT JOIN 
        [Comment] C ON U.UserId = C.UserId -- 댓글 테이블과 조인하여 댓글 수 계산
    WHERE 
        U.UserId = @UserId
    GROUP BY 
        U.UserId, U.Id, U.[Name], U.[Image], U.[Authority], U.[IsDeleted], U.[CreateDate], U.[DeleteDate] -- , U.[Password] 불필요시 제거
END

-- 테스트
EXEC adm_UserDetail '1'

/* SP 사용자 정보 수정 (Admin) */
CREATE PROCEDURE adm_UpdateUser
	@UserId int,
	@Name nvarchar(50),
	@Image int,
	@Authority nvarchar(50) 
AS
BEGIN
	UPDATE [User] SET
		[Name] = @Name,
		[Image] = @Image,
		[Authority] = @Authority
	WHERE UserId = @UserId;
END
GO

/* SP 사용자 정보 삭제 (Admin) */
CREATE PROCEDURE adm_DeleteUser
	@UserId int,
	@forceDelete int = 0 -- TODO 0으로 변경
AS
BEGIN
	IF (@forceDelete = 1)
		DELETE FROM [User] WHERE UserId = @UserId;
	ELSE
		UPDATE [User] SET IsDeleted = 1, DeleteDate = GETDATE() WHERE UserId = @UserId AND IsDeleted = 0; -- TODO GETUTCDATE (UTC 로 해야 하나?)
END
GO

-- 테스트

EXEC adm_DeleteUser '5';

-- 삭제 복구
BEGIN
UPDATE [User]
	SET
	[IsDeleted] = 0,
	[DeleteDate] = NULL
	WHERE IsDeleted = 1
END
GO


-- 게시물 관리 #########################################################################################################################

-- 테스트
EXEC adm_ReadPosts '0', 'Title', '테스트', '1';
EXEC adm_ReadPosts '0', 'PostId', '157', '1';
EXEC adm_ReadPosts '0', 'test', '3', '1';

/* SP 게시물 조회 (검색 및 페이지 처리) */
ALTER PROCEDURE adm_ReadPosts
	@BoardFilter int = 0, -- 게시판의 Category 값임
	@SearchFilter NVARCHAR(50) = '',
	@SearchWord NVARCHAR(50) = '',
	@PageIndex int = 1,
	@RowPerPage int = 10 -- 한 페이지에 노출되는 행 수 (default: 10)
AS
BEGIN
	SET NOCOUNT ON;

	-- TODO
	-- DECLARE @isHot NVARCHAR(MAX)
	-- DECLARE @isNotice NVARCHAR(MAX)

	DECLARE @query_1 NVARCHAR(MAX)
	DECLARE @query_2 NVARCHAR(MAX)

	DECLARE @condition_1 NVARCHAR(MAX) = ''
	DECLARE @condition_2 NVARCHAR(MAX) = ''
	DECLARE @order NVARCHAR(MAX) = 'PostId'

	DECLARE @Offset INT
	DECLARE @ResultCount INT
	DECLARE @PageCount INT

	-- 유효하지 않은 값 예외처리
	IF (@RowPerPage < 1)
		SET @RowPerPage = 10

	IF (@PageIndex < 1) 
		SET @PageIndex = 1

	SET @Offset = (@PageIndex - 1) * @RowPerPage

	-- BoardFilter 조건절
	IF @BoardFilter IS NOT NULL AND @BoardFilter > 2 -- 0
    BEGIN
        SET @condition_1 = ' AND a.Category = @BoardFilter '
    END

	-- TODO isHot, isNotice 조건절

	-- SearchFilter 조건절
	IF @SearchFilter = 'PostId' AND @SearchWord IS NOT NULL
    BEGIN
        SET @condition_2 = ' AND PostId = TRY_CAST(@SearchWord AS INT)';
    END
	ELSE IF @SearchFilter = 'Title' AND @SearchWord != ''
	BEGIN
		SET @condition_2 = ' AND Title LIKE ''%' + @SearchWord + '%''';
	END
	ELSE IF @SearchFilter = 'Contents' AND @SearchWord != ''
	BEGIN
		SET @condition_2 = ' AND Contents LIKE ''%' + @SearchWord + '%''';
	END
	ELSE IF @SearchFilter = 'Id' AND @SearchWord != ''
	BEGIN
		SET @condition_2 = ' AND b.Id LIKE ''%' + @SearchWord + '%''';
	END
	ELSE IF @SearchFilter = 'Name' AND @SearchWord != ''
	BEGIN
		SET @condition_2 = ' AND b.Name LIKE ''%' + @SearchWord + '%''';
	END

	-- 검색 쿼리
	SET @query_1 = 
	'SELECT a.*, b.Name, c.BoardName,
		(SELECT COUNT(*) FROM [Comment] WHERE PostId = a.PostId) AS CommentCount
		FROM [Post] AS a
		LEFT OUTER JOIN [User] AS b ON a.UserId = b.UserId
		LEFT OUTER JOIN [BoardType] AS c ON a.Category = c.Category
		WHERE 1=1' + @condition_1 + @condition_2 + 
		' ORDER BY ' + @order + ' ASC
		OFFSET @Offset ROWS
		FETCH NEXT @RowPerPage ROWS ONLY;'

	-- 검색 항목 수 쿼리
	SET @query_2 = 
	'SELECT @ResultCount = COUNT(*) 
		FROM [Post] AS a
		LEFT OUTER JOIN [User] AS b ON a.UserId = b.UserId
		LEFT OUTER JOIN [BoardType] AS c ON a.Category = c.Category
		WHERE 1=1' + @condition_1 + @condition_2

	EXEC sp_executesql @query_1, N'@BoardFilter INT, @SearchWord NVARCHAR(50), @Offset INT, @RowPerPage INT', @BoardFilter, @SearchWord, @Offset, @RowPerPage;
	EXEC sp_executesql @query_2, N'@BoardFilter INT, @SearchWord NVARCHAR(50), @ResultCount INT OUTPUT', @BoardFilter, @SearchWord, @ResultCount OUTPUT;

	-- 총 페이지 수 계산
	IF @ResultCount % @RowPerPage = 0
		SET @PageCount = @ResultCount / @RowPerPage
    ELSE
        SET @PageCount = @ResultCount / @RowPerPage + 1

		SELECT @PageCount AS TotalPageCount

		SELECT @ResultCount AS TotalResultCount

END
GO
	
-- =====================




-- 댓글 관리 ###########################################################################################################################