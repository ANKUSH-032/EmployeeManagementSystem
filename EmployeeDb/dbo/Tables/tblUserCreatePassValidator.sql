CREATE TABLE [dbo].[tblUserCreatePassValidator] (
    [ID]         BIGINT       IDENTITY (1, 1) NOT NULL,
    [Email]      VARCHAR (50) NULL,
    [EmailToken] VARCHAR (50) NULL,
    [CreatedOn]  DATETIME     NULL,
    [IsActive]   BIT          CONSTRAINT [DF_tblUserCreatePassValidator_IsActive] DEFAULT ((1)) NULL,
    CONSTRAINT [PK_tblUserCreatePassValidator] PRIMARY KEY CLUSTERED ([ID] ASC)
);

