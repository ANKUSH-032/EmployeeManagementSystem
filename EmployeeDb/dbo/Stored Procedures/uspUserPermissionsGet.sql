
CREATE PROCEDURE [dbo].[uspUserPermissionsGet]
@UserId VARCHAR(50)
AS
BEGIN
	 SELECT 1 AS [Status],'Data Fetched Successfully' AS [Message]

	 SELECT *, tu.FirstName + ' ' + tu.LastName AS UserName
	 FROM [dbo].[tblModuleMapping] m WITH(NOLOCK)  
	 JOIN [dbo].[tblModule] tm WITH(NOLOCK)  
	 ON m.ModuleId =tm.ModuleId 
	 JOIN [dbo].[tblUsers] tu WITH(NOLOCK)   
	 ON tu.UserId = m.UserId
	 WHERE m.UserId=@UserId 

	 SELECT 1 AS totalRecords
END