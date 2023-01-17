

CREATE PROCEDURE [dbo].[uspEmployeeGetDetails]
@EmployeeId VARCHAR(50)
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

      IF EXISTS (SELECT 1 FROM [dbo].[tblEmployee] WITH(NOLOCK) WHERE EmployeeId = @EmployeeId AND IsDeleted=1)   
BEGIN     
SELECT 0 AS [Status], 'Record is already deleted' AS [Message]    
RETURN;   
END  


      IF(ISNULL(@EmployeeId,'') <> '')     
 BEGIN                
 SELECT [Status] = 1, [Message] = 'Data Fetched Sucessfully', [Data] = NULL         
 SELECT               
 [EmployeeId],       
 [FirstName],        
 [MiddleName],       
 [LastName],          
 [DayofBirth],        
 [EmailId],           
 [District],         
 [State],       
 [PinCode],     
 [Address],      
 [Qualification], 
 [CurrentExperience], 
 [JoinDate],           
 [DesignationName],     
 [Gender],              
 [MaritalStatus],   
 [CompanyName],    
 [CompanyAddress],  
 [FatherName],       
 [FatherOccupation],      
 [MontherName],           
 [MontherOcupation],      
 [IsDeleted],             
 [CreatedBy],            
 [CreatedOn],            
 [UpdatedBy],            
 [UpdatedOn], 
 [DeletedOn],
 [DeletedBy], 
 [PhoneNumber],
 [HomePhoneNumber]  
 FROM [dbo].[tblEmployee] WITH(NOLOCK)  
 WHERE EmployeeId = @EmployeeId       
 AND IsDeleted=0

             
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