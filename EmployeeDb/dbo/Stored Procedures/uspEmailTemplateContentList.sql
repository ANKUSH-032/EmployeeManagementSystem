
CREATE PROC [dbo].[uspEmailTemplateContentList]
(
	@TemplateName VARCHAR(50)
)
AS
BEGIN
	SET NOCOUNT ON;

	BEGIN TRY     
	BEGIN
		SELECT [Content] FROM [dbo].[tblEmailTemplateMaster] WITH(NOLOCK) WHERE[TemplateName]= TRIM(@TemplateName)
	END   
	END TRY   
	BEGIN CATCH  
		DECLARE @Msg VARCHAR(500) = ERROR_MESSAGE();  
  
		DECLARE @ErrorSeverity INT = ERROR_SEVERITY();  
		DECLARE @ErrorState INT = ERROR_STATE();    
		RAISERROR(@Msg, @ErrorSeverity, @ErrorState);  
	END CATCH
END