using NDesk.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinGrep.Core.Services;

namespace WinGrep
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var regex = ".*";
            var contentregex = string.Empty;
            var rootdir = Environment.CurrentDirectory;
            var searchnames = true;
            var searchcontent = false;
            var recursive = false;
            var showhelp = false;

            var options = new OptionSet()
            {
                {"r=", ".net Regex for matching file names", o => regex = o},
                {"d=", "root Directory for searching", o => regex = o},
                {"R", "enable Recursive", o => recursive = true},
                {"c=", "search regex for file Contents, not just names", o => {searchcontent = true; searchnames = false; contentregex = o;}},
                {"h", "enable Recursive", o => showhelp = true},
            };

            options.Parse(args);

            if (showhelp)
            {
                options.WriteOptionDescriptions(Console.Out);
            }
            else if (searchnames)
            {
                var results = new FileLocator().FindFiles(rootdir, regex, recursive);
                foreach (var r in results)
                    Console.WriteLine(r);
            }
            else if (searchcontent)
            {
                var results = new FileLocator().FindInFiles(rootdir, regex, contentregex, recursive);
                foreach (var r in results)
                    Console.WriteLine(string.Format("{0}:{1}", r.FileName, r.FileLine));
            }
        }
    }
}
