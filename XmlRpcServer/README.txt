To run your own Firefly data collector:

1. Host Report.cgi from this directory on your webserver.
     It requires the Oracle client, and the cx_Oracle Python library.
2. Update firefly_database_info.py with your database information.
3. Run createTables.py once to create the Firefly data collector tables in your Oracle database.
4. Copy firefly_database_info.py to your webserver.
   Warning: This file must be available in your Python path, and 
     should not be under the web root.
5. Modifly line 997 of LogHandler.vb in Windows Firefly to use your own server name.
6. Build and use Windows Firefly.
