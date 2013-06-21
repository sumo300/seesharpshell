using System;
using System.Management.Automation;
using System.Text.RegularExpressions;

namespace SeeSharpShell {
    [Cmdlet(VerbsCommon.Format, "Path")]
    public class Format_Path : Cmdlet {
        [Parameter(Position = 0, Mandatory = true)]
        public string Path { get; set; }

        protected override void EndProcessing()
        {
            if (string.IsNullOrWhiteSpace(Path)) {
                WriteWarning("Path must be supplied.");
            }

            string home = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            string shortened = Path.Replace(home, "~");

            // Remove prefixes for UNC paths
            shortened = new Regex("^[^:]+::").Replace(shortened, string.Empty);

            // Make path shorter like tabs in Vim, plus handle paths starting
            // with \\ and . correctly
            WriteObject(new Regex(@"\\(\.?)([^\\])[^\\]*(?=\\)").Replace(shortened, @"\$1$2"));
        }
    }
}
