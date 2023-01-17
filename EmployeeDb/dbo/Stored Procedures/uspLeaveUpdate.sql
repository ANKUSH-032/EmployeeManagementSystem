
CREATE PROCEDURE [dbo].[uspLeaveUpdate] 
  @LeaveId VARCHAR(50),
  @FromDate VARCHAR(11),
  @ToDate VARCHAR(11), 
  @EmployeeId VARCHAR(50),
  @Reason VARCHAR(1000),
  @LeaveType INT,
  @StatusType INT, 
  @UpdatedBy VARCHAR(50)
AS
BEGIN
SET NOCOUNT ON;

    DECLARE @Status BIT = 0;     


 DECLARE @Message VARCHAR(MAX);   
  
 BEGIN TRY    
 BEGIN         
 IF(ISNULL(@LeaveId,'')= '')      
 BEGIN           
 SELECT 0 AS [Status], 'Please provide proper EmployeeId.' AS [Message]   
 RETURN     
 END

        IF EXISTS(SELECT 1 FROM [dbo].[tblLeave] WITH(NOLOCK) WHERE LeaveId = @LeaveId AND IsDeleted = 1) 
 BEGIN        
 SELECT 0 AS [Status], 'Record with this details is deleted.' AS [Message]         
 RETURN   
 END

        IF NOT EXISTS(SELECT 1 FROM [dbo].[tblLeave] WITH(NOLOCK) WHERE LeaveId = @LeaveId AND IsDeleted = 0)    
 BEGIN            
 SELECT 0 AS [Status], 'Record not present.' AS [Message]     
 RETURN       
 END

        UPDATE [dbo].[tblLeave]
 SET    
	LeaveId = @LeaveId ,
    FromDate = @FromDate ,
    ToDate = @ToDate ,
    EmployeeId = @EmployeeId ,
    Reason = @Reason ,
    LeaveType = @LeaveType ,
    StatusType = @StatusType ,
	UpdatedBy = @UpdatedBy,
	UpdatedOn = GETDATE(),
	IsDeleted = 0
  WHERE  LeaveId = @LeaveId

        SET @Status = 1;          
 SET @Message = 'Record updated successfully.';   
 SELECT @Status [Status], @Message [Message] , @LeaveId [Data]  

    END       
 END TRY     
 BEGIN CATCH  
 SET @Message = ERROR_MESSAGE();  
 DECLARE @ErrorSeverity INT = ERROR_SEVERITY();   
 DECLARE @ErrorState INT = ERROR_STATE();   
 RAISERROR(@Message, @ErrorSeverity, @ErrorState);  
 END CATCH  
END