Simple BoldReports Module For Oqtane

The base module is the standard output created using the Oqtane Admin Console.

To compile:

1. Clone this repository and Oqtane.Framework to c:\    (https://github.com/oqtane/oqtane.framework)
2. Open the Oqtane.Framework visual studio solution and rebuild all.    Run the solution and install Oqtane per their instructions.
3. Rebuild this module's solution and the 'package' project will copy the require .dll and other resources to the oqtane.framework.    (if you use a different path you must edit the debug.cmd file)
4. Run the module (IIS) and Oqtane will launch and the module should be visible from the admin console.
5. To add the module to the UI, add a page and then add the BoldReports module to the page.

This has been tested with Oqtane 5.2.1 - 5.2.3 and Bold Reports 6.2.38 as of 10/10/2024.

There is a simple API that allow the BoldReports Designer to load and save reports from server via the menu in Designer.

Known issues:
 The bold reports designer is not rendering on first load of the page.   Refreshing or shift-refreshing will usually cause it to load.
 Report preview is not working and generates errors.

 I have a ticket open with BoldReports support and they are looking at these issues.
