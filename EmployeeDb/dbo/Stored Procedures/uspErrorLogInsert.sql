
CREATE PROC [dbo].[uspErrorLogInsert] 
( 
	@Controller VARCHAR(100) null, 
	@Action  VARCHAR(100) NULL, 
	@Message VARCHAR(1000) NULL, 
	@Source  VARCHAR(3000) NULL, 
	@StackTrace VARCHAR(3000) NULL, 
	@LineNo INT NULL, 
	@RequestBody VARCHAR(MAX)=NULL,
	@CreatedBy VARCHAR(200) NULL 
) 
AS 
BEGIN 
    INSERT INTO [dbo].[tblErrorLog]
	(
	Controller,
	[Action],
	[Message],
	[Source],
	[StackTrace],
	[LineNo],
	RequestBody,
	CreatedOn,
	CreatedBy
	)
	VALUES(
	@Controller,
	@Action,
	@Message,
	@Source,
	@StackTrace,
	@LineNo,
	@RequestBody,
	GETUTCDATE(),
	@CreatedBy
	) 
END