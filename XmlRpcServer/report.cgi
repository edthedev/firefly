#! /usr/bin/python

# Copyright University of Illinois
# Published under the University of Illinois / NCSA Open Source License

# Written by Edward Delaporte
# delaport@illinois.edu

from SimpleXMLRPCServer import CGIXMLRPCRequestHandler
import time
import cx_Oracle
import os
import traceback
from firefly_database_info import *

def ReturnTrue(data):
        return True

def LogStruct(data):
        try:
				dsn = cx_Oracle.makedsn(DB_URL, DB_PORT, DB_NAME)
				conn = cx_Oracle.Connection("%s/%s@%s" % (DB_USER, DB_PASSWORD, dsn))
                c = conn.cursor()
                params = {
                        'HostName':data['HostName'],
                        'ReportSource':data['ReportSource'],
                        'ScanStarted':data['ScanStarted'],
                        'ScanFinished':data['ScanFinished'],
                        'ScanPaused':data['ScanPaused'],
                        'LocationScanned':data['LocationScanned'],
                        'TotalFilesScanned':data['TotalFilesScanned'],
                        'FilesWithResults':data['FilesWithResults'],
                        'SsnMatches':data['SsnMatches'],
                        'CcnMatches':data['CcnMatches'],
                        'SkippedFilesLarge':data['SkippedFilesLarge'],
                        'SkippedFilesInUse':data['SkippedFilesInUse'],
                        'SkippedFilesPermissions':data['SkippedFilesPermissions'],
                        'SkippedFilesError':data['SkippedFilesError'],
                        'SkippedFilesIgnoreExtension':data['SkippedFilesIgnoreExtension'],
                        'SkippedFilesUnRecExt':data['SkippedFilesUnRecExt']
                }
                cmd = """insert into FireflyScan (
                HostName,
                ReportSource,
                LocationScanned,
                TotalFilesScanned,
                FilesWithResults,
                SsnMatches,
                CcnMatches,
                SkippedFilesLarge,
                SkippedFilesInUse,
                SkippedFilesPermissions,
                SkippedFilesError,
                SkippedFilesIgnoreExtension,
                SkippedFilesUnrecognizedExt,
                ScanStarted,
                ScanFinished,
                ScanPaused )
                values
                (
                :HostName,
                :ReportSource,
                :LocationScanned,
                :TotalFilesScanned,
                :FilesWithResults,
                :SsnMatches,
                :CcnMatches,
                :SkippedFilesLarge,
                :SkippedFilesInUse,
                :SkippedFilesPermissions,
                :SkippedFilesError,
                :SkippedFilesIgnoreExtension,
                :SkippedFilesUnRecExt,
                to_date(:ScanStarted, 'yyyy/mm/dd:hh:mi:ss'),
                to_date(:ScanFinished, 'yyyy/mm/dd:hh:mi:ss'),
                :ScanPaused
                )
                """
                # Insert the scan record.
                c.execute(cmd, params)

                # Setup to insert the IpAddresses
                cmd = "insert into IpAddress (hostname, ipaddress) values (:HostName, :IpAddress)"
                ipListString = data['IpAddresses']
                ipList = ipListString.split()
                for ip in ipList:
                        params = { 'IpAddress':ip, 'HostName':data['HostName'] }
                        try:
                                c.execute(cmd, params)
                        except Exception, inst:
                                pass
                c.close()
                conn.commit()
                conn.close()
        except Exception, inst:
                return inst
                # return "Oracle Home: %s Oracle Lib: %s" % (os.environ.get('ORACLE_HOME'), os.environ.get('LD_LIBRARY_PATH'))
        else:
			return True

server = CGIXMLRPCRequestHandler()
server.register_function(LogStruct)
server.handle_request()
