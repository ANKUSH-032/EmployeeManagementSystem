

CREATE PROCEDURE [dbo].[uspAllowanceGetDetails]
@AllowanceId VARCHAR(50)
AS
BEGIN
SET NOCOUNT ON;

    DECLARE @Status BIT = 0; 
 DECLARE @Msg VARCHAR(500);     
 BEGIN TRY      
 BEGIN 

      IF NOT EXISTS (SELECT 1 FROM [dbo].[tblAllowances] WITH(NOLOCK) WHERE AllowanceId = @AllowanceId)
 BEGIN       
 SELECT 0 AS [Status], 'No such record exists with this details.' AS [Message]   
 RETURN;   
 END  

      IF EXISTS (SELECT 1 FROM  [dbo].[tblAllowances] WITH(NOLOCK) WHERE AllowanceId = @AllowanceId AND IsDeleted=1)   
BEGIN     
SELECT 0 AS [Status], 'Record is already deleted' AS [Message]    
RETURN;   
END  


      IF(ISNULL(@AllowanceId,'') <> '')     
		 BEGIN                
		 SELECT [Status] = 1, [Message] = 'Data Fetched Sucessfully', [Data] = NULL         
		 SELECT               
		 AllowanceId, 
		EmployeeId,
		BasicSalary,
		HouseAllowance,
		ConveyanceAllowance, 
		SpecialAllowance, 
		TotalAllowance AS GrossSalary, 
		[MonthName],
		[year],
		CreatedBy,
		CreatedOn,
		DeletedBy, 
		DeletedOn, 
		UpdatedBy, 
		UpdatedOn, 
		IsDeleted
		 FROM [dbo].[tblAllowances] WITH(NOLOCK)
		 WHERE AllowanceId = @AllowanceId     
		 AND IsDeleted=0

             
		RETURN;       
		END         
		ELSE          
		BEGIN         
		SELECT 0 AS [Status], 'Please check the record details' AS [Message]  
		RETURN;  
		END      
 END      
 END TRY  
 BEGIN CATCH      
 SET @Msg = ERROR_MESSAGE();   
 DECLARE @ErrorSeverity INT = ERROR_SEVERITY();   
 DECLARE @ErrorState INT = ERROR_STATE();         
 RAISERROR(@Msg, @ErrorSeverity, @ErrorState);    
 END CATCH  
END