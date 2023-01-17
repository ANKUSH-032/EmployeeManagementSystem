

-- execute [dbo].[uspLoginOrLogout] 'E2F58120-34E4-4DC0-87D3-5B8B94E42D05','vikash',0,0,'string3@gmail.com','string1@123'

CREATE PROCEDURE [dbo].[uspLoginOrLogout]
	@EmployeeId VARCHAR(50),
	@CreatedBy	VARCHAR(50),
	@IsDeleted BIT, 
	--@UpdatedBy VARCHAR(50),
	@AttendenceStatus BIT,
	--@CreatedOn DATETIME
	@EmailId VARCHAR(50)
	--@Password VARCHAR(50)
	
AS
BEGIN
	SET NOCOUNT ON;
	 DECLARE @Status BIT = 0;   
     DECLARE @Message VARCHAR(MAX);   
     DECLARE @AttendenceId VARCHAR(50)=NEWID(); 

	 BEGIN TRY       
 BEGIN


--IF EXISTS(SELECT 1 FROM [dbo].[tblAttendence]  WITH(NOLOCK) WHERE AttendenceId = @AttendenceId)  
-- BEGIN              
--       SELECT 0 AS [Status], 'Record with same subject is already existed.' AS [Message]            
--       RETURN    
-- END   
						IF EXISTS (SELECT 1 FROM [dbo].[tblEmployee] WITH(NOLOCK) WHERE EmployeeId = @EmployeeId AND IsDeleted=1)          
			BEGIN         
					SELECT 0 AS [Status], 'Employee is already deteted' AS [Message]  
					RETURN;     
			END 

			IF EXISTS (SELECT 1 FROM  [dbo].[tblAttendence] WITH(NOLOCK) WHERE EmployeeId = @EmployeeId AND AttendenceStatus=1)          
			BEGIN         
					SELECT 0 AS [Status], 'Employee already LogOut' AS [Message]  
					--RETURN;     
			END 


			 IF EXISTS(SELECT 1 FROM [dbo].[tblUsers]   WITH(NOLOCK) WHERE Email = @EmailId )--AND [Password] = @Password AND EmployeeId = @EmployeeId)  
			 BEGIN 
				
					IF(SELECT COUNT(AttendenceId) FROM [dbo].[tblAttendence] WHERE EmployeeId = @EmployeeId AND (FromTime is null OR  DATEDIFF(day,FromTime,GetDate())= 0)) = 0
				    BEGIN
						 -- Login
							INSERT INTO [dbo].[tblAttendence] 
							(AttendenceId
							,EmployeeId
							,FromTime
							,CreatedBy
							,IsDeleted
							,CreatedOn
							,AttendenceStatus)
							Values
							(@AttendenceId, 
							@EmployeeId, 
							GETDATE(),
							@CreatedBy,
							0,
							GETDATE(),
							0
							)
							SET @Status = 1;     
						    SET @Message = 'Login successfully.'; 
				    END
				  
				    ELSE
				    BEGIN
					-- Logout
							UPDATE [dbo].[tblAttendence]  SET
								ToTime = GETDATE(),
								--UpdateBy = @UpdatedBy,
								UpdatedOn = GETDATE(),
								IsDeleted = 0,
								AttendenceStatus = 1
							WHERE 
								EmployeeId=@EmployeeId AND
								ToTime is null AND
								DATEDIFF(day,FromTime,GetDate())= 0
								--or 
								--EmployeeId=@EmployeeId AND
								--ToTime is not null AND
								--DATEDIFF(day,FromTime,GetDate())= 0
								SET @Status = 1;     
							    SET @Message = 'LogOut successfully.'; 
					END
				END
				ELSE
				BEGIN
								  SELECT 0 AS [Status], 'EmailId Or Password Or EmployeeId not match' AS [Message]            
							      RETURN  
			    END
END

END TRY   
				BEGIN CATCH  
				SET @Message = ERROR_MESSAGE();    
				DECLARE @ErrorSeverity INT = ERROR_SEVERITY(); 
				DECLARE @ErrorState INT = Error_state();    
				RAISERROR(@Message, @ErrorSeverity, @ErrorState);  
				END CATCH
				SELECT @Status [Status], @Message [Message] , @AttendenceId [Data]  
END