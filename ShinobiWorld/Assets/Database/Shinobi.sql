USE [master]
GO
/****** Object:  Database [Shinobi]    Script Date: 2023/05/14 12:57:09 ******/
CREATE DATABASE [Shinobi]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'Shinobi', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\Shinobi.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'Shinobi_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\Shinobi_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [Shinobi] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [Shinobi].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [Shinobi] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [Shinobi] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [Shinobi] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [Shinobi] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [Shinobi] SET ARITHABORT OFF 
GO
ALTER DATABASE [Shinobi] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [Shinobi] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [Shinobi] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [Shinobi] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [Shinobi] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [Shinobi] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [Shinobi] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [Shinobi] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [Shinobi] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [Shinobi] SET  DISABLE_BROKER 
GO
ALTER DATABASE [Shinobi] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [Shinobi] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [Shinobi] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [Shinobi] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [Shinobi] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [Shinobi] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [Shinobi] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [Shinobi] SET RECOVERY FULL 
GO
ALTER DATABASE [Shinobi] SET  MULTI_USER 
GO
ALTER DATABASE [Shinobi] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [Shinobi] SET DB_CHAINING OFF 
GO
ALTER DATABASE [Shinobi] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [Shinobi] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [Shinobi] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [Shinobi] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'Shinobi', N'ON'
GO
ALTER DATABASE [Shinobi] SET QUERY_STORE = OFF
GO
USE [Shinobi]
GO
/****** Object:  Table [dbo].[Account]    Script Date: 2023/05/14 12:57:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Account](
	[ID] [nvarchar](50) NOT NULL,
	[RoleInGameID] [int] NOT NULL,
	[TrophiesID] [int] NOT NULL,
	[Level] [int] NULL,
	[Health] [int] NULL,
	[Charka] [int] NULL,
	[Exp] [int] NULL,
	[Speed] [int] NULL,
	[Coin] [int] NULL,
	[Power] [int] NULL,
	[Strength] [int] NULL,
	[EyeID] [int] NOT NULL,
	[HairID] [int] NOT NULL,
	[MouthID] [int] NOT NULL,
	[SkinID] [int] NOT NULL,
	[IsDead] [bit] NULL,
	[IsOnline] [bit] NULL,
	[IsTicket] [bit] NULL,
	[IsFirst] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AccountEquipment]    Script Date: 2023/05/14 12:57:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AccountEquipment](
	[AccountID] [nvarchar](50) NOT NULL,
	[EquipmentID] [int] NOT NULL,
	[Level] [int] NULL,
	[Health] [int] NULL,
	[Damage] [int] NULL,
	[Chakra] [int] NULL,
	[IsUse] [bit] NULL,
	[Delete] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[AccountID] ASC,
	[EquipmentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AccountItem]    Script Date: 2023/05/14 12:57:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AccountItem](
	[AccountID] [nvarchar](50) NOT NULL,
	[ItemID] [int] NOT NULL,
	[Amount] [int] NULL,
	[Limit] [bit] NULL,
	[Delete] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[AccountID] ASC,
	[ItemID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AccountMailBox]    Script Date: 2023/05/14 12:57:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AccountMailBox](
	[AccountID] [nvarchar](50) NOT NULL,
	[MailBoxID] [int] NOT NULL,
	[RankID] [int] NOT NULL,
	[Rank] [int] NULL,
	[IsClaim] [bit] NULL,
	[IsRead] [bit] NULL,
	[Delete] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[AccountID] ASC,
	[MailBoxID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AccountMission]    Script Date: 2023/05/14 12:57:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AccountMission](
	[AccountID] [nvarchar](50) NOT NULL,
	[MissionID] [int] NOT NULL,
	[Current] [int] NULL,
	[Status] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[AccountID] ASC,
	[MissionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AccountSkill]    Script Date: 2023/05/14 12:57:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AccountSkill](
	[AccountID] [nvarchar](50) NOT NULL,
	[SkillID] [int] NOT NULL,
	[Level] [int] NULL,
	[Cooldown] [float] NULL,
	[Damage] [int] NULL,
	[Chakra] [int] NULL,
	[Delete] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[AccountID] ASC,
	[SkillID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AccountWeapon]    Script Date: 2023/05/14 12:57:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AccountWeapon](
	[AccountID] [nvarchar](50) NOT NULL,
	[WeaponID] [int] NOT NULL,
	[Level] [int] NULL,
	[Damage] [int] NULL,
	[Delete] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[AccountID] ASC,
	[WeaponID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Boss]    Script Date: 2023/05/14 12:57:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Boss](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[TypeBossID] [int] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Health] [int] NULL,
	[Speed] [int] NULL,
	[CoinBonus] [int] NULL,
	[ExpBonus] [int] NULL,
	[Image] [nvarchar](100) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[Delete] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Equipment]    Script Date: 2023/05/14 12:57:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Equipment](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[TypeEquipmentID] [int] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Health] [int] NULL,
	[Damage] [int] NULL,
	[Chakra] [int] NULL,
	[UpgradeCost] [int] NULL,
	[SellCost] [int] NULL,
	[Image] [nvarchar](100) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[Delete] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Event]    Script Date: 2023/05/14 12:57:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Event](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[BossID] [int] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Weekday] [int] NOT NULL,
	[Description] [nvarchar](max) NULL,
	[Delete] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Eye]    Script Date: 2023/05/14 12:57:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Eye](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Image] [nvarchar](100) NOT NULL,
	[Delete] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Hair]    Script Date: 2023/05/14 12:57:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Hair](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Image] [nvarchar](100) NOT NULL,
	[Delete] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Item]    Script Date: 2023/05/14 12:57:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Item](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[HealthBonus] [int] NULL,
	[ChakraBonus] [int] NULL,
	[DamageBonus] [int] NULL,
	[BuyCost] [int] NULL,
	[Limit] [int] NULL,
	[Image] [nvarchar](100) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[Delete] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MailBox]    Script Date: 2023/05/14 12:57:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MailBox](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](50) NOT NULL,
	[Content] [nvarchar](max) NOT NULL,
	[IsRank] [bit] NOT NULL,
	[Delete] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Mission]    Script Date: 2023/05/14 12:57:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Mission](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Content] [nvarchar](max) NOT NULL,
	[Level] [int] NULL,
	[Target] [int] NULL,
	[ExpBonus] [int] NULL,
	[CoinBonus] [int] NULL,
	[Image] [nvarchar](100) NULL,
	[Delete] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Mouth]    Script Date: 2023/05/14 12:57:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Mouth](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Image] [nvarchar](100) NOT NULL,
	[Delete] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Rank]    Script Date: 2023/05/14 12:57:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Rank](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[EquipmentIDBonus] [int] NOT NULL,
	[IsEvent] [bit] NOT NULL,
	[Rank] [int] NOT NULL,
	[CoinBonus] [int] NULL,
	[EquipmentAmount] [int] NOT NULL,
	[Delete] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RoleInGame]    Script Date: 2023/05/14 12:57:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RoleInGame](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[WeaponID] [int] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Delete] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Skill]    Script Date: 2023/05/14 12:57:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Skill](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[RoleInGameID] [int] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Cooldown] [float] NULL,
	[Damage] [int] NULL,
	[Chakra] [int] NULL,
	[Uppercent] [int] NULL,
	[LevelUnlock] [int] NULL,
	[UpgradeCost] [int] NULL,
	[BuyCost] [int] NULL,
	[Image] [nvarchar](100) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[Delete] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SkillBoss]    Script Date: 2023/05/14 12:57:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SkillBoss](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[BossID] [int] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Damage] [int] NULL,
	[Image] [nvarchar](100) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[Delete] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Skin]    Script Date: 2023/05/14 12:57:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Skin](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Image] [nvarchar](100) NOT NULL,
	[Delete] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Trophies]    Script Date: 2023/05/14 12:57:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Trophies](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[BossID] [int] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[ContraitLevelAccount] [int] NULL,
	[Cost] [int] NULL,
	[Image] [nvarchar](100) NULL,
	[Description] [nvarchar](max) NULL,
	[Delete] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TypeBoss]    Script Date: 2023/05/14 12:57:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TypeBoss](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Delete] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TypeEquipment]    Script Date: 2023/05/14 12:57:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TypeEquipment](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Delete] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Weapon]    Script Date: 2023/05/14 12:57:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Weapon](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Damage] [int] NULL,
	[Uppercent] [int] NULL,
	[UpgradeCost] [int] NULL,
	[Image] [nvarchar](100) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[Delete] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
INSERT [dbo].[Account] ([ID], [RoleInGameID], [TrophiesID], [Level], [Health], [Charka], [Exp], [Speed], [Coin], [Power], [Strength], [EyeID], [HairID], [MouthID], [SkinID], [IsDead], [IsOnline], [IsTicket], [IsFirst]) VALUES (N'1', 1, 1, 1, 100, 100, 0, 5, 0, 0, 100, 1, 1, 1, 1, 0, 0, 0, 0)
GO
INSERT [dbo].[Account] ([ID], [RoleInGameID], [TrophiesID], [Level], [Health], [Charka], [Exp], [Speed], [Coin], [Power], [Strength], [EyeID], [HairID], [MouthID], [SkinID], [IsDead], [IsOnline], [IsTicket], [IsFirst]) VALUES (N'piENbG5OaZZn4WN0jNHQWhP4ZaA3', 2, 1, 1, 100, 100, 0, 5, 0, 0, 100, 3, 2, 4, 2, 0, 1, 0, 0)
GO
SET IDENTITY_INSERT [dbo].[Boss] ON 
GO
INSERT [dbo].[Boss] ([ID], [TypeBossID], [Name], [Health], [Speed], [CoinBonus], [ExpBonus], [Image], [Description], [Delete]) VALUES (1, 2, N'Kakashi', 100, 5, 100, 100, N'Image', N'Description', 0)
GO
SET IDENTITY_INSERT [dbo].[Boss] OFF
GO
SET IDENTITY_INSERT [dbo].[Eye] ON 
GO
INSERT [dbo].[Eye] ([ID], [Name], [Image], [Delete]) VALUES (1, N'Eye_Red', N'Creator/Eye_Red', 0)
GO
INSERT [dbo].[Eye] ([ID], [Name], [Image], [Delete]) VALUES (2, N'Eye_White', N'Creator/Eye_White', 0)
GO
INSERT [dbo].[Eye] ([ID], [Name], [Image], [Delete]) VALUES (3, N'Eye_Purple', N'Creator/Eye_Purple', 0)
GO
SET IDENTITY_INSERT [dbo].[Eye] OFF
GO
SET IDENTITY_INSERT [dbo].[Hair] ON 
GO
INSERT [dbo].[Hair] ([ID], [Name], [Image], [Delete]) VALUES (1, N'HairNaruto', N'Creator/Hair_Naruto', 0)
GO
INSERT [dbo].[Hair] ([ID], [Name], [Image], [Delete]) VALUES (2, N'HairSasuke', N'Creator/Hair_Sasuke', 0)
GO
SET IDENTITY_INSERT [dbo].[Hair] OFF
GO
SET IDENTITY_INSERT [dbo].[Mouth] ON 
GO
INSERT [dbo].[Mouth] ([ID], [Name], [Image], [Delete]) VALUES (1, N'Mouth_BigSmile', N'Creator/Mouth_BigSmile', 0)
GO
INSERT [dbo].[Mouth] ([ID], [Name], [Image], [Delete]) VALUES (2, N'Mouth_Normal', N'Creator/Mouth_Normal', 0)
GO
INSERT [dbo].[Mouth] ([ID], [Name], [Image], [Delete]) VALUES (3, N'Mouth_Sad', N'Creator/Mouth_Sad', 0)
GO
INSERT [dbo].[Mouth] ([ID], [Name], [Image], [Delete]) VALUES (4, N'Mouth_Smile', N'Creator/Mouth_Smile', 0)
GO
SET IDENTITY_INSERT [dbo].[Mouth] OFF
GO
SET IDENTITY_INSERT [dbo].[RoleInGame] ON 
GO
INSERT [dbo].[RoleInGame] ([ID], [WeaponID], [Name], [Delete]) VALUES (1, 1, N'Cận chiến', 0)
GO
INSERT [dbo].[RoleInGame] ([ID], [WeaponID], [Name], [Delete]) VALUES (2, 2, N'Viễn chiến', 0)
GO
INSERT [dbo].[RoleInGame] ([ID], [WeaponID], [Name], [Delete]) VALUES (3, 3, N'Hỗ trợ', 0)
GO
SET IDENTITY_INSERT [dbo].[RoleInGame] OFF
GO
SET IDENTITY_INSERT [dbo].[Skin] ON 
GO
INSERT [dbo].[Skin] ([ID], [Name], [Image], [Delete]) VALUES (1, N'SkinNaruto', N'Creator/Skin_Naruto', 0)
GO
INSERT [dbo].[Skin] ([ID], [Name], [Image], [Delete]) VALUES (2, N'SkinShinobi', N'Creator/Skin_Shinobi', 0)
GO
SET IDENTITY_INSERT [dbo].[Skin] OFF
GO
SET IDENTITY_INSERT [dbo].[Trophies] ON 
GO
INSERT [dbo].[Trophies] ([ID], [BossID], [Name], [ContraitLevelAccount], [Cost], [Image], [Description], [Delete]) VALUES (1, 1, N'Hạ đẳng', 10, 100, N'Image', N'Description', 0)
GO
INSERT [dbo].[Trophies] ([ID], [BossID], [Name], [ContraitLevelAccount], [Cost], [Image], [Description], [Delete]) VALUES (2, 1, N'Trung đẳng', 10, 100, N'Image', N'Description', 0)
GO
INSERT [dbo].[Trophies] ([ID], [BossID], [Name], [ContraitLevelAccount], [Cost], [Image], [Description], [Delete]) VALUES (3, 1, N'Thượng đẳng', 10, 100, N'Image', N'Description', 0)
GO
SET IDENTITY_INSERT [dbo].[Trophies] OFF
GO
SET IDENTITY_INSERT [dbo].[TypeBoss] ON 
GO
INSERT [dbo].[TypeBoss] ([ID], [Name], [Delete]) VALUES (1, N'Sự kiện', 0)
GO
INSERT [dbo].[TypeBoss] ([ID], [Name], [Delete]) VALUES (2, N'Đấu trường', 0)
GO
INSERT [dbo].[TypeBoss] ([ID], [Name], [Delete]) VALUES (3, N'Quái thường', 0)
GO
SET IDENTITY_INSERT [dbo].[TypeBoss] OFF
GO
SET IDENTITY_INSERT [dbo].[Weapon] ON 
GO
INSERT [dbo].[Weapon] ([ID], [Name], [Damage], [Uppercent], [UpgradeCost], [Image], [Description], [Delete]) VALUES (1, N'Kiếm', 100, 5, 100, N'Creator/Sword', N'Kiếm', 0)
GO
INSERT [dbo].[Weapon] ([ID], [Name], [Damage], [Uppercent], [UpgradeCost], [Image], [Description], [Delete]) VALUES (2, N'Phi tiêu', 100, 5, 100, N'Creator/Dart', N'Phi tiêu', 0)
GO
INSERT [dbo].[Weapon] ([ID], [Name], [Damage], [Uppercent], [UpgradeCost], [Image], [Description], [Delete]) VALUES (3, N'Bao tay', 100, 5, 100, N'Creator/Glove', N'Bao tay', 0)
GO
SET IDENTITY_INSERT [dbo].[Weapon] OFF
GO
ALTER TABLE [dbo].[Account]  WITH CHECK ADD FOREIGN KEY([EyeID])
REFERENCES [dbo].[Eye] ([ID])
GO
ALTER TABLE [dbo].[Account]  WITH CHECK ADD FOREIGN KEY([MouthID])
REFERENCES [dbo].[Mouth] ([ID])
GO
ALTER TABLE [dbo].[Account]  WITH CHECK ADD FOREIGN KEY([HairID])
REFERENCES [dbo].[Hair] ([ID])
GO
ALTER TABLE [dbo].[Account]  WITH CHECK ADD FOREIGN KEY([RoleInGameID])
REFERENCES [dbo].[RoleInGame] ([ID])
GO
ALTER TABLE [dbo].[Account]  WITH CHECK ADD FOREIGN KEY([SkinID])
REFERENCES [dbo].[Skin] ([ID])
GO
ALTER TABLE [dbo].[Account]  WITH CHECK ADD FOREIGN KEY([TrophiesID])
REFERENCES [dbo].[Trophies] ([ID])
GO
ALTER TABLE [dbo].[AccountEquipment]  WITH CHECK ADD FOREIGN KEY([AccountID])
REFERENCES [dbo].[Account] ([ID])
GO
ALTER TABLE [dbo].[AccountEquipment]  WITH CHECK ADD FOREIGN KEY([EquipmentID])
REFERENCES [dbo].[Equipment] ([ID])
GO
ALTER TABLE [dbo].[AccountItem]  WITH CHECK ADD FOREIGN KEY([AccountID])
REFERENCES [dbo].[Account] ([ID])
GO
ALTER TABLE [dbo].[AccountItem]  WITH CHECK ADD FOREIGN KEY([ItemID])
REFERENCES [dbo].[Item] ([ID])
GO
ALTER TABLE [dbo].[AccountMailBox]  WITH CHECK ADD FOREIGN KEY([AccountID])
REFERENCES [dbo].[Account] ([ID])
GO
ALTER TABLE [dbo].[AccountMailBox]  WITH CHECK ADD FOREIGN KEY([MailBoxID])
REFERENCES [dbo].[MailBox] ([ID])
GO
ALTER TABLE [dbo].[AccountMailBox]  WITH CHECK ADD FOREIGN KEY([RankID])
REFERENCES [dbo].[Rank] ([ID])
GO
ALTER TABLE [dbo].[AccountMission]  WITH CHECK ADD FOREIGN KEY([AccountID])
REFERENCES [dbo].[Account] ([ID])
GO
ALTER TABLE [dbo].[AccountMission]  WITH CHECK ADD FOREIGN KEY([MissionID])
REFERENCES [dbo].[Mission] ([ID])
GO
ALTER TABLE [dbo].[AccountSkill]  WITH CHECK ADD FOREIGN KEY([AccountID])
REFERENCES [dbo].[Account] ([ID])
GO
ALTER TABLE [dbo].[AccountSkill]  WITH CHECK ADD FOREIGN KEY([SkillID])
REFERENCES [dbo].[Skill] ([ID])
GO
ALTER TABLE [dbo].[AccountWeapon]  WITH CHECK ADD FOREIGN KEY([AccountID])
REFERENCES [dbo].[Account] ([ID])
GO
ALTER TABLE [dbo].[AccountWeapon]  WITH CHECK ADD FOREIGN KEY([WeaponID])
REFERENCES [dbo].[Weapon] ([ID])
GO
ALTER TABLE [dbo].[Boss]  WITH CHECK ADD FOREIGN KEY([TypeBossID])
REFERENCES [dbo].[TypeBoss] ([ID])
GO
ALTER TABLE [dbo].[Equipment]  WITH CHECK ADD FOREIGN KEY([TypeEquipmentID])
REFERENCES [dbo].[TypeEquipment] ([ID])
GO
ALTER TABLE [dbo].[Event]  WITH CHECK ADD FOREIGN KEY([BossID])
REFERENCES [dbo].[Boss] ([ID])
GO
ALTER TABLE [dbo].[Rank]  WITH CHECK ADD FOREIGN KEY([EquipmentIDBonus])
REFERENCES [dbo].[Equipment] ([ID])
GO
ALTER TABLE [dbo].[RoleInGame]  WITH CHECK ADD FOREIGN KEY([WeaponID])
REFERENCES [dbo].[Weapon] ([ID])
GO
ALTER TABLE [dbo].[Skill]  WITH CHECK ADD FOREIGN KEY([RoleInGameID])
REFERENCES [dbo].[RoleInGame] ([ID])
GO
ALTER TABLE [dbo].[SkillBoss]  WITH CHECK ADD FOREIGN KEY([BossID])
REFERENCES [dbo].[Boss] ([ID])
GO
ALTER TABLE [dbo].[Trophies]  WITH CHECK ADD FOREIGN KEY([BossID])
REFERENCES [dbo].[Boss] ([ID])
GO
USE [master]
GO
ALTER DATABASE [Shinobi] SET  READ_WRITE 
GO
