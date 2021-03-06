USE [master]
GO
/****** Object:  Database [CIMS_NEW]    Script Date: 04/09/2017 10:48:05 AM ******/
CREATE DATABASE [CIMS_NEW]
ALTER DATABASE [CIMS_NEW] SET COMPATIBILITY_LEVEL = 110
GO
ALTER DATABASE [CIMS_NEW] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [CIMS_NEW] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [CIMS_NEW] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [CIMS_NEW] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [CIMS_NEW] SET ARITHABORT OFF 
GO
ALTER DATABASE [CIMS_NEW] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [CIMS_NEW] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [CIMS_NEW] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [CIMS_NEW] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [CIMS_NEW] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [CIMS_NEW] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [CIMS_NEW] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [CIMS_NEW] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [CIMS_NEW] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [CIMS_NEW] SET  DISABLE_BROKER 
GO
ALTER DATABASE [CIMS_NEW] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [CIMS_NEW] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [CIMS_NEW] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [CIMS_NEW] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [CIMS_NEW] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [CIMS_NEW] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [CIMS_NEW] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [CIMS_NEW] SET RECOVERY FULL 
GO
ALTER DATABASE [CIMS_NEW] SET  MULTI_USER 
GO
ALTER DATABASE [CIMS_NEW] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [CIMS_NEW] SET DB_CHAINING OFF 
GO
ALTER DATABASE [CIMS_NEW] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [CIMS_NEW] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
EXEC sys.sp_db_vardecimal_storage_format N'CIMS_NEW', N'ON'
GO
USE [CIMS_NEW]
GO
/****** Object:  User [CIMS_User]    Script Date: 04/09/2017 10:48:05 AM ******/
CREATE USER [CIMS_User] FOR LOGIN [CIMS_User] WITH DEFAULT_SCHEMA=[dbo]
GO
/****** Object:  Table [dbo].[Action]    Script Date: 04/09/2017 10:48:05 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Action](
	[ActionID] [int] IDENTITY(1,1) NOT NULL,
	[Comment] [varchar](max) NOT NULL,
	[ActionDate] [datetime] NOT NULL,
	[InstructionID] [int] NOT NULL,
	[UserID] [int] NOT NULL,
	[StatusID] [int] NOT NULL,
 CONSTRAINT [PK_Action] PRIMARY KEY CLUSTERED 
(
	[ActionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[AuditLog]    Script Date: 04/09/2017 10:48:05 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[AuditLog](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Action] [varchar](255) NOT NULL,
	[UserID] [int] NOT NULL,
 CONSTRAINT [PK_AuditLog] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Branch]    Script Date: 04/09/2017 10:48:05 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Branch](
	[BranchID] [int] IDENTITY(1,1) NOT NULL,
	[BranchName] [varchar](255) NOT NULL,
	[Active] [bit] NOT NULL,
 CONSTRAINT [PK_Branch] PRIMARY KEY CLUSTERED 
(
	[BranchID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Client]    Script Date: 04/09/2017 10:48:05 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Client](
	[ClientID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](255) NOT NULL,
	[AccountNumber] [varchar](255) NOT NULL,
	[Portfolio] [varchar](255) NOT NULL,
	[Sector] [varchar](255) NOT NULL,
	[Active] [bit] NOT NULL,
	[CustomerNumber] [varchar](255) NOT NULL,
	[BranchID] [int] NOT NULL,
 CONSTRAINT [PK_Client] PRIMARY KEY CLUSTERED 
(
	[ClientID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Currency]    Script Date: 04/09/2017 10:48:05 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Currency](
	[CurrencyID] [int] IDENTITY(1,1) NOT NULL,
	[CurrencyName] [varchar](255) NOT NULL,
	[CurrencyCode] [varchar](255) NOT NULL,
	[Active] [bit] NOT NULL,
 CONSTRAINT [PK_Currency] PRIMARY KEY CLUSTERED 
(
	[CurrencyID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Instruction]    Script Date: 04/09/2017 10:48:05 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Instruction](
	[InstructionID] [int] IDENTITY(1,1) NOT NULL,
	[InstructionTypeID] [int] NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[CreateUser] [int] NOT NULL,
	[StatusID] [int] NOT NULL,
	[FromUser] [int] NOT NULL,
	[ToUser] [int] NOT NULL,
	[Amount] [float] NOT NULL,
	[ClientID] [int] NOT NULL,
	[CurrencyFrom] [int] NOT NULL,
	[CurrencyTo] [int] NOT NULL,
	[BranchID] [int] NOT NULL,
	[EERef] [varchar](255) NULL,
 CONSTRAINT [PK_Instruction] PRIMARY KEY CLUSTERED 
(
	[InstructionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[InstructionType]    Script Date: 04/09/2017 10:48:05 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[InstructionType](
	[InstructionTypeID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](255) NOT NULL,
	[Description] [varchar](255) NOT NULL,
	[Cutoff] [varchar](255) NOT NULL,
	[Active] [bit] NOT NULL,
 CONSTRAINT [PK_InstructionType] PRIMARY KEY CLUSTERED 
(
	[InstructionTypeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Role]    Script Date: 04/09/2017 10:48:05 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Role](
	[RoleID] [int] IDENTITY(1,1) NOT NULL,
	[RoleName] [varchar](50) NOT NULL,
	[RoleDescription] [varchar](255) NOT NULL,
	[Active] [bit] NOT NULL,
	[BranchID] [int] NULL,
 CONSTRAINT [PK_Role] PRIMARY KEY CLUSTERED 
(
	[RoleID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Status]    Script Date: 04/09/2017 10:48:05 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Status](
	[StatusID] [int] IDENTITY(1,1) NOT NULL,
	[InstructionTypeID] [int] NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[Description] [varchar](50) NOT NULL,
	[Active] [bit] NOT NULL,
	[NextStatus] [int] NOT NULL,
	[RoleID] [int] NOT NULL,
 CONSTRAINT [PK_Status] PRIMARY KEY CLUSTERED 
(
	[StatusID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[User]    Script Date: 04/09/2017 10:48:05 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[User](
	[UserID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](255) NOT NULL,
	[Surname] [varchar](255) NOT NULL,
	[Email] [varchar](255) NOT NULL,
	[Approved] [int] NULL,
	[Active] [bit] NOT NULL,
	[Username] [varchar](255) NOT NULL,
 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
(
	[UserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[UserRole]    Script Date: 04/09/2017 10:48:05 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserRole](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [int] NOT NULL,
	[RoleID] [int] NOT NULL,
	[Active] [bit] NOT NULL,
 CONSTRAINT [PK_UserRole] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET IDENTITY_INSERT [dbo].[InstructionType] ON 

GO
INSERT [dbo].[InstructionType] ([InstructionTypeID], [Name], [Description], [Cutoff], [Active]) VALUES (1, N'--', N'Unset', N'9 : 00 PM', 1)
GO
SET IDENTITY_INSERT [dbo].[InstructionType] OFF
GO
SET IDENTITY_INSERT [dbo].[Role] ON 

GO
INSERT [dbo].[Role] ([RoleID], [RoleName], [RoleDescription], [Active], [BranchID]) VALUES (1, N'Administrator', N'User who is allowed to make changes to other users and work flows.', 1, NULL)
GO
SET IDENTITY_INSERT [dbo].[Role] OFF
GO
SET IDENTITY_INSERT [dbo].[Status] ON 

GO
INSERT [dbo].[Status] ([StatusID], [InstructionTypeID], [Name], [Description], [Active], [NextStatus], [RoleID]) VALUES (1, 1, N'Unset', N'Unset', 1, 1, 1)
GO
SET IDENTITY_INSERT [dbo].[Status] OFF
GO
SET IDENTITY_INSERT [dbo].[User] ON 

GO
INSERT [dbo].[User] ([UserID], [Name], [Surname], [Email], [Approved], [Active], [Username]) VALUES (1, N'--', N'--', N'--', 1, 1, N'--')
GO
INSERT [dbo].[User] ([UserID], [Name], [Surname], [Email], [Approved], [Active], [Username]) VALUES (2, N'David', N'Sobey', N'david.sobey@standardbank.co.za', 1, 1, N'A227891')
GO
INSERT [dbo].[User] ([UserID], [Name], [Surname], [Email], [Approved], [Active], [Username]) VALUES (3, N'Lulamela', N'Dyantyi', N'lulamela.dyantyi@standardbank.co.za', 1, 1, N'A210052')
GO
SET IDENTITY_INSERT [dbo].[User] OFF
GO
SET IDENTITY_INSERT [dbo].[UserRole] ON 

GO
INSERT [dbo].[UserRole] ([ID], [UserID], [RoleID], [Active]) VALUES (1, 1, 1, 1)
GO
INSERT [dbo].[UserRole] ([ID], [UserID], [RoleID], [Active]) VALUES (2, 2, 1, 1)
GO
SET IDENTITY_INSERT [dbo].[UserRole] OFF
GO
ALTER TABLE [dbo].[Action]  WITH CHECK ADD  CONSTRAINT [FK_Action_Instruction] FOREIGN KEY([InstructionID])
REFERENCES [dbo].[Instruction] ([InstructionID])
GO
ALTER TABLE [dbo].[Action] CHECK CONSTRAINT [FK_Action_Instruction]
GO
ALTER TABLE [dbo].[Action]  WITH CHECK ADD  CONSTRAINT [FK_Action_Status] FOREIGN KEY([StatusID])
REFERENCES [dbo].[Status] ([StatusID])
GO
ALTER TABLE [dbo].[Action] CHECK CONSTRAINT [FK_Action_Status]
GO
ALTER TABLE [dbo].[Action]  WITH CHECK ADD  CONSTRAINT [FK_Action_User] FOREIGN KEY([UserID])
REFERENCES [dbo].[User] ([UserID])
GO
ALTER TABLE [dbo].[Action] CHECK CONSTRAINT [FK_Action_User]
GO
ALTER TABLE [dbo].[AuditLog]  WITH CHECK ADD  CONSTRAINT [FK_AuditLog_User] FOREIGN KEY([UserID])
REFERENCES [dbo].[User] ([UserID])
GO
ALTER TABLE [dbo].[AuditLog] CHECK CONSTRAINT [FK_AuditLog_User]
GO
ALTER TABLE [dbo].[Client]  WITH CHECK ADD  CONSTRAINT [FK_Client_Branch] FOREIGN KEY([BranchID])
REFERENCES [dbo].[Branch] ([BranchID])
GO
ALTER TABLE [dbo].[Client] CHECK CONSTRAINT [FK_Client_Branch]
GO
ALTER TABLE [dbo].[Instruction]  WITH CHECK ADD  CONSTRAINT [FK_Instruction_Branch] FOREIGN KEY([BranchID])
REFERENCES [dbo].[Branch] ([BranchID])
GO
ALTER TABLE [dbo].[Instruction] CHECK CONSTRAINT [FK_Instruction_Branch]
GO
ALTER TABLE [dbo].[Instruction]  WITH CHECK ADD  CONSTRAINT [FK_Instruction_Client] FOREIGN KEY([ClientID])
REFERENCES [dbo].[Client] ([ClientID])
GO
ALTER TABLE [dbo].[Instruction] CHECK CONSTRAINT [FK_Instruction_Client]
GO
ALTER TABLE [dbo].[Instruction]  WITH CHECK ADD  CONSTRAINT [FK_Instruction_Currency] FOREIGN KEY([CurrencyFrom])
REFERENCES [dbo].[Currency] ([CurrencyID])
GO
ALTER TABLE [dbo].[Instruction] CHECK CONSTRAINT [FK_Instruction_Currency]
GO
ALTER TABLE [dbo].[Instruction]  WITH CHECK ADD  CONSTRAINT [FK_Instruction_Currency1] FOREIGN KEY([CurrencyTo])
REFERENCES [dbo].[Currency] ([CurrencyID])
GO
ALTER TABLE [dbo].[Instruction] CHECK CONSTRAINT [FK_Instruction_Currency1]
GO
ALTER TABLE [dbo].[Instruction]  WITH CHECK ADD  CONSTRAINT [FK_Instruction_InstructionType] FOREIGN KEY([InstructionTypeID])
REFERENCES [dbo].[InstructionType] ([InstructionTypeID])
GO
ALTER TABLE [dbo].[Instruction] CHECK CONSTRAINT [FK_Instruction_InstructionType]
GO
ALTER TABLE [dbo].[Instruction]  WITH CHECK ADD  CONSTRAINT [FK_Instruction_User] FOREIGN KEY([FromUser])
REFERENCES [dbo].[User] ([UserID])
GO
ALTER TABLE [dbo].[Instruction] CHECK CONSTRAINT [FK_Instruction_User]
GO
ALTER TABLE [dbo].[Instruction]  WITH CHECK ADD  CONSTRAINT [FK_Instruction_User1] FOREIGN KEY([ToUser])
REFERENCES [dbo].[User] ([UserID])
GO
ALTER TABLE [dbo].[Instruction] CHECK CONSTRAINT [FK_Instruction_User1]
GO
ALTER TABLE [dbo].[Status]  WITH CHECK ADD  CONSTRAINT [FK_Status_InstructionType1] FOREIGN KEY([InstructionTypeID])
REFERENCES [dbo].[InstructionType] ([InstructionTypeID])
GO
ALTER TABLE [dbo].[Status] CHECK CONSTRAINT [FK_Status_InstructionType1]
GO
ALTER TABLE [dbo].[Status]  WITH CHECK ADD  CONSTRAINT [FK_Status_Role] FOREIGN KEY([RoleID])
REFERENCES [dbo].[Role] ([RoleID])
GO
ALTER TABLE [dbo].[Status] CHECK CONSTRAINT [FK_Status_Role]
GO
ALTER TABLE [dbo].[Status]  WITH CHECK ADD  CONSTRAINT [FK_Status_Status] FOREIGN KEY([NextStatus])
REFERENCES [dbo].[Status] ([StatusID])
GO
ALTER TABLE [dbo].[Status] CHECK CONSTRAINT [FK_Status_Status]
GO
ALTER TABLE [dbo].[UserRole]  WITH CHECK ADD  CONSTRAINT [FK_UserRole_Role] FOREIGN KEY([RoleID])
REFERENCES [dbo].[Role] ([RoleID])
GO
ALTER TABLE [dbo].[UserRole] CHECK CONSTRAINT [FK_UserRole_Role]
GO
ALTER TABLE [dbo].[UserRole]  WITH CHECK ADD  CONSTRAINT [FK_UserRole_User] FOREIGN KEY([UserID])
REFERENCES [dbo].[User] ([UserID])
GO
ALTER TABLE [dbo].[UserRole] CHECK CONSTRAINT [FK_UserRole_User]
GO
USE [master]
GO
ALTER DATABASE [CIMS_NEW] SET  READ_WRITE 
GO
