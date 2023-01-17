CREATE TABLE [dbo].[tblMasterState] (
    [StateId]   BIGINT        NOT NULL,
    [State]     NVARCHAR (50) NULL,
    [StateCode] VARCHAR (5)   NULL,
    CONSTRAINT [PK_tblMasterState] PRIMARY KEY CLUSTERED ([StateId] ASC)
);

