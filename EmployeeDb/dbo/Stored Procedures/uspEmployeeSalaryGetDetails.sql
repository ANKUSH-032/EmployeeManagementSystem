
-- exec [dbo].[uspEmployeeSalaryGetDetails] '1DF10166-603B-4F95-99F4-8CEB39EFFD4B'
CREATE PROCEDURE [dbo].[uspEmployeeSalaryGetDetails]
@EmployeeId VARCHAR(50),
@MonthName VARCHAR(50),
@year int
AS
BEGIN
SET NOCOUNT ON;

    DECLARE @Status BIT = 0; 
    DECLARE @Msg VARCHAR(500);     
 BEGIN TRY      
 BEGIN 

      IF NOT EXISTS (SELECT 1 FROM [dbo].[tblEmployee] WITH(NOLOCK) WHERE EmployeeId = @EmployeeId)
 BEGIN       
 SELECT 0 AS [Status], 'No such record exists with this details.' AS [Message]   
 RETURN;   
 END  

  IF NOT EXISTS (SELECT 1 FROM [dbo].[tblEmployeeSalary] WITH(NOLOCK) WHERE EmployeeId = @EmployeeId AND [MonthName]= @MonthName AND [year] = @year)
 BEGIN       
 SELECT 0 AS [Status], 'No such record exists with this details.' AS [Message]   
 RETURN;   
 END  

      IF EXISTS (SELECT 1 FROM  [dbo].[tblEmployee] WITH(NOLOCK) WHERE EmployeeId = @EmployeeId AND IsDeleted=1)   
BEGIN     
SELECT 0 AS [Status], 'Record is already deleted' AS [Message]    
RETURN;   
END  


      IF(ISNULL(@EmployeeId,'') <> '')     
		 BEGIN                
		 SELECT [Status] = 1, [Message] = 'Data Fetched Sucessfully', [Data] = NULL         
		 SELECT  
		CompanyName,
		e.EmployeeId,
		Concat(e.[FirstName],' ', LTRIM(RTRIM(CONCAT(e.[MiddleName], ' ', e.[LastName])))) as [Name],
		DayofBirth,
		EmailId, 
		--mp.District, 
		--ms.[State],
		--e.PinCode, 
		--[Address],
		Concat(e.[Address],' ', LTRIM(RTRIM(CONCAT(mp.District, ' ', e.PinCode, ' ',ms.[State])))) as [FullAddress],
		--mq.[Qualification],
		te.DesignationName,
		Gender,
		
		CompanyAddress, 
		FatherName, 
		PhoneNumber,
		--HomePhoneNumber,
		a.BasicSalary, 
		a.HouseAllowance,
		a.ConveyanceAllowance,
		a.SpecialAllowance,
		a.TotalAllowance,
		d.EmployeesProvidentFund,
		d.ToxicologicalRiskAssessments,
	    d.HouseRentAllowance,
		d.PF,
		d.TaxDeductedAtSource,
		d.TotalDeduction,
		es.[NetSalary],
		es.[MonthName],
		es.[Year]
		 FROM [dbo].[tblEmployee] e WITH(NOLOCK)
		 LEFT JOIN [dbo].[tblAllowances] a ON  a.EmployeeId = e.EmployeeId
		 LEFT JOIN [dbo].[tblDeductionSalary] d ON D.EmployeeId = e.EmployeeId
		 LEFT JOIN [dbo].[tblMasterState] ms ON ms.StateId = e.[state]
		 LEFT JOIN [dbo].[tblMasterPincode] mp ON mp.Pincode = e.District 
		 LEFT JOIN [dbo].[tblMasterTypeOfEmployee] te ON te.[TypeOfEmployeeId] = e.[DesignationName]
		 LEFT JOIN [dbo].[tblEmployeeSalary] es ON es.EmployeeId = e.EmployeeId
		 --LEFT JOIN [dbo].[tblMasterQualification] mq ON mq.[QualificationId] = e.[Qualification]
		 WHERE e.EmployeeId = @EmployeeId 
		 AND  es.[MonthName] = @MonthName
		 AND es.[year] = @year
		 AND e.IsDeleted=0

             
		RETURN;       
		END         
		ELSE          
		BEGIN         
		SELECT 0 AS [Status], 'Please check the record details' AS [Message]  
		RETURN;  
		END      
 END      
 END TRY  
 BEGIN CATCH      
 SET @Msg = ERROR_MESSAGE();   
 DECLARE @ErrorSeverity INT = ERROR_SEVERITY();   
 DECLARE @ErrorState INT = ERROR_STATE();         
 RAISERROR(@Msg, @ErrorSeverity, @ErrorState);    
 END CATCH  
END