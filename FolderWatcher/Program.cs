using System;
using System.Configuration;
using Serilog;
using Serilog.Events;

namespace FolderWatcher
{
  class Program
  {
    static void Main(string[] args)
    {
      #region Logger Settings

      Log.Logger = new LoggerConfiguration()
        .WriteTo.File($"{Environment.CurrentDirectory}\\log.txt", LogEventLevel.Information, rollingInterval: RollingInterval.Day)
        .WriteTo.Console(LogEventLevel.Fatal)
        .CreateLogger();

      #endregion Logger Settings

      Watcher watcher = new Watcher();

      while (true)
      {
        if (args.Length == 0)
          watcher.StartWatcherWithConfigOptions();
        else
          watcher.StartWatcherWithArguments(args);

        Console.WriteLine($"{Environment.NewLine} Press any key to exit or press (R) to restart the process...");

        if (Console.ReadKey(true).Key != ConsoleKey.R)
          Environment.Exit(0);
        else
          Console.Clear();

      }
    }
  }
}