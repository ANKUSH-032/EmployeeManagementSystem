
--   [uspUserGetList] 0,1,'',''
CREATE PROCEDURE [dbo].[uspUserGetList] (  
@Start INT = 0,  
@PageSize INT=-1,  
@SortCol VARCHAR(100) = NULL,  
@SearchKey VARCHAR(max) = ''  
)  
AS  
BEGIN  
SET NOCOUNT ON;  
BEGIN TRY  
BEGIN  
SET NOCOUNT ON;  
SET @SortCol = TRIM(ISNULL(@SortCol, ''));  
SET @SearchKey = TRIM(ISNULL(@SearchKey, ''));  
IF ISNULL(@Start, 0) = 0  
SET @Start = 0  
IF ISNULL(@PageSize, 0) <= 0  
SET @PageSize = - 1  
  
  
  
SELECT 1 AS [Status],'Data Fetched Successfully' AS [Message]  
  
  
  
   SELECT  
    UserId,  
    FirstName,   
    MiddleName,   
    LastName,   
    Email,  
    ContactNo AS ContactNo,   
    PasswordHash,   
    PasswordSalt,   
    u.RoleId,   
    Active,   
    IsEmail,  
    CreatedOn,   
    CreatedBy,  
    UpdatedOn,   
    UpdatedBy,   
    DeletedOn,   
    DeletedBy,   
    IsDeleted,  
    r.RoleName,  
      
    ContactNo  
   FROM [dbo].[tblUsers] u WITH(NOLOCK)    
   LEFT JOIN  [dbo].[tblMasterRole] r WITH(NOLOCK) ON u.RoleId = r.RoleId    
   WHERE u.[IsDeleted] = 0  
            AND (u.FirstName LIKE CONCAT ('%',@SearchKey,'%') OR   
   u.LastName LIKE CONCAT ('%',@SearchKey,'%'))  
   ORDER BY  
   CASE  
   WHEN @SortCol = 'firstName_asc' THEN FirstName END ASC  
   ,---it will performing sortion based upon condition  
   CASE  
   WHEN @SortCol = 'firstName_desc' THEN FirstName END DESC,  
  
   CASE  
   WHEN @SortCol = 'lastName_asc' THEN LastName END ASC  
   ,---it will performing sortion based upon condition  
   CASE  
   WHEN @SortCol = 'lastName_desc'THEN LastName END DESC,  
  
   CASE  
   WHEN @SortCol = 'roleName_asc' THEN r.RoleName END ASC  
   ,---it will performing sortion based upon condition  
   CASE  
   WHEN @SortCol = 'roleName_desc' THEN r.RoleName END DESC,  
  
   CASE  
   WHEN @SortCol = 'contactNo_asc' THEN ContactNo END ASC  
   ,---it will performing sortion based upon condition  
   CASE  
   WHEN @SortCol = 'contactNo_desc' THEN ContactNo END DESC,  
  
   CASE  
   WHEN @SortCol NOT IN   
   ('firstName_asc','firstName_desc','lastName_asc','lastName_desc','roleName_asc','roleName_desc','contactNo_asc','contactNo_desc') THEN [CreatedOn]  
   END DESC OFFSET @Start ROWS --- it will limit the no of pages  
  
  
  
FETCH NEXT (  
   CASE  
   WHEN @PageSize = - 1  
   THEN (SELECT COUNT(1) FROM  [dbo].[tblUsers] WITH(NOLOCK)  )  
   ELSE @PageSize  
   END  
   ) ROWS ONLY --- it will the next row   
  
  
  
-- TOTAL ROW COUNT  
   DECLARE @recordsTotal Int = (  
   SELECT COUNT(DISTINCT [userid]) FROM [dbo].[tblUsers] WITH(NOLOCK)  
   WHERE [IsDeleted] = 0  
   AND (  
   [FirstName] LIKE CONCAT ('%',@SearchKey,'%')OR  
   [LastName] LIKE CONCAT ('%',@SearchKey,'%') OR    
   [Email] LIKE CONCAT ('%',@SearchKey,'%') OR  
   [ContactNo] LIKE CONCAT ('%',@SearchKey,'%')  
      )  
   ) --- it will fetch total row count from table  
  
  
  
SELECT @recordsTotal AS totalRecords  
  
  
  
END  
END TRY  
BEGIN CATCH  
DECLARE @Msg nvarchar(100) =ERROR_MESSAGE() ;  
  
DECLARE @ErrorSeverity INT = ERROR_SEVERITY();  
DECLARE @ErrorState INT = ERROR_STATE();  
RAISERROR(@Msg, @ErrorSeverity, @ErrorState);  
END CATCH  
  
END