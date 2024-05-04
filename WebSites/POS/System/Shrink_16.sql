USE TOPOS_DB;
GO

--Backup

DECLARE @Sql_Query NVARCHAR(MAX)
DECLARE @Day_Of_Week NVARCHAR(MAX)

SET @Day_Of_Week = (SELECT DATENAME(dw,GETDATE()))

--
EXEC sp_configure 'show advanced options', 1;  
RECONFIGURE;  

EXEC sp_configure 'xp_cmdshell', 1;  
RECONFIGURE;  

SET @Sql_Query = '
EXEC Master.DBO.xp_cmdshell ''del E:\Backup\GARDEN_CRM\GARDEN_CRM_' + @Day_Of_Week + '.Bak''  
EXEC Master.DBO.xp_cmdshell ''del E:\Backup\TOPOS_DB\TOPOS_DB_' + @Day_Of_Week + '.Bak''  
'

EXEC(@Sql_Query)

EXEC sp_configure 'xp_cmdshell', 0;  
RECONFIGURE

SET @Sql_Query = '
USE GARDEN_CRM
BACKUP DATABASE GARDEN_CRM 
TO DISK = ''E:\Backup\GARDEN_CRM\GARDEN_CRM_' + @Day_Of_Week + '.Bak''  
WITH NAME = ''GARDEN_CRM_' + @Day_Of_Week + '''
'

EXEC(@Sql_Query)

SET @Sql_Query = '
USE TOPOS_DB
BACKUP DATABASE TOPOS_DB 
TO DISK = ''E:\Backup\TOPOS_DB\TOPOS_DB_' + @Day_Of_Week + '.Bak''  
WITH NAME = ''TOPOS_DB_' + @Day_Of_Week + '''
'

EXEC(@Sql_Query)