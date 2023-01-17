
 
CREATE PROCEDURE [dbo].[uspUserUpdate]
 @UserId VARCHAR(50),
 @FirstName VARCHAR(50), 
 @MiddleName VARCHAR(50), 
 @LastName VARCHAR(50), 
 @ContactNo VARCHAR(15),
 @Email VARCHAR(150), 
 @RoleId VARCHAR(50),    
 @UpdatedBy VARCHAR(50)
AS
BEGIN 
SET NOCOUNT ON;

	DECLARE @Status BIT = 0;  
	DECLARE @Message VARCHAR(MAX);  
	
	BEGIN TRY     
	BEGIN 
		
		IF(ISNULL(@UserId,'')= '') 
		BEGIN
			SELECT 0 AS [Status], 'Please provide proper userId.' AS [Message]
			RETURN
		END

		IF EXISTS(SELECT 1 FROM [dbo].[tblUsers] WITH(NOLOCK) WHERE UserId = @UserId AND IsDeleted = 1)
		BEGIN
				SELECT 0 AS [Status], 'Record with this details is deleted.' AS [Message]
				RETURN
		END 

		IF EXISTS(SELECT 1 FROM [dbo].[tblUsers] WITH(NOLOCK) WHERE Email = @Email AND UserId<>@UserId AND IsDeleted = 0)
		BEGIN
				SELECT 0 AS [Status], 'Record with same email id is already existed.' AS [Message]
				RETURN
		END

		IF NOT EXISTS(SELECT 1 FROM [dbo].[tblUsers] WITH(NOLOCK) WHERE UserId = @UserId AND IsDeleted = 0)
		BEGIN
				SELECT 0 AS [Status], 'Record not present.' AS [Message]
				RETURN
		END

		UPDATE [dbo].[tblUsers]
			SET
			 FirstName = @FirstName,
			 MiddleName = @MiddleName,
			 LastName = @LastName,
			 Email = @Email,
			 ContactNo = @ContactNo, 
			 RoleId = @RoleId,  
			 UpdatedBy = @UpdatedBy,
			 UpdatedOn = GETDATE()
  			
		WHERE UserId = @UserId

		SET @Status = 1;  
		SET @Message = 'Record updated successfully.'; 
  
		SELECT @Status [Status], @Message [Message] , @UserId [Data]  

	END   
	END TRY   
	BEGIN CATCH  
		SET @Message = ERROR_MESSAGE();  
  
		DECLARE @ErrorSeverity INT = ERROR_SEVERITY();  
		DECLARE @ErrorState INT = ERROR_STATE();    
		RAISERROR(@Message, @ErrorSeverity, @ErrorState);  
	END CATCH  
END