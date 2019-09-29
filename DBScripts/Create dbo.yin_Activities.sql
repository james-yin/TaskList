USE [TaskList]
GO

/****** Object: Table [dbo].[yin_Activities] Script Date: 9/28/2019 6:58:05 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[yin_Activities] (
    [ActivityId]      INT           IDENTITY (1, 1) NOT NULL,
    [TaskName]        VARCHAR (50)  NOT NULL,
    [CreatedDateTime] DATETIME2 (7) NOT NULL,
    [AssignmentCd]    VARCHAR (20)  NOT NULL
);


