--exec [dbo].[uspLeaveInsert] '17/12/2022' , '20/12/2022' , '602EE209-970B-49A8-AA69-9870FC56F76E' ,'ssfsd' , 4 ,'sdssfs'

CREATE PROCEDURE [dbo].[uspLeaveInsert]      
	@FromDate VARCHAR(11),
	@ToDate VARCHAR(11),
	@EmployeeId VARCHAR(50),
	@Reason VARCHAR(1000), 
	@LeaveType INT,
	@CreatedBy VARCHAR(50)
	AS
BEGIN
SET NOCOUNT ON;

 DECLARE @Status BIT = 0;   
 DECLARE @Message VARCHAR(MAX);   
 DECLARE @LeaveId VARCHAR(50)=NEWID(); 
 DECLARE @NumberOfLeave INT;
 DECLARE @LeaveAlreadyTaken INT;
 DECLARE @AssingLeave VARCHAR(20);
 DECLARE @LeaveTypeName VARCHAR(20);

 

    BEGIN TRY       
 BEGIN 

        IF EXISTS(SELECT 1 FROM  [dbo].[tblLeave] WITH(NOLOCK) WHERE LeaveId = @LeaveId)  
 BEGIN              
 SELECT 0 AS [Status], 'Record with same subject is already existed.' AS [Message]            
 RETURN    
 END      


			 IF NOT EXISTS( SELECT 1 FROM [dbo].[tblLeave] WITH(NOLOCK) WHERE LeaveId = @LeaveId AND IsDeleted=1) 
			 BEGIN   
			 
			 SET @NumberOfLeave =  DATEDIFF(day, FORMAT(TRY_CAST(@FromDate as date),'M/d/yyyy'), FORMAT(TRY_CAST(@ToDate as date),'M/d/yyyy')) + 1

			 --SET @NumberOfLeave =  DATEDIFF(day, convert(date, @FromDate)AS Datetime, convert(date, '@ToDate', 111)) + 1

			 SELECT @LeaveAlreadyTaken = SUM([NumberOfLeave]) FROM [dbo].[tblLeave]  WITH(NOLOCK) WHERE EmployeeId = @EmployeeId AND YEAR(FORMAT(TRY_CAST(@FromDate as date),'M/d/yyyy')) = YEAR(GETDATE())  AND  LeaveType = @LeaveType
 
			 SELECT @AssingLeave  = CountLeave FROM [dbo].[tblMasterLeaveType]  WITH(NOLOCK) WHERE  [LeaveTypeId]= @LeaveType 
			 
			 SELECT @LeaveTypeName  = [LeaveType]  FROM [dbo].[tblMasterLeaveType]  WITH(NOLOCK)  WHERE [LeaveTypeId] = @LeaveType 

			 IF(@AssingLeave < @NumberOfLeave)
			 BEGIN
					SELECT 0 AS [Status], 'Can not take leave'   AS [Message]    
					RETURN
			 END
			 
			 IF(@AssingLeave = @LeaveAlreadyTaken)
			 BEGIN
					SELECT 0 AS [Status], 'Can not take leave IT OVER'   AS [Message]    
					RETURN
			 END

			 IF(@AssingLeave < (@LeaveAlreadyTaken + @NumberOfLeave ))
			 BEGIN
					SELECT 0 AS [Status], 'You can take only ' + CONVERT(VARCHAR(10), (@AssingLeave - @LeaveAlreadyTaken))   +' from ' + @LeaveTypeName   AS [Message]   
					RETURN
			 END

			 INSERT INTO   [dbo].[tblLeave]    
			 (  
					LeaveId,
			       	FromDate, 
					ToDate, 
					EmployeeId, 
					Reason, 
					LeaveType, 
					CreatedBy, 
					CreatedOn,
					IsDeleted,
					NumberOfLeave
			 )        
			 VALUES   
			 ( 
			        @LeaveId,
			       	@FromDate, 
					@ToDate, 
					@EmployeeId, 
					@Reason, 
					@LeaveType,
					@CreatedBy,
			        GETDATE(),
			        0,
					@NumberOfLeave
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


    SELECT @Status [Status], @Message [Message] , @LeaveId [Data]  
END