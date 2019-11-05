param($installPath, $toolsPath, $package, $project)

$project.Properties.Item("PreBuildEvent").Value = "xcopy `"`$(ProjectDir)StaticFiles\*.ico`"  `"`$(ProjectDir)`$(OutDir)StaticFiles\*.ico`" /Y
xcopy `"`$(ProjectDir)nuget\*`"  `"`$(ProjectDir)`$(OutDir)nuget\*`" /Y /I /E 
xcopy `"`$(ProjectDir)StaticFiles\Content\*`"  `"`$(ProjectDir)`$(OutDir)StaticFiles\Content\*`" /Y /I /E
xcopy `"`$(ProjectDir)StaticFiles\Images\*`"  `"`$(ProjectDir)`$(OutDir)StaticFiles\Images\*`" /Y /I /E
xcopy `"`$(ProjectDir)StaticFiles\PackageManager\*`"  `"`$(ProjectDir)`$(OutDir)StaticFiles\PackageManager\*`" /Y /I /E
xcopy `"`$(ProjectDir)StaticFiles\Scripts\*`"  `"`$(ProjectDir)`$(OutDir)StaticFiles\Scripts\*`" /Y /I /E"
	
	
	