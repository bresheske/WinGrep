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
            return (recursive
                ? Directory.GetFiles(root, "*", SearchOption.AllDirectories)
                : Directory.GetFiles(root, "*", SearchOption.TopDirectoryOnly))
                .Where(x => new Regex(regex).IsMatch(Path.GetFileName(x)));
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
