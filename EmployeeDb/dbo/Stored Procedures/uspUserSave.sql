
CREATE PROCEDURE [dbo].[uspUserSave]
 @UserId VARCHAR(50)='', 
 @FirstName VARCHAR(50), 
 @MiddleName VARCHAR(50), 
 @LastName VARCHAR(50), 
 @ContactNo VARCHAR(15),
 @Email VARCHAR(150), 
 @RoleId VARCHAR(50),  
 @PasswordHash VARBINARY(MAX),  
 @PasswordSalt VARBINARY(MAX), 
 @IsEmail BIT=0,  
 @CreatedBy VARCHAR(50) = NULL,  
 @UpdatedBy VARCHAR(50) = NULL
AS
BEGIN

SET NOCOUNT ON;

	DECLARE @Status BIT = 0;  
	DECLARE @Message VARCHAR(MAX);    

	BEGIN TRY     
	BEGIN 

		IF (@UserId='')
		BEGIN
		IF EXISTS( SELECT 1 FROM [dbo].[tblUsers] WITH(NOLOCK) WHERE Email = @Email)
		BEGIN
		SELECT 0 AS [Status], 'Record with same email is already existed.' AS [Message]
		END
		ELSE
		BEGIN
			SET @UserId =NEWID()
			INSERT INTO [dbo].[tblUsers]
			(
			   UserId,
			   FirstName,
			   MiddleName,
			   LastName, 
			   Email, 
			   ContactNo,
			   PasswordHash,
			   PasswordSalt, 
			   RoleId, 
			   Active, 
			   IsEmail,
			   CreatedOn,
			   CreatedBy
			)

			VALUES
			(
			   @UserId,
			   @FirstName,
			   @MiddleName, 
			   @LastName, 
			   @Email, 
			   @ContactNo,
			   @PasswordHash,
			   @PasswordSalt,
			   @RoleId, 1, 
			   @IsEmail,
			   GETDATE(),
			   @CreatedBy
			)
				
			SET @Status = 1;  
			SET @Message = 'Record added successfully.'; 
			SELECT @Status [Status], @Message [Message] , @UserId [Data] 
		END
		END
		ElSE
		BEGIN
			UPDATE [dbo].[tblUsers]
			SET
			 FirstName = @FirstName,
			 MiddleName = @MiddleName,
			 LastName = @LastName,
			 Email = @Email,
			 ContactNo = @ContactNo,
			 PasswordHash = @PasswordHash,
			 PasswordSalt = @PasswordSalt,
			 RoleId = @RoleId, 
			 IsEmail = @IsEmail,
			 UpdatedBy = @UpdatedBy,
			 UpdatedOn = GETDATE()
		WHERE UserId = @UserId

		SET @Status = 1;  
		SET @Message = 'Record updated successfully.'; 
  
		SELECT @Status [Status], @Message [Message] , @UserId [Data]  

		END
	END   
	END TRY   
	BEGIN CATCH  
		SET @Message = ERROR_MESSAGE();  
  
		DECLARE @ErrorSeverity INT = ERROR_SEVERITY();  
		DECLARE @ErrorState INT = ERROR_STATE();    
		RAISERROR(@Message, @ErrorSeverity, @ErrorState);  
	END CATCH   
END