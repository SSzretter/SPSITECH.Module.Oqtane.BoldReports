del "*.nupkg"
"..\..\oqtane.framework\oqtane.package\nuget.exe" pack SPSITECH.Module.BoldReports.nuspec 
XCOPY "*.nupkg" "..\..\oqtane.framework\Oqtane.Server\Packages\" /Y

