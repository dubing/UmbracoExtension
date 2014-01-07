cd ..\src\packages\FluentMigrator.Tools.1.1.2.1\tools\AnyCPU\40
migrate --conn "UmbracoExtension.Core.Properties.Settings.UmbracoExtensionConnectionString" --configPath="..\..\..\..\..\UmbracoExtension.Web.UI\Web.config" --provider sqlserver2008 --assembly "..\..\..\..\..\UmbracoExtension.FM\bin\Debug\UmbracoExtension.FM.dll" --task migrate --output --outputFilename migrated.sql
pause 