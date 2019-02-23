rmdir /Q /S .\pub
del /f .\appsettings.unversioned.json
copy .\appsettings.unversioned.prod.json .\appsettings.unversioned.json
del /f .\pub.tar
del /f .\pub.tar.gz
rem call npm run tsc
dotnet publish -c Release -o ./pub -r ubuntu-x64
7zg.exe a -ttar -so pub.tar ./pub | 7zg.exe a -si pub.tar.gz
rmdir /Q /S .\pub
