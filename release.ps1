dotnet publish Flow.Launcher.Plugin.ZAi -c Release -r win-x64 --no-self-contained
Compress-Archive -LiteralPath Flow.Launcher.Plugin.ZAi/bin/Release/win-x64/publish -DestinationPath Flow.Launcher.Plugin.ZAi/bin/ZAi.zip -Force
