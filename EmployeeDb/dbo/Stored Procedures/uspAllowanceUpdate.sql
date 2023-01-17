



CREATE PROCEDURE [dbo].[uspAllowanceUpdate] 
@EmployeeId VARCHAR(50), 
	@BasicSalary DECIMAL,
	@HouseAllowance DECIMAL,
	@ConveyanceAllowance DECIMAL,
	@SpecialAllowance DECIMAL, 
	@MonthName VARCHAR(50),
	@year int,
	@UpdatedBy VARCHAR(50), 
	@IsDeleted BIT,
	@AllowanceId VARCHAR(50)
AS
BEGIN
SET NOCOUNT ON;

    DECLARE @Status BIT = 0;     


 DECLARE @Message VARCHAR(MAX);   
  DECLARE @TotalAllowance DECIMAL(18,0) = SUM(@BasicSalary + @HouseAllowance + @ConveyanceAllowance + @SpecialAllowance);
 BEGIN TRY    
 BEGIN         
 IF(ISNULL(@AllowanceId,'')= '')      
 BEGIN           
 SELECT 0 AS [Status], 'Please provide proper EmployeeId.' AS [Message]   
 RETURN     
 END

        IF EXISTS(SELECT 1 FROM [dbo].[tblAllowances] WITH(NOLOCK) WHERE AllowanceId = @AllowanceId AND IsDeleted = 1) 
 BEGIN        
 SELECT 0 AS [Status], 'Record with this details is deleted.' AS [Message]         
 RETURN   
 END

        IF NOT EXISTS(SELECT 1 FROM [dbo].[tblAllowances] WITH(NOLOCK) WHERE AllowanceId = @AllowanceId AND IsDeleted = 0)    
 BEGIN            
 SELECT 0 AS [Status], 'Record not present.' AS [Message]     
 RETURN       
 END

        UPDATE [dbo].[tblAllowances]
 SET    
	EmployeeId=@EmployeeId,
	BasicSalary=@BasicSalary ,
	HouseAllowance=@HouseAllowance ,
	ConveyanceAllowance=@ConveyanceAllowance ,
	SpecialAllowance=@SpecialAllowance, 
	AllowanceId=@AllowanceId ,
	TotalAllowance=@TotalAllowance,
	[MonthName]=@MonthName,
	[year]=@year ,
	UpdatedBy =@UpdatedBy,
	UpdatedOn = GETDATE(),
	IsDeleted = 0
  WHERE AllowanceId = @AllowanceId 

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