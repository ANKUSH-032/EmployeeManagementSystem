-- exec [dbo].[uspDeductionList]  0,5,'','','510CD2C2-05AA-41F1-AA2D-2D3FBD591180'
CREATE PROCEDURE [dbo].[uspDeductionList]   
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
    ds.DeductionId,
	ds.EmployeeId,
	ds.EPF,
	ds.TRA, 
	ds.HRA,
	ds.ProfessionalTax, 
	ds.TDS,
	ds.TotalDeduction, 
	ds.GrossSalary,
	ds.[MonthName],
	ds.[year],


	ds.IsDeleted,
	Concat(e.[FirstName],' ', LTRIM(RTRIM(CONCAT(e.[MiddleName], ' ', e.[LastName])))) as EmployeeName
   FROM [dbo].[tblDeductionSalary] ds WITH(NOLOCK)  
   LEFT JOIN [dbo].[tblEmployee] E WITH(NOLOCK) ON E.EmployeeId = ds.EmployeeId
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
   THEN (SELECT COUNT(1) FROM [dbo].[tblDeductionSalary] WITH(NOLOCK)  )  
   ELSE @PageSize  
   END  
   ) ROWS ONLY --- it will the next row  
  
  
   -- TOTAL ROW COUNT  
   DECLARE @recordsTotal INT = (  
   SELECT COUNT(DISTINCT DeductionId) FROM [dbo].[tblDeductionSalary]  WITH(NOLOCK)    
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