
using System.Collections.Generic;
namespace WinGrep.Core.Models
{
    public class ContentsResult
    {
        public string FileName { get; set; }
        public int FileLine { get; set; }
        public IEnumerable<string> CaptureGroups { get; set; }
    }
}
