USE [MVC_BOARD_DB]
GO


/* 댓글 테이블 */
/****** Object:  Table [dbo].[Comment]    Script Date: 2024-03-01 오후 2:02:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Comment](
	[CommentId] [int] IDENTITY(1,1) NOT NULL,
	[PostId] [int] NOT NULL,
	[UserId] [int] NOT NULL,
	[ParentId] [int] NULL,
	[Contents] [nvarchar](max) NOT NULL,
	[Likes] [int] NOT NULL,
	[IsAnonymous] [bit] NOT NULL,
	[CreateDate] [datetime2](7) NOT NULL,
	[UpdateDate] [datetime2](7) NULL,
	[DeleteDate] [datetime2](7) NULL,
 CONSTRAINT [PK_Comment] PRIMARY KEY CLUSTERED 
(
	[CommentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

/* 게시물 테이블 */
/****** Object:  Table [dbo].[Post]    Script Date: 2024-03-01 오후 2:02:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Post](
	[PostId] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](100) NOT NULL,
	[Contents] [nvarchar](max) NULL,
	[UserId] [int] NOT NULL,
	[Likes] [int] NOT NULL,
	[Views] [int] NOT NULL,
	[Category] [int] NOT NULL,
	[CreateDate] [datetime2](7) NOT NULL,
	[UpdateDate] [datetime2](7) NULL,
	[DeleteDate] [datetime2](7) NULL,
 CONSTRAINT [PK_Post] PRIMARY KEY CLUSTERED 
(
	[PostId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

/* 유저 테이블 */
/****** Object:  Table [dbo].[User]    Script Date: 2024-03-01 오후 2:02:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[User](
	[UserId] [int] IDENTITY(1,1) NOT NULL,
	[Id] [varchar](50) NOT NULL,
	[Password] [varchar](50) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Image] [int] NULL,
	[Authority] NVARCHAR(50) DEFAULT 'normal' NOT NULL
 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Comment] ADD  CONSTRAINT [DF_Comment_ParentId]  DEFAULT ((0)) FOR [ParentId]
GO
ALTER TABLE [dbo].[Comment] ADD  CONSTRAINT [DF_Comment_Likes]  DEFAULT ((0)) FOR [Likes]
GO
ALTER TABLE [dbo].[Comment] ADD  CONSTRAINT [DF_Comment_IsAnonymous]  DEFAULT ((0)) FOR [IsAnonymous]
GO
ALTER TABLE [dbo].[Post] ADD  CONSTRAINT [DF_Post_Likes]  DEFAULT ((0)) FOR [Likes]
GO
ALTER TABLE [dbo].[Post] ADD  CONSTRAINT [DF_Post_Views]  DEFAULT ((0)) FOR [Views]
GO
ALTER TABLE [dbo].[Post] ADD  CONSTRAINT [DF_Post_Category]  DEFAULT ((1)) FOR [Category]
GO
/****** Object:  StoredProcedure [dbo].[CreateComment]    Script Date: 2024-03-01 오후 2:02:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/* 게시판 타입 테이블 */
/****** Object:  Table [dbo].[BoardType]    Script Date: 2024-03-01 오후 2:02:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BoardType](
	[BoardId] [int] IDENTITY(1,1) NOT NULL,
	[BoardName] [varchar](50) NOT NULL,
	[Category] [int] NOT NULL,
	[ParentCategory] [int] NULL,
	[IsParent] [int] DEFAULT 0,
	[IconType] [int] NULL,
	[IsWritable] [bit] NOT NULL DEFAULT 0,
 CONSTRAINT [PK_BoardType] PRIMARY KEY CLUSTERED 
(
	[BoardId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
