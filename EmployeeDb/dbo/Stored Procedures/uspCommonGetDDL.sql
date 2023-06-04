CREATE PROCEDURE [dbo].[uspCommonGetDDL]
@Type NVARCHAR(50),
@Id NVARCHAR(50)
AS
BEGIN
	 IF(@Type='role')
	 BEGIN
		SELECT RoleId AS Id, RoleName as NAME FROM  [dbo].[tblMasterRole]	WITH(NOLOCK)
	 END
	 IF(@Type='user')
	 BEGIN
		SELECT UserId AS Id, FirstName + ' '+ LastName AS NAME FROM  [dbo].[tblUsers] WITH(NOLOCK) WHERE IsDeleted = 0
	 END 
	 -- IF(@Type='orguser')
	 --BEGIN
		--SELECT UserId AS Id, FirstName + ' '+ LastName AS Name FROM  [dbo].[tblUsers] WITH(NOLOCK) WHERE IsDeleted = 0 AND OrganizationId = @Id
	 --END 
	 --IF(@Type='module')
	 --BEGIN
		--SELECT ModuleId AS Id, ModuleName AS NAME FROM  [dbo].[tblModule]  WITH(NOLOCK) WHERE IsActive=1
	 --END
	 IF(@Type='state')
	 BEGIN
		SELECT StateId AS IdInt, STATE AS NAME FROM  [dbo].[tblMasterState] WITH(NOLOCK)
	 END
	 IF(@Type='city')
	 BEGIN
		SELECT CityId AS IdInt, City AS NAME FROM  [dbo].[tblMasterCity] WITH(NOLOCK ) WHERE StateId=@Id
	 END
	 IF(@Type='zipcode')
	 BEGIN
		SELECT CAST (Zipcode AS BIGINT) AS Id, Zipcode  AS NAME FROM  [dbo].[tblMasterZipcode] WITH(NOLOCK) WHERE CityId=@Id
	 END
	 
END