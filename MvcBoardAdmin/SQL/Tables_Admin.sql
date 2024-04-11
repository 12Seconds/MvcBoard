USE [MVC_BOARD_DB]
GO

/* 관리자 계정 테이블 */
CREATE TABLE [dbo].[AdmUser] (
	[UserNo] INT PRIMARY KEY IDENTITY(1,1) NOT NULL,
	[Id] NVARCHAR(50) UNIQUE NOT NULL,
	[Password] NVARCHAR(50) NOT NULL,
	[Name] NVARCHAR(50) NOT NULL,
	[Image] INT DEFAULT 0 NOT NULL,
	[AuthorityGroup] NVARCHAR(50) DEFAULT 'Admin' NOT NULL,
	[IsDeleted] BIT DEFAULT 0 NOT NULL,
	[CreateDate] DATETIME2 DEFAULT GETDATE() NOT NULL,
	[DeleteDate] DATETIME2 DEFAULT NULL
);

/* 권한 테이블 */
CREATE TABLE [dbo].[tb_Authority] (
	[AuthId] nvarchar(30) PRIMARY KEY NOT NULL,
	-- [AuthName] nvarchar(30) NOT NULL,
	[Controller] nvarchar(30) DEFAULT '' NOT NULL,
	-- [Action] nvarchar(30) DEFAULT '' NOT NULL -- 액션 메소드별로 권한을 나눌 경우 (현재는 사용하지 않음)
	-- [Area] nvarchar(30) DEFAULT '' NOT NULL 
)

-- 초기값
INSERT INTO tb_Authority VALUES ('ManageAdminMember', 'AdminMember');
INSERT INTO tb_Authority VALUES ('ManageMember', 'Member');
INSERT INTO tb_Authority VALUES ('ManageBoard', 'Board');
INSERT INTO tb_Authority VALUES ('ManagePost', 'Post');
INSERT INTO tb_Authority VALUES ('ManageComment', 'Comment');

/*
CREATE TABLE [dbo].[tb_Authority] (
	[AuthId] nvarchar(30) PRIMARY KEY NOT NULL,
	[AuthName] nvarchar(30) NOT NULL,
	[Controller] nvarchar(30) DEFAULT '' NOT NULL,
	-- [Action] nvarchar(30) DEFAULT '' NOT NULL -- 액션 메소드별로 권한을 나눌 경우 (현재는 사용하지 않음)
	-- [Area] nvarchar(30) DEFAULT '' NOT NULL 
)
INSERT INTO tb_Authority VALUES ('Auth000', 'ManageAdminMember', 'AdminMember');
INSERT INTO tb_Authority VALUES ('Auth010', 'ManageMember', 'Member');
INSERT INTO tb_Authority VALUES ('Auth020', 'ManageBoard', 'Board');
INSERT INTO tb_Authority VALUES ('Auth030', 'ManagePost', 'Post');
INSERT INTO tb_Authority VALUES ('Auth040', 'ManageComment', 'Comment');
*/

/* 권한 그룹 테이블 */
CREATE TABLE [dbo].[tb_AuthorityGroup] (
	[AuthGroupId] nvarchar(30) PRIMARY KEY NOT NULL
	-- [AuthGroupName] nvarchar(30) UNIQUE NOT NULL,
);

-- 초기값
INSERT INTO [tb_AuthorityGroup] VALUES ('User');
INSERT INTO [tb_AuthorityGroup] VALUES ('Master');
INSERT INTO [tb_AuthorityGroup] VALUES ('Admin');
INSERT INTO [tb_AuthorityGroup] VALUES ('UserAdmin');
INSERT INTO [tb_AuthorityGroup] VALUES ('BoardAdmin');

/*
INSERT INTO [tb_AuthorityGroup] VALUES ('AuthGroup01', 'User');
INSERT INTO [tb_AuthorityGroup] VALUES ('AuthGroup02', 'Master');
INSERT INTO [tb_AuthorityGroup] VALUES ('AuthGroup03', 'Admin');
INSERT INTO [tb_AuthorityGroup] VALUES ('AuthGroup04', 'UserAdmin');
INSERT INTO [tb_AuthorityGroup] VALUES ('AuthGroup05', 'BoardAdmin');
*/

/* 그룹-권한 매핑 테이블 */
CREATE TABLE [dbo].[tb_AuthorityMapping] (
	[AuthGroupId] nvarchar(30) NOT NULL,
	[AuthId] nvarchar(30) NOT NULL
);

-- 초기값

-- Master
INSERT INTO [tb_AuthorityMapping] VALUES ('Master', 'Auth000');
INSERT INTO [tb_AuthorityMapping] VALUES ('Master', 'Auth010');
INSERT INTO [tb_AuthorityMapping] VALUES ('Master', 'Auth020');
INSERT INTO [tb_AuthorityMapping] VALUES ('Master', 'Auth030');
INSERT INTO [tb_AuthorityMapping] VALUES ('Master', 'Auth040');
-- Admin
INSERT INTO [tb_AuthorityMapping] VALUES ('Admin', 'Auth010');
INSERT INTO [tb_AuthorityMapping] VALUES ('Admin', 'Auth020');
INSERT INTO [tb_AuthorityMapping] VALUES ('Admin', 'Auth030');
INSERT INTO [tb_AuthorityMapping] VALUES ('Admin', 'Auth040');
-- UserAdmin
INSERT INTO [tb_AuthorityMapping] VALUES ('UserAdmin', 'Auth010');
-- BoardAdmin
INSERT INTO [tb_AuthorityMapping] VALUES ('BoardAdmin', 'Auth020');
INSERT INTO [tb_AuthorityMapping] VALUES ('BoardAdmin', 'Auth030');
INSERT INTO [tb_AuthorityMapping] VALUES ('BoardAdmin', 'Auth040');

/*
-- Master
INSERT INTO [tb_AuthorityMapping] VALUES ('AuthGroup02', 'Auth000');
INSERT INTO [tb_AuthorityMapping] VALUES ('AuthGroup02', 'Auth010');
INSERT INTO [tb_AuthorityMapping] VALUES ('AuthGroup02', 'Auth020');
INSERT INTO [tb_AuthorityMapping] VALUES ('AuthGroup02', 'Auth030');
INSERT INTO [tb_AuthorityMapping] VALUES ('AuthGroup02', 'Auth040');
-- Admin
INSERT INTO [tb_AuthorityMapping] VALUES ('AuthGroup03', 'Auth010');
INSERT INTO [tb_AuthorityMapping] VALUES ('AuthGroup03', 'Auth020');
INSERT INTO [tb_AuthorityMapping] VALUES ('AuthGroup03', 'Auth030');
INSERT INTO [tb_AuthorityMapping] VALUES ('AuthGroup03', 'Auth040');
-- UserAdmin
INSERT INTO [tb_AuthorityMapping] VALUES ('AuthGroup04', 'Auth010');
-- BoardAdmin
INSERT INTO [tb_AuthorityMapping] VALUES ('AuthGroup05', 'Auth020');
INSERT INTO [tb_AuthorityMapping] VALUES ('AuthGroup05', 'Auth030');
INSERT INTO [tb_AuthorityMapping] VALUES ('AuthGroup05', 'Auth040');
*/

-- UPDATE [tb_AuthorityMapping] SET [AuthGroupId] = 'Master' WHERE [AuthGroupId] = 'AuthGroup02';