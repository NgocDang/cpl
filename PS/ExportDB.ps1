# Start Script
Set-ExecutionPolicy RemoteSigned

# Set-ExecutionPolicy -ExecutionPolicy:Unrestricted -Scope:LocalMachine
function GenerateDBScript() {
    $serverName = ".\SQLEXPRESS"
    $dbname = "CPL"
    $scriptpath = $PSScriptRoot.TrimEnd('PS') + 'SQL'
    $date = (Get-Date -format yyyyMMdd)
    [System.Reflection.Assembly]::LoadWithPartialName("Microsoft.SqlServer.SMO") | Out-Null
    [System.Reflection.Assembly]::LoadWithPartialName("System.Data") | Out-Null
    $srv = new-object "Microsoft.SqlServer.Management.SMO.Server" $serverName
    $srv.SetDefaultInitFields([Microsoft.SqlServer.Management.SMO.View], "IsSystemObject")
    $db = New-Object "Microsoft.SqlServer.Management.SMO.Database"
    $db = $srv.Databases[$dbname]
    $scr = New-Object "Microsoft.SqlServer.Management.Smo.Scripter"
    $deptype = New-Object "Microsoft.SqlServer.Management.Smo.DependencyType"
    $scr.Server = $srv
    $options = New-Object "Microsoft.SqlServer.Management.SMO.ScriptingOptions"
    $options.AllowSystemObjects = $false
    $options.IncludeDatabaseContext = $true
    $options.IncludeIfNotExists = $true
    $options.ClusteredIndexes = $true
    $options.Default = $true
    $options.DriAll = $true
    $options.Indexes = $true
    $options.NonClusteredIndexes = $true
    $options.IncludeHeaders = $false
    $options.ToFileOnly = $true
    $options.AppendToFile = $true
    $options.ScriptData = $true
    $options.ScriptSchema = $true
    $options.NoCommandTerminator = $false;
    $options.ScriptDrops = $false
    

    # Set options for SMO.Scripter
    $scr.Options = $options

    $options.FileName = $scriptpath + "\0_InitDB_$($date).sql"
    "Start generate SQl Script for database " + $database
    "File name: " + $options.FileName
    #=============
    # Tables
    #=============
    "Starting generate sqlscript for tables"
    New-Item $options.FileName -type file -force | Out-Null
    Foreach ($tb in $db.Tables) {
        If ($tb.IsSystemObject -eq $FALSE) {
            $smoObjects = New-Object Microsoft.SqlServer.Management.Smo.UrnCollection
            $smoObjects.Add($tb.Urn)
            $scr.EnumScript($smoObjects)
        }
    }

    $options.ScriptData = $true
    $scr.Options = $options
  
    #=============
    # Views
    #=============
    "Starting generate sqlscript for views"
    $views = $db.Views | where {$_.IsSystemObject -eq $false}
    Foreach ($view in $views) {
        if ($null -ne $views) {
            $scr.EnumScript($view)
        }
    }

    #=============
    # StoredProcedures
    #=============
    "Starting generate sqlscript for stored procedures"
    $StoredProcedures = $db.StoredProcedures | where {$_.IsSystemObject -eq $false}
    Foreach ($StoredProcedure in $StoredProcedures) {
        if ($null -ne $StoredProcedures) {   
            $scr.EnumScript($StoredProcedure)
        }
    } 

    #=============
    # Functions
    #=============
    "Starting generate sqlscript for funtions"
    $UserDefinedFunctions = $db.UserDefinedFunctions | where {$_.IsSystemObject -eq $false}
    Foreach ($function in $UserDefinedFunctions) {
        if ($null -ne $UserDefinedFunctions) {
            $scr.EnumScript($function)
        }
    } 

    #=============
    # DBTriggers
    #=============
    "Starting generate sqlscript for db triggers"
    $DBTriggers = $db.Triggers
    foreach ($trigger in $db.triggers) {
        if ($null -ne $DBTriggers) {
            $scr.EnumScript($DBTriggers)
        }
    }

    #=============
    # Table Triggers
    #=============
    "Starting generate sqlscript for table triggers"
    Foreach ($tb in $db.Tables) {     
        if ($null -ne $tb.triggers) {
            foreach ($trigger in $tb.triggers) {
                $scr.EnumScript($trigger)
            }
        }
    } 
}

#=============
# Execute
#=============
GenerateDBScript