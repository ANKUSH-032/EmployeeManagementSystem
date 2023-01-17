CREATE TABLE [dbo].[tblMasterTypeOfEmployee] (
    [TypeOfEmployeeId] BIGINT       NOT NULL,
    [DesignationName]  VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_tblMasterTypeOfEmployee] PRIMARY KEY CLUSTERED ([TypeOfEmployeeId] ASC)
);

