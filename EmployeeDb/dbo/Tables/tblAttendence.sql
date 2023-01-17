CREATE TABLE [dbo].[tblAttendence] (
    [AttendenceId]     VARCHAR (50) NOT NULL,
    [EmployeeId]       VARCHAR (50) NOT NULL,
    [FromTime]         DATETIME     NULL,
    [ToTime]           DATETIME     NULL,
    [CreatedBy]        VARCHAR (50) NULL,
    [CreatedOn]        DATETIME     NULL,
    [UpdateBy]         VARCHAR (50) NULL,
    [UpdatedOn]        DATETIME     NULL,
    [DeletedBy]        VARCHAR (50) NULL,
    [DeletedOn]        DATETIME     NULL,
    [IsDeleted]        BIT          NULL,
    [AttendenceStatus] BIT          NULL,
    CONSTRAINT [PK_tblAttendence] PRIMARY KEY CLUSTERED ([AttendenceId] ASC)
);

