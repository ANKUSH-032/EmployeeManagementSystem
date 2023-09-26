CREATE TABLE [dbo].[tblMasterPincode] (
    [Id]       BIGINT       NOT NULL,
    [StateId]  BIGINT       NULL,
    [Location] VARCHAR (50) NULL,
    [Pincode]  BIGINT       NULL,
    [District] VARCHAR (50) NULL,
    CONSTRAINT [PK_tblMasterPincode] PRIMARY KEY CLUSTERED ([Id] ASC)
);



