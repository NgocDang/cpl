Import-Module SQLPS;
$sqlFile = @(gci ".\..\SQL\0_InitDB_*.sql")[0]
$serverInstance = ".\SQLEXPRESS"
$database = "CPL"
"Start dropping database " + $database
$dropDatabaseScript = "USE master;ALTER DATABASE " + $database + " SET SINGLE_USER WITH ROLLBACK IMMEDIATE; DROP DATABASE IF EXISTS " + $database
Invoke-Sqlcmd -Query $dropDatabaseScript  -ServerInstance $serverInstance;
"Database " + $database + " is dropped."
$createDatabaseScript = "CREATE DATABASE " + $database
Invoke-Sqlcmd -Query $createDatabaseScript -ServerInstance $serverInstance;
"Database " + $database + " is created."
invoke-sqlcmd -inputFile $sqlFile -ServerInstance $serverInstance
"Database " + $database + " is restored successfully."