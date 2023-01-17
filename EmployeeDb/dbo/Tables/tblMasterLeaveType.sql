CREATE TABLE [dbo].[tblMasterLeaveType] (
    [LeaveTypeId] INT          IDENTITY (1, 1) NOT NULL,
    [LeaveType]   VARCHAR (20) NOT NULL,
    [CountLeave]  INT          DEFAULT ('0') NOT NULL,
    CONSTRAINT [PK_tblLeaveType] PRIMARY KEY CLUSTERED ([LeaveTypeId] ASC)
);

