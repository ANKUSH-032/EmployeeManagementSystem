CREATE TABLE [dbo].[tblModuleMapping] (
    [MappingId] BIGINT       IDENTITY (1, 1) NOT NULL,
    [UserId]    VARCHAR (50) NOT NULL,
    [ModuleId]  BIGINT       NOT NULL,
    [IsView]    BIT          NOT NULL,
    [IsAdd]     BIT          NOT NULL,
    [IsEdit]    BIT          NOT NULL,
    [IsDeleted] BIT          NOT NULL,
    [CreatedOn] DATETIME     NULL,
    [CreatedBy] VARCHAR (50) NULL,
    [UpdatedOn] DATETIME     NULL,
    [IsActive]  BIT          CONSTRAINT [DF_tblModuleMapping_IsActive] DEFAULT ((1)) NULL,
    [UpdatedBy] VARCHAR (50) NULL,
    CONSTRAINT [PK_tblModuleMapping] PRIMARY KEY CLUSTERED ([MappingId] ASC)
);



