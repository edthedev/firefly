Scans per Month
select count(hostname), trunc(scanfinished,'mm') from fireflyscan
group by trunc(scanfinished,'mm')
order by trunc(scanfinished,'mm')

Files scanned by version 1.0.7 (counts differently than 1.0.8 and later)
SELECT count(unique hostname) as UniqueHosts, SUM(fileswithresults), sum(totalfilesscanned) - sum(skippedfileserror) - sum(skippedfilespermissions) - sum(skippedfilesinuse) - sum(skippedfilesunrecognizedext) as ReallyScanned FROM Fireflyscan WHERE ReportSource = 'Firefly SSN Finder  1.0.7';

Files scanned by version 1.0.8 and later
SELECT count(*) as ReportsSubmitted, SUM(fileswithresults), sum(totalfilesscanned) - sum(skippedfileserror) - sum(skippedfilespermissions) - sum(skippedfilesinuse) as ReallyScanned FROM Fireflyscan WHERE ReportSource != 'Firefly SSN Finder  1.0.7';

Firefly 1.0.7 percentages
SELECT (sum(totalfilesscanned) - sum(skippedfileserror) - sum(skippedfilespermissions) - sum(skippedfilesinuse) - sum(skippedfilesunrecognizedext))/sum(totalfilesscanned)*100 as Scanned, sum(skippedfileserror)/sum(totalfilesscanned)*100 as Errors, sum(skippedfilespermissions)/sum(totalfilesscanned)*100 as Permissions, sum(skippedfilesinuse)/sum(totalfilesscanned)*100 as InUse, sum(skippedfilesunrecognizedext)/sum(totalfilesscanned)*100 as Unrecognized FROM Fireflyscan WHERE ReportSource = 'Firefly SSN Finder  1.0.7';

Firefly 1.0.8 percentages
SELECT sum(totalfilesscanned)/(sum(totalfilesscanned)+sum(skippedfilesunrecognizedext))*100 as Scanned, sum(skippedfileserror)/sum(totalfilesscanned)*100 as Errors, sum(skippedfilespermissions)/sum(totalfilesscanned)*100 as Permissions, sum(skippedfilesinuse)/sum(totalfilesscanned)*100 as InUse, sum(skippedfilesunrecognizedext)/sum(totalfilesscanned)*100 as Unrecognized FROM Fireflyscan WHERE ReportSource = 'Firefly SSN Finder  1.0.8';

Firefly 1.0.8 total reports
select count(*) from FireflyScan Where ReportSource = 'Firefly SSN Finder  1.0.8';

Firefly total files scanned
SELECT sum(totalfilesscanned) - sum(skippedfileserror) - sum(skippedfilespermissions) - sum(skippedfilesinuse) + 16537277 as ReallyScanned FROM Fireflyscan WHERE ReportSource != 'Firefly SSN Finder  1.0.7';

Firefly total stats
select count(unique hostname) as UniqueHosts, count(*) as ReportsSubmitted, SUM(fileswithresults) as ReportedFiles from FireflyScan;

Firefly4Mac Reports
SELECT count(*) as ReportsSubmitted, SUM(fileswithresults), sum(totalfilesscanned) - sum(skippedfileserror) - sum(skippedfilespermissions) - sum(skippedfilesinuse) as ReallyScanned FROM Fireflyscan WHERE ReportSource = 'MacFirefly';

