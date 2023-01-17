CREATE TABLE [dbo].[tblUserActivityLog] (
    [LogID]        VARCHAR (50)   CONSTRAINT [DF_tblUserActivityLog_LogID] DEFAULT (newid()) NOT NULL,
    [UserID]       VARCHAR (50)   NULL,
    [IpAddress]    VARCHAR (100)  NULL,
    [AreaAccessed] VARCHAR (1000) NULL,
    [TimpStamp]    DATETIME       NULL,
    [Body]         VARCHAR (MAX)  NULL,
    [StatusCode]   INT            NULL,
    [Method]       VARCHAR (500)  NULL,
    [CreatedOn]    DATETIME       NULL
);

