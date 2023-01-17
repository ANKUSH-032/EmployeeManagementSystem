
-- EXEC [dbo].[uspDeductionInsert] '510CD2C2-05AA-41F1-AA2D-2D3FBD591180',500,45.63,8000.36,8736.00,45,'',0

CREATE PROCEDURE [dbo].[uspDeductionInsert]      
@EmployeeId VARCHAR(50),
@GrossSalary DECIMAL,
@EPF DECIMAL(18,0),
@TRA DECIMAL(18,0),
@HRA DECIMAL(18,0), 
@ProfessionalTax DECIMAL(18,0),
@TDS DECIMAL(18,0), 
@MonthName VARCHAR(15),
@Year int,
@CreatedBy VARCHAR(50),
@IsDeleted BIT
AS
BEGIN
SET NOCOUNT ON;

    DECLARE @Status BIT = 0;   
 DECLARE @Message VARCHAR(MAX);   
 DECLARE @DeductionId VARCHAR(50)=NEWID(); 

 DECLARE @EmployeesProvidentFund DECIMAL =( @GrossSalary * @EPF)/100;
 DECLARE @ToxicologicalRiskAssessments DECIMAL =( @GrossSalary * @TRA)/100;
 DECLARE @HouseRentAllowance DECIMAL =( @GrossSalary * @HRA)/100;
 DECLARE @PF DECIMAL =( @GrossSalary * @ProfessionalTax)/100;
 DECLARE @TaxDeductedAtSource DECIMAL =( @GrossSalary * @TDS)/100;

 DECLARE @TotalDeduction DECIMAL(18,0) = SUM( @EmployeesProvidentFund + @ToxicologicalRiskAssessments + @HouseRentAllowance + @PF+ @TaxDeductedAtSource);

    BEGIN TRY       
 BEGIN 

        IF EXISTS(SELECT 1 FROM [dbo].[tblDeductionSalary] WITH(NOLOCK) WHERE DeductionId = @DeductionId)  
 BEGIN              
 SELECT 0 AS [Status], 'Record with same subject is already existed.' AS [Message]            
 RETURN    
 END      
			 IF NOT EXISTS( SELECT 1 FROM [dbo].[tblDeductionSalary] WITH(NOLOCK) WHERE DeductionId = @DeductionId AND IsDeleted=1) 
			 BEGIN         
			 INSERT INTO  [dbo].[tblDeductionSalary]        
			 (  
			 DeductionId, 
			 EmployeeId,
			 EPF,
			 TRA,
			 HRA, 
			 ProfessionalTax,
			 TDS, 
			 TotalDeduction,
			 GrossSalary,
			 [MonthName],
			 [Year],
			 CreatedBy, 
			 CreatedOn, 
			 IsDeleted ,
			 EmployeesProvidentFund,
			 ToxicologicalRiskAssessments,
			 HouseRentAllowance,
			 PF,
			 TaxDeductedAtSource
			 )        
			 VALUES   
			 ( 
			 @DeductionId,
			 @EmployeeId , 
			@EPF ,
			@TRA ,
			@HRA , 
			@ProfessionalTax ,
			@TDS ,
			@TotalDeduction ,
			@GrossSalary,
			@MonthName,
			@Year,
			@CreatedBy,
			GETDATE(),
			0,
			@EmployeesProvidentFund,
			@ToxicologicalRiskAssessments,
			@HouseRentAllowance,
			@PF,
			@TaxDeductedAtSource
			 )               
 SET @Status = 1;     
 SET @Message = 'Record added successfully.';   
 END       
 ElSE       
 BEGIN       
 SELECT 0 AS [Status], 'Record with same name is already existed.' AS [Message]   
 END  
 END  
 END TRY   
 BEGIN CATCH  
 SET @Message = ERROR_MESSAGE();    
 DECLARE @ErrorSeverity INT = ERROR_SEVERITY(); 
 DECLARE @ErrorState INT = Error_state();    
 RAISERROR(@Message, @ErrorSeverity, @ErrorState);  
 END CATCH  


    SELECT @Status [Status], @Message [Message] , @DeductionId [Data]  
END