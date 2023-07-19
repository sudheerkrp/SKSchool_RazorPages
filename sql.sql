USE [SK_School_DB]
GO

/****** Object:  Table [dbo].[Branches]    Script Date: 19-07-2023 11:56:34 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Branches](
	[code] [uniqueidentifier] NOT NULL,
	[name] [text] NOT NULL,
	[active_bit] [bit] NOT NULL,
	[update_on] [datetime] NULL,
 CONSTRAINT [PK_Branches] PRIMARY KEY CLUSTERED 
(
	[code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[Branches] ADD  CONSTRAINT [DF_Branches_active_bit_1]  DEFAULT ((1)) FOR [active_bit]
GO

ALTER TABLE [dbo].[Branches] ADD  CONSTRAINT [DF_Branches_update_on_1]  DEFAULT (NULL) FOR [update_on]
GO

USE [SK_School_DB]
GO

/****** Object:  Table [dbo].[Subjects]    Script Date: 19-07-2023 11:59:23 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Subjects](
	[code] [uniqueidentifier] NOT NULL,
	[name] [text] NOT NULL,
	[active_bit] [bit] NOT NULL,
	[update_on] [datetime] NULL,
 CONSTRAINT [PK_Subjects] PRIMARY KEY CLUSTERED 
(
	[code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[Subjects] ADD  CONSTRAINT [DF_Subjects_active_bit]  DEFAULT ((1)) FOR [active_bit]
GO

ALTER TABLE [dbo].[Subjects] ADD  CONSTRAINT [DF_Subjects_update_on]  DEFAULT (NULL) FOR [update_on]
GO

USE [SK_School_DB]
GO

/****** Object:  Table [dbo].[Students]    Script Date: 19-07-2023 11:58:03 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Students](
	[rollNo] [uniqueidentifier] NOT NULL,
	[name] [text] NOT NULL,
	[branchCode] [uniqueidentifier] NOT NULL,
	[active_bit] [bit] NOT NULL,
	[update_on] [datetime] NULL,
 CONSTRAINT [PK_Students] PRIMARY KEY CLUSTERED 
(
	[rollNo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[Students] ADD  CONSTRAINT [DF_Students_active_bit]  DEFAULT ((1)) FOR [active_bit]
GO

ALTER TABLE [dbo].[Students] ADD  CONSTRAINT [DF_Students_update_on]  DEFAULT (NULL) FOR [update_on]
GO

ALTER TABLE [dbo].[Students]  WITH CHECK ADD  CONSTRAINT [FK_Students_Branches] FOREIGN KEY([branchCode])
REFERENCES [dbo].[Branches] ([code])
GO

ALTER TABLE [dbo].[Students] CHECK CONSTRAINT [FK_Students_Branches]
GO


USE [SK_School_DB]
GO

/****** Object:  Table [dbo].[Teachers]    Script Date: 19-07-2023 11:59:53 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Teachers](
	[id] [uniqueidentifier] NOT NULL,
	[name] [text] NOT NULL,
	[branchCode] [uniqueidentifier] NOT NULL,
	[active_bit] [bit] NOT NULL,
	[update_on] [datetime] NULL,
 CONSTRAINT [PK_Teachers] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[Teachers] ADD  CONSTRAINT [DF_Teachers_active_bit]  DEFAULT ((1)) FOR [active_bit]
GO

ALTER TABLE [dbo].[Teachers] ADD  CONSTRAINT [DF_Teachers_update_on]  DEFAULT (NULL) FOR [update_on]
GO

ALTER TABLE [dbo].[Teachers]  WITH CHECK ADD  CONSTRAINT [FK_Teachers_Branches] FOREIGN KEY([branchCode])
REFERENCES [dbo].[Branches] ([code])
GO

ALTER TABLE [dbo].[Teachers] CHECK CONSTRAINT [FK_Teachers_Branches]
GO

USE [SK_School_DB]
GO

/****** Object:  Table [dbo].[Subjects_Enrollment]    Script Date: 20-07-2023 12:01:33 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Subjects_Enrollment](
	[subjCode] [uniqueidentifier] NOT NULL,
	[branchCode] [uniqueidentifier] NOT NULL,
	[active_bit] [bit] NOT NULL,
	[update_on] [datetime] NULL,
 CONSTRAINT [PK_Subjects_Enrollment] PRIMARY KEY CLUSTERED 
(
	[subjCode] ASC,
	[branchCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Subjects_Enrollment] ADD  CONSTRAINT [DF_Subjects_Enrollment_active_bit]  DEFAULT ((1)) FOR [active_bit]
GO

ALTER TABLE [dbo].[Subjects_Enrollment] ADD  CONSTRAINT [DF_Subjects_Enrollment_update_on]  DEFAULT (NULL) FOR [update_on]
GO

ALTER TABLE [dbo].[Subjects_Enrollment]  WITH CHECK ADD  CONSTRAINT [FK_Subjects_Enrollment_Branches] FOREIGN KEY([branchCode])
REFERENCES [dbo].[Branches] ([code])
GO

ALTER TABLE [dbo].[Subjects_Enrollment] CHECK CONSTRAINT [FK_Subjects_Enrollment_Branches]
GO

ALTER TABLE [dbo].[Subjects_Enrollment]  WITH CHECK ADD  CONSTRAINT [FK_Subjects_Enrollment_Subjects] FOREIGN KEY([subjCode])
REFERENCES [dbo].[Subjects] ([code])
GO

ALTER TABLE [dbo].[Subjects_Enrollment] CHECK CONSTRAINT [FK_Subjects_Enrollment_Subjects]
GO


USE [SK_School_DB]
GO

/****** Object:  Table [dbo].[Students_Enrollment]    Script Date: 20-07-2023 12:02:06 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Students_Enrollment](
	[rollNo] [uniqueidentifier] NOT NULL,
	[subjCode] [uniqueidentifier] NOT NULL,
	[active_bit] [bit] NOT NULL,
	[update_on] [datetime] NULL,
 CONSTRAINT [PK_Students_Enrollment] PRIMARY KEY CLUSTERED 
(
	[rollNo] ASC,
	[subjCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Students_Enrollment] ADD  CONSTRAINT [DF_Students_Enrollment_active_bit_1]  DEFAULT ((1)) FOR [active_bit]
GO

ALTER TABLE [dbo].[Students_Enrollment] ADD  CONSTRAINT [DF_Students_Enrollment_update_on_1]  DEFAULT (NULL) FOR [update_on]
GO

ALTER TABLE [dbo].[Students_Enrollment]  WITH CHECK ADD  CONSTRAINT [FK_Students_Enrollment_Students] FOREIGN KEY([rollNo])
REFERENCES [dbo].[Students] ([rollNo])
GO

ALTER TABLE [dbo].[Students_Enrollment] CHECK CONSTRAINT [FK_Students_Enrollment_Students]
GO

ALTER TABLE [dbo].[Students_Enrollment]  WITH CHECK ADD  CONSTRAINT [FK_Students_Enrollment_Subjects] FOREIGN KEY([subjCode])
REFERENCES [dbo].[Subjects] ([code])
GO

ALTER TABLE [dbo].[Students_Enrollment] CHECK CONSTRAINT [FK_Students_Enrollment_Subjects]
GO


USE [SK_School_DB]
GO

/****** Object:  Table [dbo].[Teachers_Enrollment]    Script Date: 20-07-2023 12:02:34 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Teachers_Enrollment](
	[empId] [uniqueidentifier] NOT NULL,
	[subjCode] [uniqueidentifier] NOT NULL,
	[active_bit] [bit] NOT NULL,
	[update_on] [datetime] NULL,
 CONSTRAINT [PK_Teachers_Enrollment] PRIMARY KEY CLUSTERED 
(
	[empId] ASC,
	[subjCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Teachers_Enrollment] ADD  CONSTRAINT [DF_Teachers_Enrollment_active_bit]  DEFAULT ((1)) FOR [active_bit]
GO

ALTER TABLE [dbo].[Teachers_Enrollment] ADD  CONSTRAINT [DF_Teachers_Enrollment_update_on]  DEFAULT (NULL) FOR [update_on]
GO

ALTER TABLE [dbo].[Teachers_Enrollment]  WITH CHECK ADD  CONSTRAINT [FK_Teachers_Enrollment_Subjects] FOREIGN KEY([subjCode])
REFERENCES [dbo].[Subjects] ([code])
GO

ALTER TABLE [dbo].[Teachers_Enrollment] CHECK CONSTRAINT [FK_Teachers_Enrollment_Subjects]
GO

ALTER TABLE [dbo].[Teachers_Enrollment]  WITH CHECK ADD  CONSTRAINT [FK_Teachers_Enrollment_Teachers] FOREIGN KEY([empId])
REFERENCES [dbo].[Teachers] ([id])
GO

ALTER TABLE [dbo].[Teachers_Enrollment] CHECK CONSTRAINT [FK_Teachers_Enrollment_Teachers]
GO


