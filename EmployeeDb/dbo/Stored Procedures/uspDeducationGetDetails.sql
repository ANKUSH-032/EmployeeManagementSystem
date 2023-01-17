

CREATE PROCEDURE [dbo].[uspDeducationGetDetails]
@DeductionId VARCHAR(50)
AS
BEGIN
SET NOCOUNT ON;

    DECLARE @Status BIT = 0; 
 DECLARE @Msg VARCHAR(500);     
 BEGIN TRY      
 BEGIN 

      IF NOT EXISTS (SELECT 1 FROM [dbo].[tblDeductionSalary] WITH(NOLOCK) WHERE DeductionId = @DeductionId)
 BEGIN       
 SELECT 0 AS [Status], 'No such record exists with this details.' AS [Message]   
 RETURN;   
 END  

      IF EXISTS (SELECT 1 FROM [dbo].[tblDeductionSalary] WITH(NOLOCK) WHERE DeductionId = @DeductionId AND IsDeleted=1)   
BEGIN     
SELECT 0 AS [Status], 'Record is already deleted' AS [Message]    
RETURN;   
END  


      IF(ISNULL(@DeductionId,'') <> '')     
		 BEGIN                
		 SELECT [Status] = 1, [Message] = 'Data Fetched Sucessfully', [Data] = NULL         
		 SELECT               
		DeductionId, EmployeeId, EPF, TRA, HRA, ProfessionalTax, TDS, TotalDeduction, CreatedBy, CreatedOn, UpdatedBy, UpdatedOn, DeletedBy, DeletedOn, IsDeleted, MonthName, year, GrossSalary
		 FROM  [dbo].[tblDeductionSalary] WITH(NOLOCK)
		 WHERE DeductionId = @DeductionId      
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