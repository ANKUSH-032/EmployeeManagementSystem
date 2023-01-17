
CREATE PROCEDURE [dbo].[uspLeaveGetDetails]
@LeaveId VARCHAR(50)
AS
BEGIN
SET NOCOUNT ON;

    DECLARE @Status BIT = 0; 
 DECLARE @Msg VARCHAR(500);     
 BEGIN TRY      
 BEGIN 

      IF NOT EXISTS (SELECT 1 FROM [dbo].[tblLeave] WITH(NOLOCK) WHERE LeaveId = @LeaveId)
 BEGIN       
 SELECT 0 AS [Status], 'No such record exists with this details.' AS [Message]   
 RETURN;   
 END  

      IF EXISTS (SELECT 1 FROM  [dbo].[tblLeave] WITH(NOLOCK) WHERE LeaveId = @LeaveId AND IsDeleted=1)   
BEGIN     
SELECT 0 AS [Status], 'Record is already deleted' AS [Message]    
RETURN;   
END  


      IF(ISNULL(@LeaveId,'') <> '')     
		 BEGIN                
		 SELECT [Status] = 1, [Message] = 'Data Fetched Sucessfully', [Data] = NULL         
		 SELECT               
			 LeaveId, 
			e.[EmailId],
			e.[PhoneNumber],
	        Concat(e.[FirstName],' ', LTRIM(RTRIM(CONCAT(e.[MiddleName], ' ', e.[LastName])))) as EmployeeName,
			FromDate,
			ToDate, 
			l.EmployeeId,
			Reason, 
			lt.LeaveType, 
			ls.StatusType,
			l.CreatedBy, 
			l.CreatedOn,
			l.UpdatedBy,
			l.UpdatedOn,
			l.DeletedBy, 
			l.DeletedOn, 
			l.IsDeleted
		 FROM [dbo].[tblLeave]  l WITH(NOLOCK)
		 LEFT JOIN [dbo].[tblMasterLeaveStatusType] ls ON ls. [StatusId] = l.StatusType
		 LEFT JOIN [dbo].[tblMasterLeaveType] lt ON lt.[LeaveTypeId] = l.LeaveType
		 LEFT JOIN [dbo].[tblEmployee] E WITH(NOLOCK) ON E.EmployeeId = l.EmployeeId
		 WHERE LeaveId = @LeaveId     
		 AND l.IsDeleted=0

             
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