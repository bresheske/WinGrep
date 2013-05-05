using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WinGrep.Core.Models;

namespace WinGrep.Core.Services
{
    public class FileLocator
    {

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
                catch (Exception ex)
                { 
                    /* TODO: Console output does not belong in the core's services. */
                    //Console.WriteLine(string.Format("Error: {0}", ex.Message)); 
                    continue; 
                }

                for (int i = 0; i < text.Length; i++)
                    if (new Regex(contentsregex).IsMatch(text[i]))
                        output.Add(new ContentsResult() { FileLine = i+1, FileName = f });

            }
            return output;
        }
    }
}
