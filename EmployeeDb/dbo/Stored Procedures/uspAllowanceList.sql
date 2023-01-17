
CREATE PROCEDURE [dbo].[uspAllowanceList]   
 @Start INT = 0,  
 @PageSize INT=5,  
 @SortCol VARCHAR(100) = NULL,  
 @SearchKey VARCHAR(MAX) = '', 
 @EmployeeId VARCHAR(50)
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
    a.AllowanceId,
	e.EmployeeId,
	a.BasicSalary, 
	a.HouseAllowance, 
	a.ConveyanceAllowance, 
	a.SpecialAllowance, 
	a.TotalAllowance AS GrossSalary,
	[MonthName],[year],
	a.IsDeleted,
	Concat(e.[FirstName],' ', LTRIM(RTRIM(CONCAT(e.[MiddleName], ' ', e.[LastName])))) as EmployeeName
   FROM  [dbo].[tblAllowances] a WITH(NOLOCK)  
   LEFT JOIN [dbo].[tblEmployee] E WITH(NOLOCK) ON E.EmployeeId = a.EmployeeId
   WHERE e.EmployeeId =@EmployeeId and e.IsDeleted = 0  
   AND (FirstName LIKE CONCAT ('%',@SearchKey,'%')OR LastName LIKE CONCAT ('%',@SearchKey,'%'))   
  
   ORDER BY CASE  
   WHEN @SortCol = 'FirstName_asc'  THEN FirstName  END ASC  
   ,---it will performing sortion based upon condition  
   CASE  
   WHEN @SortCol = 'LastName_desc'  THEN FirstName  END DESC

   ,CASE   
   WHEN @SortCol NOT IN ('FirstName_asc','LastName_desc') THEN e.CreatedOn  
   END DESC OFFSET @Start ROWS --- it will limit the no of pages  
  
  
  
   FETCH NEXT (  
   CASE  
   WHEN @PageSize = - 1  
   THEN (SELECT COUNT(1) FROM  [dbo].[tblAllowances] WITH(NOLOCK)  )  
   ELSE @PageSize  
   END  
   ) ROWS ONLY --- it will the next row  
  
  
   -- TOTAL ROW COUNT  
   DECLARE @recordsTotal INT = (  
   SELECT COUNT(DISTINCT AllowanceId) FROM  [dbo].[tblAllowances]  WITH(NOLOCK)    
   WHERE  IsDeleted = 0  
      
     
   ) --- it will fetch total row count from table  
  
 SELECT @recordsTotal AS totalRecords  
  
 END     
 END TRY     
 BEGIN CATCH    
  DECLARE @Msg NVARCHAR(100) =ERROR_MESSAGE() ;    
    
  DECLARE @ErrorSeverity INT = ERROR_SEVERITY();    
  DECLARE @ErrorState INT = ERROR_STATE();      
  RAISERROR(@Msg, @ErrorSeverity, @ErrorState);    
 END CATCH    
END