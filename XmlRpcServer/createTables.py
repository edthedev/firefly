import cx_Oracle
from firefly_database_info import *
dsn = cx_Oracle.makedsn(DB_URL, DB_PORT, DB_NAME)
connection = cx_Oracle.Connection("%s/%s@%s" % (DB_USER, DB_PASSWORD, dsn))
c = connection.cursor()

dropIpTable = "drop table IpAddress"
addIpTable = """
create table IpAddress (
HostName varchar2(256),
IpAddress varchar2(32),
Added date,
CONSTRAINT noDupes UNIQUE (HostName, IpAddress)
)
"""
addIpTrigger = """
create trigger IpAdded
before insert on IpAddress
for each row
begin
select sysdate into :new.Added from dual;
end;
"""


addScanSeq = """
create sequence ScanIdSeq
start with 1
increment by 1
"""

addScanTrigger = """
create trigger ScanAdded
before insert on FireflyScan
for each row
begin
select ScanIdSeq.nextval into :new.ScanId from dual;
end;
"""
dropScanTable = "drop table FireflyScan"
addScanTable = """
create table FireflyScan (
ScanId number,
HostName varchar2(256),
ReportSource varchar2(32), 
ScanStarted date,
ScanFinished date,
ScanPaused number(1),
LocationScanned varchar(512),
TotalFilesScanned number,
FilesWithResults number,
SsnMatches number,
CcnMatches number,
SkippedFilesLarge number,
SkippedFilesInUse number,
SkippedFilesPermissions number,
SkippedFilesError number,
SkippedFilesIgnoreExtension number,
SkippedFilesUnrecognizedExt number
)
"""

try:
	pass
	# c.execute(dropIpTable)
	# c.execute(dropScanTable)
except:
	pass


c.execute(addIpTable)
c.execute(addIpTrigger)
c.execute(addScanTable)
c.execute(addScanSeq)
c.execute(addScanTrigger)


c.close()
connection.commit()
connection.close()
