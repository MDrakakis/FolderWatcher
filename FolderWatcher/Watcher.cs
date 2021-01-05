using System;
using System.Configuration;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Reflection;
using MatthiWare.CommandLine;
using MatthiWare.CommandLine.Abstractions.Parsing;
using Serilog;

namespace FolderWatcher
{
  internal class Watcher
  {
    #region Error Codes

    private enum ExitCode
    {
      //One or more arguments are not correct.
      ERROR_BAD_ARGUMENTS = 160,

      // Parser error.
      ERROR_PARSER = 500,

      // Parser Options error.
      ERROR_PARSE_OPTIONS = 510,

      // Parser Results error.
      ERROR_PARSE_RESULTS = 520
    }

    #endregion Error Codes

    #region Public Methods

    public void StartWatcherWithConfigOptions()
    {
      Console.Clear();

      FileSystemWatcher watcher = FileWatcherSettings();

      watcher.Changed += new FileSystemEventHandler(OnChanged);
      watcher.Created += new FileSystemEventHandler(OnCreate);
      watcher.Deleted += new FileSystemEventHandler(OnDelete);
      watcher.Renamed += new RenamedEventHandler(OnRenamed);

      Console.Write(@$" - Watcher starts with the following settings -
 Path: {ConfigurationManager.AppSettings.Get("Path")}
 Include Sub Folders: {ConfigurationManager.AppSettings.Get("IncludeSubfolders")}
 Filter: {ConfigurationManager.AppSettings.Get("Filter")}
 ---------------------------------------------
 Scanning...");

      //new AutoResetEvent(false).WaitOne();
    }

    public void StartWatcherWithArguments(string[] arguments)
    {
      Console.Clear();

      CommandLineParserOptions parserOptions = ParserOptions();
      CommandLineParser<Arguments> commandLineParser = CommandLineParser(parserOptions);
      IParserResult<Arguments> parsedArgs = ParserResult(commandLineParser, arguments);

      Arguments args = parsedArgs.Result;

      FileSystemWatcher watcher = FileWatcherSettings(args);

      watcher.Changed += new FileSystemEventHandler(OnChanged);
      watcher.Created += new FileSystemEventHandler(OnCreate);
      watcher.Deleted += new FileSystemEventHandler(OnDelete);
      watcher.Renamed += new RenamedEventHandler(OnRenamed);



      Console.Write(@$" - Watcher starts with the following settings -
 Path: {args.ArgumentPath}
 Include Sub Folders: {args.IncludeSubDirectories}
 Filter: {args.Filter}
 ---------------------------------------------
 Scanning...");

      // new AutoResetEvent(false).WaitOne();
    }

    #endregion Public Methods

    #region Parser / File Wather Settings

    private FileSystemWatcher FileWatcherSettings([Optional] Arguments arguments)
    {
      FileSystemWatcher fileWatcher = new FileSystemWatcher();

      try
      {
        if (arguments != null)
        {
          fileWatcher.Path = arguments.ArgumentPath;
          fileWatcher.IncludeSubdirectories = arguments.IncludeSubDirectories;
          fileWatcher.Filter = arguments.Filter;
        }
        else
        {
          fileWatcher.Path = !string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings.Get("Path")) ? ConfigurationManager.AppSettings.Get("Path") : throw new Exception("Invalid path...");
          fileWatcher.IncludeSubdirectories = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("IncludeSubfolders"));
          fileWatcher.Filter = !string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings.Get("Filter")) ? ConfigurationManager.AppSettings.Get("Filter") : string.Empty;
        }

        fileWatcher.EnableRaisingEvents = true;
        fileWatcher.NotifyFilter = NotifyFilters.FileName | NotifyFilters.Size;
      }
      catch (Exception ex)
      {
        ex.Source = MethodBase.GetCurrentMethod().Name;
        Log.Fatal($"Source: {ex.Source}() {Environment.NewLine} Exception: {ex.Message}");
        Environment.Exit((int)ExitCode.ERROR_BAD_ARGUMENTS);
      }

      return fileWatcher;
    }

    private CommandLineParserOptions ParserOptions()
    {
      CommandLineParserOptions options = null;

      try
      {
        options = new CommandLineParserOptions()
        {
          AppName = "FolderWatcher"
        };
      }
      catch (Exception ex)
      {
        ex.Source = MethodBase.GetCurrentMethod().Name;
        Log.Fatal($"Source: {ex.Source}() {Environment.NewLine} Exception: {ex.Message}");
        Environment.Exit((int)ExitCode.ERROR_PARSE_OPTIONS);
      }

      return options;
    }

    private CommandLineParser<Arguments> CommandLineParser(CommandLineParserOptions commandLineParserOptions)
    {
      CommandLineParser<Arguments> commandLineParser = null;

      try
      {
        commandLineParser = new CommandLineParser<Arguments>(commandLineParserOptions);
      }
      catch (Exception ex)
      {
        ex.Source = MethodBase.GetCurrentMethod().Name;
        Log.Fatal($"Source: {ex.Source}() {Environment.NewLine} Exception: {ex.Message}");
        Environment.Exit((int)ExitCode.ERROR_PARSER);
      }

      return commandLineParser;
    }

    private IParserResult<Arguments> ParserResult(CommandLineParser<Arguments> commandLineParser, string[] args)
    {
      IParserResult<Arguments> results = null;

      try
      {
        results = commandLineParser.Parse(args);

        if (results.HasErrors)
          throw new Exception("Parsing commands error...");

      }
      catch (Exception ex)
      {
        ex.Source = MethodBase.GetCurrentMethod().Name;
        Log.Fatal($"Source: {ex.Source}() {Environment.NewLine} Exception: {ex.Message}");
        Environment.Exit((int)ExitCode.ERROR_PARSE_RESULTS);
      }

      return results;
    }

    #endregion Parser / File Wather Settings

    #region Events

    private static void OnChanged(object source, FileSystemEventArgs e)
    {
      Console.ForegroundColor = ConsoleColor.Yellow;
      Console.WriteLine($"File: {e.FullPath} {e.ChangeType}");
      Log.Information($"File: {e.FullPath} {e.ChangeType}");
      Console.ForegroundColor = ConsoleColor.White;
    }

    private static void OnDelete(object sender, FileSystemEventArgs e)
    {
      Console.ForegroundColor = ConsoleColor.Red;
      Console.WriteLine($"File: {e.FullPath} {e.ChangeType}");
      Log.Warning($"File: {e.FullPath} {e.ChangeType}");
      Console.ForegroundColor = ConsoleColor.White;
    }

    private static void OnRenamed(object source, RenamedEventArgs e)
    {
      Console.ForegroundColor = ConsoleColor.Cyan;
      Console.WriteLine($"File: {e.OldFullPath} {e.ChangeType} to {e.FullPath}");
      Log.Information($"File: {e.OldFullPath} {e.ChangeType} to {e.FullPath}");
      Console.ForegroundColor = ConsoleColor.White;
    }

    private static void OnCreate(object sender, FileSystemEventArgs e)
    {
      Console.ForegroundColor = ConsoleColor.Green;
      Console.WriteLine($"File: {e.FullPath} {e.ChangeType}");
      Log.Information($"File: {e.FullPath} {e.ChangeType}");
      Console.ForegroundColor = ConsoleColor.White;
    }

    #endregion Events
  }
}
