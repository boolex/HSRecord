@echo off
if exist C:\prog\HSRecord\tests\testResults.trx (
     del C:\prog\HSRecord\tests\testResults.trx
     echo results.trx deleted) else (echo result file not found)

"C:\Program Files\Microsoft Visual Studio 12.0\Common7\IDE\mstest.exe" /testcontainer:"C:/prog/HSRecord/HSRecord/HSRecord.UnitTests/bin/Debug/HSRecord.UnitTests.dll" /detail:outcometext  /resultsfile:"C:\prog\HSRecord\tests\testResults.trx"
pause