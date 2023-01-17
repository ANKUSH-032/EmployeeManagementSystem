CREATE TABLE [dbo].[tblLeave] (
    [LeaveId]       VARCHAR (50)   NOT NULL,
    [FromDate]      VARCHAR (11)   NOT NULL,
    [ToDate]        VARCHAR (11)   NOT NULL,
    [EmployeeId]    VARCHAR (50)   NOT NULL,
    [Reason]        VARCHAR (1000) NOT NULL,
    [LeaveType]     INT            NOT NULL,
    [StatusType]    INT            NULL,
    [CreatedBy]     VARCHAR (50)   NULL,
    [CreatedOn]     DATETIME       NULL,
    [UpdatedBy]     VARCHAR (50)   NULL,
    [UpdatedOn]     DATETIME       NULL,
    [DeletedBy]     VARCHAR (50)   NULL,
    [DeletedOn]     DATETIME       NULL,
    [IsDeleted]     BIT            NULL,
    [NumberOfLeave] INT            CONSTRAINT [DF__tblLeave__Number__339FAB6E] DEFAULT ('0') NULL,
    CONSTRAINT [PK_tblLeave] PRIMARY KEY CLUSTERED ([LeaveId] ASC)
);

