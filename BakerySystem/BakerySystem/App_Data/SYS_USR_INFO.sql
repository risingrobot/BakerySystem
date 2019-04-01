CREATE TABLE [dbo].SYS_USR_INFO
(
	[UserId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [UserName] NVARCHAR(50) NULL, 
    [UserPassowrd] NCHAR(10) NULL, 
    [MachineIP] NVARCHAR(50) NULL, 
    [OperatingSystem] NVARCHAR(50) NULL, 
    [LoginTime] DATETIME NULL
)

GO

