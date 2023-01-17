

CREATE PROC [dbo].[uspUserActivityLogInsert] 
( 
	@UserID VARCHAR(50) = NULL, 
	@IpAddress VARCHAR(100) = NULL, 
	@AreaAccessed VARCHAR(1000) = NULL,
	@TimeStamp [datetime] = NULL, 
	@Body VARCHAR(MAX) = NULL, 
	@StatusCode INT = NULL, 
	@Method VARCHAR(500) = NULL
) 
AS 
BEGIN 	
	INSERT INTO [dbo].[tblUserActivityLog]
	(
	[UserID],
	[IpAddress],
	[AreaAccessed],
	[TimpStamp],
	[Body],
	[StatusCode],
	[Method])
	VALUES
	(
	@UserID,
	@IpAddress,
	@AreaAccessed,
	@TimeStamp, 
	@Body,
	@StatusCode, 
	@Method
	)
END