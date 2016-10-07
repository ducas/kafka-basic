del /F /Q *.nupkg
nuget.exe pack Kafka.Basic.csproj -Prop Configuration=Release
nuget.exe setApiKey Rx78*oapaF8q
nuget.exe push *.nupkg -Source http://10.0.10.161:8098/nuget/ -ApiKey Rx78*oapaF8q -NonInteractive
pause