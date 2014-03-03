using NDesk.Options;
using System;
using System.Collections.Generic;
using System.IO;
using WinGrep.Core.Models;
using WinGrep.Core.Services;

namespace WinGrep
{
    public class Program
    {
        /// <summary>
        /// WinGrep execution.
        /// </summary>
        /// <param name="args"></param>
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

            

            if (showhelp)
            {
                options.WriteOptionDescriptions(Console.Out);
                return;
            }

            // Validation first, quit if needed.
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

            try
            {
                if (!Path.IsPathRooted(rootdir))
                    rootdir = Environment.CurrentDirectory + @"\" + rootdir;
            }
            catch 
            {
                Console.WriteLine("Directory \"{0}\" is not a well formed path.", rootdir);
                return;
            }


            // Execute our Grep.
            var formatter = new ResultFormatter();
            IEnumerable<ContentsResult> results;
            if (searchnames)
                results = new FileLocator().FindFiles(rootdir, regex, recursive);
            else
                results = new FileLocator().FindInFiles(rootdir, regex, contentregex, recursive);

            foreach (var r in results)
                Console.Write(formatter.FormatResult(r));
        }
    }
}
