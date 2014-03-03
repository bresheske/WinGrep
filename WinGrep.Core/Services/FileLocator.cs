using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using WinGrep.Core.Models;

namespace WinGrep.Core.Services
{
    public class FileLocator
    {
        /// <summary>
        /// Finds files which filename matches regex regular expression.
        /// </summary>
        /// <param name="root">Root directory to begin searching.</param>
        /// <param name="regex">Regex to match file names.</param>
        /// <param name="recursive">True to recurse subdirectories.</param>
        /// <returns></returns>
        public IEnumerable<ContentsResult> FindFiles(string root, string regex, bool recursive)
        {
            var output = new List<ContentsResult>();

            if (!recursive)
            {
                try
                {
                    var reg = new Regex(regex);
                    var files = Directory
                        .GetFiles(root, "*", SearchOption.TopDirectoryOnly)
                        .Where(x => reg.IsMatch(x));
                    foreach (var f in files)
                    {
                        var match = reg.Match(f);
                        output.Add(new ContentsResult()
                        {
                            CaptureGroups = match.Groups.Cast<Group>().Select(y => y.Value),
                            FileName = f
                        });
                    }
                }
                catch {}
            }
            else
            {
                RecurseDirectory(output, root, regex);
            }


            return output;
        }

        /// <summary>
        /// Searches file contents file contentsregex matches.
        /// </summary>
        /// <param name="root">Root directory to begin searching.</param>
        /// <param name="regex">Regex to match file names.</param>
        /// <param name="recursive">True to recurse subdirectories.</param>
        /// /// <param name="contentsregex">Rgex to match file text contents.</param>
        /// <returns></returns>
        public IEnumerable<ContentsResult> FindInFiles(string root, string fileregex, string contentsregex, bool recursive)
        {
            var output = new List<ContentsResult>();
            var files = FindFiles(root, fileregex, recursive);
            foreach (var f in files)
            {
                string[] text;
                try
                {
                    text = File.ReadAllLines(f.FileName);
                }
                catch (Exception)
                { 
                    // TODO: Verbose errors would help.
                    continue; 
                }

                var reg = new Regex(contentsregex);
                for (int i = 0; i < text.Length; i++)
                    if (reg.IsMatch(text[i]))
                    {
                        var match = reg.Match(text[i]);
                        output.Add(new ContentsResult()
                        {
                            CaptureGroups = match.Groups.Cast<Group>().Select(y => y.Value),
                            FileName = f.FileName,
                            FileLine = i + 1
                        });
                    }
            }
            return output;
        }

        #region Private Methods

        private void RecurseDirectory(List<ContentsResult> files, string root, string regex)
        {
            if (!Directory.Exists(root))
                return;

            try
            {
                files.AddRange(Directory.GetFiles(root, "*", SearchOption.TopDirectoryOnly)
                        .Where(x => new Regex(regex).IsMatch(x))
                        .Select(x => new ContentsResult(){ FileName = x}));
                foreach (var d in Directory.GetDirectories(root))
                    RecurseDirectory(files, d, regex);
            }
            catch { return; }
        }

        #endregion
        
    }
}
