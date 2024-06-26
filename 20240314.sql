USE [db_rmtools_meyzan]
GO
/****** Object:  UserDefinedFunction [dbo].[uf_LookupDynamicQueryGenerator]    Script Date: 14/03/2024 14:32:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[uf_LookupDynamicQueryGenerator] 
(
	@LookupValue VARCHAR(MAX) = '', --if given NULL, Default as empty
	@ColumnName VARCHAR(50)
)
RETURNS VARCHAR(MAX)
AS
BEGIN
	DECLARE @Query VARCHAR(MAX)

	IF @LookupValue != ''
		BEGIN
			SELECT
				@Query = 
				COALESCE(
					@Query + ' OR ',''
				) + ' ' + @ColumnName +' LIKE ''%' + ColumnData + '%''' 
			FROM [dbo].[uf_SplitString](@LookupValue, ';') --use function uf_SplitString with semi colon as separator
	
			SELECT @Query = ' AND (' + @Query + ')'
	
		END
	ELSE
		BEGIN
			SET @Query = ''
		END
	
	-- Return the result of the function
	RETURN @Query

END











GO
/****** Object:  UserDefinedFunction [dbo].[uf_ShortIndonesianDateTime]    Script Date: 14/03/2024 14:32:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[uf_ShortIndonesianDateTime](@Tanggal datetime) 
RETURNS Varchar(50) 
AS 
BEGIN 
declare @FormatDate nvarchar(100)

SELECT @FormatDate = FORMAT(@Tanggal,'dd') + ' ' + dbo.uf_SingkatanNamaBulan(FORMAT(@Tanggal,'MM')) + ' ' + FORMAT(@Tanggal,'yyyy')+ ' ' + FORMAT(@Tanggal,'HH:mm:ss')
	RETURN @FormatDate
END

GO
/****** Object:  UserDefinedFunction [dbo].[uf_SingkatanNamaBulan]    Script Date: 14/03/2024 14:32:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[uf_SingkatanNamaBulan](@NumMonth int) 
RETURNS Varchar(50) 
AS 
BEGIN 
declare @NamaBulan nvarchar(100)

SELECT @NamaBulan = (CASE WHEN @NumMonth = 1 THEN 'Jan'
			 WHEN @NumMonth = 2 THEN 'Feb'
			 WHEN @NumMonth = 3 THEN 'Mar'
			 WHEN @NumMonth = 4 THEN 'Apr'
			 WHEN @NumMonth = 5 THEN 'Mei'
			 WHEN @NumMonth = 6 THEN 'Jun'
			 WHEN @NumMonth = 7 THEN 'Jul'
			 WHEN @NumMonth = 8 THEN 'Agus'
			 WHEN @NumMonth = 9 THEN 'Sept'
			 WHEN @NumMonth = 10 THEN 'Okt'
			 WHEN @NumMonth = 11 THEN 'Nov'
			 WHEN @NumMonth = 12 THEN 'Des'
			  END)
	RETURN @NamaBulan
END

GO
/****** Object:  UserDefinedFunction [dbo].[uf_SplitString]    Script Date: 14/03/2024 14:32:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Function [dbo].[uf_SplitString] (     @Value Varchar(max), 	@Separator varchar(1) = ';' ) RETURNS @Table TABLE (ColumnData VARCHAR(100)) AS BEGIN     IF RIGHT(@Value, 1) <> @Separator     SELECT @Value = @Value + @Separator     DECLARE @Pos    BIGINT,             @OldPos BIGINT     SELECT  @Pos    = 1,             @OldPos = 1     WHILE   @Pos < LEN(@Value)         BEGIN             SELECT  @Pos = CHARINDEX(@Separator, @Value, @OldPos)             INSERT INTO @Table             SELECT  LTRIM(RTRIM(SUBSTRING(@Value, @OldPos, @Pos - @OldPos))) Col001             SELECT  @OldPos = @Pos + 1         END     RETURN END 

GO
/****** Object:  Table [dbo].[Tbl_JwtRepository]    Script Date: 14/03/2024 14:32:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tbl_JwtRepository](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NULL,
	[ClientIP] [nvarchar](max) NULL,
	[Token] [text] NULL,
	[RefreshToken] [nvarchar](max) NULL,
	[IsStop] [bit] NULL,
	[Hostname] [nvarchar](max) NULL,
	[StartTime] [datetime] NULL,
	[EndTime] [datetime] NULL,
 CONSTRAINT [PK_Tbl_JwtRepository] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Tbl_LogActivity]    Script Date: 14/03/2024 14:32:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tbl_LogActivity](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NULL,
	[Url] [nvarchar](max) NULL,
	[ActionTime] [datetime] NULL,
	[Browser] [nvarchar](max) NULL,
	[IP] [nvarchar](max) NULL,
	[OS] [nvarchar](max) NULL,
	[ClientInfo] [nvarchar](max) NULL,
	[Keterangan] [nvarchar](max) NULL,
 CONSTRAINT [PK_Tbl_LogActivity] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Tbl_LogPerubahanKelolaan]    Script Date: 14/03/2024 14:32:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tbl_LogPerubahanKelolaan](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CIF] [nvarchar](50) NULL,
	[Nama_debitur] [nvarchar](max) NULL,
	[Npp_RM] [nvarchar](50) NULL,
	[Npp_BA] [nvarchar](50) NULL,
	[Npp_RMTransaksi] [nvarchar](50) NULL,
	[Updated_at] [datetime] NULL,
	[UpdatedBy_Id] [int] NULL,
 CONSTRAINT [PK_Tbl_LogPerubahanKelolaan] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Tbl_MasterFile]    Script Date: 14/03/2024 14:32:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tbl_MasterFile](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FileName] [nvarchar](500) NULL,
	[FileSize] [nvarchar](50) NULL,
	[ext] [nvarchar](50) NULL,
	[Path] [nvarchar](500) NULL,
	[FullPath] [nvarchar](500) NULL,
	[DataSuccess] [int] NULL,
	[DataFailed] [int] NULL,
	[Created_at] [datetime] NULL,
	[Updated_at] [datetime] NULL,
	[Deleted_at] [datetime] NULL,
	[CreatedBy_Id] [int] NULL,
	[UpdatedBy_Id] [int] NULL,
	[DeletedBy_Id] [int] NULL,
	[IsDeleted] [bit] NULL,
	[IsActive] [bit] NULL,
 CONSTRAINT [PK_Tbl_MasterFile] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Tbl_MasterLookup]    Script Date: 14/03/2024 14:32:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tbl_MasterLookup](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Type] [nvarchar](50) NULL,
	[Name] [nvarchar](50) NULL,
	[Value] [int] NULL,
	[OrderBy] [int] NULL,
	[Created_at] [datetime] NULL,
	[Updated_at] [datetime] NULL,
	[Deleted_at] [datetime] NULL,
	[CreatedBy_Id] [int] NULL,
	[UpdatedBy_Id] [int] NULL,
	[DeletedBy_Id] [int] NULL,
	[IsActive] [bit] NULL,
	[IsDeleted] [bit] NULL,
 CONSTRAINT [PK_Tbl_MasterLookup] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Tbl_MasterNasabahKelolaan]    Script Date: 14/03/2024 14:32:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tbl_MasterNasabahKelolaan](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Kode_unit] [int] NULL,
	[Nama_unit] [nvarchar](500) NULL,
	[CIF_Parent] [nvarchar](500) NULL,
	[Nama_parent_nasabah] [nvarchar](max) NULL,
	[CIF] [nvarchar](50) NULL,
	[Nama_nasabah_debitur] [nvarchar](max) NULL,
	[Npp_RM] [nvarchar](50) NULL,
	[Npp_BA] [nvarchar](50) NULL,
	[Npp_RMTransaksi] [nvarchar](50) NULL,
	[Created_at] [datetime] NULL,
	[Updated_at] [datetime] NULL,
	[Deleted_at] [datetime] NULL,
	[CreatedBy_Id] [datetime] NULL,
	[UpdatedBy_Id] [datetime] NULL,
	[DeletedBy_Id] [datetime] NULL,
	[File_Id] [int] NULL,
	[StatusData] [bit] NULL,
	[Komentar] [nvarchar](max) NULL,
 CONSTRAINT [PK_Tbl_MasterNasabahKelolaan] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Tbl_MasterNasabahKelolaanForODS]    Script Date: 14/03/2024 14:32:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tbl_MasterNasabahKelolaanForODS](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Kode_unit] [int] NULL,
	[Nama_unit] [nvarchar](500) NULL,
	[CIF_Parent] [nvarchar](500) NULL,
	[Nama_parent_nasabah] [nvarchar](max) NULL,
	[CIF] [nvarchar](50) NULL,
	[Nama_nasabah_debitur] [nvarchar](max) NULL,
	[Npp_RM] [nvarchar](50) NULL,
	[Npp_BA] [nvarchar](50) NULL,
	[Npp_RMTransaksi] [nvarchar](50) NULL,
	[Created_at] [datetime] NULL,
	[Updated_at] [datetime] NULL,
	[CreatedBy_Id] [datetime] NULL,
	[UpdatedBy_Id] [datetime] NULL,
 CONSTRAINT [PK_Tbl_MasterNasabahKelolaanForODS] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Tbl_MasterNavigation]    Script Date: 14/03/2024 14:32:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tbl_MasterNavigation](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Type] [int] NULL,
	[Name] [nvarchar](50) NULL,
	[Route] [nvarchar](max) NULL,
	[Order] [int] NULL,
	[Visible] [int] NULL,
	[ParentNavigationId] [int] NULL,
	[Created_at] [datetime] NULL,
	[Updated_at] [datetime] NULL,
	[Deleted_at] [datetime] NULL,
	[CreatedBy_Id] [int] NULL,
	[UpdatedBy_Id] [int] NULL,
	[DeletedBy_Id] [int] NULL,
	[IsDeleted] [bit] NULL,
	[IconClass] [nvarchar](50) NULL,
 CONSTRAINT [PK_Tbl_MasterNavigation] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Tbl_MasterNavigationAssignment]    Script Date: 14/03/2024 14:32:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tbl_MasterNavigationAssignment](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Navigation_Id] [int] NULL,
	[UserId] [int] NULL,
	[Created_at] [datetime] NULL,
	[Updated_at] [datetime] NULL,
	[Deleted_at] [datetime] NULL,
	[CreatedBy_Id] [int] NULL,
	[UpdatedBy_Id] [int] NULL,
	[DeletedBy_Id] [int] NULL,
	[IsActive] [bit] NULL,
	[IsDeleted] [bit] NULL,
 CONSTRAINT [PK_Tbl_MasterNavigationAssignment] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Tbl_MasterParameter]    Script Date: 14/03/2024 14:32:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tbl_MasterParameter](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Key] [nvarchar](max) NULL,
	[Value] [nvarchar](max) NULL,
	[Description] [nvarchar](max) NULL,
	[Created_at] [datetime] NULL,
	[Updated_at] [datetime] NULL,
	[Deleted_at] [datetime] NULL,
	[CreatedBy_Id] [int] NULL,
	[UpdatedBy_Id] [int] NULL,
	[DeletedBy_Id] [int] NULL,
	[IsActive] [bit] NULL,
	[IsDeleted] [bit] NULL,
 CONSTRAINT [PK_Tbl_MasterParameter] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Tbl_User]    Script Date: 14/03/2024 14:32:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tbl_User](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Nama] [nvarchar](max) NULL,
	[Username] [nvarchar](50) NULL,
	[Password] [nvarchar](50) NULL,
	[Created_at] [datetime] NULL,
	[Updated_at] [datetime] NULL,
	[StartLogin] [datetime] NULL,
	[LastLogin] [datetime] NULL,
	[IsActive] [bit] NULL,
	[Token] [text] NULL,
	[UID] [nvarchar](max) NULL,
	[SecretKey] [nvarchar](max) NULL,
	[IsLogin] [bit] NULL,
 CONSTRAINT [PK_Tbl_User] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Tbl_JwtRepository] ON 

INSERT [dbo].[Tbl_JwtRepository] ([Id], [UserId], [ClientIP], [Token], [RefreshToken], [IsStop], [Hostname], [StartTime], [EndTime]) VALUES (55, 1, N'10.70.131.119', N'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJVSUQiOiI3b3E4b3pZa2Y5b3JieHdhejJSMlZRPT0iLCJJcEFkZHJlc3MiOiIxMC43MC4xMzEuMTE5IiwiSG9zdG5hbWUiOiJXREwtOTAwMjAyLTEuaHEuYm5pLmNvLmlkIiwibmJmIjoxNzA3MjA5ODYzLCJleHAiOjE3MDcyMDk5MjMsImlhdCI6MTcwNzIwOTg2MywiaXNzIjoiaHR0cDovL29mYWRldi5pZCJ9.QoOrskpohWupFWBXJMGjsFxJsVCQeE3AOT-LgIPM5_s', N'8228bf4a07a24bdd8b4f77baadc81ae0', 0, N'WDL-900202-1.hq.bni.co.id', CAST(N'2024-02-06T15:57:43.870' AS DateTime), NULL)
INSERT [dbo].[Tbl_JwtRepository] ([Id], [UserId], [ClientIP], [Token], [RefreshToken], [IsStop], [Hostname], [StartTime], [EndTime]) VALUES (56, 1, N'10.70.131.119', N'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJVSUQiOiI3b3E4b3pZa2Y5b3JieHdhejJSMlZRPT0iLCJJcEFkZHJlc3MiOiIxMC43MC4xMzEuMTE5IiwiSG9zdG5hbWUiOiJXREwtOTAwMjAyLTEuaHEuYm5pLmNvLmlkIiwibmJmIjoxNzA3MjA5OTEwLCJleHAiOjE3MDcyMDk5NzAsImlhdCI6MTcwNzIwOTkxMCwiaXNzIjoiaHR0cDovL29mYWRldi5pZCJ9.lXWf6f06Va0_TjXsuP7meqGmM6xBize8E5c21BIM074', N'e7e24c9d901f452bbb6928274cfcc3d1', 0, N'WDL-900202-1.hq.bni.co.id', CAST(N'2024-02-06T15:58:30.313' AS DateTime), NULL)
INSERT [dbo].[Tbl_JwtRepository] ([Id], [UserId], [ClientIP], [Token], [RefreshToken], [IsStop], [Hostname], [StartTime], [EndTime]) VALUES (57, 1, N'10.70.131.119', N'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJVSUQiOiI3b3E4b3pZa2Y5b3JieHdhejJSMlZRPT0iLCJJcEFkZHJlc3MiOiIxMC43MC4xMzEuMTE5IiwiSG9zdG5hbWUiOiJXREwtOTAwMjAyLTEuaHEuYm5pLmNvLmlkIiwibmJmIjoxNzA3MjA5OTU1LCJleHAiOjE3MDcyMTAwMTUsImlhdCI6MTcwNzIwOTk1NSwiaXNzIjoiaHR0cDovL29mYWRldi5pZCJ9.A4Dm3bEnSXPk7cHKjJfFEur9wQ03adi4S5E48lRWSk4', N'c694413685b34b9e975366f33f25d4dd', 0, N'WDL-900202-1.hq.bni.co.id', CAST(N'2024-02-06T15:59:15.583' AS DateTime), NULL)
INSERT [dbo].[Tbl_JwtRepository] ([Id], [UserId], [ClientIP], [Token], [RefreshToken], [IsStop], [Hostname], [StartTime], [EndTime]) VALUES (58, 1, N'10.70.131.119', N'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJVSUQiOiI3b3E4b3pZa2Y5b3JieHdhejJSMlZRPT0iLCJJcEFkZHJlc3MiOiIxMC43MC4xMzEuMTE5IiwiSG9zdG5hbWUiOiJXREwtOTAwMjAyLTEuaHEuYm5pLmNvLmlkIiwibmJmIjoxNzA3MjEwMDM1LCJleHAiOjE3MDcyMTAwOTUsImlhdCI6MTcwNzIxMDAzNSwiaXNzIjoiaHR0cDovL29mYWRldi5pZCJ9.ebffKqmetjsPCbuZcT_zW0iVStAd1X0HTRX428BOHUY', N'29c33b34c93d4cf9a2ee470b3559af00', 0, N'WDL-900202-1.hq.bni.co.id', CAST(N'2024-02-06T16:00:35.523' AS DateTime), NULL)
INSERT [dbo].[Tbl_JwtRepository] ([Id], [UserId], [ClientIP], [Token], [RefreshToken], [IsStop], [Hostname], [StartTime], [EndTime]) VALUES (59, 1, N'10.70.131.119', N'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJVSUQiOiI3b3E4b3pZa2Y5b3JieHdhejJSMlZRPT0iLCJJcEFkZHJlc3MiOiIxMC43MC4xMzEuMTE5IiwiSG9zdG5hbWUiOiJXREwtOTAwMjAyLTEuaHEuYm5pLmNvLmlkIiwibmJmIjoxNzA3MjEwMjkzLCJleHAiOjE3MDcyMTAzNTMsImlhdCI6MTcwNzIxMDI5MywiaXNzIjoiaHR0cDovL29mYWRldi5pZCJ9.Wan28Q2FtosD96q50BUNXTWI07NqsrYhXzQ_fP5Bj6Y', N'6e77ec22540e4feb9e724ee4f32ef0fd', 0, N'WDL-900202-1.hq.bni.co.id', CAST(N'2024-02-06T16:04:53.980' AS DateTime), NULL)
INSERT [dbo].[Tbl_JwtRepository] ([Id], [UserId], [ClientIP], [Token], [RefreshToken], [IsStop], [Hostname], [StartTime], [EndTime]) VALUES (60, 1, N'10.70.130.233', N'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJVSUQiOiI3b3E4b3pZa2Y5b3JieHdhejJSMlZRPT0iLCJJcEFkZHJlc3MiOiIxMC43MC4xMzAuMjMzIiwiSG9zdG5hbWUiOiJXREwtOTAwMjA0LTEuaHEuYm5pLmNvLmlkIiwibmJmIjoxNzA3MjEwNTgxLCJleHAiOjE3MDcyMTA2NDEsImlhdCI6MTcwNzIxMDU4MSwiaXNzIjoiaHR0cDovL29mYWRldi5pZCJ9.mMCEJ9yexf1Lfhnwxg3E97G2uskouZ648uG5EFePPCE', N'eb6cb41bcef047ef9f30dad612a10aee', 0, N'WDL-900204-1.hq.bni.co.id', CAST(N'2024-02-06T16:09:41.417' AS DateTime), NULL)
INSERT [dbo].[Tbl_JwtRepository] ([Id], [UserId], [ClientIP], [Token], [RefreshToken], [IsStop], [Hostname], [StartTime], [EndTime]) VALUES (61, 1, N'10.70.130.233', N'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJVSUQiOiI3b3E4b3pZa2Y5b3JieHdhejJSMlZRPT0iLCJJcEFkZHJlc3MiOiIxMC43MC4xMzAuMjMzIiwiSG9zdG5hbWUiOiJXREwtOTAwMjA0LTEuaHEuYm5pLmNvLmlkIiwibmJmIjoxNzA3MjEwNTgyLCJleHAiOjE3MDcyMTA2NDIsImlhdCI6MTcwNzIxMDU4MiwiaXNzIjoiaHR0cDovL29mYWRldi5pZCJ9.e_qEBaxpGfaXolbBN7L9tG1bK69RmLWmMqFDI_rhSLk', N'670a408683874462bea21e8d6ef66cf2', 0, N'WDL-900204-1.hq.bni.co.id', CAST(N'2024-02-06T16:09:42.417' AS DateTime), NULL)
INSERT [dbo].[Tbl_JwtRepository] ([Id], [UserId], [ClientIP], [Token], [RefreshToken], [IsStop], [Hostname], [StartTime], [EndTime]) VALUES (62, 1, N'10.70.130.233', N'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJVSUQiOiI3b3E4b3pZa2Y5b3JieHdhejJSMlZRPT0iLCJJcEFkZHJlc3MiOiIxMC43MC4xMzAuMjMzIiwiSG9zdG5hbWUiOiJXREwtOTAwMjA0LTEuaHEuYm5pLmNvLmlkIiwibmJmIjoxNzA3MjExMDQ1LCJleHAiOjE3MDcyMTExMDUsImlhdCI6MTcwNzIxMTA0NSwiaXNzIjoiaHR0cDovL29mYWRldi5pZCJ9.aZ6-ssmXSQ1VfbUkqs6_wSItAH4x2FnS-PYOtMEFvck', N'e29f5e91860b46169e908e773f07ffa7', 0, N'WDL-900204-1.hq.bni.co.id', CAST(N'2024-02-06T16:17:26.023' AS DateTime), NULL)
INSERT [dbo].[Tbl_JwtRepository] ([Id], [UserId], [ClientIP], [Token], [RefreshToken], [IsStop], [Hostname], [StartTime], [EndTime]) VALUES (63, 1, N'10.70.130.233', N'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJVSUQiOiI3b3E4b3pZa2Y5b3JieHdhejJSMlZRPT0iLCJJcEFkZHJlc3MiOiIxMC43MC4xMzAuMjMzIiwiSG9zdG5hbWUiOiJXREwtOTAwMjA0LTEuaHEuYm5pLmNvLmlkIiwibmJmIjoxNzA3MjExMDQ5LCJleHAiOjE3MDcyMTExMDksImlhdCI6MTcwNzIxMTA0OSwiaXNzIjoiaHR0cDovL29mYWRldi5pZCJ9.PEbFj_2FgjBeO4in-B1P7-_KZvAdQhtIS27ZNt7RcCM', N'45eefcac26f64f8e9e2d94e0b9310224', 0, N'WDL-900204-1.hq.bni.co.id', CAST(N'2024-02-06T16:17:29.073' AS DateTime), NULL)
INSERT [dbo].[Tbl_JwtRepository] ([Id], [UserId], [ClientIP], [Token], [RefreshToken], [IsStop], [Hostname], [StartTime], [EndTime]) VALUES (64, 1, N'10.70.130.233', N'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJVSUQiOiI3b3E4b3pZa2Y5b3JieHdhejJSMlZRPT0iLCJJcEFkZHJlc3MiOiIxMC43MC4xMzAuMjMzIiwiSG9zdG5hbWUiOiJXREwtOTAwMjA0LTEuaHEuYm5pLmNvLmlkIiwibmJmIjoxNzA3MjExMDUwLCJleHAiOjE3MDcyMTExMTAsImlhdCI6MTcwNzIxMTA1MCwiaXNzIjoiaHR0cDovL29mYWRldi5pZCJ9.4YdxKB-8n3pYAcM0uQquvq_7AtuB_yJ7ld7xLdLuEK8', N'4570a91eacb94d46a75db0673f267463', 0, N'WDL-900204-1.hq.bni.co.id', CAST(N'2024-02-06T16:17:30.267' AS DateTime), NULL)
INSERT [dbo].[Tbl_JwtRepository] ([Id], [UserId], [ClientIP], [Token], [RefreshToken], [IsStop], [Hostname], [StartTime], [EndTime]) VALUES (65, 1, N'10.70.130.233', N'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJVSUQiOiI3b3E4b3pZa2Y5b3JieHdhejJSMlZRPT0iLCJJcEFkZHJlc3MiOiIxMC43MC4xMzAuMjMzIiwiSG9zdG5hbWUiOiJXREwtOTAwMjA0LTEuaHEuYm5pLmNvLmlkIiwibmJmIjoxNzA3MjExMDUwLCJleHAiOjE3MDcyMTExMTAsImlhdCI6MTcwNzIxMTA1MCwiaXNzIjoiaHR0cDovL29mYWRldi5pZCJ9.4YdxKB-8n3pYAcM0uQquvq_7AtuB_yJ7ld7xLdLuEK8', N'bb5419b09566436fbab111b03776d4c8', 0, N'WDL-900204-1.hq.bni.co.id', CAST(N'2024-02-06T16:17:30.747' AS DateTime), NULL)
INSERT [dbo].[Tbl_JwtRepository] ([Id], [UserId], [ClientIP], [Token], [RefreshToken], [IsStop], [Hostname], [StartTime], [EndTime]) VALUES (66, 1, N'10.70.131.119', N'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJVSUQiOiI3b3E4b3pZa2Y5b3JieHdhejJSMlZRPT0iLCJJcEFkZHJlc3MiOiIxMC43MC4xMzEuMTE5IiwiSG9zdG5hbWUiOiJXREwtOTAwMjAyLTEuaHEuYm5pLmNvLmlkIiwibmJmIjoxNzA3MjExMDUzLCJleHAiOjE3MDcyMTExMTMsImlhdCI6MTcwNzIxMTA1MywiaXNzIjoiaHR0cDovL29mYWRldi5pZCJ9.xDBPL9k-xV_p_Y1CqUk5325yRgCffgV97ogABWac_c8', N'e8b4a393a17e4cfaac0e6e515e9ecdb8', 0, N'WDL-900202-1.hq.bni.co.id', CAST(N'2024-02-06T16:17:33.900' AS DateTime), NULL)
INSERT [dbo].[Tbl_JwtRepository] ([Id], [UserId], [ClientIP], [Token], [RefreshToken], [IsStop], [Hostname], [StartTime], [EndTime]) VALUES (67, 1, N'10.70.131.119', N'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJVSUQiOiI3b3E4b3pZa2Y5b3JieHdhejJSMlZRPT0iLCJJcEFkZHJlc3MiOiIxMC43MC4xMzEuMTE5IiwiSG9zdG5hbWUiOiJXREwtOTAwMjAyLTEuaHEuYm5pLmNvLmlkIiwibmJmIjoxNzA3MjExMTQ4LCJleHAiOjE3MDcyMTEyMDgsImlhdCI6MTcwNzIxMTE0OCwiaXNzIjoiaHR0cDovL29mYWRldi5pZCJ9.7H2t5EIZRy2CeElpB_eeWPc0eJ6hHzp7z3zxJ6U6qS8', N'c21c1923eb564e6099b975ea3d0cabf3', 0, N'WDL-900202-1.hq.bni.co.id', CAST(N'2024-02-06T16:19:08.040' AS DateTime), NULL)
INSERT [dbo].[Tbl_JwtRepository] ([Id], [UserId], [ClientIP], [Token], [RefreshToken], [IsStop], [Hostname], [StartTime], [EndTime]) VALUES (68, 1, N'10.70.130.233', N'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJVSUQiOiI3b3E4b3pZa2Y5b3JieHdhejJSMlZRPT0iLCJJcEFkZHJlc3MiOiIxMC43MC4xMzAuMjMzIiwiSG9zdG5hbWUiOiJXREwtOTAwMjA0LTEuaHEuYm5pLmNvLmlkIiwibmJmIjoxNzA3MjExMjYxLCJleHAiOjE3MDcyMTEzMjEsImlhdCI6MTcwNzIxMTI2MSwiaXNzIjoiaHR0cDovL29mYWRldi5pZCJ9.PWThwSNWnzXGJMwjXwJ3rk1FYDafqfdFCduyBi_j27k', N'230b58eda0204fd6b533140fcea09edf', 0, N'WDL-900204-1.hq.bni.co.id', CAST(N'2024-02-06T16:21:01.683' AS DateTime), NULL)
INSERT [dbo].[Tbl_JwtRepository] ([Id], [UserId], [ClientIP], [Token], [RefreshToken], [IsStop], [Hostname], [StartTime], [EndTime]) VALUES (69, 1, N'10.70.130.233', N'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJVSUQiOiI3b3E4b3pZa2Y5b3JieHdhejJSMlZRPT0iLCJJcEFkZHJlc3MiOiIxMC43MC4xMzAuMjMzIiwiSG9zdG5hbWUiOiJXREwtOTAwMjA0LTEuaHEuYm5pLmNvLmlkIiwibmJmIjoxNzA3MjExMjYyLCJleHAiOjE3MDcyMTEzMjIsImlhdCI6MTcwNzIxMTI2MiwiaXNzIjoiaHR0cDovL29mYWRldi5pZCJ9.Ija9GTQcVD36zA-irsu0Rw1l0pfhJQqhdAT-ZK2pQuE', N'571802ac3fce4a0089683bbd2b42e7db', 0, N'WDL-900204-1.hq.bni.co.id', CAST(N'2024-02-06T16:21:02.507' AS DateTime), NULL)
INSERT [dbo].[Tbl_JwtRepository] ([Id], [UserId], [ClientIP], [Token], [RefreshToken], [IsStop], [Hostname], [StartTime], [EndTime]) VALUES (70, 1, N'10.70.131.119', N'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJVSUQiOiI3b3E4b3pZa2Y5b3JieHdhejJSMlZRPT0iLCJJcEFkZHJlc3MiOiIxMC43MC4xMzEuMTE5IiwiSG9zdG5hbWUiOiJXREwtOTAwMjAyLTEuaHEuYm5pLmNvLmlkIiwibmJmIjoxNzA3MjExMjgwLCJleHAiOjE3MDcyMTEzNDAsImlhdCI6MTcwNzIxMTI4MCwiaXNzIjoiaHR0cDovL29mYWRldi5pZCJ9.29xrIq3D3o2oXtJrfihNYdK9KgF5yhELQ3UrXzcW3KY', N'bd4e60b147914c6181d4e09d0a4b88e4', 0, N'WDL-900202-1.hq.bni.co.id', CAST(N'2024-02-06T16:21:20.367' AS DateTime), NULL)
INSERT [dbo].[Tbl_JwtRepository] ([Id], [UserId], [ClientIP], [Token], [RefreshToken], [IsStop], [Hostname], [StartTime], [EndTime]) VALUES (71, 1, N'10.70.130.233', N'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJVSUQiOiI3b3E4b3pZa2Y5b3JieHdhejJSMlZRPT0iLCJJcEFkZHJlc3MiOiIxMC43MC4xMzAuMjMzIiwiSG9zdG5hbWUiOiJXREwtOTAwMjA0LTEuaHEuYm5pLmNvLmlkIiwibmJmIjoxNzA3MjExMjkyLCJleHAiOjE3MDcyMTEzNTIsImlhdCI6MTcwNzIxMTI5MiwiaXNzIjoiaHR0cDovL29mYWRldi5pZCJ9.r729lm_G1K7IZy3l1g2QCIOWLnsY8tfY7psTL5MFzqw', N'4523fbb3bceb4a53a82c3dbde4d10333', 0, N'WDL-900204-1.hq.bni.co.id', CAST(N'2024-02-06T16:21:32.167' AS DateTime), NULL)
INSERT [dbo].[Tbl_JwtRepository] ([Id], [UserId], [ClientIP], [Token], [RefreshToken], [IsStop], [Hostname], [StartTime], [EndTime]) VALUES (72, 1, N'10.70.130.233', N'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJVSUQiOiI3b3E4b3pZa2Y5b3JieHdhejJSMlZRPT0iLCJJcEFkZHJlc3MiOiIxMC43MC4xMzAuMjMzIiwiSG9zdG5hbWUiOiJXREwtOTAwMjA0LTEuaHEuYm5pLmNvLmlkIiwibmJmIjoxNzA3MjExMjkyLCJleHAiOjE3MDcyMTEzNTIsImlhdCI6MTcwNzIxMTI5MiwiaXNzIjoiaHR0cDovL29mYWRldi5pZCJ9.r729lm_G1K7IZy3l1g2QCIOWLnsY8tfY7psTL5MFzqw', N'6edb9ed5b50f4358b34717c6fee71e8c', 0, N'WDL-900204-1.hq.bni.co.id', CAST(N'2024-02-06T16:21:32.767' AS DateTime), NULL)
INSERT [dbo].[Tbl_JwtRepository] ([Id], [UserId], [ClientIP], [Token], [RefreshToken], [IsStop], [Hostname], [StartTime], [EndTime]) VALUES (73, 1, N'10.70.130.233', N'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJVSUQiOiI3b3E4b3pZa2Y5b3JieHdhejJSMlZRPT0iLCJJcEFkZHJlc3MiOiIxMC43MC4xMzAuMjMzIiwiSG9zdG5hbWUiOiJXREwtOTAwMjA0LTEuaHEuYm5pLmNvLmlkIiwibmJmIjoxNzA3MjExMjkyLCJleHAiOjE3MDcyMTEzNTIsImlhdCI6MTcwNzIxMTI5MiwiaXNzIjoiaHR0cDovL29mYWRldi5pZCJ9.r729lm_G1K7IZy3l1g2QCIOWLnsY8tfY7psTL5MFzqw', N'8f48ec46bf2a434b8bee6ee0834262c1', 0, N'WDL-900204-1.hq.bni.co.id', CAST(N'2024-02-06T16:21:32.957' AS DateTime), NULL)
INSERT [dbo].[Tbl_JwtRepository] ([Id], [UserId], [ClientIP], [Token], [RefreshToken], [IsStop], [Hostname], [StartTime], [EndTime]) VALUES (74, 1, N'10.70.130.233', N'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJVSUQiOiI3b3E4b3pZa2Y5b3JieHdhejJSMlZRPT0iLCJJcEFkZHJlc3MiOiIxMC43MC4xMzAuMjMzIiwiSG9zdG5hbWUiOiJXREwtOTAwMjA0LTEuaHEuYm5pLmNvLmlkIiwibmJmIjoxNzA3MjExMjkzLCJleHAiOjE3MDcyMTEzNTMsImlhdCI6MTcwNzIxMTI5MywiaXNzIjoiaHR0cDovL29mYWRldi5pZCJ9.CAK9fFeUAE54-XnUb2GfULkq5IrnXiIiIzfndkocOKA', N'34f5cb3473044bcba8b431ea6741ab30', 0, N'WDL-900204-1.hq.bni.co.id', CAST(N'2024-02-06T16:21:33.160' AS DateTime), NULL)
INSERT [dbo].[Tbl_JwtRepository] ([Id], [UserId], [ClientIP], [Token], [RefreshToken], [IsStop], [Hostname], [StartTime], [EndTime]) VALUES (75, 1, N'10.70.130.233', N'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJVSUQiOiI3b3E4b3pZa2Y5b3JieHdhejJSMlZRPT0iLCJJcEFkZHJlc3MiOiIxMC43MC4xMzAuMjMzIiwiSG9zdG5hbWUiOiJXREwtOTAwMjA0LTEuaHEuYm5pLmNvLmlkIiwibmJmIjoxNzA3MjExNTkwLCJleHAiOjE3MDcyMTE2NTAsImlhdCI6MTcwNzIxMTU5MCwiaXNzIjoiaHR0cDovL29mYWRldi5pZCJ9.PGfvpAWa6MGezdlgTVTCnJfrbx_p0MH3MH918DNMfEw', N'18dd105a7e814cf598229dba21d1cb9f', 0, N'WDL-900204-1.hq.bni.co.id', CAST(N'2024-02-06T16:26:30.097' AS DateTime), NULL)
INSERT [dbo].[Tbl_JwtRepository] ([Id], [UserId], [ClientIP], [Token], [RefreshToken], [IsStop], [Hostname], [StartTime], [EndTime]) VALUES (76, 1, N'10.70.130.233', N'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJVSUQiOiI3b3E4b3pZa2Y5b3JieHdhejJSMlZRPT0iLCJJcEFkZHJlc3MiOiIxMC43MC4xMzAuMjMzIiwiSG9zdG5hbWUiOiJXREwtOTAwMjA0LTEuaHEuYm5pLmNvLmlkIiwibmJmIjoxNzA3MjExNTkwLCJleHAiOjE3MDcyMTE2NTAsImlhdCI6MTcwNzIxMTU5MCwiaXNzIjoiaHR0cDovL29mYWRldi5pZCJ9.PGfvpAWa6MGezdlgTVTCnJfrbx_p0MH3MH918DNMfEw', N'34e0071b93c94643b043f507a1b68a61', 0, N'WDL-900204-1.hq.bni.co.id', CAST(N'2024-02-06T16:26:30.450' AS DateTime), NULL)
INSERT [dbo].[Tbl_JwtRepository] ([Id], [UserId], [ClientIP], [Token], [RefreshToken], [IsStop], [Hostname], [StartTime], [EndTime]) VALUES (77, 1, N'10.70.130.233', N'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJVSUQiOiI3b3E4b3pZa2Y5b3JieHdhejJSMlZRPT0iLCJJcEFkZHJlc3MiOiIxMC43MC4xMzAuMjMzIiwiSG9zdG5hbWUiOiJXREwtOTAwMjA0LTEuaHEuYm5pLmNvLmlkIiwibmJmIjoxNzA3MjExNTkxLCJleHAiOjE3MDcyMTE2NTEsImlhdCI6MTcwNzIxMTU5MSwiaXNzIjoiaHR0cDovL29mYWRldi5pZCJ9.JAEjyQMPA4k3-2uyrw4OpqkpuvZ5EAymGkEKqnzc2bs', N'1afd71d94ce7413a861a0649426d2ba4', 0, N'WDL-900204-1.hq.bni.co.id', CAST(N'2024-02-06T16:26:31.333' AS DateTime), NULL)
INSERT [dbo].[Tbl_JwtRepository] ([Id], [UserId], [ClientIP], [Token], [RefreshToken], [IsStop], [Hostname], [StartTime], [EndTime]) VALUES (78, 1, N'10.70.130.233', N'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJVSUQiOiI3b3E4b3pZa2Y5b3JieHdhejJSMlZRPT0iLCJJcEFkZHJlc3MiOiIxMC43MC4xMzAuMjMzIiwiSG9zdG5hbWUiOiJXREwtOTAwMjA0LTEuaHEuYm5pLmNvLmlkIiwibmJmIjoxNzA3MjEyMjkwLCJleHAiOjE3MDcyMTIzNTAsImlhdCI6MTcwNzIxMjI5MCwiaXNzIjoiaHR0cDovL29mYWRldi5pZCJ9.GPK2meL5VdgJiQc70l1LAFXnQ15aa2p3BdrjJOMGy_w', N'7b01d94cbc9c4547bfde2f9371a5e876', 0, N'WDL-900204-1.hq.bni.co.id', CAST(N'2024-02-06T16:38:10.163' AS DateTime), NULL)
INSERT [dbo].[Tbl_JwtRepository] ([Id], [UserId], [ClientIP], [Token], [RefreshToken], [IsStop], [Hostname], [StartTime], [EndTime]) VALUES (79, 1, N'10.70.130.233', N'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJVSUQiOiI3b3E4b3pZa2Y5b3JieHdhejJSMlZRPT0iLCJJcEFkZHJlc3MiOiIxMC43MC4xMzAuMjMzIiwiSG9zdG5hbWUiOiJXREwtOTAwMjA0LTEuaHEuYm5pLmNvLmlkIiwibmJmIjoxNzA3MjEyMjkwLCJleHAiOjE3MDcyMTIzNTAsImlhdCI6MTcwNzIxMjI5MCwiaXNzIjoiaHR0cDovL29mYWRldi5pZCJ9.GPK2meL5VdgJiQc70l1LAFXnQ15aa2p3BdrjJOMGy_w', N'ef5b0ad7442b409683127f25e27a787e', 0, N'WDL-900204-1.hq.bni.co.id', CAST(N'2024-02-06T16:38:10.163' AS DateTime), NULL)
INSERT [dbo].[Tbl_JwtRepository] ([Id], [UserId], [ClientIP], [Token], [RefreshToken], [IsStop], [Hostname], [StartTime], [EndTime]) VALUES (80, 1, N'10.70.130.233', N'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJVSUQiOiI3b3E4b3pZa2Y5b3JieHdhejJSMlZRPT0iLCJJcEFkZHJlc3MiOiIxMC43MC4xMzAuMjMzIiwiSG9zdG5hbWUiOiJXREwtOTAwMjA0LTEuaHEuYm5pLmNvLmlkIiwibmJmIjoxNzA3MjEyMjkwLCJleHAiOjE3MDcyMTIzNTAsImlhdCI6MTcwNzIxMjI5MCwiaXNzIjoiaHR0cDovL29mYWRldi5pZCJ9.GPK2meL5VdgJiQc70l1LAFXnQ15aa2p3BdrjJOMGy_w', N'50eec2f7f50b498e9b25b9657dcf8311', 0, N'WDL-900204-1.hq.bni.co.id', CAST(N'2024-02-06T16:38:10.163' AS DateTime), NULL)
INSERT [dbo].[Tbl_JwtRepository] ([Id], [UserId], [ClientIP], [Token], [RefreshToken], [IsStop], [Hostname], [StartTime], [EndTime]) VALUES (81, 1, N'10.70.130.233', N'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJVSUQiOiI3b3E4b3pZa2Y5b3JieHdhejJSMlZRPT0iLCJJcEFkZHJlc3MiOiIxMC43MC4xMzAuMjMzIiwiSG9zdG5hbWUiOiJXREwtOTAwMjA0LTEuaHEuYm5pLmNvLmlkIiwibmJmIjoxNzA3MjEyMjkwLCJleHAiOjE3MDcyMTIzNTAsImlhdCI6MTcwNzIxMjI5MCwiaXNzIjoiaHR0cDovL29mYWRldi5pZCJ9.GPK2meL5VdgJiQc70l1LAFXnQ15aa2p3BdrjJOMGy_w', N'46f81152c9cd46c5bbd970b612fa96e7', 0, N'WDL-900204-1.hq.bni.co.id', CAST(N'2024-02-06T16:38:10.627' AS DateTime), NULL)
INSERT [dbo].[Tbl_JwtRepository] ([Id], [UserId], [ClientIP], [Token], [RefreshToken], [IsStop], [Hostname], [StartTime], [EndTime]) VALUES (82, 1, N'10.70.130.233', N'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJVSUQiOiI3b3E4b3pZa2Y5b3JieHdhejJSMlZRPT0iLCJJcEFkZHJlc3MiOiIxMC43MC4xMzAuMjMzIiwiSG9zdG5hbWUiOiJXREwtOTAwMjA0LTEuaHEuYm5pLmNvLmlkIiwibmJmIjoxNzA3MjEyMjkxLCJleHAiOjE3MDcyMTIzNTEsImlhdCI6MTcwNzIxMjI5MSwiaXNzIjoiaHR0cDovL29mYWRldi5pZCJ9.Ht7u5sS0obTtxpMpO2lhtQMkMQUSenJXhckB5CylvOQ', N'7f12193408c14762a39a8b52a9d8cda9', 0, N'WDL-900204-1.hq.bni.co.id', CAST(N'2024-02-06T16:38:11.230' AS DateTime), NULL)
INSERT [dbo].[Tbl_JwtRepository] ([Id], [UserId], [ClientIP], [Token], [RefreshToken], [IsStop], [Hostname], [StartTime], [EndTime]) VALUES (83, 1, N'10.70.130.233', N'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJVSUQiOiI3b3E4b3pZa2Y5b3JieHdhejJSMlZRPT0iLCJJcEFkZHJlc3MiOiIxMC43MC4xMzAuMjMzIiwiSG9zdG5hbWUiOiJXREwtOTAwMjA0LTEuaHEuYm5pLmNvLmlkIiwibmJmIjoxNzA3MjEyMjk2LCJleHAiOjE3MDcyMTIzNTYsImlhdCI6MTcwNzIxMjI5NiwiaXNzIjoiaHR0cDovL29mYWRldi5pZCJ9.FZU4qC2QKoKJdAXy1W4OS-3HpAEmiONoQQ3qjGRDch8', N'ffc34c7856644cd5a82364f5be276c1d', 0, N'WDL-900204-1.hq.bni.co.id', CAST(N'2024-02-06T16:38:16.847' AS DateTime), NULL)
INSERT [dbo].[Tbl_JwtRepository] ([Id], [UserId], [ClientIP], [Token], [RefreshToken], [IsStop], [Hostname], [StartTime], [EndTime]) VALUES (84, 1, N'10.70.130.233', N'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJVSUQiOiI3b3E4b3pZa2Y5b3JieHdhejJSMlZRPT0iLCJJcEFkZHJlc3MiOiIxMC43MC4xMzAuMjMzIiwiSG9zdG5hbWUiOiJXREwtOTAwMjA0LTEuaHEuYm5pLmNvLmlkIiwibmJmIjoxNzA3MjEyMjk3LCJleHAiOjE3MDcyMTIzNTcsImlhdCI6MTcwNzIxMjI5NywiaXNzIjoiaHR0cDovL29mYWRldi5pZCJ9.DRvW5OlKr6_d2WF2dXhRpEEELWZ9SpQMBWiygQlUyPY', N'cb1c5a07958b4fdd9eaa2a60d8c02baf', 0, N'WDL-900204-1.hq.bni.co.id', CAST(N'2024-02-06T16:38:17.407' AS DateTime), NULL)
INSERT [dbo].[Tbl_JwtRepository] ([Id], [UserId], [ClientIP], [Token], [RefreshToken], [IsStop], [Hostname], [StartTime], [EndTime]) VALUES (85, 1, N'10.70.130.233', N'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJVSUQiOiI3b3E4b3pZa2Y5b3JieHdhejJSMlZRPT0iLCJJcEFkZHJlc3MiOiIxMC43MC4xMzAuMjMzIiwiSG9zdG5hbWUiOiJXREwtOTAwMjA0LTEuaHEuYm5pLmNvLmlkIiwibmJmIjoxNzA3MjEyMjk3LCJleHAiOjE3MDcyMTIzNTcsImlhdCI6MTcwNzIxMjI5NywiaXNzIjoiaHR0cDovL29mYWRldi5pZCJ9.DRvW5OlKr6_d2WF2dXhRpEEELWZ9SpQMBWiygQlUyPY', N'0a19040af4c74fb6a457122337a03021', 0, N'WDL-900204-1.hq.bni.co.id', CAST(N'2024-02-06T16:38:17.577' AS DateTime), NULL)
INSERT [dbo].[Tbl_JwtRepository] ([Id], [UserId], [ClientIP], [Token], [RefreshToken], [IsStop], [Hostname], [StartTime], [EndTime]) VALUES (86, 1, N'10.70.130.233', N'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJVSUQiOiI3b3E4b3pZa2Y5b3JieHdhejJSMlZRPT0iLCJJcEFkZHJlc3MiOiIxMC43MC4xMzAuMjMzIiwiSG9zdG5hbWUiOiJXREwtOTAwMjA0LTEuaHEuYm5pLmNvLmlkIiwibmJmIjoxNzA3MjEyMzAyLCJleHAiOjE3MDcyMTIzNjIsImlhdCI6MTcwNzIxMjMwMiwiaXNzIjoiaHR0cDovL29mYWRldi5pZCJ9.l74g9LTvTwLS_KjHg0MdZzFc8UleHQHs0WfOqk5fo4Q', N'd3db7384ec194c0ba8ecae50a75509e6', 0, N'WDL-900204-1.hq.bni.co.id', CAST(N'2024-02-06T16:38:22.180' AS DateTime), NULL)
INSERT [dbo].[Tbl_JwtRepository] ([Id], [UserId], [ClientIP], [Token], [RefreshToken], [IsStop], [Hostname], [StartTime], [EndTime]) VALUES (87, 1, N'10.70.130.233', N'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJVSUQiOiI3b3E4b3pZa2Y5b3JieHdhejJSMlZRPT0iLCJJcEFkZHJlc3MiOiIxMC43MC4xMzAuMjMzIiwiSG9zdG5hbWUiOiJXREwtOTAwMjA0LTEuaHEuYm5pLmNvLmlkIiwibmJmIjoxNzA3MjEyMzAyLCJleHAiOjE3MDcyMTIzNjIsImlhdCI6MTcwNzIxMjMwMiwiaXNzIjoiaHR0cDovL29mYWRldi5pZCJ9.l74g9LTvTwLS_KjHg0MdZzFc8UleHQHs0WfOqk5fo4Q', N'c460352d5bec4cc1ac5c231b54fd1abc', 0, N'WDL-900204-1.hq.bni.co.id', CAST(N'2024-02-06T16:38:22.347' AS DateTime), NULL)
INSERT [dbo].[Tbl_JwtRepository] ([Id], [UserId], [ClientIP], [Token], [RefreshToken], [IsStop], [Hostname], [StartTime], [EndTime]) VALUES (88, 1, N'10.70.130.233', N'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJVSUQiOiI3b3E4b3pZa2Y5b3JieHdhejJSMlZRPT0iLCJJcEFkZHJlc3MiOiIxMC43MC4xMzAuMjMzIiwiSG9zdG5hbWUiOiJXREwtOTAwMjA0LTEuaHEuYm5pLmNvLmlkIiwibmJmIjoxNzA3MjEyMzAyLCJleHAiOjE3MDcyMTIzNjIsImlhdCI6MTcwNzIxMjMwMiwiaXNzIjoiaHR0cDovL29mYWRldi5pZCJ9.l74g9LTvTwLS_KjHg0MdZzFc8UleHQHs0WfOqk5fo4Q', N'bae03349a4804dfb81d0d67f56548eee', 0, N'WDL-900204-1.hq.bni.co.id', CAST(N'2024-02-06T16:38:22.563' AS DateTime), NULL)
INSERT [dbo].[Tbl_JwtRepository] ([Id], [UserId], [ClientIP], [Token], [RefreshToken], [IsStop], [Hostname], [StartTime], [EndTime]) VALUES (89, 1, N'10.70.130.233', N'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJVSUQiOiI3b3E4b3pZa2Y5b3JieHdhejJSMlZRPT0iLCJJcEFkZHJlc3MiOiIxMC43MC4xMzAuMjMzIiwiSG9zdG5hbWUiOiJXREwtOTAwMjA0LTEuaHEuYm5pLmNvLmlkIiwibmJmIjoxNzA3MjEyMzAyLCJleHAiOjE3MDcyMTIzNjIsImlhdCI6MTcwNzIxMjMwMiwiaXNzIjoiaHR0cDovL29mYWRldi5pZCJ9.l74g9LTvTwLS_KjHg0MdZzFc8UleHQHs0WfOqk5fo4Q', N'09649531562a40ad8bbd83faac949423', 0, N'WDL-900204-1.hq.bni.co.id', CAST(N'2024-02-06T16:38:22.720' AS DateTime), NULL)
INSERT [dbo].[Tbl_JwtRepository] ([Id], [UserId], [ClientIP], [Token], [RefreshToken], [IsStop], [Hostname], [StartTime], [EndTime]) VALUES (90, 1, N'192.168.232.41', N'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJVSUQiOiI3b3E4b3pZa2Y5b3JieHdhejJSMlZRPT0iLCJJcEFkZHJlc3MiOiIxOTIuMTY4LjIzMi40MSIsIkhvc3RuYW1lIjoiSlRMUlJNVE9MU0QwMVdELmhxLmJuaS5jby5pZCIsIm5iZiI6MTcwNzIxODc3NiwiZXhwIjoxNzA3MjE4ODM2LCJpYXQiOjE3MDcyMTg3NzYsImlzcyI6Imh0dHA6Ly9vZmFkZXYuaWQifQ.vdaEuEtljjYV8i32hN2fTBFw5i1s1jTdkXdXmr-Nm0M', N'7ca452507c24452496663895fc49ab2c', 0, N'JTLRRMTOLSD01WD.hq.bni.co.id', CAST(N'2024-02-06T18:26:16.353' AS DateTime), NULL)
INSERT [dbo].[Tbl_JwtRepository] ([Id], [UserId], [ClientIP], [Token], [RefreshToken], [IsStop], [Hostname], [StartTime], [EndTime]) VALUES (91, 1, N'192.168.232.41', N'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJVSUQiOiI3b3E4b3pZa2Y5b3JieHdhejJSMlZRPT0iLCJJcEFkZHJlc3MiOiIxOTIuMTY4LjIzMi40MSIsIkhvc3RuYW1lIjoiSlRMUlJNVE9MU0QwMVdELmhxLmJuaS5jby5pZCIsIm5iZiI6MTcwNzIxODg3OCwiZXhwIjoxNzA3MjE4OTM4LCJpYXQiOjE3MDcyMTg4NzgsImlzcyI6Imh0dHA6Ly9vZmFkZXYuaWQifQ.1RDMWehFhkPD30KogHC6EVEx-SYizvV6rFphndlcGfA', N'692f5e72abdf4271acf12687590c9fa6', 0, N'JTLRRMTOLSD01WD.hq.bni.co.id', CAST(N'2024-02-06T18:27:58.797' AS DateTime), NULL)
INSERT [dbo].[Tbl_JwtRepository] ([Id], [UserId], [ClientIP], [Token], [RefreshToken], [IsStop], [Hostname], [StartTime], [EndTime]) VALUES (92, 1, N'192.168.232.41', N'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJVSUQiOiI3b3E4b3pZa2Y5b3JieHdhejJSMlZRPT0iLCJJcEFkZHJlc3MiOiIxOTIuMTY4LjIzMi40MSIsIkhvc3RuYW1lIjoiSlRMUlJNVE9MU0QwMVdELmhxLmJuaS5jby5pZCIsIm5iZiI6MTcwNzIxOTA4OCwiZXhwIjoxNzA3MjE5MTQ4LCJpYXQiOjE3MDcyMTkwODgsImlzcyI6Imh0dHA6Ly9vZmFkZXYuaWQifQ.Yb6Jg_oBneW2DudFOGx1CVxJ5fWiugS-wFlVWKLjVEk', N'59dcc562fb96446e91975acb770ef069', 0, N'JTLRRMTOLSD01WD.hq.bni.co.id', CAST(N'2024-02-06T18:31:28.713' AS DateTime), NULL)
INSERT [dbo].[Tbl_JwtRepository] ([Id], [UserId], [ClientIP], [Token], [RefreshToken], [IsStop], [Hostname], [StartTime], [EndTime]) VALUES (93, 1, N'192.168.232.41', N'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJVSUQiOiI3b3E4b3pZa2Y5b3JieHdhejJSMlZRPT0iLCJJcEFkZHJlc3MiOiIxOTIuMTY4LjIzMi40MSIsIkhvc3RuYW1lIjoiSlRMUlJNVE9MU0QwMVdELmhxLmJuaS5jby5pZCIsIm5iZiI6MTcwNzIxOTA5MiwiZXhwIjoxNzA3MjE5MTUyLCJpYXQiOjE3MDcyMTkwOTIsImlzcyI6Imh0dHA6Ly9vZmFkZXYuaWQifQ.kAAhusycRc5j90pRV2--e1FZyGamoc1S-ibShgVRVwE', N'dfd75a1a8e1a405b8ba9b505ca6498cb', 0, N'JTLRRMTOLSD01WD.hq.bni.co.id', CAST(N'2024-02-06T18:31:32.180' AS DateTime), NULL)
INSERT [dbo].[Tbl_JwtRepository] ([Id], [UserId], [ClientIP], [Token], [RefreshToken], [IsStop], [Hostname], [StartTime], [EndTime]) VALUES (94, 2, N'10.70.135.74', N'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJVSUQiOiJZcTZtY2hHdmtJTyttdGc2NCtBNWVRPT0iLCJJcEFkZHJlc3MiOiIxMC43MC4xMzUuNzQiLCJIb3N0bmFtZSI6IldETC05MDAyMDItMS5ocS5ibmkuY28uaWQiLCJuYmYiOjE3MTAzODA0MjQsImV4cCI6MTcxMDQ2NjgyNCwiaWF0IjoxNzEwMzgwNDI0LCJpc3MiOiJodHRwOi8vb2ZhZGV2LmlkIn0.KPaYIIme9mkz3vNPtHynhVQ7sgEOdNMrcVyecFZjfik', N'e42f6c432dbd405d9c7334f074f4d46c', 1, N'WDL-900202-1.hq.bni.co.id', CAST(N'2024-03-14T08:40:25.007' AS DateTime), CAST(N'2024-03-14T10:41:47.527' AS DateTime))
INSERT [dbo].[Tbl_JwtRepository] ([Id], [UserId], [ClientIP], [Token], [RefreshToken], [IsStop], [Hostname], [StartTime], [EndTime]) VALUES (95, 2, N'10.70.134.191', N'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJVSUQiOiJZcTZtY2hHdmtJTyttdGc2NCtBNWVRPT0iLCJJcEFkZHJlc3MiOiIxMC43MC4xMzQuMTkxIiwiSG9zdG5hbWUiOiJXREwtOTAwMjA0LTEuaHEuYm5pLmNvLmlkIiwibmJmIjoxNzEwMzgxNzk4LCJleHAiOjE3MTA0NjgxOTgsImlhdCI6MTcxMDM4MTc5OCwiaXNzIjoiaHR0cDovL29mYWRldi5pZCJ9.6b8e09-rGJ-OK2nIrH9wtVz0UXZZ11XzHytBhhtaRJs', N'71d08194229f4e74a8936aee22250a0a', 1, N'WDL-900204-1.hq.bni.co.id', CAST(N'2024-03-14T09:03:18.767' AS DateTime), CAST(N'2024-03-14T10:41:47.553' AS DateTime))
INSERT [dbo].[Tbl_JwtRepository] ([Id], [UserId], [ClientIP], [Token], [RefreshToken], [IsStop], [Hostname], [StartTime], [EndTime]) VALUES (96, 2, N'10.70.134.191', N'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJVSUQiOiJZcTZtY2hHdmtJTyttdGc2NCtBNWVRPT0iLCJJcEFkZHJlc3MiOiIxMC43MC4xMzQuMTkxIiwiSG9zdG5hbWUiOiJXREwtOTAwMjA0LTEuaHEuYm5pLmNvLmlkIiwibmJmIjoxNzEwMzgyNDk3LCJleHAiOjE3MTAzODYwOTcsImlhdCI6MTcxMDM4MjQ5NywiaXNzIjoiaHR0cDovL29mYWRldi5pZCJ9.YpG1BT8vcEzk1uDanLd7RLF4RyQVNYmFDOTsaC0tqr8', N'69f4db7f340349b09353f43e874f34f2', 1, N'WDL-900204-1.hq.bni.co.id', CAST(N'2024-03-14T09:14:57.347' AS DateTime), CAST(N'2024-03-14T10:41:47.573' AS DateTime))
INSERT [dbo].[Tbl_JwtRepository] ([Id], [UserId], [ClientIP], [Token], [RefreshToken], [IsStop], [Hostname], [StartTime], [EndTime]) VALUES (97, 2, N'10.70.135.74', N'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJVSUQiOiJZcTZtY2hHdmtJTyttdGc2NCtBNWVRPT0iLCJJcEFkZHJlc3MiOiIxMC43MC4xMzUuNzQiLCJIb3N0bmFtZSI6IldETC05MDAyMDItMS5ocS5ibmkuY28uaWQiLCJuYmYiOjE3MTAzODQxMDEsImV4cCI6MTcxMDQ3MDUwMSwiaWF0IjoxNzEwMzg0MTAxLCJpc3MiOiJodHRwOi8vb2ZhZGV2LmlkIn0._n15ZnLtUJJsaXstIibGIsxXyhJiEvC2Ze4Jg2SFgqA', N'125514c2faa2447584fd464d0d05b331', 1, N'WDL-900202-1.hq.bni.co.id', CAST(N'2024-03-14T09:41:41.283' AS DateTime), CAST(N'2024-03-14T10:41:47.597' AS DateTime))
INSERT [dbo].[Tbl_JwtRepository] ([Id], [UserId], [ClientIP], [Token], [RefreshToken], [IsStop], [Hostname], [StartTime], [EndTime]) VALUES (98, 2, N'10.70.134.191', N'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJVSUQiOiJZcTZtY2hHdmtJTyttdGc2NCtBNWVRPT0iLCJJcEFkZHJlc3MiOiIxMC43MC4xMzQuMTkxIiwiSG9zdG5hbWUiOiJXREwtOTAwMjA0LTEuaHEuYm5pLmNvLmlkIiwibmJmIjoxNzEwMzg1OTU3LCJleHAiOjE3MTA0NzIzNTcsImlhdCI6MTcxMDM4NTk1NywiaXNzIjoiaHR0cDovL29mYWRldi5pZCJ9.IwI4C_pMdgS-yVZSqUjLBpSEtvpnTvCj7Q0r2USpSaQ', N'3fabc483f6aa4ad1a76a151103caff6c', 1, N'WDL-900204-1.hq.bni.co.id', CAST(N'2024-03-14T10:12:37.963' AS DateTime), CAST(N'2024-03-14T10:41:47.613' AS DateTime))
INSERT [dbo].[Tbl_JwtRepository] ([Id], [UserId], [ClientIP], [Token], [RefreshToken], [IsStop], [Hostname], [StartTime], [EndTime]) VALUES (99, 2, N'10.70.134.191', N'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJVSUQiOiJZcTZtY2hHdmtJTyttdGc2NCtBNWVRPT0iLCJJcEFkZHJlc3MiOiIxMC43MC4xMzQuMTkxIiwiSG9zdG5hbWUiOiJXREwtOTAwMjA0LTEuaHEuYm5pLmNvLmlkIiwibmJmIjoxNzEwMzg3MzIwLCJleHAiOjE3MTAzOTA5MjAsImlhdCI6MTcxMDM4NzMyMCwiaXNzIjoiaHR0cDovL29mYWRldi5pZCJ9.ipYuqXxjN0Z6LpG48la0fqQQYa0seIiIfyymXqRXsQo', N'84fc9e9dd14d4752ae59ef41ab8f61cb', 1, N'WDL-900204-1.hq.bni.co.id', CAST(N'2024-03-14T10:35:20.410' AS DateTime), CAST(N'2024-03-14T10:41:47.637' AS DateTime))
INSERT [dbo].[Tbl_JwtRepository] ([Id], [UserId], [ClientIP], [Token], [RefreshToken], [IsStop], [Hostname], [StartTime], [EndTime]) VALUES (100, 2, N'10.70.134.191', N'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJVSUQiOiJZcTZtY2hHdmtJTyttdGc2NCtBNWVRPT0iLCJJcEFkZHJlc3MiOiIxMC43MC4xMzQuMTkxIiwiSG9zdG5hbWUiOiJXREwtOTAwMjA0LTEuaHEuYm5pLmNvLmlkIiwibmJmIjoxNzEwMzg3NjkxLCJleHAiOjE3MTAzOTEyOTEsImlhdCI6MTcxMDM4NzY5MSwiaXNzIjoiaHR0cDovL29mYWRldi5pZCJ9.9Cef_9Gyq7LwWLqheOJ7nR0HaqYyUfmo02jHqGuIEbA', N'76da576a4bed4676ba2eab3052b62bf9', 1, N'WDL-900204-1.hq.bni.co.id', CAST(N'2024-03-14T10:41:31.540' AS DateTime), CAST(N'2024-03-14T10:41:47.667' AS DateTime))
INSERT [dbo].[Tbl_JwtRepository] ([Id], [UserId], [ClientIP], [Token], [RefreshToken], [IsStop], [Hostname], [StartTime], [EndTime]) VALUES (101, 2, N'10.70.135.74', N'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJVSUQiOiJZcTZtY2hHdmtJTyttdGc2NCtBNWVRPT0iLCJJcEFkZHJlc3MiOiIxMC43MC4xMzUuNzQiLCJIb3N0bmFtZSI6IldETC05MDAyMDItMS5ocS5ibmkuY28uaWQiLCJuYmYiOjE3MTAzODc3MDksImV4cCI6MTcxMDQ3NDEwOSwiaWF0IjoxNzEwMzg3NzA5LCJpc3MiOiJodHRwOi8vb2ZhZGV2LmlkIn0.PE_xTl_ioSihL8mZm7MgIuwC1mn64zRCsm1NPI6LI4o', N'10dd72b5b6674cc3923d5eb2856ec446', 1, N'WDL-900202-1.hq.bni.co.id', CAST(N'2024-03-14T10:41:49.843' AS DateTime), CAST(N'2024-03-14T12:56:32.097' AS DateTime))
INSERT [dbo].[Tbl_JwtRepository] ([Id], [UserId], [ClientIP], [Token], [RefreshToken], [IsStop], [Hostname], [StartTime], [EndTime]) VALUES (102, 2, N'10.70.134.191', N'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJVSUQiOiJZcTZtY2hHdmtJTyttdGc2NCtBNWVRPT0iLCJJcEFkZHJlc3MiOiIxMC43MC4xMzQuMTkxIiwiSG9zdG5hbWUiOiJXREwtOTAwMjA0LTEuaHEuYm5pLmNvLmlkIiwibmJmIjoxNzEwMzk1NzE0LCJleHAiOjE3MTA0ODIxMTQsImlhdCI6MTcxMDM5NTcxNCwiaXNzIjoiaHR0cDovL29mYWRldi5pZCJ9.zUOTg19SvKB6C13WeomKLhR7AwEet7IBv95-O77WHtE', N'15e5f6591cb34e2b86b1047042f459b8', 1, N'WDL-900204-1.hq.bni.co.id', CAST(N'2024-03-14T12:55:14.437' AS DateTime), CAST(N'2024-03-14T12:56:31.963' AS DateTime))
INSERT [dbo].[Tbl_JwtRepository] ([Id], [UserId], [ClientIP], [Token], [RefreshToken], [IsStop], [Hostname], [StartTime], [EndTime]) VALUES (103, 2, N'10.70.134.191', N'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJVSUQiOiJZcTZtY2hHdmtJTyttdGc2NCtBNWVRPT0iLCJJcEFkZHJlc3MiOiIxMC43MC4xMzQuMTkxIiwiSG9zdG5hbWUiOiJXREwtOTAwMjA0LTEuaHEuYm5pLmNvLmlkIiwibmJmIjoxNzEwMzk1NzkyLCJleHAiOjE3MTAzOTkzOTIsImlhdCI6MTcxMDM5NTc5MiwiaXNzIjoiaHR0cDovL29mYWRldi5pZCJ9.q7_n4xqW2QVxt-BEj1w5NPJ596Suq68os_TPMtA0vZs', N'9f0837430d8946eb8c53c30f0437f09d', 1, N'WDL-900204-1.hq.bni.co.id', CAST(N'2024-03-14T12:56:32.130' AS DateTime), CAST(N'2024-03-14T12:59:00.710' AS DateTime))
INSERT [dbo].[Tbl_JwtRepository] ([Id], [UserId], [ClientIP], [Token], [RefreshToken], [IsStop], [Hostname], [StartTime], [EndTime]) VALUES (104, 2, N'10.70.134.191', N'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJVSUQiOiJZcTZtY2hHdmtJTyttdGc2NCtBNWVRPT0iLCJJcEFkZHJlc3MiOiIxMC43MC4xMzQuMTkxIiwiSG9zdG5hbWUiOiJXREwtOTAwMjA0LTEuaHEuYm5pLmNvLmlkIiwibmJmIjoxNzEwMzk1OTQwLCJleHAiOjE3MTAzOTk1NDAsImlhdCI6MTcxMDM5NTk0MCwiaXNzIjoiaHR0cDovL29mYWRldi5pZCJ9.klblz4sF4W_Sywns5zzW4n1t7ahHICVyU8vZK0hTzKI', N'305f73862f1548c583ed1dcc3030a13f', 1, N'WDL-900204-1.hq.bni.co.id', CAST(N'2024-03-14T12:59:00.870' AS DateTime), CAST(N'2024-03-14T13:02:40.383' AS DateTime))
INSERT [dbo].[Tbl_JwtRepository] ([Id], [UserId], [ClientIP], [Token], [RefreshToken], [IsStop], [Hostname], [StartTime], [EndTime]) VALUES (105, 2, N'10.70.134.191', N'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJVSUQiOiJZcTZtY2hHdmtJTyttdGc2NCtBNWVRPT0iLCJJcEFkZHJlc3MiOiIxMC43MC4xMzQuMTkxIiwiSG9zdG5hbWUiOiJXREwtOTAwMjA0LTEuaHEuYm5pLmNvLmlkIiwibmJmIjoxNzEwMzk2MTYwLCJleHAiOjE3MTAzOTk3NjAsImlhdCI6MTcxMDM5NjE2MCwiaXNzIjoiaHR0cDovL29mYWRldi5pZCJ9.VC87cIQzQ6LeLEjimrECg4OTL8PQOx1fIidxZSk3LZY', N'50df6c051a7b40169af6747788ab1af0', 1, N'WDL-900204-1.hq.bni.co.id', CAST(N'2024-03-14T13:02:40.560' AS DateTime), CAST(N'2024-03-14T13:08:30.540' AS DateTime))
INSERT [dbo].[Tbl_JwtRepository] ([Id], [UserId], [ClientIP], [Token], [RefreshToken], [IsStop], [Hostname], [StartTime], [EndTime]) VALUES (106, 2, N'10.70.134.191', N'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJVSUQiOiJZcTZtY2hHdmtJTyttdGc2NCtBNWVRPT0iLCJJcEFkZHJlc3MiOiIxMC43MC4xMzQuMTkxIiwiSG9zdG5hbWUiOiJXREwtOTAwMjA0LTEuaHEuYm5pLmNvLmlkIiwibmJmIjoxNzEwMzk2NTEwLCJleHAiOjE3MTA0MDAxMTAsImlhdCI6MTcxMDM5NjUxMCwiaXNzIjoiaHR0cDovL29mYWRldi5pZCJ9.2g4hr73e7nipQRX-y-5fKhcY0cLYdxqd5YDp0T82LLw', N'd4baee05aa424695a5ddeaf32d3246d2', 1, N'WDL-900204-1.hq.bni.co.id', CAST(N'2024-03-14T13:08:30.933' AS DateTime), CAST(N'2024-03-14T13:10:28.773' AS DateTime))
INSERT [dbo].[Tbl_JwtRepository] ([Id], [UserId], [ClientIP], [Token], [RefreshToken], [IsStop], [Hostname], [StartTime], [EndTime]) VALUES (107, 2, N'10.70.134.191', N'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJVSUQiOiJZcTZtY2hHdmtJTyttdGc2NCtBNWVRPT0iLCJJcEFkZHJlc3MiOiIxMC43MC4xMzQuMTkxIiwiSG9zdG5hbWUiOiJXREwtOTAwMjA0LTEuaHEuYm5pLmNvLmlkIiwibmJmIjoxNzEwMzk2NjMwLCJleHAiOjE3MTA0MDAyMzAsImlhdCI6MTcxMDM5NjYzMCwiaXNzIjoiaHR0cDovL29mYWRldi5pZCJ9.Sc0PlAJQegMr5wXywuWCGK8HQhUDJxVJ9ELEQTBv000', N'e68542782d144d58a79bfe8fddc6e92c', 1, N'WDL-900204-1.hq.bni.co.id', CAST(N'2024-03-14T13:10:30.420' AS DateTime), CAST(N'2024-03-14T13:20:34.830' AS DateTime))
INSERT [dbo].[Tbl_JwtRepository] ([Id], [UserId], [ClientIP], [Token], [RefreshToken], [IsStop], [Hostname], [StartTime], [EndTime]) VALUES (108, 2, N'10.70.134.191', N'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJVSUQiOiJZcTZtY2hHdmtJTyttdGc2NCtBNWVRPT0iLCJJcEFkZHJlc3MiOiIxMC43MC4xMzQuMTkxIiwiSG9zdG5hbWUiOiJXREwtOTAwMjA0LTEuaHEuYm5pLmNvLmlkIiwibmJmIjoxNzEwMzk3MjM0LCJleHAiOjE3MTA0MDA4MzQsImlhdCI6MTcxMDM5NzIzNCwiaXNzIjoiaHR0cDovL29mYWRldi5pZCJ9.fr9Q6GQI6fwl1qwYjCUSx5vtYZcafM2R_CY5nZIBqQk', N'9ad2fb29ed9a475c8f9d0215f3b316fb', 1, N'WDL-900204-1.hq.bni.co.id', CAST(N'2024-03-14T13:20:35.010' AS DateTime), CAST(N'2024-03-14T14:01:02.780' AS DateTime))
INSERT [dbo].[Tbl_JwtRepository] ([Id], [UserId], [ClientIP], [Token], [RefreshToken], [IsStop], [Hostname], [StartTime], [EndTime]) VALUES (109, 2, N'10.70.134.191', N'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJVSUQiOiJZcTZtY2hHdmtJTyttdGc2NCtBNWVRPT0iLCJJcEFkZHJlc3MiOiIxMC43MC4xMzQuMTkxIiwiSG9zdG5hbWUiOiJXREwtOTAwMjA0LTEuaHEuYm5pLmNvLmlkIiwibmJmIjoxNzEwMzk5NjYzLCJleHAiOjE3MTA0MDMyNjMsImlhdCI6MTcxMDM5OTY2MywiaXNzIjoiaHR0cDovL29mYWRldi5pZCJ9.FZFRIf4205gUSKlOX2L9Qqiq63cAHryqjZJw0qIBRiM', N'436a9878c8ac4c7fa569f060dcd4d60f', 1, N'WDL-900204-1.hq.bni.co.id', CAST(N'2024-03-14T14:01:03.110' AS DateTime), CAST(N'2024-03-14T14:05:50.757' AS DateTime))
INSERT [dbo].[Tbl_JwtRepository] ([Id], [UserId], [ClientIP], [Token], [RefreshToken], [IsStop], [Hostname], [StartTime], [EndTime]) VALUES (110, 2, N'10.70.134.191', N'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJVSUQiOiJZcTZtY2hHdmtJTyttdGc2NCtBNWVRPT0iLCJJcEFkZHJlc3MiOiIxMC43MC4xMzQuMTkxIiwiSG9zdG5hbWUiOiJXREwtOTAwMjA0LTEuaHEuYm5pLmNvLmlkIiwibmJmIjoxNzEwMzk5OTUxLCJleHAiOjE3MTA0MDM1NTEsImlhdCI6MTcxMDM5OTk1MSwiaXNzIjoiaHR0cDovL29mYWRldi5pZCJ9.BbnjxGrRrFoEu_Vz9y_iAClvsaXosHd_E-IpuWf6-Z8', N'56745e1f91af45899b01fe4ea47c48ff', 1, N'WDL-900204-1.hq.bni.co.id', CAST(N'2024-03-14T14:05:51.053' AS DateTime), CAST(N'2024-03-14T14:17:10.870' AS DateTime))
INSERT [dbo].[Tbl_JwtRepository] ([Id], [UserId], [ClientIP], [Token], [RefreshToken], [IsStop], [Hostname], [StartTime], [EndTime]) VALUES (111, 2, N'10.70.134.191', N'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJVSUQiOiJZcTZtY2hHdmtJTyttdGc2NCtBNWVRPT0iLCJJcEFkZHJlc3MiOiIxMC43MC4xMzQuMTkxIiwiSG9zdG5hbWUiOiJXREwtOTAwMjA0LTEuaHEuYm5pLmNvLmlkIiwibmJmIjoxNzEwNDAwNjMxLCJleHAiOjE3MTA0MDQyMzEsImlhdCI6MTcxMDQwMDYzMSwiaXNzIjoiaHR0cDovL29mYWRldi5pZCJ9.GCMHIBe_dPNA7RcWC7y9A-wgE9tyhaJ5pB25ppPRMK8', N'a738b3c9090a4afa843001082a1d13c4', 1, N'WDL-900204-1.hq.bni.co.id', CAST(N'2024-03-14T14:17:11.067' AS DateTime), CAST(N'2024-03-14T14:20:46.563' AS DateTime))
INSERT [dbo].[Tbl_JwtRepository] ([Id], [UserId], [ClientIP], [Token], [RefreshToken], [IsStop], [Hostname], [StartTime], [EndTime]) VALUES (112, 2, N'10.70.134.191', N'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJVSUQiOiJZcTZtY2hHdmtJTyttdGc2NCtBNWVRPT0iLCJJcEFkZHJlc3MiOiIxMC43MC4xMzQuMTkxIiwiSG9zdG5hbWUiOiJXREwtOTAwMjA0LTEuaHEuYm5pLmNvLmlkIiwibmJmIjoxNzEwNDAwODQ2LCJleHAiOjE3MTA0MDQ0NDYsImlhdCI6MTcxMDQwMDg0NiwiaXNzIjoiaHR0cDovL29mYWRldi5pZCJ9.WWScKYneWlNvmTvDYEHYOoRifb7cavJfdd42hUw1djw', N'b7c2661caec34628adbf0ea188dba281', 0, N'WDL-900204-1.hq.bni.co.id', CAST(N'2024-03-14T14:20:46.733' AS DateTime), NULL)
SET IDENTITY_INSERT [dbo].[Tbl_JwtRepository] OFF
GO
SET IDENTITY_INSERT [dbo].[Tbl_LogPerubahanKelolaan] ON 

INSERT [dbo].[Tbl_LogPerubahanKelolaan] ([Id], [CIF], [Nama_debitur], [Npp_RM], [Npp_BA], [Npp_RMTransaksi], [Updated_at], [UpdatedBy_Id]) VALUES (1, N'12345678', N'Meyzan', N'12345', N'12345', N'12345', NULL, NULL)
SET IDENTITY_INSERT [dbo].[Tbl_LogPerubahanKelolaan] OFF
GO
SET IDENTITY_INSERT [dbo].[Tbl_MasterLookup] ON 

INSERT [dbo].[Tbl_MasterLookup] ([Id], [Type], [Name], [Value], [OrderBy], [Created_at], [Updated_at], [Deleted_at], [CreatedBy_Id], [UpdatedBy_Id], [DeletedBy_Id], [IsActive], [IsDeleted]) VALUES (4, N'TypeMenu', N'Header Menu', 1, 1, CAST(N'2024-02-05T15:33:05.977' AS DateTime), NULL, NULL, 1, NULL, NULL, NULL, 0)
INSERT [dbo].[Tbl_MasterLookup] ([Id], [Type], [Name], [Value], [OrderBy], [Created_at], [Updated_at], [Deleted_at], [CreatedBy_Id], [UpdatedBy_Id], [DeletedBy_Id], [IsActive], [IsDeleted]) VALUES (5, N'TypeMenu', N'Menu', 2, 2, CAST(N'2024-02-05T15:33:44.953' AS DateTime), NULL, NULL, 1, NULL, NULL, NULL, 0)
INSERT [dbo].[Tbl_MasterLookup] ([Id], [Type], [Name], [Value], [OrderBy], [Created_at], [Updated_at], [Deleted_at], [CreatedBy_Id], [UpdatedBy_Id], [DeletedBy_Id], [IsActive], [IsDeleted]) VALUES (6, N'TypeMenu', N'Sub Menu', 3, 3, CAST(N'2024-02-05T15:33:57.787' AS DateTime), NULL, NULL, 1, NULL, NULL, NULL, 0)
INSERT [dbo].[Tbl_MasterLookup] ([Id], [Type], [Name], [Value], [OrderBy], [Created_at], [Updated_at], [Deleted_at], [CreatedBy_Id], [UpdatedBy_Id], [DeletedBy_Id], [IsActive], [IsDeleted]) VALUES (7, N'IsActive', N'Active', 1, 1, CAST(N'2024-02-05T15:55:59.290' AS DateTime), NULL, NULL, 1, NULL, NULL, NULL, 0)
INSERT [dbo].[Tbl_MasterLookup] ([Id], [Type], [Name], [Value], [OrderBy], [Created_at], [Updated_at], [Deleted_at], [CreatedBy_Id], [UpdatedBy_Id], [DeletedBy_Id], [IsActive], [IsDeleted]) VALUES (8, N'IsActive', N'Tidak Aktif', 0, 2, CAST(N'2024-02-05T15:56:22.937' AS DateTime), NULL, NULL, 1, NULL, NULL, NULL, 0)
INSERT [dbo].[Tbl_MasterLookup] ([Id], [Type], [Name], [Value], [OrderBy], [Created_at], [Updated_at], [Deleted_at], [CreatedBy_Id], [UpdatedBy_Id], [DeletedBy_Id], [IsActive], [IsDeleted]) VALUES (9, N'Test', N'Test', 1, 1, CAST(N'2024-03-14T09:36:24.763' AS DateTime), NULL, CAST(N'2024-03-14T09:36:29.147' AS DateTime), 2, NULL, 2, 0, 1)
SET IDENTITY_INSERT [dbo].[Tbl_MasterLookup] OFF
GO
SET IDENTITY_INSERT [dbo].[Tbl_MasterNavigation] ON 

INSERT [dbo].[Tbl_MasterNavigation] ([Id], [Type], [Name], [Route], [Order], [Visible], [ParentNavigationId], [Created_at], [Updated_at], [Deleted_at], [CreatedBy_Id], [UpdatedBy_Id], [DeletedBy_Id], [IsDeleted], [IconClass]) VALUES (16, 2, N'Home', N'home', 1, 1, 0, CAST(N'2024-03-14T11:29:00.810' AS DateTime), NULL, NULL, 2, NULL, NULL, 0, N'home')
INSERT [dbo].[Tbl_MasterNavigation] ([Id], [Type], [Name], [Route], [Order], [Visible], [ParentNavigationId], [Created_at], [Updated_at], [Deleted_at], [CreatedBy_Id], [UpdatedBy_Id], [DeletedBy_Id], [IsDeleted], [IconClass]) VALUES (17, 2, N'Debitur Kelolaan', N'tagging-debitur', 2, 1, 0, CAST(N'2024-03-14T11:29:26.750' AS DateTime), NULL, NULL, 2, NULL, NULL, 0, N'users')
INSERT [dbo].[Tbl_MasterNavigation] ([Id], [Type], [Name], [Route], [Order], [Visible], [ParentNavigationId], [Created_at], [Updated_at], [Deleted_at], [CreatedBy_Id], [UpdatedBy_Id], [DeletedBy_Id], [IsDeleted], [IconClass]) VALUES (18, 2, N'Reporting', N'reporting', 3, 1, 0, CAST(N'2024-03-14T11:29:55.407' AS DateTime), NULL, NULL, 2, NULL, NULL, 0, N'layers')
INSERT [dbo].[Tbl_MasterNavigation] ([Id], [Type], [Name], [Route], [Order], [Visible], [ParentNavigationId], [Created_at], [Updated_at], [Deleted_at], [CreatedBy_Id], [UpdatedBy_Id], [DeletedBy_Id], [IsDeleted], [IconClass]) VALUES (19, 3, N'Bulk Upload', N'bulk-upload', 1, 1, 17, CAST(N'2024-03-14T11:30:26.677' AS DateTime), NULL, NULL, 2, NULL, NULL, 0, N'-')
INSERT [dbo].[Tbl_MasterNavigation] ([Id], [Type], [Name], [Route], [Order], [Visible], [ParentNavigationId], [Created_at], [Updated_at], [Deleted_at], [CreatedBy_Id], [UpdatedBy_Id], [DeletedBy_Id], [IsDeleted], [IconClass]) VALUES (20, 3, N'Manual Update', N'manual-update', 2, 1, 17, CAST(N'2024-03-14T11:30:47.233' AS DateTime), NULL, NULL, 2, NULL, NULL, 0, N'-')
INSERT [dbo].[Tbl_MasterNavigation] ([Id], [Type], [Name], [Route], [Order], [Visible], [ParentNavigationId], [Created_at], [Updated_at], [Deleted_at], [CreatedBy_Id], [UpdatedBy_Id], [DeletedBy_Id], [IsDeleted], [IconClass]) VALUES (21, 3, N'Report', N'report', 1, 1, 18, CAST(N'2024-03-14T11:31:15.403' AS DateTime), NULL, NULL, 2, NULL, NULL, 0, N'-')
SET IDENTITY_INSERT [dbo].[Tbl_MasterNavigation] OFF
GO
SET IDENTITY_INSERT [dbo].[Tbl_MasterNavigationAssignment] ON 

INSERT [dbo].[Tbl_MasterNavigationAssignment] ([Id], [Navigation_Id], [UserId], [Created_at], [Updated_at], [Deleted_at], [CreatedBy_Id], [UpdatedBy_Id], [DeletedBy_Id], [IsActive], [IsDeleted]) VALUES (4, 16, 2, CAST(N'2024-03-14T11:35:34.623' AS DateTime), NULL, NULL, 2, NULL, NULL, 1, 0)
INSERT [dbo].[Tbl_MasterNavigationAssignment] ([Id], [Navigation_Id], [UserId], [Created_at], [Updated_at], [Deleted_at], [CreatedBy_Id], [UpdatedBy_Id], [DeletedBy_Id], [IsActive], [IsDeleted]) VALUES (5, 17, 2, CAST(N'2024-03-14T11:35:41.697' AS DateTime), NULL, NULL, 2, NULL, NULL, 1, 0)
INSERT [dbo].[Tbl_MasterNavigationAssignment] ([Id], [Navigation_Id], [UserId], [Created_at], [Updated_at], [Deleted_at], [CreatedBy_Id], [UpdatedBy_Id], [DeletedBy_Id], [IsActive], [IsDeleted]) VALUES (6, 18, 2, CAST(N'2024-03-14T11:35:47.960' AS DateTime), NULL, NULL, 2, NULL, NULL, 1, 0)
INSERT [dbo].[Tbl_MasterNavigationAssignment] ([Id], [Navigation_Id], [UserId], [Created_at], [Updated_at], [Deleted_at], [CreatedBy_Id], [UpdatedBy_Id], [DeletedBy_Id], [IsActive], [IsDeleted]) VALUES (7, 19, 2, CAST(N'2024-03-14T11:35:53.353' AS DateTime), NULL, NULL, 2, NULL, NULL, 1, 0)
INSERT [dbo].[Tbl_MasterNavigationAssignment] ([Id], [Navigation_Id], [UserId], [Created_at], [Updated_at], [Deleted_at], [CreatedBy_Id], [UpdatedBy_Id], [DeletedBy_Id], [IsActive], [IsDeleted]) VALUES (8, 20, 2, CAST(N'2024-03-14T11:36:03.593' AS DateTime), NULL, NULL, 2, NULL, NULL, 1, 0)
INSERT [dbo].[Tbl_MasterNavigationAssignment] ([Id], [Navigation_Id], [UserId], [Created_at], [Updated_at], [Deleted_at], [CreatedBy_Id], [UpdatedBy_Id], [DeletedBy_Id], [IsActive], [IsDeleted]) VALUES (9, 21, 2, CAST(N'2024-03-14T11:36:10.680' AS DateTime), NULL, NULL, 2, NULL, NULL, 1, 0)
SET IDENTITY_INSERT [dbo].[Tbl_MasterNavigationAssignment] OFF
GO
SET IDENTITY_INSERT [dbo].[Tbl_User] ON 

INSERT [dbo].[Tbl_User] ([Id], [Nama], [Username], [Password], [Created_at], [Updated_at], [StartLogin], [LastLogin], [IsActive], [Token], [UID], [SecretKey], [IsLogin]) VALUES (1, N'Meyzan', N'51999', N'abc123', NULL, NULL, CAST(N'2024-02-06T18:31:32.183' AS DateTime), NULL, 1, N'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJVSUQiOiI3b3E4b3pZa2Y5b3JieHdhejJSMlZRPT0iLCJJcEFkZHJlc3MiOiIxOTIuMTY4LjIzMi40MSIsIkhvc3RuYW1lIjoiSlRMUlJNVE9MU0QwMVdELmhxLmJuaS5jby5pZCIsIm5iZiI6MTcwNzIxOTA5MiwiZXhwIjoxNzA3MjE5MTUyLCJpYXQiOjE3MDcyMTkwOTIsImlzcyI6Imh0dHA6Ly9vZmFkZXYuaWQifQ.kAAhusycRc5j90pRV2--e1FZyGamoc1S-ibShgVRVwE', N'7oq8ozYkf9orbxwaz2R2VQ==', NULL, 0)
INSERT [dbo].[Tbl_User] ([Id], [Nama], [Username], [Password], [Created_at], [Updated_at], [StartLogin], [LastLogin], [IsActive], [Token], [UID], [SecretKey], [IsLogin]) VALUES (2, N'Andra', N'51888', N'abc123', NULL, NULL, CAST(N'2024-03-14T14:20:46.813' AS DateTime), CAST(N'2024-03-14T10:41:47.467' AS DateTime), 1, N'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJVSUQiOiJZcTZtY2hHdmtJTyttdGc2NCtBNWVRPT0iLCJJcEFkZHJlc3MiOiIxMC43MC4xMzQuMTkxIiwiSG9zdG5hbWUiOiJXREwtOTAwMjA0LTEuaHEuYm5pLmNvLmlkIiwibmJmIjoxNzEwNDAwODQ2LCJleHAiOjE3MTA0MDQ0NDYsImlhdCI6MTcxMDQwMDg0NiwiaXNzIjoiaHR0cDovL29mYWRldi5pZCJ9.WWScKYneWlNvmTvDYEHYOoRifb7cavJfdd42hUw1djw', N'Yq6mchGvkIO+mtg64+A5eQ==', NULL, 1)
SET IDENTITY_INSERT [dbo].[Tbl_User] OFF
GO
/****** Object:  StoredProcedure [dbo].[sp_NavigationAssignment_count]    Script Date: 14/03/2024 14:32:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_NavigationAssignment_count] 	
  @NamaMenu varchar(MAX) ='',
  @NamaUser varchar(MAX) =''
AS 
BEGIN
DECLARE @Query VARCHAR(MAX) = 'select Count(*)
								  FROM [dbo].[Tbl_MasterNavigationAssignment] N
								  LEFT JOIN dbo.Tbl_MasterNavigation MN ON N.Navigation_Id = MN.Id
								  LEFT JOIN dbo.Tbl_User NP ON N.UserId = NP.Id
								  LEFT JOIN dbo.Tbl_User PC ON N.CreatedBy_Id = PC.Id
								  LEFT JOIN dbo.Tbl_User PU ON N.UpdatedBy_Id = PU.Id
								  LEFT JOIN dbo.Tbl_User PD ON N.DeletedBy_Id = PD.Id
								  LEFT JOIN dbo.Tbl_MasterLookup AZ ON AZ.Value = N.IsDeleted AND AZ.Type = ''IsActive''
								  Where (N.IsDeleted = 0 OR N.IsDeleted is null) 
								  ',
	@QueryNamaMenu varchar(MAX) = '',
	@QueryUser varchar(MAX) = ''


SELECT @QueryNamaMenu = dbo.uf_LookupDynamicQueryGenerator(@NamaMenu, 'MN.Name')
SELECT @QueryUser = dbo.uf_LookupDynamicQueryGenerator(@NamaUser, 'NP.Nama') 

SET @Query = @Query 
				+ @QueryNamaMenu
				+ @QueryUser
				EXEC(@Query) 
END
GO
/****** Object:  StoredProcedure [dbo].[sp_NavigationAssignment_detail]    Script Date: 14/03/2024 14:32:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_NavigationAssignment_detail]
	@Id int
AS
BEGIN
	
select 
N.[Id]
,MN.[Name] as NamaMenu
,NP.Nama as NamaRole		
, AZ.Name as ActiveByUser
,dbo.[uf_ShortIndonesianDateTime](N.[Created_at]) as CreatedTime
,dbo.[uf_ShortIndonesianDateTime](N.[Updated_at]) as UpdatedTime
,dbo.[uf_ShortIndonesianDateTime](N.[Deleted_at]) as DeletedTime
,PC.Nama as CreatedBy
,PU.Nama as UpdatedBy
,PD.Nama as DeletedBy
FROM [dbo].[Tbl_MasterNavigationAssignment] N
LEFT JOIN dbo.Tbl_MasterNavigation MN ON N.Navigation_Id = MN.Id
LEFT JOIN dbo.Tbl_User NP ON N.UserId = NP.Id
LEFT JOIN dbo.Tbl_User PC ON N.CreatedBy_Id = PC.Id
LEFT JOIN dbo.Tbl_User PU ON N.UpdatedBy_Id = PU.Id
LEFT JOIN dbo.Tbl_User PD ON N.DeletedBy_Id = PD.Id
LEFT JOIN dbo.Tbl_MasterLookup AZ ON AZ.Value = N.IsActive AND AZ.Type = 'IsActive'
Where (N.IsDeleted = 0 OR N.IsDeleted is null) AND N.Id = @Id

END
GO
/****** Object:  StoredProcedure [dbo].[sp_NavigationAssignment_view]    Script Date: 14/03/2024 14:32:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_NavigationAssignment_view] 	
  @NamaMenu varchar(MAX) ='',
  @NamaUser varchar(MAX) ='',
  @sortColumn varchar(100)='Id',
  @sortColumnDir varchar(10)='desc',
  @PageNumber INT, 
  @RowsPage INT
AS 
BEGIN

DECLARE 
@SortField varchar(50)

SET @SortField = 	
				CASE @sortColumn 
				WHEN 'Nama' THEN 'N.Name'	
				WHEN 'Tipe' THEN 'N.Type'
				WHEN 'OrderBy' THEN ' N.[Order]'
				WHEN 'Parent' THEN ' NP.Name'
				WHEN 'Icon' THEN ' N.IconClass'
				WHEN 'Route' THEN ' N.Route'
				WHEN 'CreatedBy' THEN 'PC.Name'
				WHEN 'UpdatedBy' THEN 'PU.UpdatedBy_Id'
				WHEN 'Visible_Name' THEN 'L2.Name'			 				 	
				ELSE 'N.Id' end; 	 	

DECLARE @Query VARCHAR(MAX) = 'select ROW_NUMBER() OVER(ORDER BY '+@SortField+' '+@sortColumnDir+') AS Number
									  ,N.[Id]
									  ,MN.[Name] as NamaMenu
									  ,NP.Nama as NamaRole		
									  , AZ.Name as ActiveByUser
									  ,dbo.[uf_ShortIndonesianDateTime](N.[Created_at]) as CreatedTime
										,dbo.[uf_ShortIndonesianDateTime](N.[Updated_at]) as UpdatedTime
										,dbo.[uf_ShortIndonesianDateTime](N.[Deleted_at]) as DeletedTime
										,PC.Nama as CreatedBy
										,PU.Nama as UpdatedBy
										,PD.Nama as DeletedBy
									  FROM [dbo].[Tbl_MasterNavigationAssignment] N
									  LEFT JOIN dbo.Tbl_MasterNavigation MN ON N.Navigation_Id = MN.Id
									  LEFT JOIN dbo.Tbl_User NP ON N.UserId = NP.Id
									  LEFT JOIN dbo.Tbl_User PC ON N.CreatedBy_Id = PC.Id
									  LEFT JOIN dbo.Tbl_User PU ON N.UpdatedBy_Id = PU.Id
									  LEFT JOIN dbo.Tbl_User PD ON N.DeletedBy_Id = PD.Id
									  LEFT JOIN dbo.Tbl_MasterLookup AZ ON AZ.Value = N.IsActive AND AZ.Type = ''IsActive''
									  Where (N.IsDeleted = 0 OR N.IsDeleted is null) 
								  ',
	@QueryNamaMenu varchar(MAX) = '',
	@QueryUser varchar(MAX) = ''

SELECT @QueryNamaMenu = dbo.uf_LookupDynamicQueryGenerator(@NamaMenu, 'MN.Name')
SELECT @QueryUser = dbo.uf_LookupDynamicQueryGenerator(@NamaUser, 'NP.Nama') 

SET @Query = 'SELECT * FROM (' 
				+ @Query 
				+ @QueryNamaMenu
				+ @QueryUser
				+') TBL WHERE NUMBER BETWEEN (('+CONVERT(VARCHAR,@PageNumber)+'-1) * '
				+CONVERT(VARCHAR,@RowsPage)+' + 1) AND ('+CONVERT(VARCHAR,@PageNumber)+'*'+CONVERT(VARCHAR,@RowsPage)+')'
				
				EXEC(@Query) 
END
GO
/****** Object:  StoredProcedure [dbo].[sp_PengaturanLookup_Count]    Script Date: 14/03/2024 14:32:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_PengaturanLookup_Count] 	
  @Type varchar(MAX) ='',
  @Name varchar(MAX) =''
AS 
BEGIN


--Query ini nantinya akan dijadikan subquery dan fungsi order di taruh di atas dikarenakan subquery
--tidak support dengan order by, untuk itu diakalin dengan sorting number terlebih dahulu
DECLARE @Query VARCHAR(MAX) = 'select Count(*)
								from Tbl_MasterLookup L
										 LEFT JOIN dbo.Tbl_User PC ON L.CreatedBy_Id = PC.Id
										 LEFT JOIN dbo.Tbl_User PU ON L.UpdatedBy_Id = PU.Id
										 LEFT JOIN dbo.Tbl_User PD ON L.DeletedBy_Id = PD.Id
										left join Tbl_MasterLookup L2 on L.IsActive = L2.Value and L2.Type = ''IsActive''
										left join Tbl_MasterLookup L3 on L.IsDeleted = L3.Value and L3.Type = ''IsDelete''
										Where (L.IsDeleted = 0 OR L.IsDeleted is null) ',
	@QueryType varchar(MAX) = '',
	@QueryName varchar(MAX) = ''

--Ini digunakan untuk mengeset dynamic kondisi parameter dengan menggunakan bantuan function supaya rapi kodingannya
--Untuk lebih jelasnya baca alur logic function yang dipakai
--SELECT @QueryType = dbo.uf_LookupDynamicQueryGenerator(@Type, 'L.Type')
--SELECT @QueryName = dbo.uf_LookupDynamicQueryGenerator(@Name, 'L.Name') 

--Setelah mengeset nilai dari semua variabel kemudian kita gabungkan dengan query dibawah ini untuk paging
--data yang di select, pagging digunakan untuk meningkatkan performance query, dikarenakan data yang akan dikirim
--dari sini adalah data cukup data yang dibutuhkan saja, dengan kata lain kita melakukan filterisasi data terlebih
--dahulu dari sisi databasenya sebelum dikirim ke controller
SET @Query =	@Query 
				+ @QueryType
				+ @QueryName

				EXEC(@Query) 
END
GO
/****** Object:  StoredProcedure [dbo].[sp_PengaturanLookup_View]    Script Date: 14/03/2024 14:32:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_PengaturanLookup_View] 	
  @Type varchar(MAX) ='',
  @Name varchar(MAX) ='',
  @sortColumn varchar(100)='Id',
  @sortColumnDir varchar(10)='desc',
  @PageNumber INT, 
  @RowsPage INT
AS 
BEGIN

--Digunakan ketika akan sort data
DECLARE 
@SortField varchar(50)

SET @SortField = 	
				CASE @sortColumn 	
				WHEN 'Type' THEN 'L.Type'
				WHEN 'Name' THEN 'L.Name'
				WHEN 'Value' THEN ' L.Value'
				WHEN 'OrderBy' THEN ' L.OrderBy'
				WHEN 'CreatedBy' THEN 'PC.Name'
				WHEN 'UpdatedBy' THEN 'PU.UpdatedBy_Id'
				WHEN 'IsActive' THEN 'L.IsActive'
				WHEN 'IsDeleted' THEN 'L.IsDeleted'			 				 	
				ELSE 'L.Id' end; 	 	

--Query ini nantinya akan dijadikan subquery dan fungsi order di taruh di atas dikarenakan subquery
--tidak support dengan order by, untuk itu diakalin dengan sorting number terlebih dahulu
DECLARE @Query VARCHAR(MAX) = 'select ROW_NUMBER() OVER(ORDER BY '+@SortField+' '+@sortColumnDir+') AS Number,
									    L.ID as Id,
									    L.Type, 
									    L.Name, 
									    L.Value, 
									    L.OrderBy,	  
										L.IsActive as IsActive,
									    L.IsDeleted as IsDeleted
										,dbo.[uf_ShortIndonesianDateTime](L.[Created_at]) as CreatedTime
										,dbo.[uf_ShortIndonesianDateTime](L.[Updated_at]) as UpdatedTime
										,PC.Nama as CreatedBy
										,PU.Nama as UpdatedBy
										from Tbl_MasterLookup L
										 LEFT JOIN dbo.Tbl_User PC ON L.CreatedBy_Id = PC.Id
										 LEFT JOIN dbo.Tbl_User PU ON L.UpdatedBy_Id = PU.Id
										 LEFT JOIN dbo.Tbl_User PD ON L.DeletedBy_Id = PD.Id
										left join Tbl_MasterLookup L2 on L.IsActive = L2.Value and L2.Type = ''IsActive''
										left join Tbl_MasterLookup L3 on L.IsDeleted = L3.Value and L3.Type = ''IsDelete''
										Where (L.IsDeleted = 0 OR L.IsDeleted is null) ',
	@QueryType varchar(MAX) = '',
	@QueryName varchar(MAX) = ''

--Ini digunakan untuk mengeset dynamic kondisi parameter dengan menggunakan bantuan function supaya rapi kodingannya
--Untuk lebih jelasnya baca alur logic function yang dipakai
SELECT @QueryType = dbo.uf_LookupDynamicQueryGenerator(@Type, 'L.Type')
SELECT @QueryName = dbo.uf_LookupDynamicQueryGenerator(@Name, 'L.Name') 

--Setelah mengeset nilai dari semua variabel kemudian kita gabungkan dengan query dibawah ini untuk paging
--data yang di select, pagging digunakan untuk meningkatkan performance query, dikarenakan data yang akan dikirim
--dari sini adalah data cukup data yang dibutuhkan saja, dengan kata lain kita melakukan filterisasi data terlebih
--dahulu dari sisi databasenya sebelum dikirim ke controller
SET @Query = 'SELECT * FROM (' 
				+ @Query 
				+ @QueryType
				+ @QueryName
				+') TBL WHERE NUMBER BETWEEN (('+CONVERT(VARCHAR,@PageNumber)+'-1) * '
				+CONVERT(VARCHAR,@RowsPage)+' + 1) AND ('+CONVERT(VARCHAR,@PageNumber)+'*'+CONVERT(VARCHAR,@RowsPage)+')'
				--Untuk mengecek sebenarnya query seperti apa yang akan dieksekusi, 
				--ganti perintah 'EXEC' dibawah dengan menggunakan 'PRINT'
				EXEC(@Query) 
END
GO
/****** Object:  StoredProcedure [dbo].[sp_PengaturanMenu_Count]    Script Date: 14/03/2024 14:32:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_PengaturanMenu_Count] 	
  @Name varchar(MAX) ='',
  @Type varchar(MAX) ='',
  @Parent varchar(MAX) =''
AS 
BEGIN
DECLARE @Query VARCHAR(MAX) = 'select Count(*)
								  FROM [dbo].[Tbl_MasterNavigation] N
								  LEFT JOIN dbo.Tbl_MasterLookup L oN N.Type = L.Value and L.Type = ''TypeMenu''
								  LEFT JOIN dbo.Tbl_MasterNavigation NP ON N.ParentNavigationId = NP.Id
								  LEFT JOIN dbo.Tbl_MasterLookup L2 ON L2.Value = N.Visible AND L2.Type = ''IsActive''
								  LEFT JOIN dbo.Tbl_User PC ON N.CreatedBy_Id = PC.Id
								  LEFT JOIN dbo.Tbl_User PU ON N.UpdatedBy_Id = PU.Id
								  LEFT JOIN dbo.Tbl_User PD ON N.DeletedBy_Id = PD.Id
								  Where (N.IsDeleted = 0 OR N.IsDeleted is null)
								  ',
	@QueryName varchar(MAX) = '',
	@QueryType varchar(MAX) = '',
	@QueryParent varchar(MAX) = ''


SELECT @QueryName = dbo.uf_LookupDynamicQueryGenerator(@Name, 'N.Name')
SELECT @QueryType = dbo.uf_LookupDynamicQueryGenerator(@Type, 'L.Name') 
SELECT @QueryParent = dbo.uf_LookupDynamicQueryGenerator(@Parent, 'NP.Name') 

SET @Query = @Query 
				+ @QueryName
				+ @QueryType
			
				EXEC(@Query) 
END
GO
/****** Object:  StoredProcedure [dbo].[sp_PengaturanMenu_Detail]    Script Date: 14/03/2024 14:32:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_PengaturanMenu_Detail]
	@Id int
AS
BEGIN
	select 
	N.[Id]
	,N.[Name] as Nama
	,L.Name as Type
	,N.[Route]
	,N.IconClass
	,AZ.Name as IsDeleted
	,N.[Order]
	,ISNULL(NP.Name,'-') as NamaParent
	,L2.Name as Visible_Name						  
	,dbo.[uf_ShortIndonesianDateTime](N.[Created_at]) as CreatedTime
	,dbo.[uf_ShortIndonesianDateTime](N.[Updated_at]) as UpdatedTime
	,dbo.[uf_ShortIndonesianDateTime](N.[Deleted_at]) as DeletedTime
	,PC.Nama as CreatedBy
	,PU.Nama as UpdatedBy
	,PD.Nama as DeletedBy
	FROM [dbo].[Tbl_MasterNavigation] N
	LEFT JOIN dbo.Tbl_MasterLookup L ON N.Type = L.Value and L.Type = 'TypeMenu'
	LEFT JOIN dbo.Tbl_MasterNavigation NP ON N.ParentNavigationId = NP.Id
	LEFT JOIN dbo.Tbl_MasterLookup L2 ON L2.Value = N.Visible AND L2.Type = 'IsActive'
	LEFT JOIN dbo.Tbl_User PC ON N.CreatedBy_Id = PC.Id
	LEFT JOIN dbo.Tbl_User PU ON N.UpdatedBy_Id = PU.Id
	LEFT JOIN dbo.Tbl_User PD ON N.DeletedBy_Id = PD.Id
	LEFT JOIN dbo.Tbl_MasterLookup AZ ON AZ.Value = N.IsDeleted AND AZ.Type = 'IsActive'
	Where (N.IsDeleted = 0 OR N.IsDeleted is null) AND N.Id = @Id
END
GO
/****** Object:  StoredProcedure [dbo].[sp_PengaturanMenu_View]    Script Date: 14/03/2024 14:32:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_PengaturanMenu_View] 	
  @Name varchar(MAX) ='',
  @Type varchar(MAX) ='',
  @Parent varchar(MAX) ='',
  @sortColumn varchar(100)='Id',
  @sortColumnDir varchar(10)='desc',
  @PageNumber INT, 
  @RowsPage INT
AS 
BEGIN

DECLARE 
@SortField varchar(50)

SET @SortField = 	
				CASE @sortColumn 
				WHEN 'Nama' THEN 'N.Name'	
				WHEN 'Tipe' THEN 'N.Type'
				WHEN 'OrderBy' THEN ' N.[Order]'
				WHEN 'Parent' THEN ' NP.Name'
				WHEN 'Icon' THEN ' N.IconClass'
				WHEN 'Route' THEN ' N.Route'
				WHEN 'CreatedBy' THEN 'PC.Name'
				WHEN 'UpdatedBy' THEN 'PU.UpdatedBy_Id'
				WHEN 'Visible_Name' THEN 'L2.Name'			 				 	
				ELSE 'N.Id' end; 	 	

DECLARE @Query VARCHAR(MAX) = 'select ROW_NUMBER() OVER(ORDER BY '+@SortField+' '+@sortColumnDir+') AS Number
									  ,N.[Id]
									  ,N.[Name] as Nama
									  ,L.Name as Type
									  ,N.[Route]
									  ,N.IconClass
									  ,AZ.Name as IsDeleted
									  ,N.[Order]
									  ,ISNULL(NP.Name,''-'') as NamaParent
									  ,L2.Name as Visible_Name						  
									  ,dbo.[uf_ShortIndonesianDateTime](N.[Created_at]) as CreatedTime
									,dbo.[uf_ShortIndonesianDateTime](N.[Updated_at]) as UpdatedTime
									,dbo.[uf_ShortIndonesianDateTime](N.[Deleted_at]) as DeletedTime
									,PC.Nama as CreatedBy
									,PU.Nama as UpdatedBy
									,PD.Nama as DeletedBy
								  FROM [dbo].[Tbl_MasterNavigation] N
								  LEFT JOIN dbo.Tbl_MasterLookup L ON N.Type = L.Value and L.Type = ''TypeMenu''
								  LEFT JOIN dbo.Tbl_MasterNavigation NP ON N.ParentNavigationId = NP.Id
								  LEFT JOIN dbo.Tbl_MasterLookup L2 ON L2.Value = N.Visible AND L2.Type = ''IsActive''
								  LEFT JOIN dbo.Tbl_User PC ON N.CreatedBy_Id = PC.Id
								  LEFT JOIN dbo.Tbl_User PU ON N.UpdatedBy_Id = PU.Id
								  LEFT JOIN dbo.Tbl_User PD ON N.DeletedBy_Id = PD.Id
								  LEFT JOIN dbo.Tbl_MasterLookup AZ ON AZ.Value = N.IsDeleted AND AZ.Type = ''IsActive''
								  Where (N.IsDeleted = 0 OR N.IsDeleted is null) 
								  ',
	@QueryName varchar(MAX) = '',
	@QueryType varchar(MAX) = '',
	@QueryParent varchar(MAX) = ''

SELECT @QueryName = dbo.uf_LookupDynamicQueryGenerator(@Name, 'N.Name')
SELECT @QueryType = dbo.uf_LookupDynamicQueryGenerator(@Type, 'L.Name') 
SELECT @QueryParent = dbo.uf_LookupDynamicQueryGenerator(@Parent, 'NP.Name') 

SET @Query = 'SELECT * FROM (' 
				+ @Query 
				+ @QueryName
				+ @QueryType
				+ @QueryParent
				+') TBL WHERE NUMBER BETWEEN (('+CONVERT(VARCHAR,@PageNumber)+'-1) * '
				+CONVERT(VARCHAR,@RowsPage)+' + 1) AND ('+CONVERT(VARCHAR,@PageNumber)+'*'+CONVERT(VARCHAR,@RowsPage)+')'
				
				EXEC(@Query) 
END
GO
