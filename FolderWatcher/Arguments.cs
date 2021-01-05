using MatthiWare.CommandLine.Core.Attributes;

namespace FolderWatcher
{
  class Arguments
  {
    [Name("p", "path"), Description("The full path of the folder you want to monitoring.")]
    public string ArgumentPath { get; set; }

    [Name("sd", "sbir"), Description("Include sub folders? true/false.")]
    public bool IncludeSubDirectories { get; set; }

    [Name("f", "filter"), Description("The filter of the files you want to monitor (*.*, *.txt).")]
    public string Filter { get; set; }
  }
}
