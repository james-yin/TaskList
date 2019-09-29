USE [TaskList]
GO

/****** Object: Table [dbo].[yin_ActivitiesHistory] Script Date: 9/28/2019 6:59:14 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[yin_ActivitiesHistory] (
    [ActivitiesHistoryId]     INT           IDENTITY (1, 1) NOT NULL,
    [ActivityId]              INT           NOT NULL,
    [TaskName]                VARCHAR (50)  NOT NULL,
    [ActivityCreatedDateTime] DATETIME2 (7) NOT NULL,
    [CreatedDateTime]         DATETIME2 (7) NOT NULL
);


