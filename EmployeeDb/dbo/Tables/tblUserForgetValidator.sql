CREATE TABLE [dbo].[tblUserForgetValidator] (
    [ID]         BIGINT        IDENTITY (1, 1) NOT NULL,
    [Email]      VARCHAR (200) NULL,
    [EmailToken] VARCHAR (6)   NULL,
    [CreatedOn]  DATETIME      NULL,
    [IsActive]   BIT           CONSTRAINT [DF_tblUserForgetValidator_IsActive] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_tblUserForgetValidator] PRIMARY KEY CLUSTERED ([ID] ASC)
);

