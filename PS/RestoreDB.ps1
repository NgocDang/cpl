# Start Script
Set-ExecutionPolicy RemoteSigned

Import-Module SQLPS;

$sqlFiles = @(Get-ChildItem ".\..\SQL\*.sql")
$serverInstance = ".\SQLEXPRESS"

$database = "CPL"

"Start dropping database " + $database
$dropDatabaseScript = "USE master; DROP DATABASE IF EXISTS " + $database
Invoke-Sqlcmd -Query $dropDatabaseScript  -ServerInstance $serverInstance;
"Database " + $database + " is dropped."

$createDatabaseScript = "CREATE DATABASE " + $database
Invoke-Sqlcmd -Query $createDatabaseScript -ServerInstance $serverInstance;
"Database " + $database + " is created."

foreach ($sqlFile in $sqlFiles) {
    invoke-sqlcmd -inputFile $sqlFile -ServerInstance $serverInstance
}

"Database " + $database + " is restored successfully."

"Deleting files"
Get-ChildItem -Path ".\..\SQL\*.sql" | foreach { $_.Delete()}