  
  
  
CREATE PROCEDURE [dbo].[uspEmployeeInsert]        
@FirstName VARCHAR(50)      ,  
@MiddleName  VARCHAR(50)      ,  
@LastName  VARCHAR(50)        
,@DayofBirth DATETIME      ,  
@EmailId  VARCHAR(50)      ,  
@PhoneNumber  BIGINT      ,  
@HomePhoneNumber BIGINT        
,@District BIGINT       
,@State BIGINT      
,@PinCode BIGINT    
,@Address  VARCHAR(50)     
,@Qualification BIGINT    
,@CurrentExperience  VARCHAR(50)   
,@JoinDate DATETIME        
,@DesignationName BIGINT     
,@Gender  VARCHAR(50)       
,@MaritalStatus  VARCHAR(50)    
,@CompanyName  VARCHAR(50)     
,@CompanyAddress  VARCHAR(50)  
,@FatherName  VARCHAR(50)     
,@FatherOccupation  VARCHAR(50)  
,@MontherName  VARCHAR(50)       
,@MontherOcupation  VARCHAR(50)   
,@IsDeleted BIT       
,@CreatedBy  VARCHAR(50)  
,@Photopath VARCHAR(500)
,@SignaturePath VARCHAR(500)
--,@Password VARCHAR(50)  
AS  
BEGIN  
SET NOCOUNT ON;  
  
    DECLARE @Status BIT = 0;     
 DECLARE @Message VARCHAR(MAX);     
 DECLARE @EmployeeId VARCHAR(50)=NEWID();   
  
    BEGIN TRY         
 BEGIN   
  
        IF EXISTS(SELECT 1 FROM [dbo].[tblEmployee] WITH(NOLOCK) WHERE EmailId = @EmailId)    
 BEGIN                
 SELECT 0 AS [Status], 'Record with same subject is already existed.' AS [Message]              
 RETURN      
 END        
 IF NOT EXISTS( SELECT 1 FROM [dbo].[tblEmployee] WITH(NOLOCK) WHERE EmployeeId = @EmployeeId AND IsDeleted=1)   
 BEGIN           
 INSERT INTO [dbo].[tblEmployee]              
 (            [EmployeeId]       
 ,[FirstName]     
 ,[MiddleName]       
 ,[LastName]       
 ,[DayofBirth]    
 ,[EmailId]       
 ,[PhoneNumber]   
 ,[HomePhoneNumber]   
 ,District    
 ,[State]  
 ,[PinCode]     
 ,[Address]       
 ,[Qualification]    
 ,[CurrentExperience]    
 ,[JoinDate]       
 ,[DesignationName]    
 ,[Gender]      
 ,[MaritalStatus]     
 ,[CompanyName]    
 ,[CompanyAddress]   
 ,[FatherName]        
 ,[FatherOccupation]   
 ,[MontherName]       
 ,[MontherOcupation]    
 ,[IsDeleted]      
 ,[CreatedBy]        
 ,[CreatedOn]  
 ,Photopath 
 ,SignaturePath
 --,[Password]  
 )          
 VALUES     
 (       @EmployeeId   
 ,@FirstName      
 ,@MiddleName     
 ,@LastName       
 ,@DayofBirth    
 ,@EmailId       
 ,@PhoneNumber   
 ,@HomePhoneNumber   
 ,@District  
 ,@State  
 ,@PinCode     
 ,@Address     
 ,@Qualification    
 ,@CurrentExperience    
 ,@JoinDate      
 ,@DesignationName    
 ,@Gender       
 ,@MaritalStatus   
 ,@CompanyName      
 ,@CompanyAddress     
 ,@FatherName      
 ,@FatherOccupation   
 ,@MontherName     
 ,@MontherOcupation  
 ,0      
 ,@CreatedBy   
 ,GETDATE()  
 ,@Photopath 
,@SignaturePath 
 --,@Password  
 )                 
 SET @Status = 1;       
 SET @Message = 'Record added successfully.';     
 END         
 ElSE         
 BEGIN         
 SELECT 0 AS [Status], 'Record with same name is already existed.' AS [Message]     
 END    
 END    
 END TRY     
 BEGIN CATCH    
 SET @Message = ERROR_MESSAGE();      
 DECLARE @ErrorSeverity INT = ERROR_SEVERITY();   
 DECLARE @ErrorState INT = Error_state();      
 RAISERROR(@Message, @ErrorSeverity, @ErrorState);    
 END CATCH    
  
  
    SELECT @Status [Status], @Message [Message] , @EmployeeId [Data]    
END