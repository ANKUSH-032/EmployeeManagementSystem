
CREATE PROCEDURE [dbo].[uspUserPermissionsSave] 
@MappingId BIGINT = 0, 
@UserId VARCHAR(50),
@ModuleId BIGINT,
@IsView BIT, 
@IsAdd BIT, 
@IsEdit BIT, 
@IsDelete BIT, 
@UpdatedBy VARCHAR(50)
AS
BEGIN
SET NOCOUNT ON;

	DECLARE @Status BIT = 0;  
	DECLARE @Message VARCHAR(MAX);    

	BEGIN TRY     
	BEGIN 

		IF (@MappingId=0)
		BEGIN 
			INSERT INTO [dbo].[tblModuleMapping]
			(
			 UserId, 
			 ModuleId, 
			 IsView,
			 IsAdd,
			 IsEdit, 
			 IsDeleted,
			 CreatedBy, 
			 CreatedOn
			)
			VALUES
			(
			 @UserId,
			 @ModuleId,
			 @IsView,
			 @IsAdd, 
			 @IsEdit,
			 @IsDelete,
			 @UpdatedBy,
			 GETUTCDATE()
			)
				
			SET @Status = 1;  
			SET @Message = 'Record added successfully.'; 
			SELECT @Status [Status],
			@Message [Message] , 
			@UserId [Data] 
		END
		ElSE
		BEGIN
			UPDATE [dbo].[tblModuleMapping]
			SET
			 UserId = @UserId,
			 ModuleId = @ModuleId, 
			 IsView = @IsView, 
			 IsAdd = @IsAdd, 
			 IsEdit = @IsEdit, 
			 IsDeleted= @IsDelete, 
			 UpdatedBy = @UpdatedBy,
			 UpdatedOn = GETUTCDATE()
		WHERE MappingId = @MappingId

		SET @Status = 1;  
		SET @Message = 'Record updated successfully.'; 
  
		SELECT @Status [Status],
		@Message [Message] ,
		@UserId [Data]  

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