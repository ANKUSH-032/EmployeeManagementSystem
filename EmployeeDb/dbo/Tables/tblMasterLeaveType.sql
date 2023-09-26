CREATE TABLE [dbo].[tblMasterLeaveType] (
    [LeaveTypeId] INT          IDENTITY (1, 1) NOT NULL,
    [LeaveType]   VARCHAR (40) NOT NULL,
    [CountLeave]  INT          CONSTRAINT [DF__tblMaster__Count__571DF1D5] DEFAULT ('0') NOT NULL,
    CONSTRAINT [PK_tblLeaveType] PRIMARY KEY CLUSTERED ([LeaveTypeId] ASC)
);



