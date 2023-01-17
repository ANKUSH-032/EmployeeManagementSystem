
CREATE PROC [dbo].[uspCheckValidEmail]
(
	@Email VARCHAR(150)
)
AS
BEGIN
	SET NOCOUNT ON

	--OPEN SYMMETRIC KEY EncryptionKey DECRYPTION     
	--BY CERTIFICATE EncryptionCertificate

	BEGIN TRY     
	BEGIN
		IF EXISTS(SELECT 1 FROM [dbo].[tblUsers] WITH(NOLOCK) WHERE Email = @Email AND Active = 1)
		BEGIN
			SELECT [FirstName]
			FROM 
			[dbo].[tblUsers] WITH(NOLOCK) WHERE Email = @Email AND Active = 1
		END
		ELSE
		BEGIN
			SELECT 'NVU'
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

--- exec [dbo].[uspCheckValidEmail] 'rahul@osplabs.com'