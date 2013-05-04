using NDesk.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
                {"d=", "root Directory for searching", o => rootdir = o},
                {"R", "enable Recursive", o => recursive = true},
                {"c=", ".net regex for file Contents", o => {searchcontent = true; searchnames = false; contentregex = o;}},
                {"h", "shows Help", o => showhelp = true},
            };

            options.Parse(args);

            if (!Path.IsPathRooted(rootdir))
                rootdir = Environment.CurrentDirectory + @"\" + rootdir;

            if (showhelp)
            {
                options.WriteOptionDescriptions(Console.Out);
                return;
            }

            /* Validation first, quit if needed. */
            if (!new RegexValidator().Validate(regex))
            {
                Console.WriteLine(string.Format("Regex \"{0}\" is not a valid regular expression.", regex));
                return;
            }

            if (!new RegexValidator().Validate(contentregex))
            {
                Console.WriteLine(string.Format("Regex \"{0}\" is not a valid regular expression.", contentregex));
                return;
            }

            /* Execute our Grep. */
            if (searchnames)
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
