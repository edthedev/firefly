#!/usr/bin/perl

# Firefly4Mac 1.31

use strict;
use Sys::Hostname;
use IO::Socket::INET;
use File::Basename;
my $ssl_report = eval { require Date::Format } && eval { require LWP::UserAgent; } && eval { require Crypt::SSLeay; };

my $windows = 0;
my $dir = "";
if($windows)
{
  $ssl_report=0;
  $dir = "windowsTestData";
}
else
{
  $dir = `whoami`;
}

my $start_time = "";
my $finish_time = "";
if ($ssl_report) {
   $start_time = time2str("%Y/%m/%d:%H:%M:%S", time);
}
my $display_start_time = localtime;

my $minMatches = 2;

print "<html><head><title>Firefly SSN Finder Scan Results</title>";
print "
<style type='text/css'> body {     margin: 0em;    padding: 0em;            font-size: 100%;    font-family: Arial, Helvetica, sans-serif;    background-color: white;}a {    text-decoration: none;}a:link {        color: #3F3F72;    text-decoration: underline;}a:visited {    color: #666;    text-decoration: underline;}a:hover {    color: #D56132;    text-decoration: underline;}#content {    clear: both;    margin: 0 0 0 0px;    padding: 1em 0 4em .5em;    width: 44em;    float: right;    background-color: #FFF;    font-size: .8em;}h1 {    margin: 0 0 0.7em 0;    font-weight: bold;    font-size: 1.8em;    color: #333;    font-variant: small-caps;        }h2 {    margin: 0 0 0.3em 0;           padding: 1em 0 0.3em;    font-size: 1.4em;    font-weight: bold;    color:#333;    border-bottom: solid #4C56A6 1px;}p {    margin: 0;    padding: 0 1em 1.2em;    font-weight: normal;    line-height: 1.4em;    color:#000000;}</style>
";
print "</head><body>";
print "<h1>Firefly SSN Finder Scan Results</h1>";
print "<p>Firefly has identified the following files as possibly containing sensitive data. Please examine each of these files and delete or securely archive each file that contains SSNs or Credit Card numbers. For additional help see <a href='http://www.cites.uiuc.edu/ssnprogram/ssnscanning.html'>http://www.cites.uiuc.edu/ssnprogram/ssnscanning.html</a></p>
";
print "<hr/><h2>Scan Results</h2>
<table><caption>These files may contain sensitive information.</caption><tr><th>Filename</th><th>Suspected sensitive content</th><th>Count</th><th>Actions</th><th>Path to file</th></tr>";

# Now do the work.
my $hostname = "";
my $ip_address = "";
if($ssl_report) {
   $hostname = hostname;
   if ($hostname =~ /[^\.]+\.[^\.]+\./) {
       $ip_address = inet_ntoa((gethostbyname($hostname))[4]);
   }
}
# Scan user's home directory only

chomp($dir);
$dir =  "\/Users\/$dir\/";
if($windows){ $dir = "C:\/Documents and Settings\/delaport\/Desktop\/SSN"}
my $argDir = $ARGV[0];
if (length($argDir) != 0 ) { $dir = $argDir; }

my @ssnKeywords = ('ssn', 'soc*\ sec*\ num*');
my @ccnKeywords = ('cc\ \#', 'ccn', 'credit\ card\ #');
# my @regex = ('ssn', 'soc.+\ sec.+\ num.+', 'cc\D?\#', 'ccn', 'credit\ card\D?#');
# my $ssn_regex = '[12345780][1234890][0-9][\s\-]{0,1}[0-9]{2}[\s\-]{0,1}[0-9]{4}(?!</integer>)';
my $ssn_regex = '[12345780][1234890][0-9][\s\-]{0,1}[0-9]{2}[\s\-]{0,1}[0-9]{4}';
# CC# regexes do not account for CVV, which should never be stored
my $ccn_regex = '(?:\b4[0-9]{3}[\s\-]*[0-9]{4}[\s\-]*[0-9]{4}[\s\-]*[0-9]{4}\b)|(?:\b5[1-5][0-9]{2}[\s\-]*[0-9]{4}[\s\-]*[0-9]{4}[\s\-]*[0-9]{4}\b)|(?:\b6011[\s\-]*[0-9]{4}[\s\-]*[0-9]{4}[\s\-]*[0-9]{4}\b)|(?:\b3[47][0-9]{2}[\s\-]*[0-9]{6}[\s\-]*[0-9]{5}\b)|(?:\b3[068][0-9]{2}[\s\-]*[0-9]{6}[\s\-]*[0-9]{4}\b)';
my @mdout;
my %ssn_log;
my %ccn_log;
my $filesWithMatches=0;
my %ssn_matches;
my %ccn_matches;

foreach my $keyword (@ssnKeywords) 
{
    my @md_files;
    if(!$windows) 
	{ @md_files = `mdfind -onlyin $dir '$keyword'`; }
	else
    { @md_files = <"$dir/*">;}
   
   my $file;
   foreach $file (@md_files) 
   {
      if ($file =~ m/\.eml/)
      {
         # Handle email file
         $ssn_regex = '(?!<integer>)[12345780][1234890][0-9][\s\-]{0,1}[0-9]{2}[\s\-]{0,1}[0-9]{4}';
      }
      chomp $file;
   
      my @strings;
#	  if(!$windows) 
#	  { @strings = `strings '$file'`; }
#	  else
#	  {
	  	open(FILE, "$file");
	  	@strings = <FILE>;
	  	close(FiLE);
#	  }
      
      foreach (@strings) 
      {
         if ($_ =~ m/($ssn_regex)/) 
         { 
                ++$ssn_log{$file};
                my $len = length $1;
                my $frag = substr $1, $len - 3;
                my $xfrag = substr $1, 0, $len -3;
                $xfrag =~ s/\d/X/g;
                $ssn_matches{$file} = $xfrag . $frag;
         }
      }
   }
}

# print "<p>Found \t$ssnCount possible SSN matches</p>";
foreach (keys %ssn_log) {
	my $path = $_;
	my $ssn_count = $ssn_log{$path};
	if ($ssn_count >= $minMatches) {
		my @parsePath = split(//, $path);
		my $file=basename($path);
		my $thisDir=dirname($path);
		print "<tr>";
		print "<tr>";
		print "<td>$file</td>";
		print "<td>$ssn_matches{$path}</td>";
		print "<td>$ssn_count Social Security Number(s)</td>";
		print "<td><a href='$path'>View File</a>";
		print " <a href='$thisDir'>Open Folder</a></td>";
		print "<td>$path</td>";
		print "</tr>";
		++$filesWithMatches;
	}
}

foreach my $keyword (@ccnKeywords) {
    my @md_files;
    if(!$windows) 
	{ @md_files = `mdfind -onlyin $dir '$keyword'`; }
	else
    { @md_files = <"$dir/*">;}
   
   my $file;
   foreach $file (@md_files) {
       chomp $file;
       my @strings;
#	  if(!$windows) 
#	  { @strings = `strings '$file'`; }
#	  else
#	  {
	  	open(FILE, "$file");
	  	@strings = <FILE>;
	  	close(FiLE);
#	  }
       
       foreach (@strings) {
           if ($_ =~ /($ccn_regex)/) {
               ++$ccn_log{$file};
               my $len = length $1;
               my $frag = substr $1, $len - 3;
               my $xfrag = substr $1, 0, $len -3;
               $xfrag =~ s/\d/X/g;
               $ccn_matches{$file} = $xfrag . $frag;
           }
       }
   }   
}

foreach (keys %ccn_log) {
   my $path = $_;
   my $ccn_count = $ccn_log{$path};
   if ($ccn_count >= $minMatches) {
       my @parsePath = split(//, $path);
       my $file=basename($path);
		my $thisDir=dirname($path);
       print "<tr>";
       print "<td>$file</td>";
       print "<td>$ccn_matches{$path}</td>";
       print "<td>$ccn_count credit card number(s)</td>";
       print "<td><a href='$path'>View File</a>";
       print " <a href='$dir'>Open Folder</a></td>";
       print "<td>$path</td>";
       print "</tr>";
       ++$filesWithMatches;
   }
}

if ($filesWithMatches == 0)
{
   print "<tr><td colspan=*>";
   print "<i>Congratulations. None of the files scanned appear to contain sensitive information.</i>";
   print "</td></tr>";
}
print "</table>";
print "<h2>Scan Summary</h2>";

if ($ssl_report) {
   $finish_time = time2str("%Y/%m/%d:%H:%M:%S", time);
}
my $display_finish_time = localtime;
print "<p>Firefly4Mac scanned $dir<br/>";
print "Firefly4Mac found that $filesWithMatches of the files scanned may contain sensitive information.<br/>";
print "These files contain $minMatches or more matches for possible sensitive information.<br/>";
print "Started scanning at $display_start_time<br/>";
print "Finished scanning at $display_finish_time<br/>";
if ($ssl_report) {
  my $xml_report = "<?xml version='1.0'?> <methodCall>  <methodName>LogStruct</methodName>  <params>    <param>        <value><struct>
  <member><name>ScanStarted</name><value>$start_time</value></member>
  <member><name>ReportSource</name><value>MacFirefly</value></member>
  <member><name>LocationScanned</name><value>$dir</value></member>
  <member><name>HostName</name><value>$hostname</value></member>
  <member><name>IpAddresses</name><value>$ip_address</value></member>
  <member><name>ScanFinished</name><value>$finish_time</value></member>
  <member><name>TotalFilesScanned</name><value>0</value></member>
  <member><name>FilesWithResults</name><value>$filesWithMatches</value></member>
     <member><name>SkippedFilesLarge</name><value>0</value></member>
   <member><name>SkippedFilesInUse</name><value>0</value></member>
   <member><name>SkippedFilesPermissions</name><value>0</value></member>
   <member><name>SkippedFilesError</name><value>0</value></member>
   <member><name>SkippedFilesIgnoreExtension</name><value>0</value></member>
   <member><name>MatchThreshhold</name><value>$minMatches</value></member>
   <member><name>ScanPaused</name><value>0</value></member>
   <member><name>SsnMatches</name><value>0</value></member>
   <member><name>CcnMatches</name><value>0</value></member>
   <member><name>SkippedFilesUnRecExt</name><value>0</value></member>
  </struct></value>    </param>  </params></methodCall>";

   my $ua = new LWP::UserAgent;
   my $req = new HTTP::Request 'POST';

   $req->url('https://tools.cites-security.uiuc.edu/firefly/report.0.8.5.cgi');
   $req->user_agent($ua->agent);
   $req->content_type('text/xml');
   $req->content_length(length($xml_report));
   $req->content($xml_report);

   my $res = $ua->request($req);     # send the request
   my $response = $res->content;
   my $success = $res->is_success;
   unless ($success) {
       # print "Attempt to send report to CITES Security failed.<br/>\n";
   }
}
else
{
   # print "Firefly4Mac could not send a report to CITES Security.<br/>";   
}
print "</p>";
print "</body></html>"; 
