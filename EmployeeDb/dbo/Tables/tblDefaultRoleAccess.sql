CREATE TABLE [dbo].[tblDefaultRoleAccess] (
    [DRID]      BIGINT       IDENTITY (1, 1) NOT NULL,
    [RoleId]    VARCHAR (50) NOT NULL,
    [ModuleId]  BIGINT       NOT NULL,
    [IsView]    BIT          NOT NULL,
    [IsAdd]     BIT          NOT NULL,
    [IsEdit]    BIT          NOT NULL,
    [IsDeleted] BIT          NOT NULL,
    CONSTRAINT [PK_tblDefaultRoleAccess] PRIMARY KEY CLUSTERED ([DRID] ASC)
);



