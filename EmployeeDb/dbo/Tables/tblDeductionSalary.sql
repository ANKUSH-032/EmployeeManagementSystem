CREATE TABLE [dbo].[tblDeductionSalary] (
    [DeductionId]                  VARCHAR (50) NOT NULL,
    [EmployeeId]                   VARCHAR (50) NOT NULL,
    [EPF]                          DECIMAL (18) NULL,
    [TRA]                          DECIMAL (18) NULL,
    [HRA]                          DECIMAL (18) NULL,
    [ProfessionalTax]              DECIMAL (18) NULL,
    [TDS]                          DECIMAL (18) NULL,
    [TotalDeduction]               DECIMAL (18) NULL,
    [CreatedBy]                    VARCHAR (50) NULL,
    [CreatedOn]                    VARCHAR (50) NULL,
    [UpdatedBy]                    VARCHAR (50) NULL,
    [UpdatedOn]                    DATETIME     NULL,
    [DeletedBy]                    VARCHAR (50) NULL,
    [DeletedOn]                    DATETIME     NULL,
    [IsDeleted]                    BIT          NULL,
    [MonthName]                    VARCHAR (15) DEFAULT ((0)) NOT NULL,
    [year]                         INT          DEFAULT ((0)) NOT NULL,
    [GrossSalary]                  DECIMAL (18) DEFAULT ((0)) NOT NULL,
    [EmployeesProvidentFund]       DECIMAL (18) NULL,
    [ToxicologicalRiskAssessments] DECIMAL (18) NULL,
    [HouseRentAllowance]           DECIMAL (18) NULL,
    [PF]                           DECIMAL (18) NULL,
    [TaxDeductedAtSource]          DECIMAL (18) NULL,
    CONSTRAINT [PK_tblDeductionSalary] PRIMARY KEY CLUSTERED ([DeductionId] ASC)
);





