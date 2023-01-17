CREATE TABLE [dbo].[tblEmployeeSalary] (
    [SalaryId]        VARCHAR (50) NOT NULL,
    [EmployeeId]      VARCHAR (50) NOT NULL,
    [TotalAllowances] DECIMAL (18) NULL,
    [TotalDeduction]  DECIMAL (18) NULL,
    [NetSalary]       DECIMAL (18) NULL,
    [CreatedBy]       VARCHAR (50) NULL,
    [CreatedOn]       DATETIME     NULL,
    [DeletedBy]       VARCHAR (50) NULL,
    [DeletedOn]       DATETIME     NULL,
    [UpdatedBy]       VARCHAR (50) NULL,
    [UpdatedOn]       DATETIME     NULL,
    [IsDeleted]       BIT          NULL,
    [MonthName]       VARCHAR (15) DEFAULT ((0)) NOT NULL,
    [year]            INT          DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_tblEmployeeSalary] PRIMARY KEY CLUSTERED ([SalaryId] ASC)
);



