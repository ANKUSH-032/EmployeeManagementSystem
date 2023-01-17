CREATE TABLE [dbo].[tblAllowances] (
    [AllowanceId]         VARCHAR (50) NOT NULL,
    [EmployeeId]          VARCHAR (50) NOT NULL,
    [BasicSalary]         DECIMAL (18) NULL,
    [HouseAllowance]      DECIMAL (18) NULL,
    [ConveyanceAllowance] DECIMAL (18) NULL,
    [SpecialAllowance]    DECIMAL (18) NULL,
    [TotalAllowance]      DECIMAL (18) NULL,
    [CreatedBy]           VARCHAR (50) NULL,
    [CreatedOn]           DATETIME     NULL,
    [DeletedBy]           VARCHAR (50) NULL,
    [DeletedOn]           DATETIME     NULL,
    [UpdatedBy]           VARCHAR (50) NULL,
    [UpdatedOn]           DATETIME     NULL,
    [IsDeleted]           BIT          NULL,
    [MonthName]           VARCHAR (15) DEFAULT ((0)) NOT NULL,
    [year]                INT          DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_tblAllowances] PRIMARY KEY CLUSTERED ([AllowanceId] ASC)
);



