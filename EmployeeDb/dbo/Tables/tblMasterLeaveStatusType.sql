CREATE TABLE [dbo].[tblMasterLeaveStatusType] (
    [StatusId]   INT          NOT NULL,
    [StatusType] VARCHAR (20) NOT NULL,
    CONSTRAINT [PK_tblMasterLeaveStatusType] PRIMARY KEY CLUSTERED ([StatusId] ASC)
);

