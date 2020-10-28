:: fetch all tags & changes from remote, otherwise changes cannot be determined accurately

:: Assert changelog tool is installed
dotnet tool update --tool-path tools/changelog ^
CS.Changelog.Console

:: Prompt for release name
set /p name="Specify release name (optional):"

::dotnet tool install -g CS.Changelog.Console
tools\changelog\changelog.exe ^
 --verbosity Info ^
 --repositoryurl "https://github.com/Aaltuj/VxFormGenerator/commit/{0}" ^
 --filename "VxFormGenerator.Core\changelog" ^
 --issueformat "(?:#)\d{1,4}" ^
 --outputformat markdown ^
 --releasename "%name%"