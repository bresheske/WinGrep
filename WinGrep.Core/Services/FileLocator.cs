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
        public IEnumerable<string> FindFiles(string root, string regex, bool recursive)
        {
            var output = new List<string>();

            if (!recursive)
            {
                try
                {
                    output.AddRange(Directory
                        .GetFiles(root, "*", SearchOption.TopDirectoryOnly)
                        .Where(x => new Regex(regex).IsMatch(x)));
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
                    text = File.ReadAllLines(f);
                }
                catch (Exception)
                { 
                    // TODO: Verbose errors would help.
                    continue; 
                }

                for (int i = 0; i < text.Length; i++)
                    if (new Regex(contentsregex).IsMatch(text[i]))
                        output.Add(new ContentsResult() { FileLine = i+1, FileName = f });

            }
            return output;
        }

        #region Private Methods

        private void RecurseDirectory(List<string> files, string root, string regex)
        {
            if (!Directory.Exists(root))
                return;

            try
            {
                files.AddRange(Directory.GetFiles(root, "*", SearchOption.TopDirectoryOnly)
                        .Where(x => new Regex(regex).IsMatch(x)));
                foreach (var d in Directory.GetDirectories(root))
                    RecurseDirectory(files, d, regex);
            }
            catch { return; }
        }

        #endregion
        
    }
}
