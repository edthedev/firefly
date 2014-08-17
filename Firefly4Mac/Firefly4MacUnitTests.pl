#!/usr/bin/perl
# Perl unit tests for Firefly 1.31

use Test::More tests=> 13;
my $outputFile = "out.htm";
my $strOutput = readpipe "perl ~/Desktop/Firefly4Mac/Firefly4Mac.pl";
isnt($strOutput, "", 'Not empty string');
is($strOutput =~ m/<html>/, 1,"Contains HTML tag");
# isnt($strOutput =~ m/angryfish/, 1,"Does not contain 'angryfish'");
isnt($strOutput =~ m/88784\.eml/, 1, "Does not contain 88784.eml");
isnt($strOutput =~ m/innocent\.eml/, 1, "Does not list innocent.eml");
isnt($strOutput =~ m/list of ten digit numbers\.eml/, 1, "10 digit numbers");
is($strOutput =~ m/guilty\.eml/, 1, "Does list guilty.eml");
is($strOutput =~ m/List of delimited SSNs.txt/, 1, "delimited SSNs.txt");
is($strOutput =~ m/List of undelimited SSNs.txt/, 1, "undelimited SSNs.txt");
is($strOutput =~ m/List of undelimited CCNs.txt/, 1, "undelimited CCNs.txt");
is($strOutput =~ m/List of delimited CCNs.txt/, 1, "delimited CCNs.txt");
is($strOutput =~ m/SSN.tar/, 1, "SSN.tar");

# Firefly4Mac cannot detect these test files:
# is($strOutput =~ m/Undelimited SSNs.mdb/, 1, "Undelimited SSNs.MDB");
# is($strOutput =~ m/List of delimited SSNs.doc/, 1, "Delimited SSNs.doc");
# is($strOutput =~ m/List of undelimited SSNs.doc/, 1, "undelimited SSNs.doc");
# is($strOutput =~ m/List of delimited SSNs.docx/, 1, "delimited SSNs.docx");
# is($strOutput =~ m/List of undelimited SSNs.docx/, 1, "undelimited SSNs.docx");
# is($strOutput =~ m/List of delimited CCNs.docx/, 1, "delimited CCNs.docx");
# is($strOutput =~ m/List of undelimited CCNs.docx/, 1, "undelmited CCNs.docx");
# is($strOutput =~ m/Just 2 SSNs.txt/, 1, "");
# is($strOutput =~ m/Just 2 SSNs.txt/, 1, "");
# is($strOutput =~ m/SSN.zip/, 1, "SSN.zip");

#         NUnit.Framework.Assert.IsFalse(ReportContents.Contains("ShouldSkip.zip"))
isnt($strOutput =~ m/List of ten digit numbers\.txt/, 1, "Does not contain list of ten digit numbers.");
#         NUnit.Framework.Assert.IsFalse(ReportContents.Contains("List of undelimited SSNs.png"))
#         NUnit.Framework.Assert.IsFalse(ReportContents.Contains("List of undelimited SSNs.jpg"))
#         NUnit.Framework.Assert.IsFalse(ReportContents.Contains("List of undelimited SSNs.bmp"))

open(MYOUT, '>out.htm');
print MYOUT $strOutput;
close(MYOUT);
is(-e $outputFile, 1, 'Output file exists');