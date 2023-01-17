



CREATE PROCEDURE [dbo].[uspEmployeeUpdate] 
@EmployeeId VARCHAR(50)    
,@FirstName VARCHAR(50)   
,@MiddleName  VARCHAR(50) 
,@LastName  VARCHAR(50)   
,@DayofBirth DATETIME     
,@EmailId  VARCHAR(50)    
,@PhoneNumber  BIGINT     
,@HomePhoneNumber BIGINT  
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
,@UpdatedBy VARCHAR(50)
--,@Password VARCHAR(50)
AS
BEGIN
SET NOCOUNT ON;

    DECLARE @Status BIT = 0;     


 DECLARE @Message VARCHAR(MAX);   
 
					BEGIN TRY    
					BEGIN         
					IF(ISNULL(@EmployeeId,'')= '')      
					BEGIN           
					SELECT 0 AS [Status], 'Please provide proper Eventid.' AS [Message]   
					RETURN     
					END

					       IF EXISTS(SELECT 1 FROM [dbo].[tblEmployee] WITH(NOLOCK) WHERE EmployeeId = @EmployeeId AND IsDeleted = 1) 
					BEGIN        
					SELECT 0 AS [Status], 'Record with this details is deleted.' AS [Message]         
					RETURN   
					END

					       IF NOT EXISTS(SELECT 1 FROM [dbo].[tblEmployee] WITH(NOLOCK) WHERE EmployeeId = @EmployeeId AND IsDeleted = 0)    
					BEGIN            
					SELECT 0 AS [Status], 'Record not present.' AS [Message]     
					RETURN       
					END

					       UPDATE [dbo].[tblEmployee]          
					SET             EmployeeId=@EmployeeId ,  
					FirstName = @FirstName ,    
					MiddleName = @MiddleName,      
					LastName  = @LastName  ,      
					DayofBirth = @DayofBirth ,   
					EmailId  = @EmailId  ,     
					PhoneNumber  = @PhoneNumber  ,   
					HomePhoneNumber = @HomePhoneNumber,  
					District = @District ,    
					[State] = @State ,   
					PinCode = @PinCode ,  
					[Address]  = @Address,  
					Qualification = @Qualification ,  
					CurrentExperience= @CurrentExperience  ,    
					JoinDate = @JoinDate ,    
					DesignationName = @DesignationName , 
					Gender  = @Gender  ,     
					MaritalStatus  = @MaritalStatus  ,  
					CompanyName  = @CompanyName  ,   
					CompanyAddress  = @CompanyAddress ,  
					FatherName  = @FatherName  ,   
					FatherOccupation = @FatherOccupation ,  
					MontherName  =@MontherName  ,  
					MontherOcupation =@MontherOcupation ,   
					IsDeleted = 0 ,            
					UpdatedOn        =GETUTCDATE(),
					UpdatedBy        =@UpdatedBy  
					--[Password]  = @Password
					WHERE EmployeeId = @EmployeeId

					       SET @Status = 1;          
					SET @Message = 'Record updated successfully.';   
					SELECT @Status [Status], @Message [Message] , @EmployeeId [Data]  

					   END       
					END TRY     
 BEGIN CATCH  
 SET @Message = ERROR_MESSAGE();  
 DECLARE @ErrorSeverity INT = ERROR_SEVERITY();   
 DECLARE @ErrorState INT = ERROR_STATE();   
 RAISERROR(@Message, @ErrorSeverity, @ErrorState);  
 END CATCH  
END