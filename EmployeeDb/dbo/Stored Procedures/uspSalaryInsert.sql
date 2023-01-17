
-- EXEC [dbo].[uspDeductionInsert] '510CD2C2-05AA-41F1-AA2D-2D3FBD591180',500,45.63,8000.36,8736.00,45,'',0

CREATE PROCEDURE [dbo].[uspSalaryInsert]      
	
	@EmployeeId VARCHAR(50), 
	@TotalAllowances DECIMAL, 
	@TotalDeduction DECIMAL,
	@CreatedBy VARCHAR(50), 
	@MonthName VARCHAR(50),
	@year INT,
	@IsDeleted BIT
	AS
BEGIN
SET NOCOUNT ON;

    DECLARE @Status BIT = 0;   
 DECLARE @Message VARCHAR(MAX);   
 DECLARE @SalaryId VARCHAR(50)=NEWID(); 

 DECLARE @NetSalary DECIMAL(18,0) = SUM(@TotalAllowances + @TotalDeduction );

    BEGIN TRY       
 BEGIN 

        IF EXISTS(SELECT 1 FROM [dbo].[tblEmployeeSalary] WITH(NOLOCK) WHERE SalaryId = @SalaryId)  
		BEGIN              
		SELECT 0 AS [Status], 'Record with same subject is already existed.' AS [Message]            
		RETURN    
		END      
			 IF NOT EXISTS( SELECT 1 FROM [dbo].[tblEmployeeSalary] WITH(NOLOCK) WHERE SalaryId = @SalaryId AND IsDeleted=1) 
			 BEGIN         
			 INSERT INTO   [dbo].[tblEmployeeSalary]    
			 (  
			    SalaryId,
				EmployeeId,
				TotalAllowances,
				TotalDeduction,
				NetSalary,
				[MonthName],
				[year],
				CreatedBy,
				CreatedOn, 
				IsDeleted       
			 )        
			 VALUES   
			 ( 
			    @SalaryId,
				@EmployeeId,
				@TotalAllowances,
				@TotalDeduction,
				@NetSalary,
				@MonthName,
				@year,
				@CreatedBy,
			   GETDATE(),
			   0
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


    SELECT @Status [Status], @Message [Message] , @SalaryId [Data]  
END