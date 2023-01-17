



CREATE PROCEDURE [dbo].[uspDeductionUpdate] 
@EmployeeId VARCHAR(50), 
@EPF DECIMAL(18,0),
@TRA DECIMAL(18,0),
@HRA DECIMAL(18,0), 
@GrossSalary decimal,
@ProfessionalTax DECIMAL(18,0),
@TDS DECIMAL(18,0),
@MonthName VARCHAR(15),
@year int,
@UpdatedBy VARCHAR(50),
@IsDeleted BIT,
@DeductionId VARCHAR(50)
AS
BEGIN
SET NOCOUNT ON;

    DECLARE @Status BIT = 0;     


 DECLARE @Message VARCHAR(MAX);   
 DECLARE @EmployeesProvidentFund DECIMAL =( @GrossSalary * @EPF)/100;
 DECLARE @ToxicologicalRiskAssessments DECIMAL =( @GrossSalary * @TRA)/100;
 DECLARE @HouseRentAllowance DECIMAL =( @GrossSalary * @HRA)/100;
 DECLARE @PF DECIMAL =( @GrossSalary * @ProfessionalTax)/100;
 DECLARE @TaxDeductedAtSource DECIMAL =( @GrossSalary * @TDS)/100;

 DECLARE @TotalDeduction DECIMAL(18,0) = SUM( @EmployeesProvidentFund + @ToxicologicalRiskAssessments + @HouseRentAllowance + @PF+ @TaxDeductedAtSource);

 BEGIN TRY    
 BEGIN         
 IF(ISNULL(@DeductionId,'')= '')      
 BEGIN           
 SELECT 0 AS [Status], 'Please provide proper Eventid.' AS [Message]   
 RETURN     
 END

        IF EXISTS(SELECT 1 FROM [dbo].[tblDeductionSalary] WITH(NOLOCK) WHERE DeductionId = @DeductionId AND IsDeleted = 1) 
 BEGIN        
 SELECT 0 AS [Status], 'Record with this details is deleted.' AS [Message]         
 RETURN   
 END

        IF NOT EXISTS(SELECT 1 FROM [dbo].[tblDeductionSalary] WITH(NOLOCK) WHERE DeductionId = @DeductionId AND IsDeleted = 0)    
 BEGIN            
 SELECT 0 AS [Status], 'Record not present.' AS [Message]     
 RETURN       
 END

        UPDATE [dbo].[tblDeductionSalary]
 SET    
	EmployeeId =@EmployeeId ,
	EPF=@EPF,
	TRA =@TRA ,
	HRA =@HRA ,
	ProfessionalTax=@ProfessionalTax ,
	TDS =@TDS ,
	GrossSalary=@GrossSalary,
	TotalDeduction=@TotalDeduction,
	[MonthName] = @MonthName,
	[year] = @year,
	UpdatedBy =@UpdatedBy,
	UpdatedOn = GETDATE(),
	IsDeleted = 0
  WHERE DeductionId = @DeductionId

        SET @Status = 1;          
 SET @Message = 'Record updated successfully.';   
 SELECT @Status [Status], @Message [Message] , @EmployeeId [Data]  

    END       
 END TRY     
 BEGIN CATCH  
 SET @Message = ERROR_MESSAGE();  
 DECLARE @ErrorSeverity INT = ERROR_SEVERITY();   
 DECLARE @ErrorState INT = ERROR_STATE();   
 RAISERROR(@Message, @ErrorSeverity, @ErrorState);  
 END CATCH  
END