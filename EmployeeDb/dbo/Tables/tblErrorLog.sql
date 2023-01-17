CREATE TABLE [dbo].[tblErrorLog] (
    [ErrorLogID]  BIGINT         IDENTITY (1, 1) NOT NULL,
    [Controller]  VARCHAR (100)  NULL,
    [Action]      VARCHAR (100)  NULL,
    [Message]     VARCHAR (1000) NULL,
    [Source]      VARCHAR (3000) NULL,
    [StackTrace]  VARCHAR (3000) NULL,
    [LineNo]      INT            NULL,
    [RequestBody] VARCHAR (MAX)  NULL,
    [CreatedOn]   DATETIME       NULL,
    [CreatedBy]   VARCHAR (200)  NULL,
    CONSTRAINT [PK_tblErrorLog] PRIMARY KEY CLUSTERED ([ErrorLogID] ASC)
);

