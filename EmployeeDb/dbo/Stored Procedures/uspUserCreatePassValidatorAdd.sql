
 
CREATE PROC [dbo].[uspUserCreatePassValidatorAdd]    
(    
	@Email [VARCHAR](200),    
	@EmailToken [VARCHAR](6)    
)    
AS    
BEGIN     
	SET NOCOUNT ON;    
    
	DECLARE @CreatedOn DATETIME = GETUTCDATE();        
    
	BEGIN TRY
	BEGIN
		IF NOT EXISTS(SELECT 1 FROM [dbo].[tblUsers] WITH(NOLOCK) WHERE Email = @Email AND Active = 1) 
		BEGIN
		SELECT 'False'
		RETURN
		END

		IF NOT EXISTS(SELECT 1 FROM  [dbo].[tblUserCreatePassValidator] WITH(NOLOCK) WHERE Email = @Email AND IsActive = 1)    
		BEGIN         
			INSERT INTO  [dbo].[tblUserCreatePassValidator]   
			(
			Email,
			EmailToken,
			CreatedOn
			)   
			VALUES    
			(
			@Email,
			@EmailToken,
			@CreatedOn
			)     
		END    
		ELSE     
		BEGIN        
			UPDATE [dbo].[tblUserCreatePassValidator]
			SET IsActive = 0  
			WHERE Email = @Email 
			
			INSERT INTO [dbo].[tblUserCreatePassValidator]    
			(
			Email,
			EmailToken,
			CreatedOn
			)    
			VALUES    
			(
			@Email,
			@EmailToken,
			@CreatedOn
			)
		END
	END
	END TRY   
	BEGIN CATCH  
		DECLARE @Msg VARCHAR(500) = ERROR_MESSAGE();  
  
		DECLARE @ErrorSeverity INT = ERROR_SEVERITY();  
		DECLARE @ErrorState INT = ERROR_STATE();    
		RAISERROR(@Msg, @ErrorSeverity, @ErrorState);  
	END CATCH
END