CREATE TABLE [dbo].[tblModule] (
    [ModuleId]   BIGINT        IDENTITY (1, 1) NOT NULL,
    [ModuleName] NVARCHAR (50) NOT NULL,
    [CreatedOn]  DATETIME      NULL,
    [CreatedBy]  VARCHAR (50)  NULL,
    [UpdatedOn]  DATETIME      NULL,
    [UpdatedBy]  VARCHAR (50)  NULL,
    [IsActive]   BIT           CONSTRAINT [DF_tblModule_IsActive] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_tblModule] PRIMARY KEY CLUSTERED ([ModuleId] ASC)
);

