
CREATE PROCEDURE [dbo].[uspUserCreatePassword]                
(                
 @Email VARCHAR(150),                
 @PasswordHash VARBINARY(MAX),                
 @PasswordSalt VARBINARY(MAX)          
)                
AS                 
BEGIN        
        
 SET NOCOUNT ON;        
         
 DECLARE @Status BIT=0;          
 DECLARE @Msg VARCHAR(4000);          
 DECLARE @UserId VARCHAR(50);          
 DECLARE @Date DATETIME;    
 DECLARE @UpdatedBy VARCHAR(50);  
  
 SET @Date=GETUTCDATE();   
   
 --OPEN SYMMETRIC KEY EncryptionKey DECRYPTION       
 --BY CERTIFICATE EncryptionCertificate  
         
 BEGIN TRY                       
  IF EXISTS (SELECT 1 FROM [dbo].[tblUsers] WITH(NOLOCK) WHERE Email = @Email)                  
  BEGIN        
   IF EXISTS (SELECT 1 FROM [dbo].[tblUsers] WITH(NOLOCK) WHERE Email = @Email AND IsDeleted=0)        
   BEGIN        
        
    UPDATE [dbo].[tblUsers]                
    SET                 
    PasswordHash=@PasswordHash,                
    PasswordSalt=@PasswordSalt,                      
    UpdatedOn = @Date,                
    UpdatedBy= (
	SELECT TOP(1) [UserID] FROM [dbo].[tblUsers] WITH(NOLOCK) WHERE Email = @Email)       
    WHERE Email = @Email AND IsDeleted=0        
            
    SET @Status=1;          
    SET @Msg='Success';        
         
   END        
   ELSE        
   BEGIN        
    SET @Status = 0;          
    SET @Msg='DELETED';          
   END        
   END   
  ELSE        
  BEGIN        
   SET @Status = 0;          
   SET @Msg='UNF';          
  END        
 END TRY                 
 BEGIN CATCH                       
  SET @Status=0;          
  SET @Msg= ERROR_MESSAGE();                 
  DECLARE @ErrorSeverity INT = ERROR_SEVERITY();                 
  DECLARE @ErrorState INT = ERROR_STATE();                 
  RAISERROR(@Msg,@ErrorSeverity,@ErrorState);                
 END CATCH                   
          
 SELECT @Status Status, @Msg Message           
END