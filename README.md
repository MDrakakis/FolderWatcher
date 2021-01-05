# FolderWatcher
 Folder Watcher created on C# Console Application

# Required Nugets
1. MatthiWare.CommandLineParser (https://www.nuget.org/packages/MatthiWare.CommandLineParser) (@MatthiWare)
2. Serilog (https://www.nuget.org/packages/Serilog)
3. Serilog.Sinks.Console (https://www.nuget.org/packages/Serilog.Sinks.Console)
4. Serilog.Sinks.File (https://www.nuget.org/packages/Serilog.Sinks.File)
5. System.Configuration.ConfigurationManager (https://www.nuget.org/packages/System.Configuration.ConfigurationManager)

# Run the program
You can run the program with 2 diferent ways.

1. With command line arguments.
```Text
    -p  | -path   | --path      = Full path of the folder you want to monitor.
    -sd |-sbir    | --sbir      = Include sub folders ? (true/false).
    -f  | -filter | --filter  = Filter files ? (*.*, *.txt, *.html).
```
Ex.:

```Bash
    FolderWatcher.exe -p C:/ -sd true -f *.dll
```

2. With settings in App.config.
```xml
<!-- Options -->
<add key="Path" value=""/> <!-- The full path of the folder you want to monitoring. -->
<add key="Filter" value="*.*"/> <!-- The filter of the files you want to monitor (*.*, *.txt). -->
<add key="IncludeSubfolders" value="true"/> <!-- Include sub folders? true/false. -->
```

# To Clone/Download the project
1. Git Clone
```Bash 
git clone https://github.com/MDrakakis/FolderWatcher.git
```
2. Download the zip from the following link: 
```text
https://github.com/MDrakakis/FolderWatcher/archive/main.zip
```