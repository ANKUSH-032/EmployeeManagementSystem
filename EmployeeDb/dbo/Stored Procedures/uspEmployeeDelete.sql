

CREATE PROCEDURE [dbo].[uspEmployeeDelete]  
@EmployeeId VARCHAR(50),@DeletedBy  VARCHAR(50)
AS
BEGIN
SET NOCOUNT ON;

    DECLARE @Status BIT = 0;    
 DECLARE @Msg VARCHAR(500);  
 DECLARE @Data VARCHAR(50);  

         BEGIN TRY   
 BEGIN 

      IF NOT EXISTS (SELECT 1 FROM [dbo].[tblEmployee] WITH(NOLOCK) WHERE EmployeeId = @EmployeeId)         
 BEGIN          
 SELECT 0 AS [Status], 'No such record exists with this details.' AS [Message]   
 RETURN;     
 END  

      IF EXISTS (SELECT 1 FROM [dbo].[tblEmployee] WITH(NOLOCK) WHERE EmployeeId = @EmployeeId AND IsDeleted=1)          
BEGIN         
SELECT 0 AS [Status], 'Record is already deleted' AS [Message]  
RETURN;     
END  



      IF(ISNULL(@EmployeeId,'') <> '')        
 BEGIN        
 UPDATE [dbo].[tblEmployee]      
 SET             
 [IsDeleted] = 1,         
 [DeletedBy] = @DeletedBy,    
 [DeletedOn] = GETUTCDaTE()      
 WHERE EmployeeId = @EmployeeId

                SET @Status = 1;    
 SET @Msg = 'Record deleted successfully.';     
 SET @Data =@EmployeeId;     
 END       
 ELSE     
 BEGIN     
 SELECT 0 AS [Status], 'Please check the record details' AS [Message]  
 RETURN       
 END   
 END   
 END TRY  
 BEGIN CATCH 
 
 SET @Msg = ERROR_MESSAGE();   
 DECLARE @ErrorSeverity INT = ERROR_SEVERITY();  
 DECLARE @ErrorState INT = ERROR_STATE();        
 RAISERROR(@Msg, @ErrorSeverity, @ErrorState);    
 END CATCH        SELECT @Status [Status], @Msg [Message] , @Data [Data]  
END