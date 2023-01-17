
CREATE PROC [dbo].[uspUserDetailsGet]                
(                
	@UserId VARCHAR(50) = NULL,                 
	@Email VARCHAR(150) = NULL                
)                
AS                
BEGIN                
	SET NOCOUNT ON;                
	DECLARE @Status BIT=0;                
	DECLARE @Msg VARCHAR(4000);
	               
	BEGIN TRY              
		IF @UserId IS NULL AND @Email IS NULL                
		BEGIN                
			SET @Status=0;                
			SET @Msg='UserId or Email is required';                 
		END                
		ELSE                 
		BEGIN              
			IF EXISTS(SELECT 1 FROM [dbo].[tblUsers] WITH(NOLOCK) WHERE (UserId= TRIM(@UserId) OR Email = TRIM(@Email)) AND IsDeleted = 0)              
			BEGIN              
				SELECT  
					U.[UserId],              
					[Email],              
					U.[PasswordHash],              
					U.[PasswordSalt],              
					ISNULL([FirstName],'') AS [Name],                           
					R.[RoleName] AS [Role]              
				FROM [dbo].[tblUsers] U WITH(NOLOCK)              
				INNER JOIN   [dbo].[tblMasterRole] R WITH(NOLOCK) ON U.[RoleId] = R.[RoleId]                       
				WHERE (U.UserId=@UserId OR U.Email = @Email) AND ISNULL(U.Active, 1) = 1              
				           
				SET @Status=1;              
				SET @Msg='Success';            
				SELECT @Status [Status], @Msg [Message]              
				RETURN              
			END 
			ELSE              
			BEGIN              
				IF NOT EXISTS (SELECT 1 FROM [dbo].[tblUsers] WITH(NOLOCK) WHERE (UserId= TRIM(@UserId) OR Email = TRIM(@Email)) AND IsDeleted = 0 )              
				SELECT 'UserNotRegister' AS [Name] 
				ELSE IF EXISTS (SELECT 1 FROM [dbo].[tblUsers] WITH(NOLOCK) WHERE (UserId= TRIM(@UserId) OR Email = TRIM(@Email)) AND IsDeleted = 1 )              
				SELECT 'DELETED' AS [Name]
				SET @Status=0;              
				SET @Msg='Failure';              
				SELECT @Status [Status], @Msg [Message]              
				RETURN              
			END                    
		END                 
	END TRY              
	BEGIN CATCH              
		SET @Status=0;              
		SET @Msg= ERROR_MESSAGE();                 
		              
		DECLARE @ErrorSeverity INT = ERROR_SEVERITY();                     
		DECLARE @ErrorState INT = ERROR_STATE();                     
		RAISERROR(@Msg,@ErrorSeverity,@ErrorState);               
		              
		SELECT @Status [Status], @Msg [Message]              
	END CATCH               
END              
                
                
/*                 
EXEC uspUserDetailsGet                
@UserId = '',                
@Email = 'rahul@osplabs.com'                
*/