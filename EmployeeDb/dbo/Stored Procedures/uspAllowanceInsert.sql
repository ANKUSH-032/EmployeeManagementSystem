
-- EXEC [dbo].[uspDeductionInsert] '510CD2C2-05AA-41F1-AA2D-2D3FBD591180',500,45.63,8000.36,8736.00,45,'',0

CREATE PROCEDURE [dbo].[uspAllowanceInsert]      
	 
	@EmployeeId VARCHAR(50), 
	@BasicSalary DECIMAL,
	@HouseAllowance DECIMAL,
	@ConveyanceAllowance DECIMAL,
	@SpecialAllowance DECIMAL, 
	@MonthName VARCHAR(50),
	@year int,
	@CreatedBy VARCHAR(50), 
	@IsDeleted BIT
	AS
BEGIN
SET NOCOUNT ON;

    DECLARE @Status BIT = 0;   
 DECLARE @Message VARCHAR(MAX);   
 DECLARE @AllowanceId VARCHAR(50)=NEWID(); 

 DECLARE @TotalAllowance DECIMAL(18,0) = SUM(@BasicSalary + @HouseAllowance + @ConveyanceAllowance + @SpecialAllowance);

    BEGIN TRY       
 BEGIN 

        IF EXISTS(SELECT 1 FROM [dbo].[tblAllowances] WITH(NOLOCK) WHERE AllowanceId = @AllowanceId)  
 BEGIN              
 SELECT 0 AS [Status], 'Record with same subject is already existed.' AS [Message]            
 RETURN    
 END      
			 IF NOT EXISTS( SELECT 1 FROM [dbo].[tblAllowances] WITH(NOLOCK) WHERE AllowanceId = @AllowanceId AND IsDeleted=1) 
			 BEGIN         
			 INSERT INTO   [dbo].[tblAllowances]    
			 (  
			    AllowanceId,
				EmployeeId , 
				BasicSalary ,
				HouseAllowance ,
				ConveyanceAllowance ,
				SpecialAllowance ,
				TotalAllowance,
				[MonthName],
				[year],
				CreatedBy , 
				CreatedOn,
				IsDeleted         
			 )        
			 VALUES   
			 ( 
			 @AllowanceId,
				@EmployeeId , 
				@BasicSalary ,
				@HouseAllowance ,
				@ConveyanceAllowance ,
				@SpecialAllowance ,
				@TotalAllowance,
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


    SELECT @Status [Status], @Message [Message] , @AllowanceId [Data]  
END