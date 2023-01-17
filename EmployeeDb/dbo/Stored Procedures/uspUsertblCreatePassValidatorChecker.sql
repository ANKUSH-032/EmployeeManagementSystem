
CREATE PROC [dbo].[uspUsertblCreatePassValidatorChecker]
(  
	@Email [VARCHAR](200),  
	@EmailToken [VARCHAR](6)  
)  
AS
BEGIN
	SET NOCOUNT ON;
	
	BEGIN TRY     
	BEGIN
		IF EXISTS(SELECT 1 FROM [dbo].[tblUserCreatePassValidator] WITH(NOLOCK) WHERE Email = @Email AND EmailToken = @EmailToken COLLATE SQL_Latin1_General_CP1_CS_AS)  
		BEGIN
			IF EXISTS(SELECT 1 FROM [dbo].[tblUserCreatePassValidator] WITH(NOLOCK) WHERE Email = @Email AND EmailToken = @EmailToken COLLATE SQL_Latin1_General_CP1_CS_AS AND IsActive = 1)  
			BEGIN
				UPDATE [dbo].[tblUserCreatePassValidator] 
				SET IsActive = 0  
				WHERE Email = @Email 

				SELECT 'valid'
			END	 
			ELSE
			BEGIN
				SELECT 'tokenexpired'
			END
		END
		ELSE
		BEGIN
			SELECT 'invaliddetails'
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