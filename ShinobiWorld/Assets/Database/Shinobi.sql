USE [master]
GO
/****** Object:  Database [Shinobi]    Script Date: 2023/05/19 13:49:33 ******/
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
/****** Object:  Table [dbo].[Account]    Script Date: 2023/05/19 13:49:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Account](
	[ID] [nvarchar](50) NOT NULL,
	[RoleInGameID] [nvarchar](20) NOT NULL,
	[TrophiesID] [nvarchar](20) NOT NULL,
	[Level] [int] NULL,
	[Health] [int] NULL,
	[CurrentHealth] [int] NULL,
	[Charka] [int] NULL,
	[CurrentCharka] [int] NULL,
	[Exp] [int] NULL,
	[Speed] [int] NULL,
	[Coin] [int] NULL,
	[Power] [int] NULL,
	[Strength] [int] NULL,
	[CurrentStrength] [int] NULL,
	[Uppercent] [int] NULL,
	[EyeID] [nvarchar](20) NOT NULL,
	[HairID] [nvarchar](20) NOT NULL,
	[MouthID] [nvarchar](20) NOT NULL,
	[SkinID] [nvarchar](20) NOT NULL,
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
/****** Object:  Table [dbo].[AccountEquipment]    Script Date: 2023/05/19 13:49:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AccountEquipment](
	[AccountID] [nvarchar](50) NOT NULL,
	[EquipmentID] [nvarchar](20) NOT NULL,
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
/****** Object:  Table [dbo].[AccountItem]    Script Date: 2023/05/19 13:49:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AccountItem](
	[AccountID] [nvarchar](50) NOT NULL,
	[ItemID] [nvarchar](20) NOT NULL,
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
/****** Object:  Table [dbo].[AccountMailBox]    Script Date: 2023/05/19 13:49:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AccountMailBox](
	[AccountID] [nvarchar](50) NOT NULL,
	[MailBoxID] [nvarchar](20) NOT NULL,
	[RankID] [nvarchar](20) NOT NULL,
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
/****** Object:  Table [dbo].[AccountMission]    Script Date: 2023/05/19 13:49:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AccountMission](
	[AccountID] [nvarchar](50) NOT NULL,
	[MissionID] [nvarchar](20) NOT NULL,
	[Current] [int] NULL,
	[Status] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[AccountID] ASC,
	[MissionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AccountSkill]    Script Date: 2023/05/19 13:49:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AccountSkill](
	[AccountID] [nvarchar](50) NOT NULL,
	[SkillID] [nvarchar](20) NOT NULL,
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
/****** Object:  Table [dbo].[AccountWeapon]    Script Date: 2023/05/19 13:49:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AccountWeapon](
	[AccountID] [nvarchar](50) NOT NULL,
	[WeaponID] [nvarchar](20) NOT NULL,
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
/****** Object:  Table [dbo].[Boss]    Script Date: 2023/05/19 13:49:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Boss](
	[ID] [nvarchar](20) NOT NULL,
	[TypeBossID] [nvarchar](20) NOT NULL,
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
/****** Object:  Table [dbo].[Equipment]    Script Date: 2023/05/19 13:49:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Equipment](
	[ID] [nvarchar](20) NOT NULL,
	[TypeEquipmentID] [nvarchar](20) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Health] [int] NULL,
	[Damage] [int] NULL,
	[Chakra] [int] NULL,
	[UpgradeCost] [int] NULL,
	[Uppercent] [int] NULL,
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
/****** Object:  Table [dbo].[Event]    Script Date: 2023/05/19 13:49:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Event](
	[ID] [nvarchar](20) NOT NULL,
	[BossID] [nvarchar](20) NOT NULL,
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
/****** Object:  Table [dbo].[Eye]    Script Date: 2023/05/19 13:49:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Eye](
	[ID] [nvarchar](20) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Image] [nvarchar](100) NOT NULL,
	[Delete] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Hair]    Script Date: 2023/05/19 13:49:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Hair](
	[ID] [nvarchar](20) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Image] [nvarchar](100) NOT NULL,
	[Delete] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Item]    Script Date: 2023/05/19 13:49:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Item](
	[ID] [nvarchar](20) NOT NULL,
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
/****** Object:  Table [dbo].[MailBox]    Script Date: 2023/05/19 13:49:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MailBox](
	[ID] [nvarchar](20) NOT NULL,
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
/****** Object:  Table [dbo].[Mission]    Script Date: 2023/05/19 13:49:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Mission](
	[ID] [nvarchar](20) NOT NULL,
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
/****** Object:  Table [dbo].[Mouth]    Script Date: 2023/05/19 13:49:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Mouth](
	[ID] [nvarchar](20) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Image] [nvarchar](100) NOT NULL,
	[Delete] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Rank]    Script Date: 2023/05/19 13:49:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Rank](
	[ID] [nvarchar](20) NOT NULL,
	[EquipmentIDBonus] [nvarchar](20) NOT NULL,
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
/****** Object:  Table [dbo].[RoleInGame]    Script Date: 2023/05/19 13:49:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RoleInGame](
	[ID] [nvarchar](20) NOT NULL,
	[WeaponID] [nvarchar](20) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Delete] [bit] NULL,
	[Health] [int] NOT NULL,
	[Charka] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Skill]    Script Date: 2023/05/19 13:49:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Skill](
	[ID] [nvarchar](20) NOT NULL,
	[RoleInGameID] [nvarchar](20) NOT NULL,
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
/****** Object:  Table [dbo].[SkillBoss]    Script Date: 2023/05/19 13:49:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SkillBoss](
	[ID] [nvarchar](20) NOT NULL,
	[BossID] [nvarchar](20) NOT NULL,
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
/****** Object:  Table [dbo].[Skin]    Script Date: 2023/05/19 13:49:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Skin](
	[ID] [nvarchar](20) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Image] [nvarchar](100) NOT NULL,
	[Delete] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Trophies]    Script Date: 2023/05/19 13:49:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Trophies](
	[ID] [nvarchar](20) NOT NULL,
	[BossID] [nvarchar](20) NOT NULL,
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
/****** Object:  Table [dbo].[TypeBoss]    Script Date: 2023/05/19 13:49:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TypeBoss](
	[ID] [nvarchar](20) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Delete] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TypeEquipment]    Script Date: 2023/05/19 13:49:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TypeEquipment](
	[ID] [nvarchar](20) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Delete] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Weapon]    Script Date: 2023/05/19 13:49:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Weapon](
	[ID] [nvarchar](20) NOT NULL,
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
INSERT [dbo].[Account] ([ID], [RoleInGameID], [TrophiesID], [Level], [Health], [CurrentHealth], [Charka], [CurrentCharka], [Exp], [Speed], [Coin], [Power], [Strength], [CurrentStrength], [Uppercent], [EyeID], [HairID], [MouthID], [SkinID], [IsDead], [IsOnline], [IsTicket], [IsFirst]) VALUES (N'SJIJJ71XjAZh6vhY19IIbxrRnYt1', N'Role_Range', N'Trophie_None', 1, 1250, 900, 1130, 1000, 0, 5, 0, 0, 100, 100, 5, N'Eye_Byakugan', N'Hair_Naruto', N'Mouth_Normal', N'Skin_Naruto', 0, 1, 0, 0)
GO
INSERT [dbo].[Account] ([ID], [RoleInGameID], [TrophiesID], [Level], [Health], [CurrentHealth], [Charka], [CurrentCharka], [Exp], [Speed], [Coin], [Power], [Strength], [CurrentStrength], [Uppercent], [EyeID], [HairID], [MouthID], [SkinID], [IsDead], [IsOnline], [IsTicket], [IsFirst]) VALUES (N'SJIJJ71XjAZh6vhY19IIbxrRnYt12', N'Role_Range', N'Trophie_None', 1, 1050, NULL, 1000, NULL, 0, 5, 0, 0, 100, NULL, NULL, N'Eye_Byakugan', N'Hair_Naruto', N'Mouth_Normal', N'Skin_Naruto', 0, 1, 0, 0)
GO
INSERT [dbo].[AccountSkill] ([AccountID], [SkillID], [Level], [Cooldown], [Damage], [Chakra], [Delete]) VALUES (N'SJIJJ71XjAZh6vhY19IIbxrRnYt1', N'Skill_MeleeOne', 1, 5, 10, 15, 0)
GO
INSERT [dbo].[AccountSkill] ([AccountID], [SkillID], [Level], [Cooldown], [Damage], [Chakra], [Delete]) VALUES (N'SJIJJ71XjAZh6vhY19IIbxrRnYt1', N'Skill_MeleeThree', 10, 5, 10, 15, 0)
GO
INSERT [dbo].[AccountSkill] ([AccountID], [SkillID], [Level], [Cooldown], [Damage], [Chakra], [Delete]) VALUES (N'SJIJJ71XjAZh6vhY19IIbxrRnYt1', N'Skill_MeleeTwo', 15, 5, 10, 15, 0)
GO
INSERT [dbo].[AccountSkill] ([AccountID], [SkillID], [Level], [Cooldown], [Damage], [Chakra], [Delete]) VALUES (N'SJIJJ71XjAZh6vhY19IIbxrRnYt12', N'Skill_RangeOne', 1, 5, 10, 15, 0)
GO
INSERT [dbo].[AccountSkill] ([AccountID], [SkillID], [Level], [Cooldown], [Damage], [Chakra], [Delete]) VALUES (N'SJIJJ71XjAZh6vhY19IIbxrRnYt12', N'Skill_RangeThree', 1, 5, 10, 15, 0)
GO
INSERT [dbo].[AccountSkill] ([AccountID], [SkillID], [Level], [Cooldown], [Damage], [Chakra], [Delete]) VALUES (N'SJIJJ71XjAZh6vhY19IIbxrRnYt12', N'Skill_RangeTwo', 1, 5, 10, 15, 0)
GO
INSERT [dbo].[AccountSkill] ([AccountID], [SkillID], [Level], [Cooldown], [Damage], [Chakra], [Delete]) VALUES (N'SJIJJ71XjAZh6vhY19IIbxrRnYt12', N'Skill_SupportOne', 1, 5, 10, 15, 0)
GO
INSERT [dbo].[AccountSkill] ([AccountID], [SkillID], [Level], [Cooldown], [Damage], [Chakra], [Delete]) VALUES (N'SJIJJ71XjAZh6vhY19IIbxrRnYt12', N'Skill_SupportThree', 1, 5, 10, 15, 0)
GO
INSERT [dbo].[AccountSkill] ([AccountID], [SkillID], [Level], [Cooldown], [Damage], [Chakra], [Delete]) VALUES (N'SJIJJ71XjAZh6vhY19IIbxrRnYt12', N'Skill_SupportTwo', 1, 5, 10, 15, 0)
GO
INSERT [dbo].[AccountWeapon] ([AccountID], [WeaponID], [Level], [Damage], [Delete]) VALUES (N'SJIJJ71XjAZh6vhY19IIbxrRnYt1', N'Weapon_Dart', 30, 110, 0)
GO
INSERT [dbo].[AccountWeapon] ([AccountID], [WeaponID], [Level], [Damage], [Delete]) VALUES (N'SJIJJ71XjAZh6vhY19IIbxrRnYt12', N'Weapon_Glove', 1, 15, 0)
GO
INSERT [dbo].[AccountWeapon] ([AccountID], [WeaponID], [Level], [Damage], [Delete]) VALUES (N'SJIJJ71XjAZh6vhY19IIbxrRnYt12', N'Weapon_Sword', 1, 15, 0)
GO
INSERT [dbo].[Boss] ([ID], [TypeBossID], [Name], [Health], [Speed], [CoinBonus], [ExpBonus], [Image], [Description], [Delete]) VALUES (N'Boss_Bat', N'BossType_Normal', N'Dơi Qủy', 100, 5, 100, 100, N'Image', N'Description', 0)
GO
INSERT [dbo].[Boss] ([ID], [TypeBossID], [Name], [Health], [Speed], [CoinBonus], [ExpBonus], [Image], [Description], [Delete]) VALUES (N'Boss_Kakashi', N'BossType_Event', N'Kakashi', 100, 5, 100, 100, N'Image', N'Description', 0)
GO
INSERT [dbo].[Eye] ([ID], [Name], [Image], [Delete]) VALUES (N'Eye_Byakugan', N'Bạch Nhãn', N'Creator/Eye_Byakugan', 0)
GO
INSERT [dbo].[Eye] ([ID], [Name], [Image], [Delete]) VALUES (N'Eye_Rinnegan', N'Mắt Rinnegan', N'Creator/Eye_Rinnegan', 0)
GO
INSERT [dbo].[Eye] ([ID], [Name], [Image], [Delete]) VALUES (N'Eye_Sharingan', N'Mắt Sharingan', N'Creator/Eye_Sharingan', 0)
GO
INSERT [dbo].[Hair] ([ID], [Name], [Image], [Delete]) VALUES (N'Hair_Naruto', N'Tóc Naruto', N'Creator/Hair_Naruto', 0)
GO
INSERT [dbo].[Hair] ([ID], [Name], [Image], [Delete]) VALUES (N'Hair_Sasuke', N'Tóc Sasuke', N'Creator/Hair_Sasuke', 0)
GO
INSERT [dbo].[Item] ([ID], [Name], [HealthBonus], [ChakraBonus], [DamageBonus], [BuyCost], [Limit], [Image], [Description], [Delete]) VALUES (N'Item_Charka', N'Item_Charka', 10, 20, 30, 100, 10, N'Item/Item_Charka', N'Item_Charka', 0)
GO
INSERT [dbo].[Item] ([ID], [Name], [HealthBonus], [ChakraBonus], [DamageBonus], [BuyCost], [Limit], [Image], [Description], [Delete]) VALUES (N'Item_Damage', N'Item_Damage', 10, 20, 30, 100, 10, N'Item/Item_Damage', N'Item_Damage', 0)
GO
INSERT [dbo].[Item] ([ID], [Name], [HealthBonus], [ChakraBonus], [DamageBonus], [BuyCost], [Limit], [Image], [Description], [Delete]) VALUES (N'Item_Health', N'Item_Health', 10, 20, 30, 100, 10, N'Item/Item_Health', N'Item_Health', 0)
GO
INSERT [dbo].[Mouth] ([ID], [Name], [Image], [Delete]) VALUES (N'Mouth_BigSmile', N'Miệng bự', N'Creator/Mouth_BigSmile', 0)
GO
INSERT [dbo].[Mouth] ([ID], [Name], [Image], [Delete]) VALUES (N'Mouth_Normal', N'Miệng bình thường', N'Creator/Mouth_Normal', 0)
GO
INSERT [dbo].[Mouth] ([ID], [Name], [Image], [Delete]) VALUES (N'Mouth_Sad', N'Miệng buồn', N'Creator/Mouth_Sad', 0)
GO
INSERT [dbo].[Mouth] ([ID], [Name], [Image], [Delete]) VALUES (N'Mouth_Smile', N'Miệng cười', N'Creator/Mouth_Smile', 0)
GO
INSERT [dbo].[RoleInGame] ([ID], [WeaponID], [Name], [Delete], [Health], [Charka]) VALUES (N'Role_Melee', N'Weapon_Sword', N'Cận Chiến', 0, 2455, 1195)
GO
INSERT [dbo].[RoleInGame] ([ID], [WeaponID], [Name], [Delete], [Health], [Charka]) VALUES (N'Role_Range', N'Weapon_Dart', N'Viễn Chiến', 0, 2300, 1650)
GO
INSERT [dbo].[RoleInGame] ([ID], [WeaponID], [Name], [Delete], [Health], [Charka]) VALUES (N'Role_Support', N'Weapon_Glove', N'Hỗ Trợ', 0, 2600, 1200)
GO
INSERT [dbo].[Skill] ([ID], [RoleInGameID], [Name], [Cooldown], [Damage], [Chakra], [Uppercent], [LevelUnlock], [UpgradeCost], [BuyCost], [Image], [Description], [Delete]) VALUES (N'Skill_MeleeOne', N'Role_Melee', N'Kiếm chính nghĩa', 5, 0, 10, 5, 1, 100, 100, N'Image', N'Description', 0)
GO
INSERT [dbo].[Skill] ([ID], [RoleInGameID], [Name], [Cooldown], [Damage], [Chakra], [Uppercent], [LevelUnlock], [UpgradeCost], [BuyCost], [Image], [Description], [Delete]) VALUES (N'Skill_MeleeThree', N'Role_Melee', N'', 5, 20, 10, 5, 1, 100, 100, N'', N'', 0)
GO
INSERT [dbo].[Skill] ([ID], [RoleInGameID], [Name], [Cooldown], [Damage], [Chakra], [Uppercent], [LevelUnlock], [UpgradeCost], [BuyCost], [Image], [Description], [Delete]) VALUES (N'Skill_MeleeTwo', N'Role_Melee', N'', 5, 20, 10, 5, 1, 100, 100, N'', N'', 0)
GO
INSERT [dbo].[Skill] ([ID], [RoleInGameID], [Name], [Cooldown], [Damage], [Chakra], [Uppercent], [LevelUnlock], [UpgradeCost], [BuyCost], [Image], [Description], [Delete]) VALUES (N'Skill_RangeOne', N'Role_Range', N'', 5, 20, 10, 5, 1, 100, 100, N'', N'', 0)
GO
INSERT [dbo].[Skill] ([ID], [RoleInGameID], [Name], [Cooldown], [Damage], [Chakra], [Uppercent], [LevelUnlock], [UpgradeCost], [BuyCost], [Image], [Description], [Delete]) VALUES (N'Skill_RangeThree', N'Role_Range', N'', 5, 20, 10, 5, 1, 100, 100, N'', N'', 0)
GO
INSERT [dbo].[Skill] ([ID], [RoleInGameID], [Name], [Cooldown], [Damage], [Chakra], [Uppercent], [LevelUnlock], [UpgradeCost], [BuyCost], [Image], [Description], [Delete]) VALUES (N'Skill_RangeTwo', N'Role_Range', N'', 5, 20, 10, 5, 1, 100, 100, N'', N'', 0)
GO
INSERT [dbo].[Skill] ([ID], [RoleInGameID], [Name], [Cooldown], [Damage], [Chakra], [Uppercent], [LevelUnlock], [UpgradeCost], [BuyCost], [Image], [Description], [Delete]) VALUES (N'Skill_SupportOne', N'Role_Support', N'', 5, 20, 10, 5, 1, 100, 100, N'', N'', 0)
GO
INSERT [dbo].[Skill] ([ID], [RoleInGameID], [Name], [Cooldown], [Damage], [Chakra], [Uppercent], [LevelUnlock], [UpgradeCost], [BuyCost], [Image], [Description], [Delete]) VALUES (N'Skill_SupportThree', N'Role_Support', N'', 5, 20, 10, 5, 1, 100, 100, N'', N'', 0)
GO
INSERT [dbo].[Skill] ([ID], [RoleInGameID], [Name], [Cooldown], [Damage], [Chakra], [Uppercent], [LevelUnlock], [UpgradeCost], [BuyCost], [Image], [Description], [Delete]) VALUES (N'Skill_SupportTwo', N'Role_Support', N'', 5, 20, 10, 5, 1, 100, 100, N'', N'', 0)
GO
INSERT [dbo].[SkillBoss] ([ID], [BossID], [Name], [Damage], [Image], [Description], [Delete]) VALUES (N'SkillBoss_Chidori', N'Boss_Kakashi', N'Lôi Điện', 1, N'Image', N'Description', 0)
GO
INSERT [dbo].[Skin] ([ID], [Name], [Image], [Delete]) VALUES (N'Skin_Naruto', N'Trang Phục Naruto', N'Creator/Skin_Naruto', 0)
GO
INSERT [dbo].[Skin] ([ID], [Name], [Image], [Delete]) VALUES (N'Skin_Shinobi', N'Trang Phục Shinobi', N'Creator/Skin_Shinobi', 0)
GO
INSERT [dbo].[Trophies] ([ID], [BossID], [Name], [ContraitLevelAccount], [Cost], [Image], [Description], [Delete]) VALUES (N'Trophie_Chunin', N'Boss_Kakashi', N'Trung đẳng', 10, 100, N'Image', N'Description', 0)
GO
INSERT [dbo].[Trophies] ([ID], [BossID], [Name], [ContraitLevelAccount], [Cost], [Image], [Description], [Delete]) VALUES (N'Trophie_Genin', N'Boss_Kakashi', N'Hạ đẳng', 10, 100, N'Image', N'Description', 0)
GO
INSERT [dbo].[Trophies] ([ID], [BossID], [Name], [ContraitLevelAccount], [Cost], [Image], [Description], [Delete]) VALUES (N'Trophie_Jonin', N'Boss_Kakashi', N'Thượng đẳng', 10, 100, N'Image', N'Description', 0)
GO
INSERT [dbo].[Trophies] ([ID], [BossID], [Name], [ContraitLevelAccount], [Cost], [Image], [Description], [Delete]) VALUES (N'Trophie_None', N'Boss_Kakashi', N'Tập sự', 0, 0, N'Image', N'Description', 0)
GO
INSERT [dbo].[TypeBoss] ([ID], [Name], [Delete]) VALUES (N'BossType_Arena', N'Đấu trường', 0)
GO
INSERT [dbo].[TypeBoss] ([ID], [Name], [Delete]) VALUES (N'BossType_Event', N'Sự kiện', 0)
GO
INSERT [dbo].[TypeBoss] ([ID], [Name], [Delete]) VALUES (N'BossType_Normal', N'Quái thường', 0)
GO
INSERT [dbo].[Weapon] ([ID], [Name], [Damage], [Uppercent], [UpgradeCost], [Image], [Description], [Delete]) VALUES (N'Weapon_Dart', N'Phi tiêu', 105, 5, 100, N'Creator/Dart', N'Phi tiêu', 0)
GO
INSERT [dbo].[Weapon] ([ID], [Name], [Damage], [Uppercent], [UpgradeCost], [Image], [Description], [Delete]) VALUES (N'Weapon_Glove', N'Bao tay', 120, 5, 100, N'Creator/Glove', N'Bao tay', 0)
GO
INSERT [dbo].[Weapon] ([ID], [Name], [Damage], [Uppercent], [UpgradeCost], [Image], [Description], [Delete]) VALUES (N'Weapon_Sword', N'Kiếm', 135, 5, 100, N'Creator/Sword', N'Kiếm', 0)
GO
ALTER TABLE [dbo].[Account] ADD  CONSTRAINT [DF_Account_RoleInGameID]  DEFAULT (N'Role_Melee') FOR [RoleInGameID]
GO
ALTER TABLE [dbo].[Account] ADD  CONSTRAINT [DF_Account_TrophiesID]  DEFAULT (N'Trophie_None') FOR [TrophiesID]
GO
ALTER TABLE [dbo].[Account] ADD  CONSTRAINT [DF_Account_Level]  DEFAULT ((1)) FOR [Level]
GO
ALTER TABLE [dbo].[Account] ADD  CONSTRAINT [DF_Account_Health]  DEFAULT ((0)) FOR [Health]
GO
ALTER TABLE [dbo].[Account] ADD  CONSTRAINT [DF_Account_CurrentHealth]  DEFAULT ((0)) FOR [CurrentHealth]
GO
ALTER TABLE [dbo].[Account] ADD  CONSTRAINT [DF_Account_Charka]  DEFAULT ((0)) FOR [Charka]
GO
ALTER TABLE [dbo].[Account] ADD  CONSTRAINT [DF_Account_CurrentCharka]  DEFAULT ((0)) FOR [CurrentCharka]
GO
ALTER TABLE [dbo].[Account] ADD  CONSTRAINT [DF_Account_Exp]  DEFAULT ((0)) FOR [Exp]
GO
ALTER TABLE [dbo].[Account] ADD  CONSTRAINT [DF_Account_Speed]  DEFAULT ((5)) FOR [Speed]
GO
ALTER TABLE [dbo].[Account] ADD  CONSTRAINT [DF_Account_Coin]  DEFAULT ((0)) FOR [Coin]
GO
ALTER TABLE [dbo].[Account] ADD  CONSTRAINT [DF_Account_Power]  DEFAULT ((0)) FOR [Power]
GO
ALTER TABLE [dbo].[Account] ADD  CONSTRAINT [DF_Account_Strength]  DEFAULT ((100)) FOR [Strength]
GO
ALTER TABLE [dbo].[Account] ADD  CONSTRAINT [DF_Account_CurrentStrength]  DEFAULT ((100)) FOR [CurrentStrength]
GO
ALTER TABLE [dbo].[Account] ADD  CONSTRAINT [DF_Account_Uppercent]  DEFAULT ((5)) FOR [Uppercent]
GO
ALTER TABLE [dbo].[Account] ADD  CONSTRAINT [DF_Account_EyeID]  DEFAULT (N'Eye_Byakugan') FOR [EyeID]
GO
ALTER TABLE [dbo].[Account] ADD  CONSTRAINT [DF_Account_HairID]  DEFAULT (N'Hair_Naruto') FOR [HairID]
GO
ALTER TABLE [dbo].[Account] ADD  CONSTRAINT [DF_Account_MouthID]  DEFAULT (N'Mouth_Normal') FOR [MouthID]
GO
ALTER TABLE [dbo].[Account] ADD  CONSTRAINT [DF_Account_SkinID]  DEFAULT (N'Skin_Naruto') FOR [SkinID]
GO
ALTER TABLE [dbo].[Account] ADD  CONSTRAINT [DF_Account_IsDead]  DEFAULT ((0)) FOR [IsDead]
GO
ALTER TABLE [dbo].[Account] ADD  CONSTRAINT [DF_Account_IsOnline]  DEFAULT ((0)) FOR [IsOnline]
GO
ALTER TABLE [dbo].[Account] ADD  CONSTRAINT [DF_Account_IsTicket]  DEFAULT ((0)) FOR [IsTicket]
GO
ALTER TABLE [dbo].[Account] ADD  CONSTRAINT [DF_Account_IsFirst]  DEFAULT ((1)) FOR [IsFirst]
GO
ALTER TABLE [dbo].[Account]  WITH CHECK ADD FOREIGN KEY([EyeID])
REFERENCES [dbo].[Eye] ([ID])
GO
ALTER TABLE [dbo].[Account]  WITH CHECK ADD FOREIGN KEY([HairID])
REFERENCES [dbo].[Hair] ([ID])
GO
ALTER TABLE [dbo].[Account]  WITH CHECK ADD FOREIGN KEY([MouthID])
REFERENCES [dbo].[Mouth] ([ID])
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
/****** Object:  StoredProcedure [dbo].[GetPower]    Script Date: 2023/05/19 13:49:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create     PROCEDURE [dbo].[GetPower] @AccountID nvarchar(50)
AS
DECLARE @AccountSkillOne int, @AccountSkillTwo int, @AccountSkillThree int
BEGIN
	if exists (select * from AccountSkill where AccountID = @AccountID and SkillID like '%One')
		BEGIN
				Set @AccountSkillOne = (select [Level] from AccountSkill where AccountID = @AccountID and SkillID like '%One')
		END
	else
		BEGIN
				Set @AccountSkillOne = 0
		END

    if exists (select * from AccountSkill where AccountID = @AccountID and SkillID like '%Two')
		BEGIN
				Set @AccountSkillTwo = (select [Level] from AccountSkill where AccountID = @AccountID and SkillID like '%Two')
		END
	else
		BEGIN
				Set @AccountSkillTwo = 0
		END

    if exists (select * from AccountSkill where AccountID = @AccountID and SkillID like '%Three')
		BEGIN
				Set @AccountSkillThree = (select [Level] from AccountSkill where AccountID = @AccountID and SkillID like '%Three')
		END
	else
		BEGIN
				Set @AccountSkillThree = 0
		END


	Select (@AccountSkillOne * 700) + (@AccountSkillTwo * 1000) + (@AccountSkillThree * 1500) 
	+ (select Health * 5 from Account where ID = @AccountID)
	+ (select Charka * 5 from Account where ID = @AccountID)
	+ (select Damage  * 50 from AccountWeapon where AccountID = @AccountID)
END
GO
USE [master]
GO
ALTER DATABASE [Shinobi] SET  READ_WRITE 
GO
