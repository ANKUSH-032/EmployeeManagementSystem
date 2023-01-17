CREATE PROCEDURE [dbo].[uspEmployeeList]   
 @Start INT = 0,  
 @PageSize INT=5,  
 @SortCol VARCHAR(100) = NULL,  
 @SearchKey VARCHAR(MAX) = '' 
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
     EmployeeId,
	 FirstName,
	 MiddleName,
	 LastName, 
	 DayofBirth, 
	 EmailId, 
	 [District], 
	 [State],
	 PinCode,
	 [Address],
	 Qualification,
	 CurrentExperience,
	 JoinDate, 
	 [DesignationName], 
	 Gender, 
	 MaritalStatus,
	 CompanyName, 
	 CompanyAddress,
	 FatherName,
	 FatherOccupation,
	 MontherName,
	 MontherOcupation, 
	 IsDeleted,
	 CreatedBy,
	 CreatedOn,
	 UpdatedBy,
	 UpdatedOn,
	 DeletedOn,
	 DeletedBy,
	 PhoneNumber,
	 HomePhoneNumber
   FROM [dbo].[tblEmployee]  WITH(NOLOCK)    
   WHERE [IsDeleted] = 0  
   AND (FirstName LIKE CONCAT ('%',@SearchKey,'%')OR LastName LIKE CONCAT ('%',@SearchKey,'%'))   
  
   ORDER BY CASE  
   WHEN @SortCol = 'FirstName_asc'  THEN FirstName  END ASC  
   ,---it will performing sortion based upon condition  
   CASE  
   WHEN @SortCol = 'LastName_desc'  THEN FirstName  END DESC

   ,CASE   
   WHEN @SortCol NOT IN ('FirstName_asc','LastName_desc') THEN CreatedOn  
   END DESC OFFSET @Start ROWS --- it will limit the no of pages  
  
  
  
   FETCH NEXT (  
   CASE  
   WHEN @PageSize = - 1  
   THEN (SELECT COUNT(1) FROM [dbo].[tblEmployee] WITH(NOLOCK)  )  
   ELSE @PageSize  
   END  
   ) ROWS ONLY --- it will the next row  
  
  
   -- TOTAL ROW COUNT  
   DECLARE @recordsTotal INT = (  
   SELECT COUNT(DISTINCT EmployeeId) FROM[dbo].[tblEmployee] WITH(NOLOCK)    
   WHERE  IsDeleted = 0  
   AND (  
  FirstName LIKE CONCAT ('%',@SearchKey,'%')OR LastName LIKE CONCAT ('%',@SearchKey,'%')  
   )    
     
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