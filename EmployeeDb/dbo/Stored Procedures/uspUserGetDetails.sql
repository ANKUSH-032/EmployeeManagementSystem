
CREATE PROCEDURE [dbo].[uspUserGetDetails]
@UserId VARCHAR(50)
AS 
BEGIN
	SET NOCOUNT ON;

	DECLARE @Status BIT = 0;  
	DECLARE @Msg VARCHAR(500);  
 	 
	BEGIN TRY     
	BEGIN 

	  IF NOT EXISTS (SELECT 1 FROM [dbo].[tblUsers] WITH(NOLOCK) WHERE UserId = @UserId AND IsDeleted = 0)        
	  BEGIN  
		SELECT 0 AS [Status], 'No such record exists with this details.' AS [Message]  
		RETURN;  
	  END  


	  IF EXISTS (SELECT 1 FROM [dbo].[tblUsers] WITH(NOLOCK) WHERE UserId = @UserId AND IsDeleted=1)        
	  BEGIN  
		SELECT 0 AS [Status], 'Record is already deleted' AS [Message]  
		RETURN;  
	  END  



	  IF(ISNULL(@UserId,'') <> '') 
		 BEGIN  
				SET @Status = 1;  
				SET @Msg = 'Record get successfully.'; 

				SELECT  @Status [Status] ,@Msg [Message]
				
				SELECT 
				 UserId,
				 FirstName,
				 MiddleName,
				 LastName,
				 Email,
				 RoleId,
				 ContactNo,
				 PasswordHash,
				 PasswordSalt
				
				FROM [dbo].[tblUsers] WITH(NOLOCK)  
				WHERE UserId = @UserId
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