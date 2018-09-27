for %%G in (*.sql) do sqlcmd /S .\SQLEXPRESS /d CPL -E -i"%%G"
pause