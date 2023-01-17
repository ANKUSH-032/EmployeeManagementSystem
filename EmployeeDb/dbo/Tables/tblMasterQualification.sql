CREATE TABLE [dbo].[tblMasterQualification] (
    [QualificationId] BIGINT       NOT NULL,
    [Qualification]   VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_tblMasterQualification] PRIMARY KEY CLUSTERED ([QualificationId] ASC)
);

